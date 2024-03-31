namespace MZZT.Input {
	public class UserInputBlocker : IDisposable {
		public UserInputBlocker() => count++;

		private static byte count = 0;

		public static bool IsUserInput => count == 0;

		#region IDisposable Support
		private bool disposedValue = false;

		protected virtual void Dispose(bool disposing) {
			if (!this.disposedValue) {
				if (disposing) {
					count--;
				}

				this.disposedValue = true;
			}
		}

		public void Dispose() {
			this.Dispose(true);
		}
		#endregion
	}
}
