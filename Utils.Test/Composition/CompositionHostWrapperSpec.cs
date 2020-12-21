using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Composition.Hosting;
using System.Composition.Hosting.Core;
using System.Text;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Utils.Composition.Implementations;
using Xunit;

namespace Utils.Test.Composition
{
    public class CompositionHostWrapperSpec
    {
        public class Constructor
        {
            [Fact]
            public void InitializeNull()
            {
                var test = new CompositionHostWrapper(null);
                test.Wrapped.Should().BeNull();
            }

            [Fact]
            public void Initialize()
            {
                var input = CompositionHost.CreateCompositionHost();
                var test = new CompositionHostWrapper(input);
                test.Wrapped.Should().Be(input);
            }
        }

        public class Dispose
        {
            [Fact]
            public void DisposeNull()
            {
                Action t = () => new CompositionHostWrapper(null).Dispose();
                t.Should().NotThrow();
            }
        }

        public class WrappedMethods
        {
            [Fact]
            public void TryGetExport()
            {
                var result = new CompositionHostWrapper(null).TryGetExport(null, out var objectOut);
                result.Should().BeFalse();
                objectOut.Should().BeNull();
            }

            [Fact]
            public void GetExport()
            {
                var result = new CompositionHostWrapper(null).GetExport(default(Type));
                result.Should().BeNull();

                result = new CompositionHostWrapper(null).GetExport(default(Type), "");
                result.Should().BeNull();

                result = new CompositionHostWrapper(null).GetExport(default(CompositionContract));
                result.Should().BeNull();

                result = new CompositionHostWrapper(null).GetExport<object>();
                result.Should().BeNull();

                result = new CompositionHostWrapper(null).GetExport<object>("");
                result.Should().BeNull();
            }

            [Fact]
            public void GetExports()
            {
                var result = new CompositionHostWrapper(null).GetExports<object>();
                result.Should().BeNull();

                result = new CompositionHostWrapper(null).GetExports<object>("");
                result.Should().BeNull();

                result = new CompositionHostWrapper(null).GetExports(default(Type));
                result.Should().BeNull();

                result = new CompositionHostWrapper(null).GetExports(default(Type), "");
                result.Should().BeNull();
            }
        }
    }
}
