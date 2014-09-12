using System.IO;
using CollectN.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectN.Tests.Core
{
    [TestFixture]
    class ConfigParserTests
    {

        [Test]
        public void LoadPluginAddsPluginNameToPluginsList()
        {
            // Arrange
            var contents = "LoadPlugin cpu";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(contents));
            stream.Seek(0, SeekOrigin.Begin);

            var parser = new ConfigParser();

            // Act
            var config = parser.Parse(stream);

            // Assert
            Assert.AreEqual(1, config.Plugins.Count);
            Assert.True(config.Plugins.Contains("cpu"));
        }

        
        [Test]
        public void IgnoreWhitespaceLine()
        {
            // Arrange
            var contents = @"  ";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(contents));
            stream.Seek(0, SeekOrigin.Begin);

            var parser = new ConfigParser();

            // Act
            var config = parser.Parse(stream);

            // Assert
            Assert.AreEqual(0, config.Count);
        }

        [Test]
        public void IgnoreCommentLine()
        {
            // Arrange
            var contents = @"#";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(contents));
            stream.Seek(0, SeekOrigin.Begin);

            var parser = new ConfigParser();

            // Act
            var config = parser.Parse(stream);

            // Assert
            Assert.AreEqual(0, config.Count);
        }


        [Test]
        public void ParserParsesSimpleKeyValue()
        {
            // Arrange
            var contents = "Key Value";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(contents));
            stream.Seek(0, SeekOrigin.Begin);

            var parser = new ConfigParser();

            // Act
            var config = parser.Parse(stream);

            // Assert
            Assert.True(config.ContainsKey("Key"));
            Assert.AreEqual(config["Key"], "Value");
        }

    }
}
