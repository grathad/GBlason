using Ebnf;
using FluentAssertions;
using GBlasonWebAPI.Controllers;
using GBlasonWebAPI.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;

namespace GBlason.WebApi.Test
{
    public class GBlasonWebAPISpec
    {
        protected static ILogger<EbnfController>? mockLogger;

        public GBlasonWebAPISpec()
        {
            mockLogger = new Mock<ILogger<EbnfController>>().Object;
        }

        public class EbnfControllerWrapper : EbnfController
        {
            public EbnfControllerWrapper(ILogger<EbnfController> logger) : base(logger)
            {
            }

            public new string BuildSafeTree()
            {
                return base.BuildSafeTree();
            }
        }

        public class Tree
        {
            EbnfController controller;


            public Tree()
            {
                controller = new EbnfController(mockLogger);
            }


            [Fact]
            public void NoTreeInMemoryCreateNewOne()
            {

            }


            [Fact]
            public void NoTreePossibleToBuildReturnError()
            {

            }

            [Fact]
            public void NoParametersReturnsHead()
            {

            }

            [Fact]
            public void InvalidParameterReturnError()
            {

            }

            [Fact]
            public void ValidParameterReturnSubtree()
            {
                //mocking a test tree like this (Id)
                //              [A]
                //      [B]             [C]
                // [D]      [E]     [F]         [G]
                var mockedTree = new Collection<TreeElementReference>();
                var nodeA = TreeElementReference.CreateNew(new TreeElement { Name = "nodeA" });
                var nodeB = TreeElementReference.CreateNew(new TreeElement { Name = "nodeB" });
                var nodeC = TreeElementReference.CreateNew(new TreeElement { Name = "nodeC" });
                var nodeD = TreeElementReference.CreateNew(new TreeElement { Name = "nodeD" });
                var nodeE = TreeElementReference.CreateNew(new TreeElement { Name = "nodeE" });
                var nodeF = TreeElementReference.CreateNew(new TreeElement { Name = "nodeF" });
                var nodeG = TreeElementReference.CreateNew(new TreeElement { Name = "nodeG" });

                nodeA.Children.Add(nodeB);
                nodeA.Children.Add(nodeC);

                nodeB.Children.Add(nodeD);
                nodeB.Children.Add(nodeE);

                nodeC.Children.Add(nodeF);
                nodeC.Children.Add(nodeG);

                mockedTree.Add(nodeA);
                mockedTree.Add(nodeB);
                mockedTree.Add(nodeC);
                mockedTree.Add(nodeD);
                mockedTree.Add(nodeE);
                mockedTree.Add(nodeF);
                mockedTree.Add(nodeG);

                var overridenCtrl = new Mock<EbnfController>(mockLogger) { CallBase = true };

                overridenCtrl.SetupGet(c => c.MemoryTree).Returns(mockedTree);

                var validSubTree = overridenCtrl.Object.Tree(nodeB.ElementId);

                validSubTree.Should().NotBeEmpty();
                //now checking the subtree validity
                var result = JsonSerializer.Deserialize<TreeElementReference>(validSubTree);
                result.Should().NotBeNull();
                result.ElementId.Should().Be(nodeB.ElementId);
                result.Children.Should().HaveCount(2);
                result.Children[0].ElementId.Should().Be(nodeD.ElementId);
                result.Children[1].ElementId.Should().Be(nodeE.ElementId);

            }
        }


        public class BuildSafeTree
        {
            Mock<EbnfControllerWrapper> bstMock;
            public BuildSafeTree()
            {
                bstMock = new Mock<EbnfControllerWrapper>(mockLogger) { CallBase = true };
            }

            [Fact]
            public void ValidStreamBuildValidTree()
            {
                var testValidGrammar =
                    @"nodeA =  nodeB | (nodeC , [nodeD] , nodeE , [nodeF] );
nodeB = nodeF | nodeG | nodeE;
nodeC = ""textC"";
nodeD = ""textD"";
nodeE = ""textE"";
nodeF = ""textF"";
nodeG = ""textG"";";
                var testValidTreeStream = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(testValidGrammar)));
                bstMock.Setup(b => b.RawEbnf).Returns(testValidTreeStream.BaseStream);

                var result = bstMock.Object.BuildSafeTree();

                result.Should().BeEmpty();
                var resultTree = bstMock.Object.MemoryTree;
                resultTree.Count.Should().Be(9);
                resultTree.First().RealElement.Name.Should().Be("nodeA");
                resultTree[1].RealElement.Name.Should().Be("nodeB");

                var nodeA = resultTree[0];
                var nodeB = resultTree[1];
                var nodeE = resultTree[3];

                nodeA.Children.First().ElementId.Should().Be(nodeB.ElementId);
                nodeA.Children[0].Children[2].ElementId.Should().Be(nodeE.ElementId);
            }

            [Fact]
            public void ValidTreeUidForAllNodes()
            {
                var testValidGrammar =
                    @"nodeA =  nodeB | nodeC;
nodeB = nodeD | nodeE;
nodeC = ""textC"";
nodeD = ""textD""; 
nodeE = ""textE""; ";
                var testValidTreeStream = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(testValidGrammar)));
                bstMock.Setup(b => b.RawEbnf).Returns(testValidTreeStream.BaseStream);

                var result = bstMock.Object.BuildSafeTree();

                result.Should().BeEmpty();
                var resultTree = bstMock.Object.MemoryTree;
                resultTree.Count.Should().Be(5);

                //we are checking that the links of unique ID from the nodeA all the way to the nodeE (going through B is compatible)

                var resultNodeA = resultTree.First();
                var resultNodeB = resultTree[1];
                var resultNodeE = resultTree[4];
                resultNodeA.RealElement.Name.Should().Be("nodeA");
                resultNodeB.RealElement.Name.Should().Be("nodeB");
                resultNodeE.RealElement.Name.Should().Be("nodeE");

                resultNodeA.Children.Should().Contain(resultNodeB);
                resultNodeA.Children.First().ElementId.Should().Be(resultNodeB.ElementId);

                resultNodeB.Children.Should().Contain(resultNodeE);
                resultNodeB.Children[1].ElementId.Should().Be(resultNodeE.ElementId);
            }
        }
    }
}