using System.Reflection;
using FluentAssertions;
using Moq;
using Utils.Assembly;
using Utils.TypeHelper;
using Xunit;
using Xunit.Abstractions;

namespace Utils.Test.Assembly
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AssemblyAttributeHelperSpec
    {

        public class Constructor
        {

            [Fact]
            public void Initialization()
            {
                var test = new AssemblyAttributeHelper(null);
                test.Type.Should().BeNull();

                var assMock = new Mock<System.Reflection.Assembly>().Object;
                test = new AssemblyAttributeHelper(assMock);
                test.Type.Should().BeSameAs(assMock);
            }
        }

        public class GetCustomAttributes
        {
            [Fact]
            public void SameAsBaseAssembly()
            {
                var origin = System.Reflection.Assembly.GetAssembly(typeof(AssemblyAttributeHelperSpec));
                var test = new AssemblyAttributeHelper(origin);
                var expectedResult = origin.GetCustomAttributes<AssemblyVersionAttribute>();
                test.GetCustomAttributes<AssemblyVersionAttribute>().Should()
                     .AllBeEquivalentTo(expectedResult);
            }
        }

        public class GetName
        {
            [Fact]
            public void SameAsBaseAssembly()
            {
                var origin = System.Reflection.Assembly.GetAssembly(typeof(AssemblyAttributeHelperSpec));
                var test = new AssemblyAttributeHelper(origin);
                var expectedResult = origin.GetName();
                test.GetName().Should().BeEquivalentTo(expectedResult);
            }
        }
    }
}
