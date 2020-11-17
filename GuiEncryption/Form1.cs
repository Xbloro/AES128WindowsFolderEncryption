using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace GuiEncryption
{
    public partial class Form1 : Form
    {
        static manage_rsa rs = new manage_rsa();
        public Form1()
        {
            InitializeComponent();
        }

        static string GetFile(string title, string ext, string filter)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = title;
                ofd.Filter = filter;
                ofd.DefaultExt = ext;
                DialogResult result = ofd.ShowDialog();

                if (result == DialogResult.OK && !String.IsNullOrWhiteSpace(ofd.FileName))
                {
                    return ofd.FileName;
                }
                else if (result == DialogResult.Cancel)
                {
                    return null;
                }
                else
                {
                    MessageBox.Show("Error opening file : " + ofd.FileName, "ERROR");
                    return null;
                }
            }
        }
        static string GetDir(string title)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = title;
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !String.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    return fbd.SelectedPath;
                }
                else if (result == DialogResult.Cancel)
                {
                    return null;
                }
                else
                {
                    MessageBox.Show("Error opening file : " + fbd.SelectedPath, "ERROR");
                    return null;
                }
            }
        }
        public string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
        static string GenerateAleaKey()
        {
            manage_rngcsp cp = new manage_rngcsp();
            var alea = cp.GetAlea(1500);
            return alea;
        }
        static void ZipFolder(string pathFolder, string pathZipped)
        {
            manage_zip zipped = new manage_zip();
            int res;
            if (File.Exists(pathZipped))
            {
                DialogResult dialogResult = MessageBox.Show("Zipped file already exist, should we delet it ?", "Oh NOOOOOO", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        File.Delete(pathZipped);
                        res = zipped.ZipFolder(pathFolder, pathZipped);
                        if (res == 0) { MessageBox.Show("error zipping folder", "error"); };
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("error occured : " + ex.ToString());
                        return;
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    MessageBox.Show("Ok abording");
                    return;
                }
            }
            else
            {
                try
                {
                    Console.WriteLine("file does no exist");
                    res = zipped.ZipFolder(pathFolder, pathZipped);
                    if (res == 0) { MessageBox.Show("error zipping folder", "error"); };
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    MessageBox.Show("error occured : " + ex.ToString());
                    return;
                }
            }

        }
        static void UnzipData(string pathfile, string pathOut)
        {
            manage_zip ziped = new manage_zip();
            ziped.UnzipFile(pathOut, pathfile);
        }
        static void AESEncryptData(string password, string pathFile)
        {
            manage_aes aesChiff = new manage_aes();
            aesChiff.EncryptFile(pathFile, password);
        }
        static void AESDecryptData(string pathToFile, string pathDecrypted, string password)
        {
            manage_aes aesChiff = new manage_aes();
            aesChiff.DecryptFile(pathToFile, pathDecrypted, password);
        }
        static int RsaEncryptExtKey(string password, string pathToPubKey, string pathToOutCyperedKey)
        {

            if (rs.CypherPassRSAExt(pathToOutCyperedKey, pathToPubKey, password) == 1)
            {
                Console.WriteLine("Key correctly ciphered");
                return 1;
            }
            else
            {
                Console.WriteLine("Error ciphering key");
                return 0;
            }
        }
        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnRSA_Click(object sender, EventArgs e)
        {
            var outKeys = GetDir("Select location to save RSA Keys");
            if (outKeys == null) { return; };
            manage_rsa rs = new manage_rsa();
            rs.ExportKeysAsXml(Path.Combine(outKeys, "pub.xml"), Path.Combine(outKeys, "priv.xml"));
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            string dirToEncrypt = GetDir("Select folder you want to encrypt");
            if (dirToEncrypt == null) { return; }
            string pathToRSAPubKey = GetFile("Get Public Key file", "xml", "XML file|*.xml");
            if (pathToRSAPubKey == null) { return; }
            string pathToOut = GetDir("Select folder you want to save the encrypted File");
            if (pathToOut == null) { return; }

            var nameDest = Path.GetFileName(dirToEncrypt);
            string zippedFile = Path.Combine(pathToOut, nameDest) + ".zip";
            Console.WriteLine(zippedFile);
            Console.WriteLine(dirToEncrypt);
            ZipFolder(dirToEncrypt, zippedFile);
            var aesKey = GenerateAleaKey();
            AESEncryptData(aesKey, zippedFile);
            RsaEncryptExtKey(aesKey, pathToRSAPubKey, Path.Combine(pathToOut, nameDest + "_cypheredAeskey.txt"));
            File.Delete(zippedFile);
            MessageBox.Show("finito", "Yeah");
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            
        
            
            //manage_rsa rsa = new manage_rsa();
            string pathToCypherAesKey = GetFile("Get Cypher AES KEY", "txt", "TXT file|*.txt");
            if (pathToCypherAesKey == null) { return; }
            string pathToPrivKey = GetFile("Get Private Key file", "xml", "XML file|*.xml");
            if (pathToPrivKey == null) { return; }
            string pathFile = GetFile("Get AES file", "aes", "AES files|*.aes"); // on recup le fichier aes
            if (pathFile == null) { return; }
            string pathToOut = GetDir("Where do you want to extract data ?");
            if (pathToOut == null) { return; }


            

            string DecypheredPass = rs.DecypherRSAKey(pathToPrivKey, pathToCypherAesKey);
            var cle = RemoveSpecialCharacters(DecypheredPass);
            string pathToDecryptedFile = Path.Combine(pathToOut, Path.GetFileNameWithoutExtension(pathFile));
            string pathToOutFolder = Path.Combine(pathToOut, Path.GetFileNameWithoutExtension(pathToDecryptedFile));
            Directory.CreateDirectory(pathToOutFolder);
  
            AESDecryptData(pathFile, pathToDecryptedFile, cle); // on le déchiffre la ou on l'a trouvé
            string pathNewDir = Path.Combine(pathToOut, Path.GetFileNameWithoutExtension(pathToDecryptedFile));
            UnzipData(pathToDecryptedFile, pathNewDir);  //on dezip
            Console.WriteLine(pathToDecryptedFile);
            File.Delete(pathToDecryptedFile);
            MessageBox.Show("finito","Yeah");

        }


    }
}
