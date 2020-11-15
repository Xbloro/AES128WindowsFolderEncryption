

using System;
using System.IO;
namespace GuiEncryption
{
    class manage_zip
    {


        public manage_zip()
        {
            ;
        }
      
        public int ZipFolder(string pathFolder, string pathZip)
        {
            try
            {
                if (!File.Exists(pathZip))
                {
                    System.IO.Compression.ZipFile.CreateFromDirectory(pathFolder, pathZip);
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (System.Security.SecurityException ex)
            {
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }

        public int UnzipFolder(string pathFolder, string pathZip)
        {
            try
            {
                if (!File.Exists(pathZip))
                {
                    System.IO.Compression.ZipFile.ExtractToDirectory(pathFolder, pathZip);

                    return 1;
                }
                else
                {
                   // Directory.Delete(pathFolder);
                    System.IO.Compression.ZipFile.ExtractToDirectory(pathFolder, pathZip);
                    return 1;
                }

            }
            catch (System.Security.SecurityException ex)
            {
                Console.WriteLine(ex.ToString());
                return 0;
            }
         }

        public int ZipFile(string pathFolder, string pathZip)
        {
            try
            {
                if (!File.Exists(pathZip))
                {
                    File.SetAttributes(pathZip, FileAttributes.Normal);
                    System.IO.Compression.ZipFile.CreateFromDirectory(pathFolder, pathZip);
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }


        public void UnzipFile(string pathFolder, string pathZip)
        {
            try
            {
                File.SetAttributes(pathZip, FileAttributes.Normal);
                System.IO.Compression.ZipFile.ExtractToDirectory(pathZip, pathFolder);

            }
            catch (System.Security.SecurityException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
