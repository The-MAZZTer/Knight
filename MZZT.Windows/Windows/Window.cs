using MZZT.Diagnostics;
using MZZT.WinApi.PInvoke;
using System.ComponentModel;
using System.Runtime.InteropServices;
using static MZZT.WinApi.PInvoke.DwmApi;
using static MZZT.WinApi.PInvoke.User32;
using static MZZT.WinApi.PInvoke.UxTheme;
using static MZZT.WinApi.PInvoke.Windef;

namespace MZZT.Windows {
	public class Window : IWin32Window, IEquatable<IWin32Window> {
		public static Window ActiveWindow {
			get {
				IntPtr handle = GetForegroundWindow();
				return handle == IntPtr.Zero ? null : new Window(handle);
			}
			set => SetForegroundWindow(value.Handle);
		}

		public static Window FindWindow(string className = null, string title = null) {
			IntPtr handle = User32.FindWindow(className, title);
			if (handle == IntPtr.Zero) {
				return null;
			}
			return new Window(handle);
		}

		public static void ForEachRootWindow(Func<Window, bool> callback) =>
			EnumDesktopWindows(IntPtr.Zero, (hWnd, lParam) => callback(new Window(hWnd)), IntPtr.Zero);

		public static Window[] RootWindows {
			get {
				List<Window> ret = [];
				ForEachRootWindow(window => {
					ret.Add(window);
					return true;
				});
				return [.. ret];
			}
		}

		public static Window Taskbar => FindWindow("Shell_TrayWnd");

		public Window(IntPtr hWnd) => this.Handle = hWnd;
		public Window(IWin32Window form) => this.Handle = form.Handle;

		public IntPtr Handle {
			get; private set;
		}

		public WindowThumbnail CreateThumbnail() => new(this);

		public void Activate() => ActiveWindow = this;

		public void Close() => PostMessage(this.Handle, WM.CLOSE, IntPtr.Zero, IntPtr.Zero);

		public bool Equals(IWin32Window other) {
			if (other == null) {
				return false;
			}

			return this.Handle == other.Handle;
		}

		public override bool Equals(object obj) => this.Equals(obj as IWin32Window);

		public override int GetHashCode() => (int)this.Handle;

		public Padding GetWindowBorderSize(bool includeMenu) {
			includeMenu = includeMenu && !GetMenu(this.Handle).Equals(IntPtr.Zero);

			RECT rect = new() {
				top = 500,
				left = 500,
				right = 1000,
				bottom = 1000
			};

			if (!AdjustWindowRectEx(ref rect, this.Style, includeMenu, this.StyleEx)) {
				throw new Win32Exception();
			}

			return new Padding(500 - rect.left, 500 - rect.top, rect.right - 1000, rect.bottom - 1000);
		}

		public void MakeBackgroundSheetOfGlass() => this.GlassMargin = new Padding(-1);

		public void Maximize() => ShowWindow(this.Handle, SW.MAXIMIZE);

		public void Minimize() => ShowWindow(this.Handle, SW.MINIMIZE);

		public void MoveInFrontOf(IWin32Window window) =>
			SetWindowPos(window.Handle, this.Handle, 0, 0, 0, 0, SWP.ASYNCWINDOWPOS |
				SWP.DEFERERASE | SWP.NOACTIVATE | SWP.NOCOPYBITS | SWP.NOMOVE |
				SWP.NOOWNERZORDER | SWP.NOREDRAW | SWP.NOREPOSITION | SWP.NOSIZE);

		public void OpenSystemMenu(Point location) =>
			PostMessage(this.Handle, WM.GETSYSMENU, IntPtr.Zero,
				new IntPtr((location.X & 0xFFFF) | (location.Y << 16)));

		public Point PointToScreen(Point point) {
			POINT p = new(point);
			if (!ClientToScreen(this.Handle, ref p)) {
				throw new Win32Exception();
			}
			return p.ToPoint();
		}

		public void Refresh() {
			this.gotLargeIcon = false;
			this.gotLargeIconBitmap = false;
			this.gotOwner = false;
			this.gotParent = false;
			this.gotProcess = false;
			this.gotRect = false;
			this.gotRoot = false;
			this.gotSmallIcon = false;
			this.gotSmallIconBitmap = false;
			this.gotStyle = false;
			this.gotStyleEx = false;
			this.gotTitle = false;
		}

		public void Restore() => ShowWindow(this.Handle, SW.RESTORE);

