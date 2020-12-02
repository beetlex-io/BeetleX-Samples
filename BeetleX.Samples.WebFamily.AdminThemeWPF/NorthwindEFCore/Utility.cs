using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyModel;

namespace NorthwindEFCoreSqlite
{
    public static class Utility
    {
        private const string NorthwindDatabase = "Northwind.sqlite";
        private const string NotebooksPath = "notebooks";
        private const string LibSqLitePclRaw = "SQLitePCLRaw.core.dll";
        private const string NativeLib = "libe_sqlite3.so";

        private static readonly string HomePath = Environment.GetEnvironmentVariable("HOME");
        private static readonly string NugetPath = Path.Combine(HomePath, ".nuget", "packages");

        public static void Init()
        {
            CopyDatabase();
            CopyNativeLib();
        }
        
        private static void CopyDatabase()
        {
            var dbFile = Directory.GetFiles(NugetPath, NorthwindDatabase, SearchOption.AllDirectories).First();
            var destFile = Path.Combine(HomePath, NotebooksPath, NorthwindDatabase);

            File.Copy(dbFile, destFile, true);
        }

        // SQLitePCLRaw is attempting to load the "libe_sqlite3.so" library using System.Runtime.InteropServices.NativeLibrary.
        // This doesn't account for the runtimes/$rid/native/*.dll layout.
        // This method will copy libe_sqlite3.so to the same path where SQLitePCLRaw is.
        private static void CopyNativeLib()
        {
            var nativeLibFiles = Directory.GetFiles(NugetPath, NativeLib, SearchOption.AllDirectories);
            var nativeLibFile = GetNativeLibFileLinux64(nativeLibFiles);
            var libSQLitePCLRawPath = Directory.GetFiles(NugetPath, LibSqLitePclRaw, SearchOption.AllDirectories).Single();
            var destFolder = Path.GetDirectoryName(libSQLitePCLRawPath);
            var destFile = Path.Combine(destFolder, NativeLib);

            File.Copy(nativeLibFile, destFile, true);
        }

        private static string GetNativeLibFileLinux64(string[] nativeLibFiles)
        {
            return nativeLibFiles.First(nativeLib => nativeLib.EndsWith($"/runtimes/linux-x64/native/{NativeLib}"));
        }

        // cannot use it, interactive is buggy and cannot find 'Microsoft.DotNet.PlatformAbstractions'
        private static string GetNativeLibFile(string[] nativeLibFiles)
        {
            var fallbacks = GetFallbacks();

            return nativeLibFiles.First(nativeLib => fallbacks.Any(f => nativeLib.EndsWith($"/runtimes/{f}/native/{NativeLib}")));
        }

        private static IReadOnlyList<string> GetFallbacks()
        {
            var rti = Microsoft.DotNet.PlatformAbstractions.RuntimeEnvironment.GetRuntimeIdentifier();
            var runtimeFallbacks = DependencyContext.Default.RuntimeGraph.Single(x => x.Runtime == rti);

            return runtimeFallbacks.Fallbacks;
        }
    }
}
