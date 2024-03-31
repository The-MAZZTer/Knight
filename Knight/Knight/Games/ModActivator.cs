namespace MZZT.Knight.Games {
	public static class ModActivator {
		private static ModActivationChanges GetModActivationChanges(Game game, IEnumerable<Mod> desiredActive) {
			Dictionary<bool, Mod[]> mods = game.Mods.Values.GroupBy(x => x.Active).
				ToDictionary(x => x.Key, x => x.ToArray());
			return new ModActivationChanges() {
				PendingActivations = mods.TryGetValue(false, out Mod[] value) ? value.Intersect(desiredActive).ToArray() : desiredActive.ToArray(),
				PendingDractivations = mods.TryGetValue(true, out Mod[] value2) ? value2.Except(desiredActive).ToArray() : []
			};
		}

		public static bool GetModActivationChangesPending(Game game, IEnumerable<Mod> desiredActive) {
			ModActivationChanges changes = GetModActivationChanges(game, desiredActive);
			return changes.PendingActivations.Length > 0 || changes.PendingDractivations.Length > 0;
		}

		public static async Task SetActivatedMods(Game game, IEnumerable<Mod> desiredActive) {
			ModActivationChanges changes = GetModActivationChanges(game, desiredActive);
			if (changes.PendingActivations.Length == 0 && changes.PendingDractivations.Length == 0) {
				return;
			}

			foreach (Mod mod in changes.PendingDractivations) {
				await mod.Deactivate();
			}

			foreach (Mod mod in changes.PendingActivations) {
				await mod.Activate();
			}
		}

		public static async Task PlaySingleplayer(Game game, IEnumerable<Mod> desiredActive) {
			if (desiredActive.Count() > 0) {
				await SetActivatedMods(game, desiredActive);
			}

			game.Run(desiredActive);

			switch (Program.Settings.RunAction) {
				case Settings.RunActions.Minimize:
					Application.OpenForms[0].WindowState = FormWindowState.Minimized;
					break;
				case Settings.RunActions.Close:
					Application.Exit();
					break;
			}
		}

		public static async Task PlayMultiplayer(Game game, IEnumerable<Mod> desiredActive) {
			if (desiredActive.Count() > 0) {
				await SetActivatedMods(game, desiredActive);
			}

			game.RunMultiplayer(desiredActive);

			switch (Program.Settings.RunAction) {
				case Settings.RunActions.Minimize:
					Application.OpenForms[0].WindowState = FormWindowState.Minimized;
					break;
				case Settings.RunActions.Close:
					Application.Exit();
					break;
			}
		}

		public static async Task Play(Game game, IEnumerable<Mod> desiredActive) {
			bool multi = game.HasSeparateMultiplayerBinary && desiredActive.Count() > 0 &&
				game.AreModsMultiplayerOnly(desiredActive);
			if (multi) {
				await PlayMultiplayer(game, desiredActive);
			} else {
				await PlaySingleplayer(game, desiredActive);
			}
		}
	}

	public struct ModActivationChanges {
		public Mod[] PendingActivations;
		public Mod[] PendingDractivations;
	}
}
