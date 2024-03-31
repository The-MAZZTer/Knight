using System.Windows.Forms;

namespace MZZT.Settings {
	public interface ISettingUserControl {
		object Value { get; }
	}

	public class SettingUserControl<T> : UserControl, ISettingUserControl {
		private SettingUserControl() { }

		public SettingUserControl(SettingAttribute attribute, T value = default) : base() {
			this.Setting = attribute;
			this.Value = value;
		}
		protected SettingAttribute Setting { get; private set; }

		public virtual T Value { get; protected set; }

		object ISettingUserControl.Value => this.Value;
	}
}
