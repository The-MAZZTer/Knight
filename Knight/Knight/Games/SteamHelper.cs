using Microsoft.Win32;
using MZZT.Steam;

namespace MZZT.Knight.Games {
	public static class SteamHelper {
		public enum AppIds {
			JediAcademy = 6020,
			JediOutcast = 6030,
			JediKnight = 32380,
			MysteriesOfTheSith = 32390,
			DarkForces = 32400
		}

		public static bool IsAppInstalled(AppIds appId) {
			if (Registry.GetValue($@"HKEY_CURRENT_USER\Software\Valve\Steam\Apps\{(int)appId}", "Installed", 0) is int value) {
				return value > 0;
			}
			return false;
		}
		
		public static string SteamPath {
			get {
				if (Registry.GetValue(@"HKEY_CURRENT_USER\Software\Valve\Steam", "SteamPath", null) is string path) {
					return Path.GetFullPath(path);
				}
				return null;
			}
		}

		public static string[] LibraryFolders {
			get {
				string path = SteamPath;
				if (path == null) {
					return [];
				}

				List<string> libraryFolders = [
				path
			];

				path = Path.Combine(path, @"steamapps\libraryfolders.vdf");
				if (!File.Exists(path)) {
					return [.. libraryFolders];
				}

				ValveDefinitionFile libraryFoldersVdf = ValveDefinitionFile.ReadAsync(path).GetAwaiter().GetResult();

				int pos = 0;
				while (pos < libraryFoldersVdf.Tokens.Count) {
					ValveDefinitionFile.Token token = libraryFoldersVdf.Tokens[pos];
					pos++;

					string property = (token as ValveDefinitionFile.StringToken)?.Text;
					if (property != "path" || pos >= libraryFoldersVdf.Tokens.Count) {
						continue;
					}

					token = libraryFoldersVdf.Tokens[pos];
					pos++;
					string value = (token as ValveDefinitionFile.StringToken)?.Text;
					if (value == null) {
						continue;
					}

					libraryFolders.Add(value);
				}

				return [.. libraryFolders];
			}
		}
	}
}
