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

namespace Phantom.Core.Builtins
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Xml;
    using Language;

    public class xmlpoke : IRunnable<bool> {
        /// <summary>
        /// The name of the file that contains the XML document that is going 
        /// to be poked.
        /// </summary>
        public string file { get; set; }

        /// <summary>
        /// The XPath expression used to select which nodes are to be modified.
        /// </summary>
        public string xpath { get; set; }

        /// <summary>
        /// The value that replaces the contents of the selected nodes.
        /// </summary>
        public string value { get; set; }       

        /// <summary>
        /// Namespace definitions to resolve prefixes in the XPath expression.
        /// </summary>
        public bool PreserveWhitespace { get; set; }        

        public bool Run() {
            var XmlFile = new FileInfo(file);

            // ensure the specified xml file exists
            if (!XmlFile.Exists)
            {
                throw new Exception(string.Format("The xml file {0} does not exist!", XmlFile.FullName));
            }

            //try
            //{
            XmlDocument document = XmlTools.LoadDocument(XmlFile.FullName, PreserveWhitespace);

            XmlNamespaceManager nsMgr = new XmlNamespaceManager(document.NameTable);
            //foreach (XmlNamespace xmlNamespace in Namespaces)
            //{
            //    if (xmlNamespace.IfDefined && !xmlNamespace.UnlessDefined)
            //    {
            //        nsMgr.AddNamespace(xmlNamespace.Prefix, xmlNamespace.Uri);
            //    }
            //}

            XmlNodeList nodes = XmlTools.SelectNodes(xpath, document, nsMgr);

            // don't bother trying to update any nodes or save the
            // file if no nodes were found in the first place.
            if (nodes.Count > 0)
            {
                XmlTools.UpdateNodes(nodes, value);
                XmlTools.SaveDocument(document, XmlFile.FullName);
            }
            //}
            //catch (BuildException ex)
            //{
            //    throw ex;
            //}
            //catch (Exception ex)
            //{
            //    throw new BuildException(string.Format(CultureInfo.InvariantCulture,
            //                                           ResourceUtils.GetString("NA1159"), XmlFile.FullName),
            //        Location, ex);
            //}

            return true;
        }
    }
}
