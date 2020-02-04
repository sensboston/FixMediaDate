using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FixMediaDate
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void FileNamesWithDateTime()
        {
            Dictionary<string, DateTime> fileNamesWithDateTime = new Dictionary<string, DateTime>
            {
                { "WP_20131221_11_33_51_Pro.jpg",   new DateTime(2013,12,21, 11, 33, 51) },
                { "VID-20160110-WA0001.mp4",        new DateTime(2016,01,10) },
                { "20190131_202839.jpg",            new DateTime(2019,01,31, 20, 28, 39) },
                { "IMG-20160101-WA0001.jpg",        new DateTime(2016,01,01) },
                { "WP_20130622_001.jpg",            new DateTime(2013,06,22) },
                { "WP_20101011_195409.jpg",         new DateTime(2010,10,11, 19, 54, 09) },
                { "WP_20121123_210510Z.mp4",        new DateTime(2012,11,23, 21, 05, 10) },
                { "20170801_234906_HoloLens.jpg",   new DateTime(2017,08,01, 23, 49, 06) },
                
            };

            foreach (var entry in fileNamesWithDateTime)
                Assert.AreEqual(Program.ExtractDateFromFileName(entry.Key), entry.Value);
        }

        [TestMethod]
        public void FileNamesWithoutDateTime()
        {
            List<string> fileNamesWithoutDateTime = new List<string>
            {
                { "P1010001.JPG" },
                { "IMG_0221.JPG" },
                { "WP_000020.jpg" },
                { "4f4c0324c5acb.jpg"},
                { "548463_206623602805988_141662352_n.jpg"}
            };

            foreach (var entry in fileNamesWithoutDateTime)
                Assert.AreEqual(Program.ExtractDateFromFileName(entry), DateTime.MinValue);
        }
    }
}
