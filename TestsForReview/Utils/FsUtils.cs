using System;
using System.IO;

namespace TestsForReview.Utils
{
    public class FsUtils
    {
        public static string FindPath(string relativePath)
        {
            var current = AppDomain.CurrentDomain.BaseDirectory;
            while (true)
            {
                var testPath = Path.Combine(current, relativePath);
                if (File.Exists(testPath))
                    return testPath;
                if (Directory.Exists(testPath))
                    return testPath;

                current = Path.Combine(current, @"..\");
                var fullPath = Path.GetFullPath(current);
                if (fullPath.Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries).Length == 1) // C:\sdfsdf\wer.txt
                    return null;
            }
        }

        public static string GetPath(string relativePath)
        {
            var res = FindPath(relativePath);
            if (res == null)
                throw new Exception("File " + relativePath + " not found");
            return res;
        }
    }
}
