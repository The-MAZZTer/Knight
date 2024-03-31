using System.Collections.Generic;
using System.Management;
using System.Text.RegularExpressions;

namespace MZZT.WinApi.Wmi {
	public class Win32_MountPoint : WmiBase {
		public static IEnumerable<Win32_MountPoint> MountPoints {
			get {
				return GetCache<Win32_MountPoint>();
			}
		}

		public static void Refresh() {
			Refresh<Win32_MountPoint>();
		}

		protected Win32_MountPoint(ManagementObject obj) : base(obj) {
			using (obj) {
				this.Directory = Regex.Unescape(directoryRegex.Match(this.Directory).Groups[1].Value);
				this.Volume = Regex.Unescape(volumeRegex.Match(this.Volume).Groups[1].Value);
			}
		}

		private static readonly Regex directoryRegex = new Regex("Win32_Directory.Name=\"(.*?)\"",
			RegexOptions.Compiled);
		public string Directory {
			get; private set;
		}

		private static readonly Regex volumeRegex = new Regex("Win32_Volume.DeviceID=\"(.*?)\"",
			RegexOptions.Compiled);
		public string Volume {
			get; private set;
		}
	}
}
