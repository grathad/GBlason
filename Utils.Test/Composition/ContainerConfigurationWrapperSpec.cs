using System;
using System.Collections.Generic;
using System.Composition.Convention;
using System.Composition.Hosting;
using System.Composition.Hosting.Core;
using System.Reflection;
using System.Text;
using FluentAssertions;
using Moq;
using Utils.Composition.Contracts;
using Utils.Composition.Implementations;
using Xunit;
using RefAssembly = System.Reflection.Assembly;

namespace Utils.Test.Composition
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ContainerConfigurationWrapperSpec
    {
        public class Constructor
        {
            [Fact]
            public void Default()
            {
                var test = new ContainerConfigurationWrapper();
                test.Wrapped.Should().NotBeNull();
            }

            [Fact]
            public void Initialize()
            {
                var input = new Mock<ContainerConfiguration>().Object;
                var test = new ContainerConfigurationWrapper(input);
                test.Wrapped.Should().Be(input);
            }
        }

        public class Cast
        {
            [Fact]
            public void ToContainerConfiguration()
            {
                var input = new Mock<ContainerConfiguration>().Object;
                var test = (ContainerConfiguration)new ContainerConfigurationWrapper(input);
                test.Should().Be(input);
            }


            [Fact]
            public void ToCompositionWrapper()
            {
                var input = new Mock<ContainerConfiguration>().Object;
                var test = (ContainerConfigurationWrapper)input;
                test.Wrapped.Should().Be(input);
            }
        }

        public class CreateContainer
        {
            [Fact]
            public void NullWrapper()
            {
                var test = new ContainerConfigurationWrapper(null);
                test.CreateContainer()
                    .Should().BeAssignableTo<ICompositionHost>()
                    .And.NotBeNull();
            }
        }


        public class WithAssemblies
        {
            [Fact]
            public void NullWrapper_ReturnNull()
            {
                var test = new ContainerConfigurationWrapper(null);
                test.WithAssemblies(default(IEnumerable<RefAssembly>)).Should().Be(test);
                test.Wrapped.Should().BeNull();
                test.WithAssemblies(default(IEnumerable<RefAssembly>), default(AttributedModelProvider)).Should().Be(test);
                test.Wrapped.Should().BeNull();
            }

            [Fact]
            public void NullInput_Throw()
            {
                var wrap = new Mock<ContainerConfiguration>();

                var test = new ContainerConfigurationWrapper(wrap.Object);

                Action t = () => test.WithAssemblies(null);
                t.Should().Throw<ArgumentNullException>();
                test.Wrapped.Should().Be(wrap.Object);

                t = () => test.WithAssemblies(default(IEnumerable<RefAssembly>), default(AttributedModelProvider));
                t.Should().Throw<ArgumentNullException>();
                test.Wrapped.Should().Be(wrap.Object);
            }
        }

        public class WithAssembly
        {
            
        [Fact]
            public void NullWrapper_ReturnNull()
            {
                var test = new ContainerConfigurationWrapper(null);
                test.WithAssembly(default(RefAssembly)).Should().Be(test);
                test.Wrapped.Should().BeNull();
                test.WithAssembly(default(RefAssembly), default(AttributedModelProvider)).Should().Be(test);
                test.Wrapped.Should().BeNull();
            }
        }

        public class WithDefaultConventions
        {
            [Fact]
            public void NullWrapper_ReturnNull()
            {
                var test = new ContainerConfigurationWrapper(null);
                test.WithDefaultConventions(default(AttributedModelProvider)).Should().Be(test);
                test.Wrapped.Should().BeNull();

            }

            [Fact]
            public void NullInput_Throw()
            {
                var wrap = new Mock<ContainerConfiguration>();

                var test = new ContainerConfigurationWrapper(wrap.Object);

                Action t = () => test.WithDefaultConventions(null);
                t.Should().Throw<ArgumentNullException>();
                test.Wrapped.Should().Be(wrap.Object);
            }
        }

        public class WithPart
        {
            [Fact]
            public void NullWrapper_ReturnNull()
            {
                var test = new ContainerConfigurationWrapper(null);
                test.WithPart(default(Type)).Should().Be(test);
                test.Wrapped.Should().BeNull();
                test.WithPart(default(Type), default(AttributedModelProvider)).Should().Be(test);
                test.Wrapped.Should().BeNull();
                test.WithPart<object>().Should().Be(test);
                test.Wrapped.Should().BeNull();
                test.WithPart<object>(default(AttributedModelProvider)).Should().Be(test);
                test.Wrapped.Should().BeNull();
            }

            [Fact]
            public void NullInput_Throw()
            {
                var wrap = new Mock<ContainerConfiguration>();

                var test = new ContainerConfigurationWrapper(wrap.Object);

                Action t = () => test.WithPart(null);
                t.Should().Throw<ArgumentNullException>();
                test.Wrapped.Should().Be(wrap.Object);
            }
        }

        public class WithParts
        {
            [Fact]
            public void NullWrapper_ReturnNull()
            {
                var test = new ContainerConfigurationWrapper(null);
                test.WithParts(default(Type[])).Should().Be(test);
                test.Wrapped.Should().BeNull();
                test.WithParts(default(IEnumerable<Type>)).Should().Be(test);
                test.Wrapped.Should().BeNull();
                test.WithParts(default(IEnumerable<Type>), default(AttributedModelProvider)).Should().Be(test);
                test.Wrapped.Should().BeNull();
            }

            [Fact]
            public void NullInput_Throw()
            {
                var wrap = new Mock<ContainerConfiguration>();

                var test = new ContainerConfigurationWrapper(wrap.Object);

                Action t = () => test.WithParts(default(Type[]));
                t.Should().Throw<ArgumentNullException>();
                test.Wrapped.Should().Be(wrap.Object);

                t = () => test.WithParts(default(IEnumerable<Type>));
                t.Should().Throw<ArgumentNullException>();
                test.Wrapped.Should().Be(wrap.Object);

                t = () => test.WithParts(default(IEnumerable<Type>), default(AttributedModelProvider));
                t.Should().Throw<ArgumentNullException>();
                test.Wrapped.Should().Be(wrap.Object);
            }
        }

        public class WithProvider
        {

            [Fact]
            public void NullWrapper_ReturnNull()
            {
                var test = new ContainerConfigurationWrapper(null);
                test.WithProvider(default(ExportDescriptorProvider)).Should().Be(test);
                test.Wrapped.Should().BeNull();
            }

            [Fact]
            public void NullInput_Throw()
            {
                var wrap = new Mock<ContainerConfiguration>();

                var test = new ContainerConfigurationWrapper(wrap.Object);

                Action t = () => test.WithProvider(default(ExportDescriptorProvider));
                t.Should().Throw<ArgumentNullException>();
                test.Wrapped.Should().Be(wrap.Object);
            }
        }
    }
}
