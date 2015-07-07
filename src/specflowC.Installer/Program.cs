using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace specflowC.Installer
{
    internal class Program
    {
        private const string V_FLAG = "-v";
        private const string COMMON7_IDE = @"\common7\ide\";

        private static int Main(string[] args)
        {
            if (args.Length < 0)
            {
                PrintHelp();
                return -1;
            }

            var argList = new List<string>(args);
            int pos;
            
            if (argList.Contains(V_FLAG) && argList.Count >= 2)
            {
                pos = argList.IndexOf(V_FLAG);
                var version = argList[pos + 1];
                var path = GetVisualStudioInstallationPath(version);

                if (path == null)
                {
                    PrintVersionNotFound(version);
                    return -1;
                }

                path = path.ToLower();
                if (!path.Contains(COMMON7_IDE))
                {
                    PrintUnexpectedPath(path);
                }
                else
                {
                    path = path.Replace(COMMON7_IDE, string.Empty);
                }

                var projectItemsFolder = path + @"\VC\vcprojectitems\";
                if (!Directory.Exists(projectItemsFolder))
                {
                    PrintFolderNotFound(projectItemsFolder);
                    return -1;
                }
                var projectItemsFile = projectItemsFolder + @"VCProjectItems.vsdir";
                if (!File.Exists(projectItemsFile))
                {
                    PrintFileNotFound(projectItemsFile);
                    return -1;
                }

                CreateFeatureFileTemplate(projectItemsFolder);
                AppendToVCProjectItemsVsDir(projectItemsFile);
                InstallSpecFlowCParser(path);

                PrintInstallSuccess();
                return 0;
            }
            else
            {
                PrintHelp();
                return -1;
            }
        }

        private static void InstallSpecFlowCParser(string path)
        {
            const string SpecFlowCExe = "specflowC.Parser.exe";
            const string SpecFlowCDll = "TechTalk.SpecFlow.dll";

            var installDir = string.Format("{0}specflowC\\", Path.GetPathRoot(path));
            Directory.CreateDirectory(installDir);
            Console.WriteLine(string.Format("installed: {0}", installDir));
            File.Copy(SpecFlowCExe, string.Format("{0}{1}", installDir, SpecFlowCExe), true);
            Console.WriteLine(string.Format("installed: {0}", SpecFlowCExe));
            File.Copy(SpecFlowCDll, string.Format("{0}{1}", installDir, SpecFlowCDll), true);
            Console.WriteLine(string.Format("installed: {0}", SpecFlowCDll));
        }

        private static void CreateFeatureFileTemplate(string projectItemsFolder)
        {
            var nameSpace = Assembly.GetExecutingAssembly().GetName().Name;
            var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Format("{0}.MyFeature.feature", nameSpace));
            if (resourceStream == null)
                throw new Exception("Missing resource: MyFeature.feature");
            using (var reader = new StreamReader(resourceStream))
            {
                var contents = reader.ReadToEnd();
                var filename = projectItemsFolder + "MyFeature.feature";
                File.WriteAllText(filename, contents);
                Console.WriteLine(string.Format("installed: {0}", filename));
            }
        }

        private static void AppendToVCProjectItemsVsDir(string projectItemsFile)
        {
            var nameSpace = Assembly.GetExecutingAssembly().GetName().Name;
            var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Format("{0}.VCProjectItems_vsdir_append.txt", nameSpace));
            if (resourceStream == null)
                throw new Exception("Missing resource: VCProjectItems_vsdir_append.txt");

            bool needsNewLine = false;
            var readContents = File.ReadAllText(projectItemsFile);
            if (!readContents.EndsWith(Environment.NewLine))
            {
                needsNewLine = true;
            }

            using (var reader = new StreamReader(resourceStream))
            {
                string contents;
                if (needsNewLine)
                {
                    contents = Environment.NewLine + reader.ReadToEnd();
                }
                else
                {
                    contents = reader.ReadToEnd();
                }

                File.AppendAllText(projectItemsFile, contents);
                Console.WriteLine(string.Format("installed: {0}", projectItemsFile));
            }
        }

        private static string GetVisualStudioInstallationPath(string version)
        {
            string installationPath = null;
            if (Environment.Is64BitOperatingSystem)
            {
                installationPath = (string)Registry.GetValue(
                   string.Format("HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Microsoft\\VisualStudio\\{0}\\", version),
                   "InstallDir",
                   null);
            }
            else
            {
                installationPath = (string)Registry.GetValue(
                    string.Format("HKEY_LOCAL_MACHINE\\SOFTWARE  \\Microsoft\\VisualStudio\\10.0\\", version),
                    "InstallDir",
                    null);
            }
            return installationPath;
        }

        private static void PrintHelp()
        {
            Console.WriteLine(string.Format("usage:{0}\t-v <Studio Visual version number, ex: 10.0 , 11.0 , 12.0>{0}\t", Environment.NewLine));
        }

        private static void PrintVersionNotFound(string version)
        {
            Console.WriteLine(string.Format("error: version not found, {0}", version));
        }

        private static void PrintUnexpectedPath(string path)
        {
            Console.WriteLine(string.Format("warning: unexpected path, {0}", path));
            Console.WriteLine(string.Format("warning: attempting install as base path"));
        }

        private static void PrintFolderNotFound(string path)
        {
            Console.WriteLine(string.Format("error: Visual C++ folder not found, {0}", path));
        }

        private static void PrintFileNotFound(string path)
        {
            Console.WriteLine(string.Format("error: Visual C++ project items file not found, {0}", path));
        }

        private static void PrintInstallSuccess()
        {
            Console.WriteLine("Install completed");
        }
    }
}