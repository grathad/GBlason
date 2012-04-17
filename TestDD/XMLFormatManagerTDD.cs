using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using FormatManager;
using FormatManager.Serializer;
using GBL.Repository.CoatOfArms;
using GBL.Repository.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace TestDD
{
    [TestClass]
    public class XmlFormatManagerTdd
    {
        #region tools

        private const String FilePathTest = "testGBS.gbs";

        #endregion

        [TestMethod]
        public void GenerateGbsFile()
        {
            try
            {
                var coaGenerator = new CoatOfArmsTest();
                var coa = coaGenerator.ShapeTest();
                var saver = new GbsManager();
                saver.SaveAsGbs(coa, FilePathTest);
                Assert.IsTrue(File.Exists(FilePathTest), "the file have not been created");
            }
            catch (Exception ex)
            {
                Assert.Fail("Exception while saving : " + ex.Message);
            }
        }

        [TestMethod]
        public void LoadGbsFile()
        {
            try
            {
                var loader = new GbsManager();
                var coaResult = loader.LoadGbsFile(FilePathTest);
                Assert.IsNotNull(coaResult, "The file deserialization failed, the result is null");
            }
            catch (Exception)
            {
                Assert.Fail("Exception while trying to open and deserialize " + FilePathTest);
            }
        }


        [TestMethod]
        public void GenerateGbrFile()
        {
            var shape = new Collection<Shape>
                            {
                                new Shape
                                    {
                                        Geometry = "M 0,0 L200,0 200,200 0,200 Z",
                                        Identifier = Guid.NewGuid(),
                                        TypeOfResource = ResourceType.Original,
                                        Name = "BannerShape",
                                        PathHeight = 200,
                                        PathWidth = 200
                                    },
                                new Shape
                                    {
                                        Geometry = "M 100,0 L 200,100 L 100,200 L0,100 Z",
                                        Identifier = Guid.NewGuid(),
                                        TypeOfResource = ResourceType.Original,
                                        Name = "DamselShape",
                                        PathHeight = 200,
                                        PathWidth = 200
                                    },
                                new Shape
                                    {
                                        Geometry = "M 0,0 H 200 Q 180,10 180,20 V 160 A 20,20 90 0 1 160,180 H 120 Q 100,180 100,200 M 0,0 Q 20,10 20,20 V160 A 20,20 90 0 0 40,180 H 80 Q 100,180 100,200",
                                        Identifier = Guid.NewGuid(),
                                        TypeOfResource = ResourceType.Original,
                                        Name = "EnglishShape",
                                        PathHeight = 200,
                                        PathWidth = 200
                                    },
                                new Shape
                                    {
                                        Geometry ="M 10,50 C 30,10 70,20 80,30 C 100,0 170,0 190,10 C 150,40 200,80 200,120 C 200,150 190,200 100,200 C 10,200 0,150 0,120 C 5,90 10,80 10,70 C 45,90 45,30 10,50",
                                        Identifier = Guid.NewGuid(),
                                        TypeOfResource = ResourceType.Original,
                                        Name = "GermanShape",
                                        PathHeight = 200,
                                        PathWidth = 200
                                    },
                                new Shape
                                    {
                                        Geometry = "M 40 0 H 160 S 165,0 170,5 S 178,20 200,30 C 200,70 200,90 170,130 C 160,140 160,160 160,160 Q 160,200 100,200 M 40,0 S 35,0 30,5 S 22,20 0,30 C 0,70 0,90 30,130 C 40,140 40,160 40,160 Q 40,200 100,200",
                                        Identifier = Guid.NewGuid(),
                                        TypeOfResource = ResourceType.Original,
                                        Name = "ItalianShape",
                                        PathHeight = 200,
                                        PathWidth = 200
                                    },
                                new Shape
                                    {
                                        Geometry = "M 0,100 A 100,100 180 1 1 200,100 A 100,100 180 1 1 0,100",
                                        Identifier = Guid.NewGuid(),
                                        TypeOfResource = ResourceType.Original,
                                        Name = "LadiesShape",
                                        PathHeight = 200,
                                        PathWidth = 200
                                    },
                                new Shape
                                    {
                                        Geometry = "M 0,0 H 200 V 160 A 20,20 90 0 1 180,180 H 120 Q 100,180 100,200 M 0,0 V 160 A 20,20 90 0 0 20,180 H 80 Q 100,180 100,200",
                                        Identifier = Guid.NewGuid(),
                                        TypeOfResource = ResourceType.Original,
                                        Name = "ModernFrenchShape",
                                        PathHeight = 200,
                                        PathWidth = 200
                                    },
                                new Shape
                                    {
                                        Geometry = "M 0,0 H 200 V 70 Q 200,150 100,200 M 0,0 V 70 Q 0,150 100,200",
                                        Identifier = Guid.NewGuid(),
                                        TypeOfResource = ResourceType.Original,
                                        Name = "OldFrenchShape",
                                        PathHeight = 200,
                                        PathWidth = 200
                                    },
                                new Shape
                                    {
                                        Geometry = "M 30,15 C 50,15 70,0 80,0 Q 95,0 100,5 Q 105,0 120,0 C 130,0 150,15 170,15 Q 170,40 180,50 C 165,60 165,90 195,90 C 220,200 110,180 100,200 C 90,180 -20,200 5,90 C 35,90 35,60 20,50 Q 30,40 30,15",
                                        Identifier = Guid.NewGuid(),
                                        TypeOfResource = ResourceType.Original,
                                        Name = "PolishShape",
                                        PathHeight = 200,
                                        PathWidth = 200
                                    },
                                new Shape
                                    {
                                        Geometry = "M 0,0 H 200 V 100 A 100,100 90 0 1 100,200 A 100,100 90 0 1 0,100 Z",
                                        Identifier = Guid.NewGuid(),
                                        TypeOfResource = ResourceType.Original,
                                        Name = "SpanishShape",
                                        PathHeight = 200,
                                        PathWidth = 200
                                    },
                                new Shape
                                    {
                                        Geometry =
                                            "M 0,0 A 80,100 90 0 0 100,0 A 80,100 90 0 0 200,0 Q 200,150 100,200 M 0,0 Q 0,150 100,200",
                                        Identifier = Guid.NewGuid(),
                                        TypeOfResource = ResourceType.Original,
                                        Name = "SwissShape",
                                        PathHeight = 200,
                                        PathWidth = 200
                                    }
                            };
            GbrFormat enveloppe = new GbrFormat();
            enveloppe.Shapes = new List<Shape>(shape);
            Stream tampon = new MemoryStream();
            XmlManager.Serialize(enveloppe, ref tampon);

            tampon.Position = 0;
            StreamReader reader = new StreamReader(tampon);
            string text = reader.ReadToEnd();
            tampon.Dispose();
        }


    }
}