		public bool AllowActivation => (this.StyleEx & WS_EX.NOACTIVATE) == 0;

		public Rectangle Bounds {
			get => this.Rect.ToRectangle();
			set => this.Rect = new RECT(value);
		}

		public DisplayStates Display {
			get {
				WINDOWPLACEMENT placement = new();
				placement.InitLength();
				GetWindowPlacement(this.Handle, out placement);
				return placement.showCmd switch {
					SW.HIDE => DisplayStates.Hidden,
					SW.FORCEMINIMIZE or SW.MINIMIZE or SW.SHOWMINIMIZED or SW.SHOWMINNOACTIVE => DisplayStates.Minimized,
					SW.MAXIMIZE => DisplayStates.Maximized,
					_ => DisplayStates.Restored,
				};
			}
			set {
				SW state = SW.SHOWNA;
				switch (value) {
					case DisplayStates.Hidden:
						state = SW.HIDE;
						break;
					case DisplayStates.Minimized:
						state = SW.FORCEMINIMIZE;
						break;
					case DisplayStates.Restored:
						state = SW.RESTORE;
						break;
					case DisplayStates.Maximized:
						state = SW.MAXIMIZE;
						break;
				}
				ShowWindow(this.Handle, state);
			}
		}

		public DockStyle Dock {
			get {
				Screen screen = this.Screen;
				Rectangle bounds = this.Bounds;

				bool dockTop = bounds.Top - screen.Bounds.Top <= 0 &&
					bounds.Top - screen.Bounds.Top >= -5;
				bool dockLeft = bounds.Left - screen.Bounds.Left <= 0 &&
					bounds.Left - screen.Bounds.Left >= -5;
				bool dockRight = screen.Bounds.Right - bounds.Right <= 0 &&
					screen.Bounds.Right - bounds.Right >= 5;
				bool dockBottom = screen.Bounds.Bottom - bounds.Bottom <= 0 &&
					screen.Bounds.Bottom - bounds.Bottom >= 5;

				if (dockTop && dockLeft && dockRight && dockBottom) {
					return DockStyle.Fill;
				}

				if (dockTop && dockLeft && dockRight) {
					return DockStyle.Top;
				}

				if (dockLeft && dockTop && dockBottom) {
					return DockStyle.Left;
				}

				if (dockRight && dockTop && dockBottom) {
					return DockStyle.Right;
				}

				if (dockBottom && dockLeft && dockRight) {
					return DockStyle.Bottom;
				}

				return DockStyle.None;
			}
		}

		public bool ForceShowInTaskbar => (this.StyleEx & WS_EX.APPWINDOW) != 0;

		private Padding glassMargin = Padding.Empty;
		public Padding GlassMargin {
			get => this.glassMargin;
			set {
				if (this.glassMargin.Equals(value)) {
					return;
				}
				MARGINS margins = new() { cyTopHeight = value.Top, cxRightWidth = value.Right, cyBottomHeight = value.Bottom, cxLeftWidth = value.Left };
				int result = DwmExtendFrameIntoClientArea(this.Handle, ref margins);
				if (result > 0) {
					throw new Win32Exception(result);
				}
				this.glassMargin = value;
			}
		}
		public bool HasRedirectionBitmap => (this.StyleEx & WS_EX.NOREDIRECTIONBITMAP) == 0;
		public bool IsResizble => (this.Style & WS.SIZEBOX) != 0;
		public bool IsRootWindow => this.Root?.Equals(this) ?? false;
		public bool IsToolWindow => (this.StyleEx & WS_EX.TOOLWINDOW) != 0;

		private Icon largeIcon = null;
		private bool gotLargeIcon = false;
		public Icon LargeIcon {
			get {
				if (!this.gotLargeIcon) {
					this.gotLargeIcon = true;
					this.largeIcon = null;

					IntPtr icon = IntPtr.Zero;
					try {
						icon = Helpers.SendMessageAutoTimeout(this.Handle, WM.GETICON,
							new IntPtr((int)ICON.BIG), IntPtr.Zero);
					} catch (TimeoutException) {
					} catch (Win32Exception) {
					}
					if (icon == IntPtr.Zero) {
						icon = new IntPtr(GetClassLong(this.Handle, GCL.HICON));
					}
					if (icon != IntPtr.Zero) {
						this.largeIcon = Icon.FromHandle(icon);
					};
					if (this.largeIcon == null || this.largeIcon.Width == 0 ||
						this.largeIcon.Height == 0) {

						this.largeIcon = this.SmallIcon;
					}
					if (this.largeIcon == null || this.largeIcon.Width == 0 ||
						this.largeIcon.Height == 0) {

						try {
							this.largeIcon = new Icon(this.Process.ImageName, new Size(32, 32));
						} catch (ArgumentException) {

						}
					}
					if (this.largeIcon != null && (this.largeIcon.Width == 0 ||
						this.largeIcon.Height == 0)) {

						this.largeIcon = null;
					}
				}
				return this.largeIcon;
			}
		}

