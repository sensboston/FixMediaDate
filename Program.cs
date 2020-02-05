using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Linq;
using System.Runtime.Serialization;

namespace FixMediaDate
{
    public class Program
    {
        static readonly CultureInfo enUS = new CultureInfo("en-US");
        static bool error = false;
        static int filesCount = 0, errorsCount = 0, filesModified = 0;

        static void Main(string[] args)
        {
            var mediaPath = args.Length > 0 ? args[0] : AppDomain.CurrentDomain.BaseDirectory;
            if (Directory.Exists(mediaPath))
            {
                Console.WriteLine($"Processing all media files at {mediaPath}:");
                foreach (var fileName in Directory.GetFiles(mediaPath, "*.*", SearchOption.AllDirectories))
                {
                    var ext = Path.GetExtension(fileName).ToLower();
                    if (ext.Contains("jpg") || ext.Contains("jpeg") || ext.Contains("mp4"))
                    {
                        error = false;
                        var origDate = ExtractDateFromFileName(Path.GetFileName(fileName));
                        if (origDate > DateTime.MinValue)
                        {
                            filesCount++;
                            if (ext.Contains("jpg") || ext.Contains("jpeg")) SetExifDate(fileName, origDate);
                            SetFileDates(fileName, origDate);
                            Console.WriteLine(error ? $"error processing file {Path.GetFileName(fileName)}" :
                                $"new date {origDate.ToString("MM-dd-yyyy")} set to {Path.GetFileName(fileName)}");
                            if (error) errorsCount++;
                        }
                    }
                }
                Console.WriteLine($"{filesCount} files processed, {filesModified} files modified, {errorsCount} errors found");
            }
        }

        /// <summary>
        /// Tries to parse media file names (pictures and videos) and extract date and time
        /// from file name
        /// </summary>
        /// <param name="fileName">file name to process</param>
        /// <returns>Extracted DateTime, or DateTime.MinValue if fails</returns>
        public static DateTime ExtractDateFromFileName(string fileName)
        {
            var newDate = DateTime.MinValue;
            var match = Regex.Match(fileName, @"(?:.*)?([0-9]{4})([0-9]{2})([0-9]{2})(?:.?)(([0-9]{2})(?:.?)([0-9]{2})(?:.?)([0-9]{2})|(?:))");
            if (match.Success)
            {
                var strNewDate = $"{match.Groups[1].Value}{match.Groups[2].Value}{match.Groups[3].Value}{match.Groups[5].Value}{match.Groups[6].Value}{match.Groups[7].Value}";
                DateTime.TryParseExact(strNewDate, strNewDate.Length == 8 ? "yyyyMMdd" : "yyyyMMddHHmmss", enUS, DateTimeStyles.None, out newDate);
            }
            return newDate;
        }

        /// <summary>
        /// Set EXIF tags to the specific date and time (if not set)
        /// </summary>
        /// <param name="fileName">file to process</param>
        /// <param name="date">specific date and time</param>
        static void SetExifDate(string fileName, DateTime date)
        {
            bool tagsModified = false;
            var origExt = Path.GetExtension(fileName);
            var origFilename = fileName;
            using (var image = new Bitmap(fileName))
            {
                PropertyItem[] propItems = image.PropertyItems;

                // EXIF tag DateTimeDigitized
                var dateTakenProperty1 = propItems.Where(a => a.Id.ToString("x") == "9004").FirstOrDefault();
                if (dateTakenProperty1 == null)
                {
                    dateTakenProperty1 = (PropertyItem)FormatterServices.GetUninitializedObject(typeof(PropertyItem));
                    dateTakenProperty1.Id = 0x9004;
                    dateTakenProperty1.Type = 2;
                    dateTakenProperty1.Value = Encoding.UTF8.GetBytes(date.ToString("yyyy:MM:dd HH:mm:ss") + '\0');
                    image.SetPropertyItem(dateTakenProperty1);
                    tagsModified = true;
                }

                // EXIF tag DateTimeOriginal
                var dateTakenProperty2 = propItems.Where(a => a.Id.ToString("x") == "9003").FirstOrDefault();
                if (dateTakenProperty2 == null)
                {
                    dateTakenProperty2 = (PropertyItem)FormatterServices.GetUninitializedObject(typeof(PropertyItem));
                    dateTakenProperty2.Id = 0x9003;
                    dateTakenProperty2.Type = 2;
                    dateTakenProperty2.Value = Encoding.UTF8.GetBytes(date.ToString("yyyy:MM:dd HH:mm:ss") + '\0');
                    image.SetPropertyItem(dateTakenProperty2);
                    tagsModified = true;
                }

                if (tagsModified)
                {
                    fileName = Path.ChangeExtension(fileName, "xxx");
                    try 
                    { 
                        image.Save(fileName);
                    }
                    catch 
                    { 
                        error = true; 
                        tagsModified = false; 
                    }
                }
            }

            if (tagsModified)
            {
                File.Delete(origFilename);
                File.Move(fileName, origFilename);
                filesModified++;
            }
        }

        /// <summary>
        /// Sets all file dates (creation, write and access) to the specific date
        /// </summary>
        /// <param name="fileName">file to process</param>
        /// <param name="date">specific date and time</param>
        static void SetFileDates(string fileName, DateTime date)
        {
            if (!error)
            {
                try
                {
                    File.SetCreationTime(fileName, date);
                    File.SetLastWriteTime(fileName, date);
                    File.SetLastAccessTime(fileName, date);
                }
                catch 
                { 
                    error = true; 
                }
            }
        }
    }
}
