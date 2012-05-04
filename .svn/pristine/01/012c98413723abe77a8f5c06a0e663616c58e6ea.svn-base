using GBL.Repository.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GBL.Repository.CoatOfArms;
using System.Collections.Generic;
using System.Globalization;

namespace TestDD
{
    
    
    /// <summary>
    ///This is a test class for ResourceManagerTest and is intended
    ///to contain all ResourceManagerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ResourceManagerTest
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
        ///A test for ResourceManager Constructor
        ///</summary>
        [TestMethod()]
        public void ResourceManagerConstructorTest()
        {
            ResourceManager target = new ResourceManager();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for AddNewShapes
        ///</summary>
        [TestMethod()]
        public void AddNewShapesTest()
        {
            ResourceManager target = new ResourceManager(); // TODO: Initialize to an appropriate value
            IEnumerable<Shape> toAdd = null; // TODO: Initialize to an appropriate value
            target.AddNewShapes(toAdd);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for UpdateResourceFromGbr
        ///</summary>
        [TestMethod()]
        public void UpdateResourceFromGbrTest()
        {
            ResourceManager target = new ResourceManager(); // TODO: Initialize to an appropriate value
            string gbrFilePath = string.Empty; // TODO: Initialize to an appropriate value
            target.UpdateResourceFromGbr(gbrFilePath);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for UpdateResourceFromGbr
        ///</summary>
        [TestMethod()]
        public void UpdateResourceFromGbrTest1()
        {
            ResourceManager target = new ResourceManager(); // TODO: Initialize to an appropriate value
            string gbrFilePath = string.Empty; // TODO: Initialize to an appropriate value
            CultureInfo culture = null; // TODO: Initialize to an appropriate value
            target.UpdateResourceFromGbr(gbrFilePath, culture);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for UpdateResourceFromWebService
        ///</summary>
        [TestMethod()]
        public void UpdateResourceFromWebServiceTest()
        {
            ResourceManager target = new ResourceManager(); // TODO: Initialize to an appropriate value
            target.UpdateResourceFromWebService();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Shapes
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Repository.dll")]
        public void ShapesTest()
        {
            ResourceManager_Accessor target = new ResourceManager_Accessor(); // TODO: Initialize to an appropriate value
            IEnumerable<Shape> expected = null; // TODO: Initialize to an appropriate value
            IEnumerable<Shape> actual;
            target.Shapes = expected;
            actual = target.Shapes;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
