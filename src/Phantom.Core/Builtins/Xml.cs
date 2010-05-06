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

            XmlDocument document = LoadDocument(filename);
            return GetNodeContents(xpath, document, 0);
        }

        /// <summary>
        /// Loads an XML document from a file on disk.
        /// </summary>
        /// <param name="fileName">The file name of the file to load the XML document from.</param>
        /// <returns>
        /// A <see cref="XmlDocument">document</see> containing
        /// the document object representing the file.
        /// </returns>
        private static XmlDocument LoadDocument(string fileName)
        {
            XmlDocument document = null;

            document = new XmlDocument();
            document.Load(fileName);
            return document;            
        }

        /// <summary>
        /// Gets the contents of the node specified by the XPath expression.
        /// </summary>
        /// <param name="xpath">The XPath expression used to determine which nodes to choose from.</param>
        /// <param name="document">The XML document to select the nodes from.</param>
        /// <param name="nodeIndex">The node index in the case where multiple nodes satisfy the expression.</param>
        /// <returns>
        /// The contents of the node specified by the XPath expression.
        /// </returns>
        private static string GetNodeContents(string xpath, XmlDocument document, int nodeIndex)
        {
            string contents = null;
            XmlNodeList nodes;

            try
            {
                XmlNamespaceManager nsMgr = new XmlNamespaceManager(document.NameTable);
                //foreach (XmlNamespace xmlNamespace in Namespaces)
                //{
                //    if (xmlNamespace.IfDefined && !xmlNamespace.UnlessDefined)
                //    {
                //        nsMgr.AddNamespace(xmlNamespace.Prefix, xmlNamespace.Uri);
                //    }
                //}
                nodes = document.SelectNodes(xpath, nsMgr);
            }
            catch (Exception ex)
            {
                //throw new BuildException(string.Format(CultureInfo.InvariantCulture,
                //    ResourceUtils.GetString("NA1155"), xpath),
                //    Location, ex);
                throw;
            }

            if (nodes == null || nodes.Count == 0)
            {
                //throw new BuildException(string.Format(CultureInfo.InvariantCulture,
                //    ResourceUtils.GetString("NA1156"), xpath),
                //    Location);
                throw new Exception("Node not found");
            }

            Console.WriteLine(string.Format("Found '{0}' nodes with the XPath expression '{1}'.", nodes.Count, xpath));

            if (nodeIndex >= nodes.Count)
            {
                throw new Exception("Something");
                //throw new BuildException(string.Format(CultureInfo.InvariantCulture,
                //    ResourceUtils.GetString("NA1157"), nodeIndex), Location);
            }

            XmlNode selectedNode = nodes[nodeIndex];
            contents = selectedNode.InnerXml;
            return contents;
        }
    }
}
