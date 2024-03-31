using System;
using System.Collections.Generic;
using System.Management;

namespace MZZT.WinApi.Wmi {
	public class Win32_Processor : WmiBase {
		public static IEnumerable<Win32_Processor> Processors {
			get {
				return GetCache<Win32_Processor>();
			}
		}

		public static void Refresh() {
			Refresh<Win32_Processor>();
		}

		protected Win32_Processor(ManagementObject obj) : base(obj) { }

		public ushort AddressWidth {
			get; private set;
		}

		public ushort Architecture {
			get; private set;
		}

		public string AssetTag {
			get; private set;
		}

		public ushort Availability {
			get; private set;
		}

		public string Caption {
			get; private set;
		}

		public uint Characteristics {
			get; private set;
		}

		public uint ConfigManagerErrorCode {
			get; private set;
		}

		public bool ConfigManagerUserConfig {
			get; private set;
		}

		public ushort CpuStatus {
			get; private set;
		}

		public string CreationClassName {
			get; private set;
		}

		public uint CurrentClockSpeed {
			get; private set;
		}

		public ushort CurrentVoltage {
			get; private set;
		}

		public ushort DataWidth {
			get; private set;
		}

		public string Description {
			get; private set;
		}

		public string DeviceID {
			get; private set;
		}

		public bool ErrorCleared {
			get; private set;
		}

		public string ErrorDescription {
			get; private set;
		}

		public uint ExtClock {
			get; private set;
		}

		public ushort Family {
			get; private set;
		}

		public DateTime InstallDate {
			get; private set;
		}

		public uint L2CacheSize {
			get; private set;
		}

		public uint L2CacheSpeed {
			get; private set;
		}

		public uint L3CacheSize {
			get; private set;
		}

		public uint L3CacheSpeed {
			get; private set;
		}

		public uint LastErrorCode {
			get; private set;
		}

		public ushort Level {
			get; private set;
		}

		public ushort LoadPercentage {
			get; private set;
		}

		public string Manufacturer {
			get; private set;
		}

		public uint MaxClockSpeed {
			get; private set;
		}

		public string Name {
			get; private set;
		}

		public uint NumberOfCores {
			get; private set;
		}

		public uint NumberOfEnabledCore {
			get; private set;
		}

		public uint NumberOfLogicalProcessors {
			get; private set;
		}

		public string OtherFamilyDescription {
			get; private set;
		}

		public string PartNumber {
			get; private set;
		}

		public string PNPDeviceID {
			get; private set;
		}

		public ushort[] PowerManagementCapabilities {
			get; private set;
		}

		public bool PowerManagementSupported {
			get; private set;
		}

		public string ProcessorId {
			get; private set;
		}

		public ushort ProcessorType {
			get; private set;
		}

		public ushort Revision {
			get; private set;
		}

		public string Role {
			get; private set;
		}

		public bool SecondLevelAddressTranslationExtensions {
			get; private set;
		}

		public string SerialNumber {
			get; private set;
		}

		public string SocketDesignation {
			get; private set;
		}

		public string Status {
			get; private set;
		}

		public ushort StatusInfo {
			get; private set;
		}

		public string Stepping {
			get; private set;
		}

		public string SystemCreationClassName {
			get; private set;
		}

		public string SystemName {
			get; private set;
		}

		public uint ThreadCount {
			get; private set;
		}

		public string UniqueId {
			get; private set;
		}

		public ushort UpgradeMethod {
			get; private set;
		}

		public string Version {
			get; private set;
		}

		public bool VirtualizationFirmwareEnabled {
			get; private set;
		}

		public bool VMMonitorModeExtensions {
			get; private set;
		}

		public uint VoltageCaps {
			get; private set;
		}
	}
}
