using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.IO.Compression;
using System.Text;

namespace CloudDataService.Helper
{
    public class ZipHelper
    {
        public static void ZipStream(MemoryStream InStream, Stream OutStream)
        {
            ZipOutputStream zipstream = new ZipOutputStream(OutStream);
            ZipEntry entry = new ZipEntry("order.txt");
            zipstream.PutNextEntry(entry);
            zipstream.Write(InStream.ToArray(), 0, Convert.ToInt32(InStream.Length));
            zipstream.Close();
        }

        public static void UnzipStream(Stream InStream, MemoryStream OutStream)
        {
            using (ZipInputStream zipInStream = new ZipInputStream(InStream))
            {
                ZipEntry entry = zipInStream.GetNextEntry();
                int size;
                do
                {
                    size = zipInStream.ReadByte();
                    if (size != -1)
                    {
                        OutStream.WriteByte((byte)size);
                    }
                } while (size != -1);
            }
            InStream.Close();
        }

        public static string UnzipStream(Stream ZipStream)
        {
            string outstring = string.Empty;
            using (ZipInputStream zipInStream = new ZipInputStream(ZipStream))
            {
                ZipEntry entry = zipInStream.GetNextEntry();
                using (StreamReader reader = new StreamReader(zipInStream))
                {
                    outstring = reader.ReadToEnd();
                }
            }

            return outstring;
        }

        public static byte[] ZipString(string InString)
        {
            //MemoryStream ms = new MemoryStream();
            //StreamWriter swriter = new StreamWriter(ms);
            //swriter.WriteLine(InString);
            //swriter.Close();

            //MemoryStream unzipms = new MemoryStream(ms.ToArray());
            //MemoryStream OutStream = new MemoryStream();
            //ZipOutputStream zipstream = new ZipOutputStream(OutStream);
            //ZipEntry entry = new ZipEntry("data.txt");
            //zipstream.PutNextEntry(entry);
            //zipstream.Write(unzipms.ToArray(), 0, Convert.ToInt32(unzipms.Length));
            //zipstream.Close();
            //byte[] t = OutStream.ToArray();

            MemoryStream zipStream = new MemoryStream();

            using (ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Create))
            {

                ZipArchiveEntry readmeEntry = archive.CreateEntry("data.txt");
                using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                {
                    writer.WriteLine(InString);
                }
            }

            return zipStream.ToArray();
        }
        public static byte[] ZipStringV2(string InString)
        {
            byte[] data = Encoding.UTF8.GetBytes(InString);

            using (MemoryStream OutStream = new MemoryStream())
            {
                using (ZipOutputStream zipstream = new ZipOutputStream(OutStream))
                {
                    ZipEntry entry = new ZipEntry("data.txt");
                    zipstream.PutNextEntry(entry);
                    zipstream.Write(data, 0, Convert.ToInt32(data.Length));
                    zipstream.Close();
                }
                return OutStream.ToArray();
            }
        }
    }
}