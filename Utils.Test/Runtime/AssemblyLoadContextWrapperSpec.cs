using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using FluentAssertions;
using Moq;
using Utils.Runtime;
using Xunit;

namespace Utils.Test.Runtime
{
    /// <summary>
    /// Not testing the 3rd party microsoft lib, just making sure we define the sepc and cover it with minimum error edge case handling for exception situations
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AssemblyLoadContextWrapperSpec
    {
        public class Constructor
        {
            [Fact]
            public void Default()
            {
                var test = new AssemblyLoadContextWrapper();
                test.Wrapped.Should().Be(AssemblyLoadContext.Default);
            }

            [Fact]
            public void Initialization()
            {
                var input = new Mock<AssemblyLoadContext>().Object;
                var test = new AssemblyLoadContextWrapper(input);
                test.Wrapped.Should().Be(input);
            }
        }

        public class Methods
        {
            [Fact]
            public void LoadFromAssemblyName()
            {
                new AssemblyLoadContextWrapper().LoadFromAssemblyName(null).Should().BeNull();
            }
            [Fact]
            public void LoadFromAssemblyPath()
            {
                new AssemblyLoadContextWrapper().LoadFromAssemblyPath(null).Should().BeNull();
            }
            [Fact]
            public void LoadFromNativeImagePath()
            {
                new AssemblyLoadContextWrapper().LoadFromNativeImagePath(null, null).Should().BeNull();
                new AssemblyLoadContextWrapper().LoadFromNativeImagePath("", null).Should().BeNull();
                new AssemblyLoadContextWrapper().LoadFromNativeImagePath("", "").Should().BeNull();
            }
            [Fact]
            public void LoadFromStream()
            {
                new AssemblyLoadContextWrapper().LoadFromStream(null).Should().BeNull();
                new AssemblyLoadContextWrapper().LoadFromStream(null, null).Should().BeNull();
            }

            [Fact]
            public void SetProfileOptimizationRoot()
            {
                Action t = () => new AssemblyLoadContextWrapper().SetProfileOptimizationRoot(null);
                t.Should().NotThrow();
            }

            [Fact]
            public void StartProfileOptimization()
            {
                Action t = () => new AssemblyLoadContextWrapper().StartProfileOptimization(null);
                t.Should().NotThrow();
            }
        }
    }
}
