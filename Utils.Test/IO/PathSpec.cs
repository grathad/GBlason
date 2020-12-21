using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FluentAssertions;
using Xunit;
using Path = Utils.IO.Path;

namespace Utils.Test.IO
{
    public class PathSpec
    {

        [Fact]
        public void GetDirectoryName()
        {
            Path.GetDirectoryName("test")
                .Should().Be(System.IO.Path.GetDirectoryName("test"));
        }


        [Fact]
        public void Combine()
        {
            Path.Combine(string.Empty, string.Empty)
                .Should().Be(System.IO.Path.Combine(string.Empty, string.Empty));
        }
    }
}
