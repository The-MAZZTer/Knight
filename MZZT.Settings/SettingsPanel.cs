using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace MZZT.Settings {
	public class SettingsPanel<T> : FlowLayoutPanel {
		public SettingsPanel(T settings) : base() {
			this.Settings = settings;

			if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || this.DesignMode) {
				return;
			}

			Type type = typeof(T);
			foreach ((MemberInfo member, SettingAttribute attribute) in type.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Select(x => (x, x.GetCustomAttribute<SettingAttribute>())).Where(x => x.Item2 != null)) {
				object value = member.MemberType switch {
					MemberTypes.Field => (member as FieldInfo).GetValue(settings),
					MemberTypes.Property => (member as PropertyInfo).GetValue(settings),
					MemberTypes.Method => (member as MethodInfo).Invoke(settings, []),
					_ => throw new NotSupportedException()
				};

				SettingTypes settingType = attribute.SettingType;
				if (settingType == SettingTypes.Undefined) {
					settingType = this.FindDefaultSettingType(member);
				}

				controlCache ??= AppDomain.CurrentDomain
						.GetAssemblies()
						.SelectMany(x => x.GetTypes())
						.Select(x => (x, x.GetCustomAttribute<SettingPanelAttribute>()))
						.Where(x => x.Item2 != null)
						.ToDictionary(x => x.Item2.SettingType, x => x.x);

				UserControl control = (UserControl)controlCache[settingType]
					.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
					.First()
					.Invoke([attribute, value]);

				this.map[(ISettingUserControl)control] = member;
				this.Controls.Add(control);

				control.Width = this.Width - this.Padding.Horizontal - control.Margin.Horizontal;
			}
		}
		private static Dictionary<SettingTypes, Type> controlCache;
		public T Settings { get; private set; }
		private readonly Dictionary<ISettingUserControl, MemberInfo> map = [];

		private SettingTypes FindDefaultSettingType(MemberInfo member) {
			Type type = member.MemberType switch {
				MemberTypes.Field => (member as FieldInfo).FieldType,
				MemberTypes.Property => (member as PropertyInfo).PropertyType,
				MemberTypes.Method => (member as MethodInfo).ReturnType,
				_ => throw new NotSupportedException()
			};

			if (type == typeof(bool)) {
				return SettingTypes.Checkbox;
			} else if (type == typeof(string)) {
				return SettingTypes.Text;
			}
			return SettingTypes.Undefined;
		}

		protected override void OnResize(EventArgs eventargs) {
			base.OnResize(eventargs);

			foreach (UserControl control in this.map.Keys) {
				control.Width = this.Width - this.Padding.Horizontal - control.Margin.Horizontal;
			}
		}

		public void Apply() {
			foreach ((object value, MemberInfo member) in this.map.Select(x => (x.Key.Value, x.Value))) {
				switch (member.MemberType) {
					case MemberTypes.Field:
						(member as FieldInfo).SetValue(this.Settings, value);
						break;
					case MemberTypes.Property:
						(member as PropertyInfo).SetValue(this.Settings, value);
						break;
					case MemberTypes.Method:
						(member as MethodInfo).Invoke(this.Settings, [value]);
						break;
					default:
						throw new NotSupportedException();
				}
			}
		}
	}
}
