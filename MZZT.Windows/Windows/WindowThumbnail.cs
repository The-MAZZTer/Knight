using System.ComponentModel;
using static MZZT.WinApi.PInvoke.DwmApi;
using static MZZT.WinApi.PInvoke.User32;
using static MZZT.WinApi.PInvoke.Windef;

namespace MZZT.Windows {
	public class WindowThumbnail : IDisposable {
		public WindowThumbnail(IWin32Window source) {
			this.callback = this.WinEventCallback;
			this.source = source;
			this.Visible = true;
		}

		public void Attach(Control destination) {
			if (this.handle != IntPtr.Zero) {
				throw new InvalidOperationException();
			}

			this.destination = destination;

			int err = DwmRegisterThumbnail(destination.FindForm().Handle, this.source.Handle,
				out this.handle);
			if (err != 0) {
				throw new Win32Exception(err);
			}

			this.hook = SetWinEventHook(EVENT.OBJECT_DESTROY, EVENT.OBJECT_LOCATIONCHANGE, IntPtr.Zero,
				this.callback, new Window(this.source).Process.Id, 0,
				WINEVENT.OUTOFCONTEXT | WINEVENT.SKIPOWNPROCESS);
			destination.Resize += this.Destination_Resize;

			while (destination != null && destination is not Form) {
				destination.LocationChanged += this.Destination_Resize;
				destination = destination.Parent;
			}

			this.Update(DWM_TNP.RECTDESTINATION);
		}
		private IntPtr handle;
		private IntPtr hook;
		private readonly IWin32Window source;
		private Control destination;

		private readonly WinEventProc callback;
		private void WinEventCallback(IntPtr hWinEventHook, EVENT @event, IntPtr hwnd,
			OBJID idObject, int idChild, int dwEventThread, int dwmsEventTime) {

			if (idObject != OBJID.WINDOW || idChild != CHILDID_SELF || hwnd != this.source.Handle) {
				return;
			}

			switch (@event) {
				case EVENT.OBJECT_DESTROY:
				case EVENT.OBJECT_HIDE:
					this.Dispose();
					break;
				case EVENT.OBJECT_LOCATIONCHANGE:
					this.Update(DWM_TNP.RECTDESTINATION);
					break;
			}
		}

		private void Destination_Resize(object sender, EventArgs e) {
			this.Update(DWM_TNP.RECTDESTINATION);
		}
		private RectangleF ScaleToContainInside(Size source, Size container) {
			float sourceAspect = source.Width / (float)source.Height;
			float destAspect = container.Width / (float)container.Height;
			SizeF finalSize = container;
			if (destAspect > sourceAspect) {
				finalSize.Width = (float)container.Height / source.Height * source.Width;
			} else if (sourceAspect > destAspect) {
				finalSize.Height = (float)container.Width / source.Width * source.Height;
			}

			return new RectangleF(new PointF((container.Width - finalSize.Width) / 2,
				(container.Height - finalSize.Height) / 2), finalSize);
		}

		private DWM_TNP pendingUpdates = 0;
		private int deferUpdate = 0;
		private RECT lastThumbnailBounds;
		private void Update(DWM_TNP updates = 0) {
			if (this.deferUpdate > 0 || this.handle == IntPtr.Zero) {
				this.pendingUpdates |= updates;
				return;
			}
			updates |= this.pendingUpdates;
			this.pendingUpdates = 0;

			if (updates == 0) {
				return;
			}

			DWM_THUMBNAIL_PROPERTIES props = new();

			int err;
			foreach (DWM_TNP flag in Enum.GetValues(typeof(DWM_TNP))) {
				if ((updates & flag) == 0) {
					continue;
				}

				switch (flag) {
					case DWM_TNP.RECTDESTINATION:
						SIZE windowSize = default;
						err = DwmQueryThumbnailSourceSize(this.handle, out windowSize);
						if (err != 0) {
							throw new Win32Exception(err);
						}

						if (this.lastWindowSize.cx != windowSize.cx ||
							this.lastWindowSize.cy != windowSize.cy) {

							this.lastWindowSize = windowSize;

							this.SourceSizeChanged?.Invoke(this, new EventArgs());
						}

						double width = this.destination.Width;
						double height = this.destination.Height;
						if (!this.AlwaysStretchToFit) {
							width = Math.Min(windowSize.cx, width);
							height = Math.Min(windowSize.cy, height);
						}
						if (this.PreserveAspectRatio) {
							SizeF newSize = this.ScaleToContainInside(windowSize.ToSize(),
								new Size((int)width, (int)height)).Size;
							width = newSize.Width;
							height = newSize.Height;
						}

						var left = this.Alignment switch {
							ContentAlignment.TopLeft or ContentAlignment.MiddleLeft or ContentAlignment.BottomLeft => 0,
							ContentAlignment.TopRight or ContentAlignment.MiddleRight or ContentAlignment.BottomRight => this.destination.Width - width,
							_ => (this.destination.Width - width) / 2,
						};
						var top = this.Alignment switch {
							ContentAlignment.TopLeft or ContentAlignment.TopCenter or ContentAlignment.TopRight => 0,
							ContentAlignment.BottomLeft or ContentAlignment.BottomCenter or ContentAlignment.BottomRight => this.destination.Height - height,
							_ => (this.destination.Height - height) / 2,
						};
						Point formPoint = new((int)Math.Round(left), (int)Math.Round(top));
						Control current = this.destination;
						while (current is not Form) {
							formPoint = new Point(formPoint.X + current.Left, formPoint.Y + current.Top);
							current = current.Parent;
						}

						RECT newRect = new() {
							left = formPoint.X,
							top = formPoint.Y,
							right = formPoint.X + (int)Math.Round(width),
							bottom = formPoint.Y + (int)Math.Round(height)
						};

						if (newRect.left == this.lastThumbnailBounds.left &&
							newRect.top == this.lastThumbnailBounds.top &&
							newRect.right == this.lastThumbnailBounds.right &&
							newRect.bottom == this.lastThumbnailBounds.bottom) {

							break;
						}
						this.lastThumbnailBounds = newRect;

						props.dwFlags |= DWM_TNP.RECTDESTINATION;
						props.rcDestination = newRect;
						break;
					case DWM_TNP.OPACITY:
						props.dwFlags |= DWM_TNP.OPACITY;
						props.opacity = this.Opacity;
						break;
					case DWM_TNP.VISIBLE:
						props.dwFlags |= DWM_TNP.VISIBLE;
						props.fVisible = this.Visible;
						break;
					case DWM_TNP.SOURCECLIENTAREAONLY:
						props.dwFlags |= DWM_TNP.SOURCECLIENTAREAONLY;
						props.fSourceClientAreaOnly = this.SourceClientAreaOnly;
						break;
				}
			}

			if (props.dwFlags == 0) {
				return;
			}

			err = DwmUpdateThumbnailProperties(this.handle, ref props);
			if (err != 0) {
				throw new Win32Exception(err);
			}
		}