		private Bitmap largeIconBitmap = null;
		private bool gotLargeIconBitmap = false;
		public Bitmap LargeIconBitmap {
			get {
				if (!this.gotLargeIconBitmap) {
					this.gotLargeIconBitmap = true;
					this.largeIconBitmap = this.LargeIcon?.ToBitmap();
				}
				return this.largeIconBitmap;
			}
		}

		public Point Location {
			get => new(this.Rect.left, this.Rect.top);
			set {
				if (this.gotRect) {
					this.rect = new RECT() {
						left = value.X,
						top = value.Y,
						right = this.rect.right - this.rect.left + value.X,
						bottom = this.rect.bottom - this.rect.top + value.X
					};
				}
				SetWindowPos(this.Handle, IntPtr.Zero, value.X, value.Y, 0, 0,
					SWP.ASYNCWINDOWPOS | SWP.NOACTIVATE | SWP.NOOWNERZORDER | SWP.NOZORDER | SWP.NOSIZE | SWP.DEFERERASE | SWP.NOREDRAW);
			}
		}

		public Window NearestTaskbarVisibleAncestor => this.VisibleInTaskbar ? this : this.Owner?.NearestTaskbarVisibleAncestor;

		private Window owner = null;
		private bool gotOwner = false;
		public Window Owner {
			get {
				if (!this.gotOwner) {
					this.gotOwner = true;

					IntPtr handle = GetWindow(this.Handle, GW.OWNER);
					this.owner = handle == IntPtr.Zero ? null : new Window(handle);
				}
				return this.owner;
			}
		}

		private Window parent = null;
		private bool gotParent = false;
		public Window Parent {
			get {
				if (!this.gotParent) {
					this.gotParent = true;

					IntPtr handle = GetAncestor(this.Handle, GA.PARENT);
					this.parent = handle == IntPtr.Zero ? null : new Window(handle);
				}
				return this.parent;
			}
		}

		private Process process = null;
		private int thread = 0;
		private bool gotProcess = false;
		public Process Process {
			get {
				if (!this.gotProcess) {
					this.gotProcess = true;

					this.thread = GetWindowThreadProcessId(this.Handle, out int id);
					this.process = this.thread > 0 ? new Process(id) : null;
				}
				return this.process;
			}
		}

		private RECT rect;
		private bool gotRect = false;
		private RECT Rect {
			get {
				if (!this.gotRect) {
					this.gotRect = true;

					this.rect = new RECT();
					GetWindowRect(this.Handle, out this.rect);
				}
				return this.rect;
			}
			set {
				SetWindowPos(this.Handle, IntPtr.Zero, value.left, value.top, value.right - value.left, value.bottom - value.top,
					SWP.ASYNCWINDOWPOS | SWP.NOACTIVATE | SWP.NOOWNERZORDER | SWP.NOZORDER);
				this.rect = value;
				this.gotRect = true;
			}
		}

		private Window root;
		private bool gotRoot = false;
		public Window Root {
			get {
				if (!this.gotRoot) {
					this.gotRoot = true;

					IntPtr handle = GetAncestor(this.Handle, GA.ROOT);
					this.root = handle == IntPtr.Zero ? null : new Window(handle);
				}
				return this.root;
			}
		}
		
		public Screen Screen => Screen.FromHandle(this.Handle);

		public Size Size => new(this.Rect.right - this.Rect.left, this.Rect.bottom - this.Rect.top);

