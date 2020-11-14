using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GuiEncryption
{
    class manage_rsa
    {

        private static RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
        private RSAParameters privateKey;
        private RSAParameters publickey;
        private String m_pubKey = ""; // put ur rsakey here but not recomended

        public manage_rsa()
        {
            privateKey = csp.ExportParameters(true);
            publickey = csp.ExportParameters(false);
        }

        public string Encrypting(string plaintext)
        {
            var data = Encoding.Unicode.GetBytes(plaintext);
            var cypher = csp.Encrypt(data, false);
            return Convert.ToBase64String(cypher);
        }
        public string Decrypting(string cyphertext)
        {
            var dataBytes = Convert.FromBase64String(cyphertext);
            var plaintext = csp.Decrypt(dataBytes, false);
            return Encoding.Unicode.GetString(plaintext);
        }
        public void ExportKeysAsXml(string pathToOutPubKey, string pathToOutPrivKey)
        {

            TextWriter writer = new StreamWriter(pathToOutPubKey);
            string publicKey = csp.ToXmlString(false);
            writer.Write(publicKey);
            writer.Close();

            writer = new StreamWriter(pathToOutPrivKey);
            string privateKey = csp.ToXmlString(true);
            writer.Write(privateKey);
            writer.Close();


        }
        public void ImportPrivKeysAsXml(string pathTotPrivKey)
        {
            TextReader reader = new StreamReader(pathTotPrivKey);
            string privateKey = reader.ReadToEnd();
            reader.Close();
            csp.FromXmlString(privateKey);
        }
        public int ImportPubKeysAsXml(string pathTotPubKey)
        {
            try
            {
                TextReader reader = new StreamReader(pathTotPubKey);
                string publicKey = reader.ReadToEnd();
                reader.Close();
                csp.FromXmlString(publicKey);

                return 1;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return 0;
            }

        }
        public string DecypherRSAKey(string pathToPrivKey, string pathToEncKey)
        {
            ImportPrivKeysAsXml(pathToPrivKey);
            string pass = Decrypting(File.ReadAllText(pathToEncKey).Replace("\r\n", ""));
            return pass;
        }
        public void CypherPassRSAIn(string pathOutToEncKey, string Password)
        {
            csp.FromXmlString(m_pubKey);
            var cypher = Encrypting(Password);
            File.WriteAllText(pathOutToEncKey, cypher);
        }

        public int CypherPassRSAExt(string pathOutToEncKey, string pathTotPubKey, string Password)
        {
            if (ImportPubKeysAsXml(pathTotPubKey) == 1)
            {
                
                try
                {
                    var cypher = Encrypting(Password);
                    File.WriteAllText(pathOutToEncKey, cypher);
                    return 1;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return 0;
                }
            }
            else
            {
                Console.WriteLine("can't open Pubkey file : " + pathOutToEncKey);
                return 0;
            }
        }   

    }
 }
