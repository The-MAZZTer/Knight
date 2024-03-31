using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MZZT.Updates {
	public class Updater {
		public Updater() { }

		public Updater(Uri uri, Assembly assembly = null) {
			this.updateUri = uri;
			this.assembly = assembly;
		}

		private async Task Download(Uri uri, Stream dest) {
			using HttpClient client = new();
			using HttpResponseMessage response = await client.GetAsync(uri);
			if (this.Aborted) {
				throw new OperationCanceledException();
			}

			using Stream stream = await response.Content.ReadAsStreamAsync();
			if (this.Aborted) {
				throw new OperationCanceledException();
			}

			int bufferSize = 64 * 1024 * 1024;
			byte[] buffer = new byte[bufferSize];
			long total = 0;
			int bytesRead = 1;
			while (bytesRead > 0) {
				bytesRead = await stream.ReadAsync(buffer.AsMemory(0, bufferSize));
				total += bytesRead;

				if (this.Aborted) {
					throw new OperationCanceledException();
				}

				if (bytesRead > 0) {
					await dest.WriteAsync(buffer.AsMemory(0, bytesRead));
				}

				if (this.Aborted) {
					throw new OperationCanceledException();
				}

				this.Progress?.Invoke(this, new UpdaterProgressEventArgs(total, response.Content.Headers.ContentLength.Value));
			}
		}

		public event EventHandler<UpdaterProgressEventArgs> Progress;

		public async Task ReadAsync() {
			this.Aborted = false;

			Updater update;
			using (MemoryStream stream = new()) {
				await this.Download(this.updateUri, stream);

				stream.Position = 0;

				update = await JsonSerializer.DeserializeAsync<Updater>(stream);
			}
			this.DownloadUri = update.DownloadUri;
			this.LatestVersion = update.LatestVersion;

			if (!this.DownloadUri.IsAbsoluteUri) {
				this.DownloadUri = new Uri(this.updateUri, this.DownloadUri);
			}
		}

		public void Read() {
			_ = this.ReadAsync();
		}

		public void Abort() {
			this.Aborted = true;
		}

		private bool abort = false;
		[JsonIgnore]
		public bool Aborted {
			get {
				lock (this) {
					return this.abort;
				}
			}
			private set {
				lock (this) {
					this.abort = value;
				}
			}
		}

		[JsonPropertyName("version")]
		public Version LatestVersion { get; set; }

		[JsonPropertyName("uri")]
		public Uri DownloadUri { get; set; }

		[JsonIgnore]
		private readonly Uri updateUri;

		[JsonIgnore]
		private readonly Assembly assembly;
		
		[JsonIgnore]
		public Version CurrentVersion {
			get {
				return this.assembly.GetName().Version;
			}
		}

		public bool IsNewer {
			get {
				return this.LatestVersion > this.CurrentVersion;
			}
		}

		public void CleanUp() {
			string file = this.DownloadLocation;
			if (File.Exists(file)) {
				try {
					File.Delete(file);
				} catch (IOException) {
				} catch (UnauthorizedAccessException) {
				}
			}
		}

		[JsonIgnore]
		public bool IsDownloaded {
			get {
				return File.Exists(this.DownloadLocation);
			}
		}

		[JsonIgnore]
		public string DownloadLocation {
			get {
				return Path.Combine(Path.GetTempPath(), $"{Assembly.GetEntryAssembly().GetName().Name} Setup.exe");
			}
		}

		public async Task DownloadUpdateAsync(bool allowAutoInstall = true) {
			this.Aborted = false;

			this.CleanUp();

			using (FileStream stream = new(this.DownloadLocation,
				FileMode.Create, FileAccess.Write, FileShare.None)) {

				await this.Download(this.DownloadUri, stream);
			}

			if (allowAutoInstall && !this.pendingRestart) {
				this.pendingRestart = true;
				Application.ApplicationExit += this.Application_ApplicationExit;
			}
		}

		private bool pendingRestart = false;
		private void Application_ApplicationExit(object sender, EventArgs e) {
			this.ApplyUpdate(false);
		}

		public void DownloadUpdate(bool allowAutoInstall = true) {
			_ = this.DownloadUpdateAsync(allowAutoInstall);
		}

		public static bool ExitingForUpdate { get; private set; }

		public static string AppDirectory { get; set; } = Path.GetDirectoryName(Application.ExecutablePath);

		public void ApplyUpdate(bool restart) {
			if (this.pendingRestart) {
				Application.ApplicationExit -= this.Application_ApplicationExit;
			}

			string restartText = restart ? " /restart" : "";
			Process.Start(this.DownloadLocation,
				$"/update=\"{AppDirectory}\"{restartText}");
			ExitingForUpdate = true;
			Application.Exit();
		}
	}

	public class UpdaterProgressEventArgs(long position, long length) : EventArgs() {
		public long Position { get; private set; } = position;
		public long Length { get; private set; } = length;
	}
}
