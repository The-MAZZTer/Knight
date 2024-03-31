using MZZT.Diagnostics;
using static MZZT.WinApi.PInvoke.User32;

namespace MZZT.Windows {
	public class WindowMonitor : IDisposable {
		public WindowMonitor() {
			// Because a new WinEventProc object is created from the .NET function,
			// we need to keep a reference to it around so it won't get garbage collected.
			this.callback = this.WinEventCallback;
		}

		public void SetDesiredEvents(IEnumerable<EVENT> events) {
			EVENT minEvent = events.Min();
			EVENT maxEvent = events.Max();
			if (minEvent == this.minEvent && maxEvent == this.maxEvent) {
				return;
			}

			this.minEvent = minEvent;
			this.maxEvent = maxEvent;

			if (this.IsMonitoring) {
				this.Stop();
				this.Start();
			}
		}
		private EVENT minEvent = EVENT.MIN;
		private EVENT maxEvent = EVENT.MAX;

		public List<Window> Windows {
			get; private set;
		}
		public Window ActiveWindow {
			get; private set;
		}

		public bool OnlyTaskbarWindows {
			get; set;
		} = true;

		public event EventHandler<WindowEventArgs> WindowAdded;
		private void OnWindowAdded(Window window) {
			if (window == null || this.Windows.Contains(window) || (this.OnlyTaskbarWindows && !window.VisibleInTaskbar)) {
				return;
			}

			this.Windows.Add(window);
			this.WindowAdded?.Invoke(this, new WindowEventArgs(window));
		}

		public event EventHandler<WindowEventArgs> WindowRemoved;
		private void OnWindowRemoved(Window window) {
			if (window == null || !this.Windows.Contains(window)) {
				return;
			}
			window = this.Windows.First(w => w.Equals(window));

			WindowEventArgs ea = new(window);
			this.Windows.Remove(window);
			this.WindowRemoved?.Invoke(this, ea);
		}

		public event EventHandler<WindowFocusEventArgs> ActiveWindowChanged;
		private void CheckFocusChange(Window newFocus = null) {
			Window oldFocus = this.ActiveWindow;
			if (this.SyncActiveWindow(newFocus, true)) {
				this.ActiveWindowChanged?.Invoke(this, new WindowFocusEventArgs(oldFocus, this.ActiveWindow));
			}
		}

		private bool SyncActiveWindow(Window window = null, bool add = false) {
			window ??= Window.ActiveWindow;
			if (this.OnlyTaskbarWindows) {
				window = window?.NearestTaskbarVisibleAncestor;
			}
			if (add) {
				this.OnWindowAdded(window);
			}
			if (this.ActiveWindow?.Equals(window) ?? (window == null)) {
				return false;
			}
			if (window != null) {
				window = this.Windows.FirstOrDefault(w => w.Equals(window)) ?? window;
			}
			this.ActiveWindow = window;
			return true;
		}

		public event EventHandler<WindowEventArgs> WindowRenamed;
		private void OnWindowRenamed(Window window) {
			/*string oldTitle = this.Windows.First(w => w.Equals(window)).Title;
			if (window.Title == oldTitle) {
				return;
			}*/

			this.Windows.Remove(window);
			this.Windows.Add(window);
			this.WindowRenamed?.Invoke(this, new WindowEventArgs(window));
		}

		public event EventHandler<WindowEventArgs> WindowBoundsChanged;
		private void OnWindowBoundsChanged(Window window) {
			this.WindowBoundsChanged?.Invoke(this, new WindowEventArgs(window));
		}

		public event EventHandler<WindowEventArgs> WindowCaptureStart;
		private void OnWindowCaptureStart(Window window) {
			this.WindowCaptureStart?.Invoke(this, new WindowEventArgs(window));
		}

		public event EventHandler<WindowEventArgs> WindowCaptureEnd;
		private void OnWindowCaptureEnd(Window window) {
			this.WindowCaptureEnd?.Invoke(this, new WindowEventArgs(window));
		}

		public event EventHandler<WindowEventArgs> WindowMinimized;
		private void OnWindowMinimized(Window window) {
			this.WindowMinimized?.Invoke(this, new WindowEventArgs(window));
		}

		public event EventHandler<WindowEventArgs> WindowUnminimized;
		private void OnWindowUnminimized(Window window) {
			this.WindowUnminimized?.Invoke(this, new WindowEventArgs(window));
		}

