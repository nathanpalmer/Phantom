using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phantom.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class XmlTester : ScriptTest {

        [Test]
        public void XmlPeek_Revision()
        {
            ScriptFile = "Scripts/Xml.boo";
            Execute("xmlpeek_property");
            AssertOutput("xmlpeek_property:", "Found '1' nodes with the XPath expression '/info/entry/@revision'.", "57");
        }

        [Test]
        public void XmlPoke()
        {
            ScriptFile = "Scripts/Xml.boo";
            Execute("xmlpoke");
            AssertOutput(
                "xmlpoke:",
                "Found '1' nodes matching XPath expression '/info/entry/@kind'.");
        }

    }
}
