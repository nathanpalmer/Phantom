using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phantom.Core.Builtins {
    using System.Globalization;
    using System.IO;
    using System.Xml;

    public static class XmlTools {
        /// <summary>
        /// Loads an XML document from a file on disk.
        /// </summary>
        /// <param name="fileName">The file name of the file to load the XML document from.</param>
        /// <returns>
        /// A <see cref="XmlDocument">document</see> containing
        /// the document object representing the file.
        /// </returns>
        public static XmlDocument LoadDocument(string fileName)
        {
            XmlDocument document = null;

            document = new XmlDocument();
            document.Load(fileName);
            return document;
        }

        /// <summary>
        /// Loads an XML document from a file on disk.
        /// </summary>
        /// <param name="fileName">
        /// The file name of the file to load the XML document from.
        /// </param>
        /// <param name="preserveWhitespace">
        /// Value for XmlDocument.PreserveWhitespace that is set before the xml is loaded.
        /// </param>
        /// <returns>
        /// An <see cref="System.Xml.XmlDocument" /> containing
        /// the document object model representing the file.
        /// </returns>
        public static XmlDocument LoadDocument(string fileName, bool preserveWhitespace)
        {
            XmlDocument document = null;

            //Console.WriteLine("Attempting to load XML document in file '{0}'.", fileName);

            document = new XmlDocument();
            document.PreserveWhitespace = preserveWhitespace;
            document.Load(fileName);

            //Console.WriteLine("XML document in file '{0}' loaded successfully.", new FileInfo(fileName).Name);
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
        public static string GetNodeContents(string xpath, XmlDocument document, int nodeIndex)
        {
            string contents = null;
            XmlNodeList nodes;

            //try
            //{
                XmlNamespaceManager nsMgr = new XmlNamespaceManager(document.NameTable);
                //foreach (XmlNamespace xmlNamespace in Namespaces)
                //{
                //    if (xmlNamespace.IfDefined && !xmlNamespace.UnlessDefined)
                //    {
                //        nsMgr.AddNamespace(xmlNamespace.Prefix, xmlNamespace.Uri);
                //    }
                //}
                nodes = document.SelectNodes(xpath, nsMgr);
            //}
            //catch (Exception ex)
            //{
            //    //throw new BuildException(string.Format(CultureInfo.InvariantCulture,
            //    //    ResourceUtils.GetString("NA1155"), xpath),
            //    //    Location, ex);
            //    throw;
            //}

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

        /// <summary>
        /// Given an XML document and an expression, returns a list of nodes
        /// which match the expression criteria.
        /// </summary>
        /// <param name="xpath">
        /// The XPath expression used to select the nodes.
        /// </param>
        /// <param name="document">
        /// The XML document that is searched.
        /// </param>
        /// <param name="nsMgr">
        /// An <see cref="XmlNamespaceManager" /> to use for resolving namespaces 
        /// for prefixes in the XPath expression.
        /// </param>
        /// <returns>
        /// An <see cref="XmlNodeList" /> containing references to the nodes 
        /// that matched the XPath expression.
        /// </returns>
        public static XmlNodeList SelectNodes(string xpath, XmlDocument document, XmlNamespaceManager nsMgr)
        {
            XmlNodeList nodes = null;

            //try
            //{
                //Console.WriteLine("Selecting nodes with XPath expression '{0}'.", xpath);

                nodes = document.SelectNodes(xpath, nsMgr);

                // report back how many we found if any. If not then
                // log a message saying we didn't find any.
                if (nodes.Count != 0)
                {
                    Console.WriteLine("Found '{0}' nodes matching XPath expression '{1}'.", nodes.Count, xpath);
                }
                else
                {
                    Console.WriteLine("No matching nodes were found with XPath expression '{0}'.", xpath);
                }

                return nodes;
            //}
            //catch (Exception ex)
            //{
            //    throw new BuildException(string.Format(CultureInfo.InvariantCulture,
            //        ResourceUtils.GetString("NA1161"),
            //        xpath), Location, ex);
            //}
        }

        /// <summary>
        /// Given a node list, replaces the XML within those nodes.
        /// </summary>
        /// <param name="nodes">
        /// The list of nodes to replace the contents of.
        /// </param>
        /// <param name="value">
        /// The text to replace the contents with.
        /// </param>
        public static void UpdateNodes(XmlNodeList nodes, string value)
        {
            Console.WriteLine("Updating nodes with value '{0}'.", value);

            int index = 0;
            foreach (XmlNode node in nodes)
            {
                //Console.WriteLine("Updating node '{0}'.", index);
                node.InnerXml = value;
                index++;
            }

            //Console.WriteLine("Updated all nodes successfully.", value);
        }

        /// <summary>
        /// Saves the XML document to a file.
        /// </summary>
        /// <param name="document">The XML document to be saved.</param>
        /// <param name="fileName">The file name to save the XML document under.</param>
        public static void SaveDocument(XmlDocument document, string fileName)
        {
            //Console.WriteLine("Attempting to save XML document to '{0}'.", fileName);
            document.Save(fileName);
            //Console.WriteLine("XML document successfully saved to '{0}'.", fileName);            
        }
    }
}