		private WinEventProc callback;
		private void WinEventCallback(IntPtr hWinEventHook, EVENT @event, IntPtr hwnd,
			OBJID idObject, int idChild, int dwEventThread, int dwmsEventTime) {

			if (idObject != OBJID.WINDOW || idChild != CHILDID_SELF) {
				return;
			}

			//System.Diagnostics.Debug.WriteLine($"EVENT_{@event}");
			Window window = new(hwnd);
			switch (@event) {
				case EVENT.OBJECT_CREATE:
				case EVENT.OBJECT_SHOW:
					if (this.Windows.Contains(window)) {
						return;
					}
					this.OnWindowAdded(window);
					this.CheckFocusChange(null);
					break;
				case EVENT.OBJECT_DESTROY:
				case EVENT.OBJECT_HIDE:
					if (!this.Windows.Contains(window)) {
						break;
					}
					this.OnWindowRemoved(window);
					this.CheckFocusChange(null);
					break;
				case EVENT.OBJECT_PARENTCHANGE:
					bool oldInTaskbar;
					oldInTaskbar = this.Windows.Contains(window);
					if (!this.OnlyTaskbarWindows || oldInTaskbar != window.VisibleInTaskbar) {
						if (oldInTaskbar) {
							this.OnWindowRemoved(window);
						} else {
							this.OnWindowAdded(window);
						}
						this.CheckFocusChange(null);
					}
					break;
				case EVENT.OBJECT_NAMECHANGE:
					if (!this.Windows.Contains(window)) {
						break;
					}
					this.OnWindowRenamed(window);
					break;
				case EVENT.SYSTEM_FOREGROUND:
					this.CheckFocusChange(window);
					break;
				case EVENT.OBJECT_LOCATIONCHANGE:
					this.OnWindowBoundsChanged(window);
					this.CheckFocusChange(null);
					break;
				case EVENT.SYSTEM_CAPTURESTART:
					this.OnWindowCaptureStart(window);
					break;
				case EVENT.SYSTEM_CAPTUREEND:
					this.OnWindowCaptureEnd(window);
					break;
				case EVENT.SYSTEM_MINIMIZESTART:
					this.OnWindowMinimized(window);
					break;
				case EVENT.SYSTEM_MINIMIZEEND:
					this.OnWindowUnminimized(window);
					break;
				default:
					return;
			}
		}

		private IntPtr handle;
		public bool IsMonitoring {
			get {
				return this.handle != IntPtr.Zero;
			}
		}

		public void Start(bool skipOwnProcess = true) {
			if (this.IsMonitoring) {
				this.Stop();
			}

			this.handle = SetWinEventHook(this.minEvent, this.maxEvent, IntPtr.Zero, this.callback, 0, 0,
				WINEVENT.OUTOFCONTEXT | (skipOwnProcess ? WINEVENT.SKIPOWNPROCESS : 0));

			this.Windows = Window.RootWindows.Where(w => !this.OnlyTaskbarWindows || w.VisibleInTaskbar).ToList();
			this.SyncActiveWindow();
		}

		public void Start(Process process) {
			if (this.IsMonitoring) {
				this.Stop();
			}

			this.handle = SetWinEventHook(this.minEvent, this.maxEvent, IntPtr.Zero, this.callback, process.Id, 0,
				WINEVENT.OUTOFCONTEXT);

			this.Windows = Window.RootWindows.Where(w => w.Process == process && (!this.OnlyTaskbarWindows || w.VisibleInTaskbar)).ToList();
			this.SyncActiveWindow();
		}

		public void Start(int thread) {
			if (this.IsMonitoring) {
				this.Stop();
			}

			this.handle = SetWinEventHook(this.minEvent, this.maxEvent, IntPtr.Zero, this.callback, 0, thread,
				WINEVENT.OUTOFCONTEXT);

			this.Windows = Window.RootWindows.Where(w => w.Thread == thread && (!this.OnlyTaskbarWindows || w.VisibleInTaskbar)).ToList();
			this.SyncActiveWindow();
		}

		public void Stop() {
			if (!this.IsMonitoring) {
				return;
			}

			UnhookWinEvent(this.handle);
			this.handle = IntPtr.Zero;
			this.Windows = null;
			this.ActiveWindow = null;
		}

		#region IDisposable Support
		private bool disposedValue = false;

		protected virtual void Dispose(bool disposing) {
			if (!this.disposedValue) {
				this.Stop();
				if (disposing) {
					this.callback = null;
				}

				this.disposedValue = true;
			}
		}

		~WindowMonitor() {
			this.Dispose(false);
		}

		public void Dispose() {
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion
	}

	public class WindowEventArgs(Window window) : EventArgs() {
		public Window Window {
			get; private set;
		} = window;
	}

	public class WindowFocusEventArgs(Window oldFocus, Window newFocus) : EventArgs() {
		public Window NewFocus {
			get; private set;
		} = newFocus;

		public Window OldFocus {
			get; private set;
		} = oldFocus;
	}
}
