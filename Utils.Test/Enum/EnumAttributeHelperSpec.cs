using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Utils.Enum;
using Xunit;

namespace Utils.Test.Enum
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EnumAttributeHelperSpec
    {

        public class Constructor
        {
            private enum EnumTest
            {
                Value
            }

            [Fact]
            public void Initialization()
            {
                var test = new EnumAttributeHelper(null);
                test.Enumeration.Should().BeNull();

                const EnumTest input = EnumTest.Value;
                test = new EnumAttributeHelper(input);
                test.Enumeration.Should().Be(input);
            }
        }

        public class GetAttribute
        {
            private class AttributeTest : Attribute
            {

            }

            private enum EnumTest
            {
                [AttributeTest]
                WithAttribute,
                WithoutAttribute
            }

            [Fact]
            public void EnumerationIsNull_ThrowException()
            {
                var test = new EnumAttributeHelper(null);

                Action t = () => test.GetAttribute<Attribute>();

                t.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("Enumeration");
            }

            [Fact]
            public void AttributePresent_ReturnAttribute()
            {
                var test = new EnumAttributeHelper(EnumTest.WithAttribute);
                test.GetAttribute<AttributeTest>().Should().NotBeNull();
            }

            [Fact]
            public void AttributeNotPresent_ReturnNull()
            {
                var test = new EnumAttributeHelper(EnumTest.WithoutAttribute);
                test.GetAttribute<AttributeTest>().Should().BeNull();
            }
        }

        public class GetAttributes
        {
            private class AttributeTest : Attribute
            {

            }

            private enum EnumTest
            {
                [AttributeTest]
                WithAttribute,
                WithoutAttribute
            }

            [Fact]
            public void EnumerationIsNull_ThrowException()
            {
                var test = new EnumAttributeHelper(null);

                Action t = () => test.GetAttributes<Attribute>();

                t.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("Enumeration");
            }

            [Fact]
            public void AttributePresent_ReturnAttribute()
            {
                var test = new EnumAttributeHelper(EnumTest.WithAttribute);
                test.GetAttributes<AttributeTest>().Should().NotBeNull();
            }

            [Fact]
            public void AttributeNotPresent_ReturnNull()
            {
                var test = new EnumAttributeHelper(EnumTest.WithoutAttribute);
                test.GetAttributes<AttributeTest>().Should().BeEmpty();
            }
        }

        public class FindAttributes
        {
            private class AttributeTest : Attribute
            {
                internal readonly string Test;
                public AttributeTest(string test)
                {
                    Test = test;
                }
            }

            private enum NoAttributeEnumTest
            {
                WithAttribute,
                WithoutAttribute
            }

            [AttributeTest("parent")]
            private enum EnumAttributeTest
            {
                WithAttribute,
                WithoutAttribute
            }

            [Fact]
            public void NoEnumAndNoValueLevelsAttributes_ReturnEmpty()
            {
                var test = new EnumAttributeHelper(NoAttributeEnumTest.WithAttribute);
                test.FindAttributes<AttributeTest>().Should().BeEmpty();
            }

            [Fact]
            public void EnumLevelsAttributesOnly_ReturnEnumLevelAttribute()
            {
                var test = new EnumAttributeHelper(EnumAttributeTest.WithAttribute);

                var result = test.FindAttributes<AttributeTest>();
                result.Should().HaveCount(1).And
                    .Subject.Should().OnlyContain(attributeTest => attributeTest.Test == "parent");
            }

            [Fact]
            public void EnumValueLevelsAttributesOnly_ReturnEnumValueLevelAttribute()
            {
                var mock = new Mock<EnumAttributeHelper>(NoAttributeEnumTest.WithoutAttribute)
                { CallBase = true };
                var valueAttributeList = new List<AttributeTest> { new AttributeTest("value") };
                mock.Setup(m => m.GetAttributes<AttributeTest>()).Returns(valueAttributeList);

                var test = mock.Object.FindAttributes<AttributeTest>();
                test.Should().HaveCount(1).And
                    .Subject.Should().OnlyContain(attributeTest => attributeTest.Test == "value");
            }

            [Fact]
            public void BothLevelPresent_ReturnEnumValueLevelOnly()
            {
                var mock = new Mock<EnumAttributeHelper>(EnumAttributeTest.WithoutAttribute)
                { CallBase = true };
                var valueAttributeList = new List<AttributeTest> { new AttributeTest("value") };
                mock.Setup(m => m.GetAttributes<AttributeTest>()).Returns(valueAttributeList);

                var test = mock.Object.FindAttributes<AttributeTest>();
                test.Should().HaveCount(1).And
                    .Subject.Should().OnlyContain(attributeTest => attributeTest.Test == "value");
            }
        }
    }
}
