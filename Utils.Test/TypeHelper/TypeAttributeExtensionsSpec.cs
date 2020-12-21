using System;
using System.Reflection;
using FluentAssertions;
using Moq;
using Utils.Enum;
using Utils.TypeHelper;
using Xunit;

namespace Utils.Test.TypeHelper
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class TypeAttributeExtensionsSpec
    {

        public class Constructor
        {
            [Fact]
            public void Default()
            {
                typeof(TypeAttributeExtensions).GetTypeInfo().TypeInitializer.Invoke(null, null);
                TypeAttributeExtensions.TypeAttributeFactory.Should().NotBeNull();
            }
        }

        public class Type : IDisposable
        {
            public Type()
            {
                typeof(TypeAttributeExtensions).GetTypeInfo().TypeInitializer.Invoke(null, null);
            }

            [Fact]
            public void CallFactory()
            {
                var result = typeof(object);

                ITypeAttribute Mock(System.Type type)
                {
                    result = type;
                    return new Mock<ITypeAttribute>().Object;
                }

                TypeAttributeExtensions.TypeAttributeFactory = Mock;
                TypeAttributeExtensions.Type(null);
                result.Should().BeNull();
                typeof(string).Type();
                result.Should().Be(typeof(string));
            }

            public void Dispose()
            {
                typeof(TypeAttributeExtensions).GetTypeInfo().TypeInitializer.Invoke(null, null);
            }
        }
    }
}
