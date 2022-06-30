namespace TinyWorld {

	internal record WorldSave {

		public uint Seed { get; init; }
		public int Position { get; init; }
		public uint WorldIndex { get; init; }
		public bool IsDark { get; init; }

	}

}