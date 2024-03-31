using MZZT.Diagnostics;
using System.ComponentModel;
using System.Runtime.InteropServices;
using static MZZT.WinApi.PInvoke.User32;

namespace MZZT.Windows {
	public class Desktop(string name) : IDisposable {
		public static Desktop ActiveDesktop {
			get {
				return FromHandle(OpenInputDesktop(0, false,
					DESKTOP.CREATEMENU | DESKTOP.CREATEWINDOW | DESKTOP.ENUMERATE | DESKTOP.HOOKCONTROL |
					DESKTOP.JOURNALPLAYBACK | DESKTOP.JOURNALRECORD | DESKTOP.READOBJECTS |
					DESKTOP.SWITCHDESKTOP | DESKTOP.WRITEOBJECTS));
			}
		}

		public static Desktop CurrentThreadDesktop {
			get {
#pragma warning disable CS0618 // Type or member is obsolete
				return FromHandle(GetThreadDesktop(AppDomain.GetCurrentThreadId()));
#pragma warning restore CS0618 // Type or member is obsolete
			}
		}

		public static Desktop FromHandle(IntPtr handle) {
			IntPtr buffer = IntPtr.Zero;
			GetUserObjectInformation(handle, UOI.NAME, buffer, 0, out int needed);
			buffer = Marshal.AllocHGlobal(needed);
			string name = null;
			try {
				if (!GetUserObjectInformation(handle, UOI.NAME, buffer, needed, out needed)) {
					throw new Win32Exception();
				}
				name = Marshal.PtrToStringAuto(buffer, needed / Marshal.SystemDefaultCharSize - 1);
			} finally {
				Marshal.FreeHGlobal(buffer);
			}

			return new Desktop(name) {
				handle = handle
			};
		}

		private IntPtr handle = IntPtr.Zero;
		public string Name { get; private set; } = name;

		private void Open() {
			if (this.handle != IntPtr.Zero) {
				return;
			}

			this.handle = CreateDesktop(this.Name, IntPtr.Zero, IntPtr.Zero, 0, DESKTOP.GENERIC_ALL,
				IntPtr.Zero);
			if (this.handle == IntPtr.Zero) {
				throw new Win32Exception();
			}
		}

		public void Activate() {
			this.Open();

			if (!SwitchDesktop(this.handle)) {
				throw new Win32Exception();
			}
		}

		public void AssignCurrentThread() {
			this.Open();

			if (!SetThreadDesktop(this.handle)) {
				throw new Win32Exception();
			}
		}

		public Process StartProcess(string filename, string commandLine = null,
			string workingDirectory = null) {

			return Process.Create(filename, commandLine, workingDirectory, this);
		}

		#region IDisposable Support
		private bool disposedValue = false;

		protected virtual void Dispose(bool disposing) {
			if (!this.disposedValue) {
				if (this.handle != IntPtr.Zero) {
					CloseDesktop(this.handle);
					this.handle = IntPtr.Zero;
				}

				this.disposedValue = true;
			}
		}

		~Desktop() {
			this.Dispose(false);
		}

		public void Dispose() {
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}
