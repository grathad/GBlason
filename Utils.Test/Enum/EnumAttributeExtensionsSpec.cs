using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using Moq;
using Utils.Enum;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace Utils.Test.Enum
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EnumAttributeExtensionsSpec
    {

        public class Constructor
        {
            [Fact]
            public void Default()
            {
                typeof(EnumAttributeExtensions).GetTypeInfo().TypeInitializer.Invoke(null, null);
                EnumAttributeExtensions.EnumAttributeFactory.Should().NotBeNull();
            }
        }

        public class Attributes : IDisposable
        {
            private enum Test
            {
                Value
            }

            [Fact]
            public void CallFactoryNull()
            {
                var result = new object();

                IEnumAttribute Mock(System.Enum @enum)
                {
                    result = @enum;
                    return new Mock<IEnumAttribute>().Object;
                }

                EnumAttributeExtensions.EnumAttributeFactory = Mock;
                EnumAttributeExtensions.Attributes(null);
                result.Should().BeNull();
            }

            [Fact]
            public void CallFactoryValidInput()
            {
                var result = new object();

                IEnumAttribute Mock(System.Enum @enum)
                {
                    result = @enum;
                    return new Mock<IEnumAttribute>().Object;
                }

                EnumAttributeExtensions.EnumAttributeFactory = Mock;
                Test.Value.Attributes();
                result.Should().Be(Test.Value);
            }

            public void Dispose()
            {
                typeof(EnumAttributeExtensions).GetTypeInfo().TypeInitializer.Invoke(null, null);
            }
        }
    }
}
