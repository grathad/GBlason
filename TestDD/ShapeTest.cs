using GBL.Repository.CoatOfArms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GBL.Repository.Resources;

namespace TestDD
{
    
    
    /// <summary>
    ///This is a test class for ShapeTest and is intended
    ///to contain all ShapeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ShapeTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for Shape Constructor
        ///</summary>
        [TestMethod()]
        public void ShapeConstructorTest()
        {
            Shape target = new Shape();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for Description
        ///</summary>
        [TestMethod()]
        public void DescriptionTest()
        {
            Shape target = new Shape(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.Description = expected;
            actual = target.Description;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Geometry
        ///</summary>
        [TestMethod()]
        public void GeometryTest()
        {
            Shape target = new Shape(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.Geometry = expected;
            actual = target.Geometry;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Identifier
        ///</summary>
        [TestMethod()]
        public void IdentifierTest()
        {
            Shape target = new Shape(); // TODO: Initialize to an appropriate value
            Guid expected = new Guid(); // TODO: Initialize to an appropriate value
            Guid actual;
            target.Identifier = expected;
            actual = target.Identifier;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Name
        ///</summary>
        [TestMethod()]
        public void NameTest()
        {
            Shape target = new Shape(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.Name = expected;
            actual = target.Name;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for TypeOfResource
        ///</summary>
        [TestMethod()]
        public void TypeOfResourceTest()
        {
            Shape target = new Shape(); // TODO: Initialize to an appropriate value
            ResourceType expected = new ResourceType(); // TODO: Initialize to an appropriate value
            ResourceType actual;
            target.TypeOfResource = expected;
            actual = target.TypeOfResource;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
