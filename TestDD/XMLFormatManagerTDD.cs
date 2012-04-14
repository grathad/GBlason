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
                Assert.Fail("Exception while trying to open and deserialize "+ FilePathTest);
            }
        }


        [TestMethod]
        public void GenerateGbrFile()
        {
            var shape = new Collection<Shape>
                            {
                                new Shape
                                    {
                                        Geometry = "M 0,0 L120,0 120,120 0,120 Z",
                                        Identifier = Guid.NewGuid(),
                                        TypeOfResource = ResourceType.Original,
                                        Name = "BannerShape"
                                    },
                                new Shape
                                    {
                                        Geometry = "M 60,0 L 110,60 L 60,120 L10,60 Z",
                                        Identifier = Guid.NewGuid(),
                                        TypeOfResource = ResourceType.Original,
                                        Name = "DamselShape"
                                    },
                                new Shape
                                    {
                                        Geometry =
                                            "M 0,150 H 120 Q 110,155 110,160 V 250 Q 110,260 100,260 H 80 Q 70,260 60,270 Q 50,260 40,260 H 20 Q 10,260 10,250 V 160 Q 10,155 0,150 Z",
                                        Identifier = Guid.NewGuid(),
                                        TypeOfResource = ResourceType.Original,
                                        Name = "EnglishShape"
                                    },
                                new Shape
                                    {
                                        Geometry =
                                            "M 86,0 c -17,0 -32,6 -45,19 c -13,-8 -34,-1 -36,15 c 2,-1 4,-2 6,-2  c 6,0 11,5 11,12 c 0,7 -5,12 -11,12 c -2,0 -4,0 -5,-1  c 0,17 -6,25 -6,41 c 0,32 30,62 60,62 c 30,0 58,-28 58,-61 c 0,-25 -19,-37 -19,-62 c 0,-24 10,-33 10,-33 c -8,-2 -17,-2 -24,-2 Z",
                                        Identifier = Guid.NewGuid(),
                                        TypeOfResource = ResourceType.Original,
                                        Name = "GermanShape"
                                    },
                                new Shape
                                    {
                                        Geometry =
                                            "M 34.346021 3.7599543 c -9.82466 0 -17.27687 19.5402597 -33.90783066 21.6768297 0 53.54101 27.00433066 62.46452 27.00433066 88.769976 0 5.37705 -1.10449 6.46272 -1.10449 12.49516 0 15.607 9.78488 36.18165 28.66194 36.18165 18.87711 0 28.662 -20.57465 28.662 -36.18165 0 -6.03244 -1.10449 -7.11811 -1.10449 -12.49516 0 -26.305456 27.004329 -35.228966 27.004329 -88.769976 C 92.931722 23.301084 85.478631 3.7599543 75.653981 3.7599543 l -20.65401 0 -20.65395 0 0 0 Z",
                                        Identifier = Guid.NewGuid(),
                                        TypeOfResource = ResourceType.Original,
                                        Name = "ItalianShape"
                                    },
                                new Shape
                                    {
                                        Geometry = "M 0,60 A 50,60 180 1 1 100,60 A 50,60 180 1 1 0,60",
                                        Identifier = Guid.NewGuid(),
                                        TypeOfResource = ResourceType.Original,
                                        Name = "LadiesShape"
                                    },
                                new Shape
                                    {
                                        Geometry =
                                            "M 300,0 H 420 V 100 A 10,10 90 0 1 410,110 H 380 Q 370,110 360,120 Q 350,110 340,110 H310 A 10,10 90 0 1 300,100 Z",
                                        Identifier = Guid.NewGuid(),
                                        TypeOfResource = ResourceType.Original,
                                        Name = "ModernFrenchShape"
                                    },
                                new Shape
                                    {
                                        Geometry = "M 0,0 H 120 V 30 Q 120,90 60,120 Q 0,90 0,30 Z",
                                        Identifier = Guid.NewGuid(),
                                        TypeOfResource = ResourceType.Original,
                                        Name = "OldFrenchShape"
                                    },
                                new Shape
                                    {
                                        Geometry =
                                            "M 52.189212 0 c -10.254994 0 -16.649394 8.92073 -32.596428 11.42888 0.01288 9.67009 -3.932115 19.1825 -9.191804 25.4488 0 0 10.246156 4.91713 10.246156 15.79821 0 16.42972 -16.9764711 16.45817 -16.9764711 16.45817 0 0 -2.86249329 6.04605 -2.86249329 18.28685 0 28.01804 18.61424839 49.19324 32.74751339 55.72374 14.133255 6.5303 22.267497 9.3067 26.469632 15.239 4.202126 -5.9323 12.285743 -8.7087 26.418999 -15.239 14.133264 -6.5305 32.747514 -27.7083 32.747514 -55.72374 0 -12.2408 -2.86249 -18.28685 -2.86249 -18.28685 0 0 -16.976476 -0.0284 -16.976476 -16.45817 0 -10.88108 10.246156 -15.79821 10.246156 -15.79821 -5.2605 -6.26873 -9.20386 -15.77871 -9.19181 -25.4488 -15.947832 -2.50895 -22.342233 -11.42888 -32.596422 -11.42888 -5.847141 0 -7.785471 3.04781 -7.785471 3.04781 0 0 -1.988963 -3.04781 -7.836105 -3.04781 Z",
                                        Identifier = Guid.NewGuid(),
                                        TypeOfResource = ResourceType.Original,
                                        Name = "PolishShape"
                                    },
                                new Shape
                                    {
                                        Geometry = "M 0,0 H 120 V 60 A 60,60 90 0 1 60,120 A 60,60 90 0 1 0,60 Z",
                                        Identifier = Guid.NewGuid(),
                                        TypeOfResource = ResourceType.Original,
                                        Name = "SpanishShape"
                                    },
                                new Shape
                                    {
                                        Geometry =
                                            "M 0,0 A 40,60 90 0 0 60,0 A 40,60 90 0 0 120,0 Q 120,100 60,120 M 0,0 Q 0,100 60,120",
                                        Identifier = Guid.NewGuid(),
                                        TypeOfResource = ResourceType.Original,
                                        Name = "SwissShape"
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
