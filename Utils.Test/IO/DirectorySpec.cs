using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FluentAssertions;
using Xunit;
using Directory = Utils.IO.Directory;

namespace Utils.Test.IO
{
    public class DirectorySpec
    {
        [Fact]
        public void Exists()
        {
            Directory.Exists(string.Empty)
                .Should().Be(System.IO.Directory.Exists(string.Empty));
        }


        [Fact]
        public void GetFiles()
        {
            //they are identical they throw empty parameter exception ...
            Assert.Throws<ArgumentException>(() => Directory.GetFiles(string.Empty, string.Empty, SearchOption.AllDirectories));
            Assert.Throws<ArgumentException>(() => System.IO.Directory.GetFiles(string.Empty, string.Empty, SearchOption.AllDirectories));
        }
    }
}
