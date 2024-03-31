using System;

namespace MZZT.Settings {
	public class SettingAttribute(string title, SettingTypes settingType) : Attribute {
		public string Title { get; private set; } = title;
		public SettingTypes SettingType { get; private set; } = settingType;
	}

	public class SettingPanelAttribute(SettingTypes settingType) : Attribute {
		public SettingTypes SettingType { get; private set; } = settingType;
	}

	public enum SettingTypes {
		Undefined,
		Text,
		FolderPath,
		Checkbox
	}
}
