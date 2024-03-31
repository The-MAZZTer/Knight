using System;

namespace MZZT.Steam {
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class SteamNameAttribute(string name = null) : Attribute() {
		public string Name { get; set; } = name;
	}
}