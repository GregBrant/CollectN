﻿using System.IO;
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