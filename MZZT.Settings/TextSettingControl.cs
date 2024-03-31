using System;
using System.Windows.Forms;

namespace MZZT.Settings {
	[SettingPanel(SettingTypes.Text)]
	public partial class TextSettingControl : SettingUserControl<string> {
		public TextSettingControl(SettingAttribute attribute, string value = null) : base(attribute, value) {
			this.InitializeComponent();

			this.label.Text = $"{attribute.Title}:";
			this.textbox.Text = value ?? "";
		}

		private void Textbox_TextChanged(object sender, EventArgs e) {
			this.Value = (sender as TextBox).Text;
		}
	}
}
