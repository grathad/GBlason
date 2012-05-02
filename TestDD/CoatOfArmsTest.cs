using GBL.Repository.CoatOfArms;
using GBL.Repository.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestDD
{
    
    
    /// <summary>
    ///This is a test class for CoatOfArmsTest and is intended
    ///to contain all CoatOfArmsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CoatOfArmsTest
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
        ///A test for CoatOfArms Constructor
        ///</summary>
        [TestMethod()]
        public void CoatOfArmsConstructorTest()
        {
            var target = new CoatOfArms();
            //Version 0.9.0 no particular comportment on the constructor side
            Assert.IsNotNull(target, "Unable to construct a coat of armss");
        }

        /// <summary>
        ///A test for Shape
        ///</summary>
        [TestMethod]
        public CoatOfArms ShapeTest()
        {
            var target = new CoatOfArms
                             {
                                 Shape =
                                     new Shape
                                         {
                                             Geometry = "M 0,0 L 100,0 100,100 0,100 Z",
                                             Identifier = Guid.NewGuid(),
                                             TypeOfResource = ResourceType.Custom,
                                             Name = "Test"
                                         }
                             };
            Assert.IsNotNull(target, "Unable to construct a coat of arms with a custom shape");
            return target;
        }
    }
}
