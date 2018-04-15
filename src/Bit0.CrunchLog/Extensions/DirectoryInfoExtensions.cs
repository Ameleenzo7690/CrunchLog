﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Bit0.CrunchLog.Extensions
{
    public static class DirectoryInfoExtensions
    {
        public static DirectoryInfo CombineDirPath(this DirectoryInfo dir, params String[] paths)
        {
            var pathList = new List<String>(new[] { dir.FullName });
            pathList.AddRange(paths);

            return new DirectoryInfo(Path.Combine(pathList.ToArray()).NormalizePath());
        }
        
        public static FileInfo CombineFilePath(this DirectoryInfo dir, String extension, params String[] paths)
        {
            var pathList = new List<String>(new[] { dir.FullName });
            pathList.AddRange(paths);

            return new FileInfo(Path.ChangeExtension(dir.CombineDirPath(pathList.ToArray()).FullName, extension).NormalizePath());
        }

        public static FileInfo CombineFilePath(this DirectoryInfo dir, String file)
        {
            return dir.CombineFilePath(Path.GetExtension(file), Path.GetFileNameWithoutExtension(file));
        }

        public static String NormalizePath(this String path)
        {
            return path.Replace('\\', '/').Replace('/', Path.DirectorySeparatorChar);
        }


        public static void Copy(this DirectoryInfo dir, DirectoryInfo destDir, Boolean copySubDirs = true)
        {
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    message: "Source directory does not exist or could not be found: "
                    + dir.FullName);
            }

            var dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!destDir.Exists)
            {
                destDir.Create();
            }

            // Get the files in the directory and copy them to the new location.
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                var temppath = destDir.CombineDirPath(file.Name);
                file.CopyTo(destFileName: temppath.FullName, overwrite: true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (!copySubDirs)
            {
                return;
            }
            foreach (var subdir in dirs)
            {
                var temppath = destDir.CombineDirPath(subdir.Name);
                subdir.Copy(temppath);
            }
        }
    }
}
