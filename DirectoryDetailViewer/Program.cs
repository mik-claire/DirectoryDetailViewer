using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using NLog;

namespace DirectoryDetailViewer
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            logger.Info("=== Activated! ===");

            Console.WriteLine("Please enter full directory path.");
            string directoryPath = Console.ReadLine();

            while (directoryPath.Trim() != string.Empty)
            {
                if (directoryPath == "exit")
                {
                    logger.Info("=== Exit... ===");
                    return;
                }

                logger.Info("Input: {0}", directoryPath);

                if (!existsDirectory(directoryPath))
                {
                    Console.WriteLine("The requested directory was NOT found.");
                    logger.Warn("The requested directory was NOT found.");
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
                logger.Info("Total File Size : {0}", formatedSize);
                logger.Info("Total File Count: {0} Files", count.ToString("#,0"));

                Console.WriteLine();
                Console.WriteLine("Please enter full directory path.");
                directoryPath = Console.ReadLine();
            }

            Console.WriteLine("Please enter any key to exit...");

            logger.Info("=== Exit... ===");
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
        private static string indent = "└";
        private static void addByteSize(string directoryPath)
        {
            if (!existsDirectory(directoryPath))
            {
                return;
            }

            DirectoryInfo root = new DirectoryInfo(directoryPath);
            logger.Debug("{0}{1}", indent, root.Name);
            var dis = root.GetDirectories();
            if (0 < dis.Length)
            {
                foreach(var di in dis)
                {
                    if (!existsDirectory(di.FullName))
                    {
                        continue;
                    }

                    indent = indent.Insert(0, "  ");
                    addByteSize(di.FullName);
                    indent = indent.Substring(2, indent.Length - 2);
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
                logger.Debug("{0}{1}: {2}", indent, fi.Name, formatSize(fi.Length));
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
