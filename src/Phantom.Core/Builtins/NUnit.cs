#region License

// Copyright Jeremy Skinner (http://www.jeremyskinner.co.uk) and Contributors
// 
// Licensed under the Microsoft Public License. You may
// obtain a copy of the license at:
// 
// http://www.microsoft.com/opensource/licenses.mspx
// 
// By using this source code in any fashion, you are agreeing
// to be bound by the terms of the Microsoft Public License.
// 
// You must not remove this notice, or any other, from this software.

#endregion

namespace Phantom.Core.Builtins {
	using System;
	using Boo.Lang;

	/// <summary>
	/// NUnit integration
	/// </summary>
	public class nunit : ExecutableTool<nunit> {
		public nunit() {
			toolPath = "lib/nunit/nunit-console.exe";
		    dotnet_version = "2.0";
		    version = "2.4.8";
		}

		public string include { get; set; }
		public string exclude { get; set; }
        public string version { get; set; }
        public string dotnet_version { get; set; }
		public string[] assemblies { get; set; }
		public string assembly { get; set; }

		protected override void Execute() {
			if ((assemblies == null || assemblies.Length == 0) && string.IsNullOrEmpty(assembly)) {
				throw new InvalidOperationException("Please specify either the 'assembly' or the 'assemblies' property when calling 'nunit'");
			}

			//single assembly takes precedence.
			if (!string.IsNullOrEmpty(assembly)) {
				assemblies = new[] {assembly};
			}

			var args = new List<string>();

		    bool enableTeamCity = false;
		    string teamcityLauncherPath = UtilityFunctions.env("teamcity.dotnet.nunitlauncher");
		    if (!string.IsNullOrEmpty(teamcityLauncherPath)) {
                enableTeamCity = true;
            }

			if (enableTeamCity) {
                string teamCityArgs = string.Format("v{0} x86 NUnit-{1}", dotnet_version, version);

				if (!string.IsNullOrEmpty(teamcityLauncherPath)) {
					toolPath = teamcityLauncherPath;
					args.Add(teamCityArgs);
				}
				else {
					enableTeamCity = false;
				}
			}

			if (!string.IsNullOrEmpty(include)) {
				if (enableTeamCity) {
					args.Add(string.Format("/category-include:{0}", exclude));
				}
				else {
					args.Add(string.Format("/include:{0}", include));
				}
			}
			else if (!string.IsNullOrEmpty(exclude)) {
				if (enableTeamCity) {
					args.Add(string.Format("/category-exclude:{0}", exclude));
				}
				else {
					args.Add(string.Format("/exclude:{0}", exclude));
				}
			}

            if (!string.IsNullOrEmpty(dotnet_version)) {
                if (!enableTeamCity && new Version(version) >= new Version("2.5.5")) {
                    args.Add(string.Format("/framework=net-{0}", dotnet_version));
                }
            }

			foreach (var asm in assemblies) {
				var nunitArgs = new List<string>(args) {
				                                       	asm
				                                       };

				Execute(nunitArgs.JoinWith(" "));
			}
		}
	}
}