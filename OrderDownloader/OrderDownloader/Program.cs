using MyCBlobstorageFileHandler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderDownloader
{
    class Program
    {
        static string _localPathName;
        static string _remotePathName = "Auftraege";
        static string _container = "livingkitzbuehel";
        static bool _pause;
        static string _prx = ".XML";
        static BlobstorageFileHandler blobhandler;
        static List<string> _files = new List<string>();

        static void Main(string[] args)
        {
            if (!ParseArgs(args))
            {
                PrintUsage();
                Environment.Exit(-1);
            }

            if (!Directory.Exists(_localPathName))
            {
                Console.WriteLine("Please ensure that the local target directory exists.");
                Environment.Exit(-1);
            }

            blobhandler = new BlobstorageFileHandler("myconvenoftp", "ZZiN0Tl+eejzQc9ymh/vXBTziGa5n68/OVrLmGpA6FIN+Xm61yDVeadquGdSesRIoRtBXmUG586b9RjCERs5hg==", _container);

            fillFilesArchiv();

            DirectoryInfo dir = new DirectoryInfo(_localPathName);
            string fileName = string.Empty;
            string filePath = string.Empty;
            foreach (var f in _files)
            {
                fileName = f.Split('/').Last();

                filePath = Path.Combine(_localPathName, fileName);

                File.WriteAllBytes(filePath, blobhandler.DownloadFile(f));
                Console.WriteLine(fileName);
                blobhandler.MoveFileToArchive(f);
            }
        }
        private static bool ParseArgs(string[] args)
        {

            for (int index = 0; index < args.Length; ++index)
            {
                if (args[index].ToLower() == "-path")
                    _localPathName = args[index + 1];              
            }
            return !string.IsNullOrEmpty(_localPathName);
        }

        private static void fillFilesArchiv()
        {
            _files.Clear();
            _files.AddRange(blobhandler.FileList(_prx, _remotePathName));
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage: OrderDownloader -path [LocalPath]");
            Console.ReadLine();
        }
    }
}
