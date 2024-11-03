using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace WindowsOptimizer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("starting optimization...");

            // call clear temporary files
            ClearTemporaryFiles();

            // call manage startup
            ManageStartupPrograms();

            // call optimize memory
            OptimizeMemory();

            Console.WriteLine("optimization complete!");
            Console.ReadLine();
        }

        // method to clear temporary files
        static void ClearTemporaryFiles()
        {
            Console.WriteLine("clearing temporary files...");

            string[] tempPaths = {
                Path.GetTempPath(),
                Environment.GetFolderPath(Environment.SpecialFolder.InternetCache),
                Environment.GetFolderPath(Environment.SpecialFolder.History)
            };

            foreach (var path in tempPaths)
            {
                try
                {
                    var directoryInfo = new DirectoryInfo(path);
                    foreach (FileInfo file in directoryInfo.GetFiles())
                    {
                        try
                        {
                            file.Delete();
                        }
                        catch { }
                    }
                    foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
                    {
                        try
                        {
                            dir.Delete(true);
                        }
                        catch { }
                    }
                    Console.WriteLine($"cleared: {path}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"error clearing {path}: {ex.Message}");
                }
            }
        }

        // method to manage startup
        static void ManageStartupPrograms()
        {
            Console.WriteLine("managing startup programs...");

            string keyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyPath, true))
            {
                if (key == null)
                {
                    Console.WriteLine("startup key not found.");
                    return;
                }

                string[] startupPrograms = key.GetValueNames();
                foreach (string program in startupPrograms)
                {
                    Console.WriteLine($"startup Program: {program} - path: {key.GetValue(program)}");
                }
            }
        }

        // method to manage memory
        static void OptimizeMemory()
        {
            Console.WriteLine("optimizing memory usage...");

            try
            {
                Process currentProcess = Process.GetCurrentProcess();
                SetProcessWorkingSetSize(currentProcess.Handle, -1, -1);
                Console.WriteLine("memory optimization successful.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error optimizing memory: {ex.Message}");
            }
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool SetProcessWorkingSetSize(IntPtr proc, int min, int max);
    }
}