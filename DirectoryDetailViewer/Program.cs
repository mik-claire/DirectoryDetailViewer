using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DirectoryDetailViewer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter full directory path.");
            string directoryPath = Console.ReadLine();

            while (directoryPath.Trim() != string.Empty)
            {
                if (directoryPath == "exit")
                {
                    return;
                }

                if (!existsDirectory(directoryPath))
                {
                    Console.WriteLine("The requested directory was NOT found.");
                    Console.WriteLine();
                    Console.WriteLine("Please enter full directory path.");
                    directoryPath = Console.ReadLine();
                    continue;
                }

                size = 0;
                count = 0;
                addByteSize(directoryPath);

                string formatedSize = formatSize(size);
                if (formatedSize.Substring(formatedSize.Length - 1, 1) == "B")
                {
                    formatedSize += string.Format(" ({0} bytes)", size.ToString("#,0"));
                }

                Console.WriteLine("Total File Size : {0}", formatedSize);
                Console.WriteLine("Total File Count: {0} Files", count.ToString("#,0"));

                Console.WriteLine();
                Console.WriteLine("Please enter full directory path.");
                directoryPath = Console.ReadLine();
            }

            Console.WriteLine("Please enter any key to exit...");
            Console.ReadLine();
        }

        private static bool existsDirectory(string directoryPath)
        {
            string path = directoryPath.Trim();
            if (path == string.Empty)
            {
                return false;
            }

            if (!Directory.Exists(path))
            {
                return false;
            }

            return true;
        }

        private static bool existsFile(string filePath)
        {
            string path = filePath.Trim();
            if (path == string.Empty)
            {
                return false;
            }

            if (!File.Exists(path))
            {
                return false;
            }

            return true;
        }

        private static long size = 0;
        private static int count = 0;
        private static void addByteSize(string directoryPath)
        {
            if (!existsDirectory(directoryPath))
            {
                return;
            }

            DirectoryInfo root = new DirectoryInfo(directoryPath);

            var dis = root.GetDirectories();
            if (0 < dis.Length)
            {
                foreach(var di in dis)
                {
                    if (!existsDirectory(di.FullName))
                    {
                        continue;
                    }

                    addByteSize(di.FullName);
                }
            }

            var fis = root.GetFiles();
            foreach(var fi in fis)
            {
                if (!existsFile(fi.FullName))
                {
                    continue;
                }

                size += fi.Length;
                count++;
            }
        }

        private static string formatSize(long size)
        {
            // TeraByte
            if (size >= Math.Pow(2, 40)) 
            {
                return Math.Round(size / Math.Pow(2, 40), 3).ToString("#,0") + " TB";
            }

            // GigaByte
            if (size >= Math.Pow(2, 30))
            {
                return Math.Round(size / Math.Pow(2, 30), 3).ToString("#,0") + " GB";
            }

            // MegaByte
            if (size >= Math.Pow(2, 20)) 
            {
                return Math.Round(size / Math.Pow(2, 20), 3).ToString("#,0") + " MB";
            }

            // KiloByte
            if (size >= Math.Pow(2, 10)) 
            {
                return Math.Round(size / Math.Pow(2, 10), 3).ToString("#,0") + " KB";
            }

            //byte
            return size.ToString("#,0") + " Bytes";
        }
    }
}
