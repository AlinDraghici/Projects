using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aplicatia2
{
    public partial class Form1 : Form
    {

        private byte[] key;
        private byte[] iv;

        public Form1()
        {
            InitializeComponent();
        }

        private void Encrypt_Click(object sender, EventArgs e)
        {
            string plainText = textBox1.Text;
            string encryptedText = EncryptTextAES(plainText);

            textBox2.Text = encryptedText;
        }

        private void Decrypt_Click(object sender, EventArgs e)
        {
            string encryptedText = textBox1.Text;
            string decryptedText = DecryptTextAES(textBox2.Text);

            textBox3.Text = decryptedText;
        }

        private string EncryptTextAES(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.GenerateKey();
                aesAlg.GenerateIV();

                key = aesAlg.Key;
                iv = aesAlg.IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(key, iv);

                byte[] encryptedBytes;
                using (var msEncrypt = new System.IO.MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                        csEncrypt.Write(plainBytes, 0, plainBytes.Length);
                    }
                    encryptedBytes = msEncrypt.ToArray();
                }

                string encryptedText = Convert.ToBase64String(encryptedBytes);
                return encryptedText;
            }
        }

        private string DecryptTextAES(string encryptedText)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(key, iv);

                byte[] decryptedBytes;
                using (var msDecrypt = new System.IO.MemoryStream(encryptedBytes))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var decryptedStream = new System.IO.MemoryStream())
                        {
                            int bytesRead;
                            byte[] buffer = new byte[1024];
                            while ((bytesRead = csDecrypt.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                decryptedStream.Write(buffer, 0, bytesRead);
                            }
                            decryptedBytes = decryptedStream.ToArray();
                        }
                    }
                }

                string decryptedText = Encoding.UTF8.GetString(decryptedBytes);
                return decryptedText;
            }
        }

    }

}