using System;
using System.Threading;
using Mathy;
using TinyWorld.Builder;
using TinyWorld.Util;

namespace TinyWorld {

	internal sealed class App : AppConfig {

		internal App (TickConfig ticksConfig) {
			Ticks = ticksConfig;
			PerformConsoleAdjustments();
			Clear();
			Console.CancelKeyPress += Restore;
		}

		private enum AppAction {

			Exit,
			PreviousSeed,
			NextSeed,
			PreviousWorld,
			NextWorld,
			ToggleDark,
			Load

		}

		private readonly Vec2 _infoOrigin = new(1, 7);

		internal void Run () {
			Rand rand = new();
			uint worldIndex = 0;
			int locationOffset = 0;
			bool dark = false;
			bool sleep = !Program.NoSleep;

			do {
				worldIndex %= 64;
				rand = new Rand(0, (uint)(rand.Seed + locationOffset));
				locationOffset = 0;

				switch (Start(sleep, rand, (int)worldIndex, dark)) {
					case AppAction.Exit:
						Restore();

						return;
					case AppAction.PreviousSeed:
						--locationOffset;

						break;
					case AppAction.NextSeed:
						++locationOffset;

						break;
					case AppAction.PreviousWorld:
						--worldIndex;

						break;
					case AppAction.NextWorld:
						++worldIndex;

						break;
					case AppAction.ToggleDark:
						dark = !dark;

						break;
					case AppAction.Load:
						WorldSave? save = IOManager.Load<WorldSave>("world.json");

						if (save != null) {
							rand = new Rand(save.Position, save.Seed);
							worldIndex = save.WorldIndex;
							Position(_infoOrigin);
							Write("\x1b[49m\x1b[38;5;46mWorld loaded.       \x1b[39m");
						} else {
							Position(_infoOrigin);
							Write("\x1b[49m\x1b[38;5;196mNo world save found.\x1b[39m");
						}

						break;
				}

				// Only do intro sleep on the first run
				sleep = false;
			} while (true);
		}

		private AppAction Start (bool sleep, Rand rand, int worldIndex, bool dark) {
			// Top-left corner of the map
			Vec2 mapOrigin = new(30, 1);
			Vec2 mapSize = new Vec2(Console.WindowWidth, Console.WindowHeight) - mapOrigin - Vec2.One;

			UIBuilder.Config uiBuilderConfig = new() {
				TitleOrigin = Vec2.Zero,
				InfoOrigin = _infoOrigin,
				InfoSize = new Vec2(mapOrigin.X - 2, Console.WindowHeight - 1) - _infoOrigin,
				MapOrigin = mapOrigin,
				MapSize = mapSize,
				WorldHueIndex = worldIndex
			};

			UIBuilder uiBuilder = new(sleep, uiBuilderConfig, rand);
			IMap map = new Map(mapOrigin, mapSize);
			MapEntityBuilder mapEntityBuilder = new(map, mapSize, rand);

			ConvertTicks();
			BuildUI(sleep, uiBuilder);
			mapEntityBuilder.FillTerrain(worldIndex, dark);
			mapEntityBuilder.FillCreatures(worldIndex);

			return AppLoop(map, uiBuilder, rand, worldIndex);
		}

		private AppAction AppLoop (IMap map, UIBuilder ui, Rand rand, int worldIndex) {
			double animationTimer = 0;
			double actionTimer = 0;
			Ticker.Tick();

			do {
				while (!Console.KeyAvailable) {
					actionTimer += Ticker.DeltaTime;
					animationTimer += Ticker.DeltaTime;
					bool uiChanged = false;

					if (map.DidChange) {
						map.DrawMap();
						uiChanged = true;
					}

					if (actionTimer > SecondsPerActionTick) {
						map.ActionTick();
						actionTimer -= SecondsPerActionTick
							* (int)(actionTimer / SecondsPerActionTick);
						uiChanged = true;
					}

					if (animationTimer > SecondsPerAnimationTick) {
						map.AnimationTick();
						map.DrawCreatures();

						animationTimer -= SecondsPerAnimationTick
							* (int)(animationTimer / SecondsPerAnimationTick);
						uiChanged = true;
					}

					if (uiChanged) {
						ui.DisplayFrameTime(Ticker.DeltaTime);
					}

					int remainingDelay = (int)((SecondsPerTick - Ticker.DeltaTime) * 1000);

					if (remainingDelay > 0) {
						Thread.Sleep(remainingDelay);
					}

					Ticker.Tick();
				}

				// Process input
				switch (Console.ReadKey(true).Key) {
					case ConsoleKey.Escape: return AppAction.Exit;
					case ConsoleKey.A: return AppAction.PreviousSeed;
					case ConsoleKey.D: return AppAction.NextSeed;
					case ConsoleKey.Q: return AppAction.PreviousWorld;
					case ConsoleKey.E: return AppAction.NextWorld;
					case ConsoleKey.F: return AppAction.ToggleDark;
					case ConsoleKey.F2: return AppAction.Load;
					case ConsoleKey.F1:
						WorldSave save = new() {
							Seed = rand.Seed,
							Position = rand.Position,
							WorldIndex = (uint)worldIndex
						};

						IOManager.Save(save, "world.json");
						Position(_infoOrigin);
						Write("\x1b[49m\x1b[38;5;46mWorld saved.        \x1b[39m");

						break;
				}
			} while (true);
		}

		private static void BuildUI (bool sleep, UIBuilder uiBuilder) {
			uiBuilder.DrawTitleLoading();
			Thread.Sleep(sleep ? 750 : 0);
			uiBuilder.DrawTitle();
			Thread.Sleep(sleep ? 750 : 0);
			uiBuilder.DrawUIEdges();
			Thread.Sleep(sleep ? 50 : 0);

			if (sleep) {
				uiBuilder.DrawPreMapImage();
				Thread.Sleep(sleep ? 1000 : 0);
			}

			uiBuilder.DisplayInfo();
		}

		private static void Restore (object? sender, ConsoleCancelEventArgs e) {
			Restore();
		}

		private static void Restore () {
			Clear();
			RevertConsoleAdjustments();
		}

	}

}