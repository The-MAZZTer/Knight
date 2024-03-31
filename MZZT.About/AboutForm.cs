using System.Reflection;

namespace MZZT.Forms {
	public partial class AboutForm : Form {
		public AboutForm(Assembly assembly = null) {
			this.InitializeComponent();

			if (assembly == null) {
				assembly = Assembly.GetEntryAssembly();
			}

			AssemblyName name = assembly.GetName();
			this.Text = string.Format(this.Text, name.Name);
			this.name.Text = string.Format(this.name.Text, name.Name);

			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			AssemblyName[] assemblyNames = assembly.GetReferencedAssemblies()
				.Concat(assemblies.Select(x => x.GetName()))
				.OrderBy(x => x.Name)
				.GroupBy(x => x.Name)
				.Select(x => x.First())
				.ToArray();

			DateTime build;
			string libraries = string.Join(Environment.NewLine, assemblyNames.Select(x => {
				Assembly libAssembly = assemblies.FirstOrDefault(y => x.FullName == y.FullName) ?? Assembly.Load(x);
				if (libAssembly == assembly || this.AppDirectory != Path.GetDirectoryName(new Uri(libAssembly.Location).LocalPath)) {
					return null;
				}

				/*if (assembly.GetType("FXAssembly") != null) {
					return null;
				}

				AssemblyProductAttribute product = assembly.GetCustomAttribute<AssemblyProductAttribute>();
				if (product?.Product == "Microsoft® .NET Framework") {
					return null;
				}*/

				build = DateTime.MinValue;
				try {
					build = this.GetBuildDate(libAssembly);
				} catch (Exception) { }

				string library = $"{x.Name} Version {x.Version}";
				if (build > DateTime.MinValue) {
					library += $" ({build})";
				}
				return library;
			}).Where(x => x != null));

			build = this.GetBuildDate(assembly);

			this.version.Text = string.Format(this.version.Text, name.Name, name.Version, build, libraries);
		}

		public string AppDirectory { get; set; } = Path.GetDirectoryName(Application.ExecutablePath);

		public DateTime GetBuildDate(Assembly me) {
			using Stream stream = me.GetManifestResourceStream(me.GetManifestResourceNames().First(x => x.Contains("BuildDate")));
			using StreamReader reader = new(stream);
			return new DateTime(long.Parse(reader.ReadToEnd()));
		}
	}
}
