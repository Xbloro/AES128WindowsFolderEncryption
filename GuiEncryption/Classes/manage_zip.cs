using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace GuiEncryption
{
    class Manage_Zip
    {
        /// <summary>
        /// This class provide a simple way to manage to zip things.
        /// </summary>
        public Manage_Zip()
        {
            ;
        }

        /// <summary>
        /// Zip a folder and its children.
        /// </summary>
        /// <returns>
        /// type Int, 0 if success 1 if failled
        /// </returns>
        /// <param name="pathToFolderToZip">A string containing the full path of the folder which will be zipped.</param>
        /// <param name="pathToZipArchive">A string containing the full path of the output file.</param>
        public int ZipFolder(string pathToFolderToZip, string pathToZipArchive)
        {
            try
            {
                ZipFile.CreateFromDirectory(pathToFolderToZip, pathToZipArchive);
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return 1;
            }
        }

        /// <summary>
        /// UnZip a folder and its children.
        /// </summary>
        /// <returns>
        /// type Int, 0 if success 1 if failled
        /// </returns>
        /// <param name="pathToOutFolder">A string containing the full path of the folder where content will be extracted.</param>
        /// <param name="pathToZipArchive">A string containing the full path of the archive zip file.</param>
        public int UnzipFolder(string pathToZipArchive, string pathToOutFolder)
        {
            try
            {
                var extractDirectoryName = Path.GetFileNameWithoutExtension(pathToZipArchive);
                var exctractDirectoryPath = Path.Combine(pathToOutFolder, extractDirectoryName);
                var i = 0;
                while (Directory.Exists(exctractDirectoryPath))
                {
                    exctractDirectoryPath += i.ToString();
                    i++;
                }
                Directory.CreateDirectory(exctractDirectoryPath);
                ZipFile.ExtractToDirectory(pathToZipArchive, exctractDirectoryPath);
                return 0;   
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return 1;
            }
        }


        /// <summary>
        /// Zip a folder and its children. It set the file attribute "normal" to the file
        /// </summary>
        /// <returns>
        /// type Int, 0 if success 1 if failled
        /// </returns>
        /// <param name="pathToFolderToZip">A string containing the full path of the folder which will be zipped.</param>
        /// <param name="pathToZipArchive">A string containing the full path of the output file.</param>
        public int ZipFolderSetAttribute(string pathToFolderToZip, string pathToZipArchive)
        {
            try
            {
                    File.SetAttributes(pathToZipArchive, FileAttributes.Normal);
                    ZipFile.CreateFromDirectory(pathToFolderToZip, pathToZipArchive);
                    return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return 1;
            }
        }

        /// <summary>
        /// UnZip a folder and its children.It set the file attribute "normal" to the file
        /// </summary>
        /// <returns>
        /// type Int, 0 if success 1 if failled
        /// </returns>
        /// <param name="pathToOutFolder">A string containing the full path of the folder where content will be extracted.</param>
        /// <param name="pathToZipArchive">A string containing the full path of the archive zip file.</param>
        public int UnZipFolderSetAttribute(string pathToZipArchive, string pathToOutFolder)
        {
            try
            {
                File.SetAttributes(pathToZipArchive, FileAttributes.Normal);
                var extractDirectoryName = Path.GetFileNameWithoutExtension(pathToZipArchive);
                var exctractDirectoryPath = Path.Combine(pathToOutFolder, extractDirectoryName);
                var i = 0;
                while (Directory.Exists(exctractDirectoryPath))
                {
                    exctractDirectoryPath = exctractDirectoryPath + i.ToString();
                    i++;
                }
                Directory.CreateDirectory(exctractDirectoryPath);
                ZipFile.ExtractToDirectory(pathToZipArchive, exctractDirectoryPath);
                return 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return 1;
            }
        }

        /// <summary>
        /// Add a file to a zip archive.
        /// </summary>
        /// <returns>
        /// type Int, 0 if success 1 if failled
        /// </returns>
        /// <param name="pathToFileToAdd">A string containing the full path of the file which will be add to the archive.</param>
        /// <param name="pathToZipArchive">A string containing the full path of the archive zip file.</param>
        public int AddFileToZipArchive(string pathToFileToAdd,string pathToZipArchive)
        {
            try
            {
                FileStream zipToOpen = new FileStream(pathToZipArchive, FileMode.Open);
                ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update);
                var fileInfo = new FileInfo(pathToFileToAdd);
                archive.CreateEntryFromFile(fileInfo.FullName, fileInfo.Name);
                return 0;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return 1;
            }

        }

        /// <summary>
        /// Add multiple files to a zip archive.
        /// </summary>
        /// <returns>
        /// type Int, 0 if success 1 if failled
        /// </returns>
        /// <param name="ListPathFileToAdd">A tab of string containing the full path of the files which will be add to the archive.</param>
        /// <param name="pathToZipArchive">A string containing the full path of the archive zip file.</param>
        public int AddFilesToZipArchive(string[] ListPathFileToAdd, string pathToZipArchive)
        {
            try
            {
                if (ListPathFileToAdd == null || ListPathFileToAdd.Length == 0)
                {
                    return 0;
                }

                FileStream zipToOpen = new FileStream(pathToZipArchive, FileMode.Open);
                ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update);
                foreach(string file in ListPathFileToAdd)
                {
                    var fileInfo = new FileInfo(file);
                    archive.CreateEntryFromFile(fileInfo.FullName, fileInfo.Name);
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return 1;
            }

        }

    }


}

