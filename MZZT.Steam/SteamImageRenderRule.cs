namespace MZZT.Steam {
	[SteamName("image")]
	public class SteamImageRenderRule(string left, string top, string right, string bottom,
		SteamSubstitutionValue<string> path) : SteamRenderRule(left, top, right, bottom) {
		public SteamSubstitutionValue<string> Path { get; set; } = path;
	}
}