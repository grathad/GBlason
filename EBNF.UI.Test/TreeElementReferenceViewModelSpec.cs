using FluentAssertions;

namespace EBNF.UI.Test
{
    public class TreeElementReferenceViewModelSpec
    {
        public class BuildSubTree
        {
            [Fact]
            public void EmptyCtrReturnNullProperties()
            {
                var newReferenceT = new TreeElementReferenceViewModel();
                newReferenceT.RealElement.Should().BeNull();
                newReferenceT.Reference.Should().BeNull();
            }


            [Fact]
            public void NullParamsCtrReturnNullProperties()
            {
                var newReferenceT = new TreeElementReferenceViewModel(null);
                newReferenceT.RealElement.Should().BeNull();
                newReferenceT.Reference.Should().BeNull();
            }

            [Fact]
            public void NoChildrenSourceReturnSource()
            {
                var root = new TreeElement();

                var newReferenceT = new TreeElementReferenceViewModel(source: root);
                newReferenceT.RealElement.Should().Be(root);
                newReferenceT.Reference.Should().BeNull();
            }

            [Fact]
            public void NoCyclicReferenceReturnFullTree()
            {
                var root = new TreeElement();
                var child = new TreeElement();
                var grandchild = new TreeElement();

                root.Children.Add(child);
                child.Children.Add(grandchild);

                var newReferenceT = new TreeElementReferenceViewModel(source: root);
                newReferenceT.RealElement.Should().Be(root);
                newReferenceT.Reference.Should().BeNull();
                newReferenceT.Children.Should().NotBeEmpty();
                newReferenceT.Children.Should().OnlyContain(c => c.RealElement == child);
                newReferenceT.Children.First().Children.Should().OnlyContain(c => c.RealElement == grandchild);
            }

            [Fact]
            public void CyclicReferenceReturnReference()
            {
                var root = new TreeElement() { Name = "root" };
                var child = new TreeElement() { Name = "child" };
                var grandchild = new TreeElement() { Name = "grandchild" }; 

                root.Children.Add(child);
                child.Children.Add(root);
                child.Children.Add(grandchild);

                var newReferenceT = new TreeElementReferenceViewModel(source: root);
                newReferenceT.RealElement.Should().Be(root);
                newReferenceT.Reference.Should().BeNull();
                newReferenceT.Children.Should().NotBeEmpty();
                newReferenceT.Children.Should().OnlyContain(c => c.RealElement == child);

                var childRef = newReferenceT.Children.First();
                childRef.Children.Should().Contain(c => c.RealElement == grandchild).And.Contain(c => c.RealElement == root);
                //the child's children treeelementref that represent the root should be a reference to the root and have NO child of its own
                var grandChildRoot = childRef.Children.First(c => c.RealElement == root);
                grandChildRoot.Reference.Should().Be(newReferenceT);
                grandChildRoot.Children.Should().BeEmpty();
            }
        }
    }
}