using Ebnf;
using FluentAssertions;
using GBlasonWebAPI.Models;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace GBlason.WebApi.Test
{
    public class TreeElementReferenceSpec
    {
        public class BuildSubTree
        {

            [Fact]
            public void NullParamsCtrReturnNullProperties()
            {
                var newReferenceT = TreeElementReference.CreateNew(null, null);
                newReferenceT.RealElement.Should().BeNull();
                newReferenceT.Reference.Should().BeNull();
            }

            [Fact]
            public void NoChildrenSourceReturnSource()
            {
                var root = new TreeElement();

                var newReferenceT = TreeElementReference.CreateNew(source: root, null);
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

                var newReferenceT = TreeElementReference.CreateNew(source: root, null);
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

                var memory = new Collection<TreeElementReference>();

                var newReferenceT =  TreeElementReference.CreateNew(root, memory);
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

            [Fact]
            public void SingleReferenceObjectCanSerializeWithoutReference()
            {
                var root = new TreeElement() { Name = "root" };

                var memory = new Collection<TreeElementReference>();

                var newReferenceT =  TreeElementReference.CreateNew(root, memory);
                //cyclic setup for test
                newReferenceT.Reference = newReferenceT;

                var json = JsonSerializer.Serialize(newReferenceT);

                json.Should().NotBeNullOrEmpty();
            }
        }
    }
}