		private Icon smallIcon = null;
		private bool gotSmallIcon = false;
		public Icon SmallIcon {
			get {
				if (!this.gotSmallIcon) {
					this.gotSmallIcon = true;
					this.smallIcon = null;

					IntPtr icon = IntPtr.Zero;
					try {
						icon = Helpers.SendMessageAutoTimeout(this.Handle, WM.GETICON,
							new IntPtr((int)ICON.SMALL2), IntPtr.Zero);
					} catch (TimeoutException) {
					} catch (Win32Exception) {
					}
					if (icon == IntPtr.Zero) {
						try {
							icon = Helpers.SendMessageAutoTimeout(this.Handle, WM.GETICON,
								new IntPtr((int)ICON.SMALL), IntPtr.Zero);
						} catch (TimeoutException) {
						} catch (Win32Exception) {
						}
					}
					if (icon == IntPtr.Zero) {
						icon = new IntPtr(GetClassLong(this.Handle, GCL.HICONSM));
					}
					if (icon != IntPtr.Zero) {
						this.smallIcon = Icon.FromHandle(icon);
					};
					if (this.smallIcon == null || this.smallIcon.Width == 0 ||
						this.smallIcon.Height == 0) {

						this.smallIcon = this.LargeIcon;
					}
					if (this.smallIcon == null || this.smallIcon.Width == 0 ||
						this.smallIcon.Height == 0) {

						try {
							this.smallIcon = new Icon(this.Process.ImageName, new Size(16, 16));
						} catch (ArgumentException) {

						}
					}
					if (this.smallIcon != null && (this.smallIcon.Width == 0 ||
						this.smallIcon.Height == 0)) {

						this.smallIcon = null;
					}
				}
				return this.smallIcon;
			}
		}

		private Bitmap smallIconBitmap = null;
		private bool gotSmallIconBitmap = false;
		public Bitmap SmallIconBitmap {
			get {
				if (!this.gotSmallIconBitmap) {
					this.gotSmallIconBitmap = true;
					this.smallIconBitmap = this.SmallIcon?.ToBitmap();
				}
				return this.smallIconBitmap;
			}
		}

		private WS style;
		private bool gotStyle = false;
		private WS Style {
			get {
				if (!this.gotStyle) {
					this.gotStyle = true;

					this.style = (WS)GetWindowLong(this.Handle, GWL.STYLE);
				}
				return this.style;
			}
		}

		private WS_EX styleEx;
		private bool gotStyleEx = false;
		private WS_EX StyleEx {
			get {
				if (!this.gotStyleEx) {
					this.gotStyleEx = true;

					this.styleEx = (WS_EX)GetWindowLong(this.Handle, GWL.EXSTYLE);
				}
				return this.styleEx;
			}
		}

		public int Thread {
			get {
				if (!this.gotProcess) {
					this.gotProcess = true;

					this.thread = GetWindowThreadProcessId(this.Handle, out int id);
					this.process = this.thread > 0 ? new Process(id) : null;
				}
				return this.thread;
			}
		}

		private string title = null;
		private bool gotTitle = false;
		public string Title {
			get {
				if (!this.gotTitle) {
					this.gotTitle = true;
					this.title = null;

					try {
						int len = Helpers.SendMessageAutoTimeout(this.Handle, WM.GETTEXTLENGTH,
							IntPtr.Zero, IntPtr.Zero).ToInt32();
						if (len == 0) {
							this.title = "";
						} else {
							IntPtr buffer = Marshal.AllocHGlobal(Marshal.SystemDefaultCharSize * (len + 1));
							try {
								int copied = Helpers.SendMessageAutoTimeout(this.Handle, WM.GETTEXT,
									new IntPtr(len + 1), buffer).ToInt32();

								this.title = Marshal.PtrToStringAuto(buffer, copied);
							} finally {
								Marshal.FreeHGlobal(buffer);
							}
						}
					} catch (TimeoutException) {
					} catch (Win32Exception) {
					}
				}
				return this.title;
			}
			set {
				IntPtr buffer = Marshal.StringToHGlobalAuto(value);
				try {
					PostMessage(this.Handle, WM.SETTEXT, IntPtr.Zero, buffer);

					this.gotTitle = true;
					this.title = value;
				} finally {
					Marshal.FreeHGlobal(buffer);
				}
			}
		}

		public bool Visible => (this.Style & WS.VISIBLE) != 0;

		public bool VisibleInTaskbar =>
			this.IsRootWindow && this.Visible && (this.ForceShowInTaskbar ||
			(!this.IsToolWindow && this.AllowActivation && this.Owner == null)) &&
			this.HasRedirectionBitmap;
	}

	public enum DisplayStates {
		Hidden,
		Minimized,
		Restored,
		Maximized
	}
}
