using System.Reflection;

namespace MZZT.Updates {
	public partial class UpdateForm : Form {
		public UpdateForm(Uri updateUri, Assembly assembly = null) {
			this.InitializeComponent();

			if (assembly == null) {
				assembly = Assembly.GetEntryAssembly();
			}

			this.updater = new Updater(updateUri, assembly);
			this.updater.Progress += this.Updater_Progress;

			AssemblyName name = assembly.GetName();
			this.assemblyName = name.Name;
			this.Text = string.Format(this.Text, name.Name);

			_ = this.CheckForUpdate();
		}

		private readonly string assemblyName;
		private readonly Updater updater;
		private void Updater_Progress(object sender, UpdaterProgressEventArgs e) {
			if (!this.Created) {
				return;
			}

			this.Invoke(new Action(() => {
				this.progress.Value = (int)((double)e.Position / e.Length * 10000);
			}));
		}

		public async Task CheckForUpdate() {
			this.progress.Value = 0;
			this.status.Text = "Checking for update...";
			this.go.Text = "&Cancel";

			try {
				await this.updater.ReadAsync();
			} catch (OperationCanceledException) {
				//this.Invoke(new Action(() => {
					this.progress.Value = 0;
					this.status.Text = "Cancelled";
					this.go.Text = "&Check";
				//}));
				return;
			} catch (Exception e) {
				//this.Invoke(new Action(() => {
					this.progress.Value = 0;
					this.status.Text = e.Message;
					this.go.Text = "&Check";
				//}));
				return;
			}

			//this.Invoke(new Action(() => {
				this.progress.Value = 0;
				if (this.updater.IsNewer) {
					this.status.Text = $"Version {this.updater.LatestVersion} is available.";
					this.go.Text = "&Download";
				} else {
					this.status.Text = $"{this.assemblyName} is up-to-date.";
					this.go.Text = "&Check";
				}
			//}));
		}

		public async Task Download() {
			this.progress.Value = 0;
			this.status.Text = "Downloading...";
			this.go.Text = "&Cancel";

			try {
				await this.updater.DownloadUpdateAsync(false);
			} catch (OperationCanceledException) {
				//this.Invoke(new Action(() => {
					this.progress.Value = 0;
					this.status.Text = "Cancelled";
					this.go.Text = "&Download";
				//}));
				return;
			} catch (Exception e) {
				//this.Invoke(new Action(() => {
					this.progress.Value = 0;
					this.status.Text = e.Message;
					this.go.Text = "&Download";
				//}));
				return;
			}

			//this.Invoke(new Action(() => {
				this.progress.Value = 0;
				this.status.Text = "Update downloaaded. Click Update to install and restart.";
				this.go.Text = "&Update";
			//}));
		}

		private async void Go_Click(object sender, EventArgs e) {
			switch (this.go.Text) {
				case "&Cancel":
					this.updater.Abort();
					break;
				case "&Check":
					await this.CheckForUpdate();
					break;
				case "&Download":
					await this.Download();
					break;
				case "&Update":
					this.updater.ApplyUpdate(true);
					break;
			}
		}

		protected override void OnFormClosing(FormClosingEventArgs e) {
			base.OnFormClosing(e);

			if (e.CloseReason == CloseReason.UserClosing) {
				e.Cancel = true;

				this.Visible = false;
			}
		}
	}
}
