using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Utils.Test.LinqHelper
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class LinqHelperSpec
    {
        public class GetMaxElement
        {
            [Fact]
            public void SourceNull_Throw()
            {
                Action t = () => Utils.LinqHelper.LinqHelper.GetMaxElement<object>(null, null);
                t.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("source");
            }

            [Fact]
            public void ComparisonNull_Throw()
            {
                Action t = () => Utils.LinqHelper.LinqHelper.GetMaxElement(new List<object>(), null);
                t.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("comparison");
            }

            [Fact]
            public void SourceEmpty_ReturnDefault()
            {
                Utils.LinqHelper.LinqHelper.GetMaxElement(new List<object>(), o => 0)
                    .Should().Be(default(object));
            }

            [Fact]
            public void HappyPath_ReturnBiggestElement()
            {
                Utils.LinqHelper.LinqHelper.GetMaxElement(new List<int> { 1, 5, 2, 81, 0 }, o => o)
                    .Should().Be(81);
            }
        }
    }
}
