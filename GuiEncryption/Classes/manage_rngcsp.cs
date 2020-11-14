using System;
using System.Security.Cryptography;

namespace GuiEncryption
{
    class manage_rngcsp
    {
        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
        // Main method.

        public string GetAlea(int nbNumbers)
        {
            byte[] randomNumber = new byte[10];
            var i = 0;
            string st = "";
            do
            {
                // Fill the array with a random value.
                rngCsp.GetBytes(randomNumber);
                i++;
            }
            while (i < nbNumbers);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(randomNumber);

            foreach (var it in randomNumber)
            {
                
                st += it.ToString();
               
            }
            return st;
            
        }

    }
}
