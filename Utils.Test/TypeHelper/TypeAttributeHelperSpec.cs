using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Utils.Enum;
using Utils.TypeHelper;
using Xunit;

namespace Utils.Test.TypeHelper
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class TypeAttributeHelperSpec
    {

        public class Constructor
        {

            [Fact]
            public void Initialization()
            {
                var test = new TypeAttributeHelper(null);
                test.Type.Should().BeNull();

                test = new TypeAttributeHelper(typeof(object));
                test.Type.Should().Be(typeof(object));
            }
        }

        public class ContainsInterface
        {

            private interface ITest { }

            private interface INotImplemented { }

            private class CTest : ITest { }
            
            [Fact]
            public void TypeIsNull_ReturnFalse()
            {
                var test = new TypeAttributeHelper(null);

               test.ContainsInterface<ITest>().Should().BeFalse();
            }

            [Fact]
            public void TypeInherit_ReturnTrue()
            {
                var test = new TypeAttributeHelper(typeof(CTest));
                test.ContainsInterface<ITest>().Should().BeTrue();
            }

            [Fact]
            public void TypeNotInherit_ReturnFalse()
            {
                var test = new TypeAttributeHelper(typeof(CTest));
                test.ContainsInterface<INotImplemented>().Should().BeFalse();
            }
        }
    }
}
