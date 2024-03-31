using System.ComponentModel;
using System.Runtime.Serialization;
using static MZZT.WinApi.PInvoke.Kernel32;
using static MZZT.WinApi.PInvoke.User32;

namespace MZZT.Input {
	[DataContract]
	public class HotKey : IDisposable {
		public static bool HandleMessage(IWin32Window window, Message message) {
			if ((WM)message.Msg != WM.HOTKEY) {
				return false;
			}

			long hash = MakeUid(window, message.LParam);
			if (!hotkeys.TryGetValue(hash, out HotKey value)) {
				return false;
			}

			value.Invoke();
			return true;
		}
		private static long MakeUid(IWin32Window form, IntPtr lParam) {
			return (((long)(form?.Handle.ToInt32() ?? 0)) << 32) | (long)lParam.ToInt32();
		}
		private static readonly Dictionary<long, HotKey> hotkeys = [];
		
		public HotKey() {
			this.Key = Keys.None;
			this.Modifier = MOD.NONE;
		}

		public HotKey(Keys hotkey, MOD modifier = MOD.NONE) {
			this.Key = hotkey;
			this.Modifier = modifier;
		}

		[IgnoreDataMember]
		private long Uid {
			get {
				return (((long)(this.Form?.Handle.ToInt32() ?? 0)) << 32) | ((long)this.Key << 16) |
					(long)this.Modifier;
			}
		}

		private void Form_Disposed(object sender, EventArgs e) {
			this.Form = null;
			this.Enabled = false;
		}

		[IgnoreDataMember]
		private Form form = null;
		[IgnoreDataMember]
		public Form Form {
			get {
				return this.form;
			}
			set {
				if (this.form == value) {
					return;
				}

				bool enabled = this.Enabled;
				this.Enabled = false;
				if (this.form != null) {
					this.form.Disposed -= this.Form_Disposed;
				}
				this.form = value;
				if (value != null) {
					value.Disposed += this.Form_Disposed;
				}
				this.Enabled = enabled;
			}
		}

		[DataMember]
		private Keys key;
		[IgnoreDataMember]
		public Keys Key {
			get {
				return this.key;
			}
			set {
				if (this.key == value) {
					return;
				}
				bool enabled = this.Enabled;
				this.Enabled = false;
				this.key = value;
				this.Enabled = enabled;
			}
		}

		[DataMember]
		private MOD modifier;
		[IgnoreDataMember]
		public MOD Modifier {
			get {
				return this.modifier;
			}
			set {
				if (this.modifier == value) {
					return;
				}
				bool enabled = this.Enabled;
				this.Enabled = false;
				this.modifier = value;
				this.Enabled = enabled;
			}
		}

		private void Invoke() {
			this.Pressed?.Invoke(this, new EventArgs());
		}
		public event EventHandler Pressed;

		[IgnoreDataMember]
		private short atom;
		[IgnoreDataMember]
		public bool Enabled {
			get {
				return this.atom != 0;
			}
			set {
				if (this.Enabled == value) {
					return;
				}

				if (!value) {
					if (this.Form != null) {
						UnregisterHotKey(this.Form.Handle, this.atom);
					}
					GlobalDeleteAtom(this.atom);
					this.atom = 0;
					hotkeys.Remove(this.Uid);
					return;
				}

				if (this.key == Keys.None) {
					throw new InvalidOperationException();
				}

				string atomName = nameof(MZZT) + "." + nameof(WinApi) + "." + nameof(HotKey) + "." +
					this.Uid.ToString();
				this.atom = GlobalAddAtom(atomName);
				if (this.atom == 0) {
					throw new Win32Exception();
				}

				if (this.Form == null) {
					return;
				}

				try {
					if (!RegisterHotKey(this.Form.Handle, this.atom, this.Modifier, (VK)this.Key)) {
						throw new Win32Exception();
					}
				} catch (Exception) {
					GlobalDeleteAtom(this.atom);
					this.atom = 0;
					throw;
				}

				hotkeys[this.Uid] = this;
			}
		}

		public override string ToString() {
			string ret = this.Key switch {
				Keys.D1 or Keys.D2 or Keys.D3 or Keys.D4 or Keys.D5 or Keys.D6 or Keys.D7 or Keys.D8 or Keys.D9 or Keys.D0 => this.Key.ToString()[1..],
				Keys.None => this.Modifier == MOD.NONE ? this.Key.ToString() : "",
				_ => this.Key.ToString(),
			};
			if ((this.Modifier & MOD.ALT) > 0) {
				ret = "Alt + " + ret;
			}
			if ((this.Modifier & MOD.WIN) > 0) {
				ret = "Win + " + ret;
			}
			if ((this.Modifier & MOD.CONTROL) > 0) {
				ret = "Control + " + ret;
			}
			if ((this.Modifier & MOD.SHIFT) > 0) {
				ret = "Shift + " + ret;
			}
			return ret;
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing) {
			if (!this.disposedValue) {
				if (disposing) {
					if (this.Form != null) {
						this.Form.Disposed -= this.Form_Disposed;
					}
				}

				this.Enabled = false;

				this.disposedValue = true;
			}
		}

		~HotKey() {
			this.Dispose(false);
		}

		public void Dispose() {
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}
