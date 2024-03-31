using System.Collections.Generic;
using System.Management;

namespace MZZT.WinApi.Wmi {
	public class Win32_ComputerSystemProduct : WmiBase {
		public static IEnumerable<Win32_ComputerSystemProduct> ComputerSystemProducts {
			get {
				return GetCache<Win32_ComputerSystemProduct>();
			}
		}

		public static void Refresh() {
			Refresh<Win32_ComputerSystemProduct>();
		}

		protected Win32_ComputerSystemProduct(ManagementObject obj) : base(obj) { }

		public string Caption {
			get; private set;
		}

		public string Description {
			get; private set;
		}

		public string IdentifyingNumber {
			get; private set;
		}

		public string Name {
			get; private set;
		}

		public string SKUNumber {
			get; private set;
		}

		public string UUID {
			get; private set;
		}

		public string Vendor {
			get; private set;
		}

		public string Version {
			get; private set;
		}
	}
}
