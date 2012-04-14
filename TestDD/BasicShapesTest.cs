using GBL.Repository.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.Resources;
using ResourceManager = GBL.Repository.Resources.ResourceManager;

namespace TestDD
{
    
    
    /// <summary>
    ///This is a test class for BasicShapesTest and is intended
    ///to contain all BasicShapesTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BasicShapesTest
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
        ///A test for BasicShapes Constructor
        ///</summary>
        [TestMethod()]
        public void BasicShapesConstructorTest()
        {
            BasicShapes target = new BasicShapes();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for BannerShape
        ///</summary>
        [TestMethod()]
        public void BannerShapeTest()
        {
            string actual;
            actual = BasicShapes.BannerShape;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Culture
        ///</summary>
        [TestMethod()]
        public void CultureTest()
        {
            CultureInfo expected = null; // TODO: Initialize to an appropriate value
            CultureInfo actual;
            BasicShapes.Culture = expected;
            actual = BasicShapes.Culture;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DamselShape
        ///</summary>
        [TestMethod()]
        public void DamselShapeTest()
        {
            string actual;
            actual = BasicShapes.DamselShape;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for EnglishShape
        ///</summary>
        [TestMethod()]
        public void EnglishShapeTest()
        {
            string actual;
            actual = BasicShapes.EnglishShape;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GermanShape
        ///</summary>
        [TestMethod()]
        public void GermanShapeTest()
        {
            string actual;
            actual = BasicShapes.GermanShape;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ItalianShape
        ///</summary>
        [TestMethod()]
        public void ItalianShapeTest()
        {
            string actual;
            actual = BasicShapes.ItalianShape;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for LadiesShape
        ///</summary>
        [TestMethod()]
        public void LadiesShapeTest()
        {
            string actual;
            actual = BasicShapes.LadiesShape;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ModernFrenchShape
        ///</summary>
        [TestMethod()]
        public void ModernFrenchShapeTest()
        {
            string actual;
            actual = BasicShapes.ModernFrenchShape;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for OldFrenchShape
        ///</summary>
        [TestMethod()]
        public void OldFrenchShapeTest()
        {
            string actual;
            actual = BasicShapes.OldFrenchShape;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for PolishShape
        ///</summary>
        [TestMethod()]
        public void PolishShapeTest()
        {
            string actual;
            actual = BasicShapes.PolishShape;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ResourceManager
        ///</summary>
        [TestMethod()]
        public void ResourceManagerTest()
        {
            System.Resources.ResourceManager actual;
            actual = BasicShapes.ResourceManager;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SpanishShape
        ///</summary>
        [TestMethod()]
        public void SpanishShapeTest()
        {
            string actual;
            actual = BasicShapes.SpanishShape;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SwissShape
        ///</summary>
        [TestMethod()]
        public void SwissShapeTest()
        {
            string actual;
            actual = BasicShapes.SwissShape;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