		public void BeginUpdate() {
			this.deferUpdate++;
		}

		public void EndUpdate() {
			if (this.deferUpdate <= 0) {
				throw new InvalidOperationException();
			}
			this.deferUpdate--;
			if (this.deferUpdate == 0 && this.pendingUpdates > 0) {
				this.Update();
			}
		}

		public void InvalidateSize() {
			this.Update(DWM_TNP.RECTDESTINATION);
		}

		private ContentAlignment alignment = ContentAlignment.MiddleCenter;
		public ContentAlignment Alignment {
			get {
				return this.alignment;
			}
			set {
				if (this.alignment == value) {
					return;
				}
				this.alignment = value;

				this.Update(DWM_TNP.RECTDESTINATION);
			}
		}

		private bool alwaysStretchToFit = false;
		public bool AlwaysStretchToFit {
			get {
				return this.alwaysStretchToFit;
			}
			set {
				if (this.alwaysStretchToFit == value) {
					return;
				}
				this.alwaysStretchToFit = value;

				this.Update(DWM_TNP.RECTDESTINATION);
			}
		}

		private byte opacity = 255;
		public byte Opacity {
			get {
				return this.opacity;
			}
			set {
				if (this.opacity == value) {
					return;
				}
				this.opacity = value;

				this.Update(DWM_TNP.OPACITY);
			}
		}

		private bool preserveAspectRatio = true;
		public bool PreserveAspectRatio {
			get {
				return this.preserveAspectRatio;
			}
			set {
				if (this.preserveAspectRatio == value) {
					return;
				}
				this.preserveAspectRatio = value;

				this.Update(DWM_TNP.RECTDESTINATION);
			}
		}

		private bool sourceClientAreaOnly = false;
		public bool SourceClientAreaOnly {
			get {
				return this.sourceClientAreaOnly;
			}
			set {
				if (this.sourceClientAreaOnly == value) {
					return;
				}
				this.sourceClientAreaOnly = value;

				this.Update(DWM_TNP.SOURCECLIENTAREAONLY);
			}
		}

		public event EventHandler SourceSizeChanged;
		private SIZE lastWindowSize;
		public Size SourceSize => this.lastWindowSize.ToSize();

		private bool visible = false;
		public bool Visible {
			get {
				return this.visible;
			}
			set {
				if (this.visible == value) {
					return;
				}
				this.visible = value;

				this.Update(DWM_TNP.VISIBLE);
			}
		}

		#region IDisposable Support
		private bool disposedValue = false;

		protected virtual void Dispose(bool disposing) {
			if (!this.disposedValue) {
				if (disposing) {
					if (this.hook != 0) {
						UnhookWinEvent(this.hook);
					}

					this.destination.Resize -= this.Destination_Resize;

					Control control = this.destination;
					while (control != null && control is not Form) {
						control.LocationChanged -= this.Destination_Resize;
						control = control.Parent;
					}
				}

				if (this.handle != IntPtr.Zero) {
					DwmUnregisterThumbnail(this.handle);
					this.handle = IntPtr.Zero;
				}

				this.disposedValue = true;
			}
		}

		~WindowThumbnail() {
			this.Dispose(false);
		}

		public void Dispose() {
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}
