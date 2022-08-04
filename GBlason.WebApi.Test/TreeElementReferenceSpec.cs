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

                var newReferenceT = TreeElementReference.CreateNew(root, memory);
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

                var newReferenceT = TreeElementReference.CreateNew(root, memory);
                //cyclic setup for test
                newReferenceT.Reference = newReferenceT;

                var json = JsonSerializer.Serialize(newReferenceT);

                json.Should().NotBeNullOrEmpty();
            }


            [Fact]
            public void BuildingTreeReferenceWithReference()
            {
                //A = B | C | (B, C)
                //B = "b"
                //C = "c"
                var A = new TreeElement() { Name = "A" };
                var GroupB = new TreeElement() { Name = "Group#B" };
                var GroupC = new TreeElement() { Name = "Group#C" };
                var GroupBC = new TreeElement() { Name = "Group#BC" };
                var B = new TreeElement() { Name = "B", IsLeaf = true };
                var C = new TreeElement() { Name = "C", IsLeaf = true };

                ((List<TreeElement>)A.Children).AddRange(new[] { GroupB, GroupC, GroupBC });
                GroupB.Children.Add(B);
                GroupC.Children.Add(C);
                ((List<TreeElement>)GroupBC.Children).AddRange(new[] { B, C });
                //so the test is meant to verify that the children of GroupBC (as a treeelementreference) are all references and not real elements

                var itemsRef = new Collection<TreeElementReference>();
                var newReferenceT = TreeElementReference.CreateNew(source: A, itemsRef);

                newReferenceT.Should().NotBeNull();
                itemsRef.Should().HaveCount(6);

                var firstBInstance = newReferenceT.Children.First().Children.First();       //the real element wrapped
                var secondBInstance = newReferenceT.Children.Last().Children.First();       //a reference to the real element

                firstBInstance.RealElement.Should().Be(B);
                secondBInstance.RealElement.Should().Be(B);

                secondBInstance.ReferenceToElement.Should().Be(firstBInstance.ElementId);
                secondBInstance.Reference.Should().Be(firstBInstance);
            }
        }

        public class FindParent
        {
            [Fact]
            public void NullInputMeansUnchangedBranch()
            {
                /* Tests this block
                 
            if (this == null || root == null)
            {
                return;
            }            
                 */
            }

            [Fact]
            public void CyclicBranchReturnsNodes()
            {
                /* Tests this block
                             
            if (cycle.Any(c => c.ElementId == ElementId))
            {
                //we are in a cyclic branch, returning
                return;
            }
                 */
            }

            [Fact]
            public void FindParentFromRootReturnsRoot()
            {
                /* Tests this block
            cycle.Add(this);
            if (this == root)
            {
                //we reached the root, we are done
                return;
            }
                 */
            }

            [Fact]
            public void NodeWithoutParentReturnsBranch()
            {
                /* Tests this block (with actual nodes in the branch possible, we reached a root which is not the one in the input)
            if (Parent == null)
            {
                //no way to continue
                return;
            }
                 */
            }


            [Fact]
            public void HappyPathReturnFullBranch()
            {
                //here we have a simple tree,
                // Root => [A,B]
                // A => [Bref,C]
                // C => D
                // looking for the parent of C should return Root => A => C without D

                var d = new TreeElement { Name = "D" };
                var c = new TreeElement { Name = "C" };
                c.Children.Add(d);
                var b = new TreeElement { Name = "B" };
                var a = new TreeElement { Name = "A" };
                a.Children.Add(b);
                a.Children.Add(c);
                var treeRoot = new TreeElement { Name = "root" };
                treeRoot.Children.Add(a);
                treeRoot.Children.Add(b);

                //if I was doing it clean, I should mock this one and only return the final version of the reference tree, but lazy
                var referenceRoot = TreeElementReference.CreateNew(treeRoot);
                var referenceA = referenceRoot.Children.First();
                var referenceC = referenceA.Children[1];

                var branchResult = new Collection<TreeElementReference>();
                referenceC.FindParent(ref branchResult, referenceRoot);

                branchResult.Should().HaveCount(3);
                branchResult.Should().ContainInOrder(new[] { referenceRoot, referenceA, referenceC });
            }
        }
    }
}