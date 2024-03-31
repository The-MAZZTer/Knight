using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MZZT.WinApi.Wmi {
	public abstract class WmiBase {
		private static readonly Dictionary<Type, IEnumerable<WmiBase>> cache =
			new Dictionary<Type, IEnumerable<WmiBase>>();
		protected static IEnumerable<T> GetCache<T>() where T : WmiBase {
			while (!cache.ContainsKey(typeof(T)) || cache[typeof(T)] == null) {
				using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(string.Format("Select * from {0}", typeof(T).Name))) {
					try {
						cache[typeof(T)] = searcher.Get().Cast<ManagementObject>()
							.Select(x => (T)Activator.CreateInstance(typeof(T),
							BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] { x }, null));
					} catch (COMException) {
					}
				}
			}
			return cache[typeof(T)] as IEnumerable<T>;
		}

		protected static void Refresh<T>() {
			if (cache.ContainsKey(typeof(T))) {
				cache.Remove(typeof(T));
			}
		}

		protected WmiBase(ManagementObject obj) {
			using (obj) {
				foreach (PropertyInfo property in
					this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)) {

					object value = obj[property.Name];
					if (value == null) {
						continue;
					}
					
					if (property.PropertyType == typeof(DateTime) && value is string stringValue) {
						int index = stringValue.IndexOf("-");
						if (index < 0) {
							index = stringValue.IndexOf("+");
						}
						DateTime dateTime = DateTime.ParseExact(stringValue.Substring(0, index), "yyyyMMddHHmmss.ffffff", CultureInfo.CurrentCulture);
						TimeSpan offset = TimeSpan.FromMinutes(int.Parse(stringValue.Substring(index)));
						value = new DateTimeOffset(dateTime, offset).LocalDateTime;
					}

					property.SetValue(this, value);
				}
			}
		}
	}
}
