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
    using System.Globalization;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Xml;

    [CompilerGlobalScope]
    public sealed class Xml {

        public static string xmlpeek(string filename, string xpath, string property) {
            //Console.WriteLine(string.Format("Peeking at '{0}' with XPath expression '{1}'.", filename, xpath));

            if (!File.Exists(filename)) {
                throw new FileNotFoundException(string.Format("The file {0} was not found", filename), filename);
            }

            XmlDocument document = XmlTools.LoadDocument(filename);
            return XmlTools.GetNodeContents(xpath, document, 0);
        }
        
    }
}
