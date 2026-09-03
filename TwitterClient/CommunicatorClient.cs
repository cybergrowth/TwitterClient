using TwitterClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TwitterClient
{
    /// <summary>
    /// handles all communication with the server and encryption
    /// </summary>
    class CommunicatorClient
    {
        internal const int RsaKeySize = 2048;

        TcpClient tcpClient;

        RSA rSA;
        Aes aes;

        bool isNotClosed;

        
        
        /// <summary>
        /// creates a secure encrypted connection
        /// </summary>
        /// <param name="tcpClient"></param>
        public CommunicatorClient(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            rSA = RSA.Create(RsaKeySize);

            RSAParameters privateKey = rSA.ExportParameters(true);
            RSAParameters publicKey = rSA.ExportParameters(false);

            //send server public key
            byte[] data = Encoding.UTF8.GetBytes(rSA.ToXmlString(false));
            tcpClient.GetStream().Write(data, 0, data.Length);

            //aes
            aes = Aes.Create();
            aes.Padding = PaddingMode.PKCS7;

            data = new byte[tcpClient.ReceiveBufferSize];
            tcpClient.GetStream().Read(data, 0, Convert.ToInt32(tcpClient.ReceiveBufferSize));
            string aesData = Encoding.UTF8.GetString(rSA.Decrypt(data, RSAEncryptionPadding.OaepSHA1));
            aes.Key = Convert.FromBase64String(aesData.Split('|')[0]);
            aes.IV = Convert.FromBase64String(aesData.Split('|')[1]);

            isNotClosed = true;

        }

        /// <summary>
        /// get the tcpClient instance used by the class to communicate with the server
        /// </summary>
        /// <returns></returns>
        public TcpClient GetTcpClient()
        {
            return tcpClient;
        }

        /// <summary>
        /// writing bytes to the server
        /// </summary>
        /// <param name="data"></param>
        public void WriteBytes(Byte[] data)
        {
            byte[] lenBytes = new byte[4];
            byte[] encrypted = aes.CreateEncryptor().TransformFinalBlock(data, 0, data.Length);
            BitConverter.GetBytes(encrypted.Length).CopyTo(lenBytes, 0);
            try
            {
                tcpClient.GetStream().Write(lenBytes, 0, 4);
                tcpClient.GetStream().Write(encrypted, 0, encrypted.Length);
            }
            catch (Exception ex)
            {
                //server closed
                TwitterClientMain.SwitchToLoginPage();
                this.Close();
            }
        }

        /// <summary>
        /// reading bytes from the server
        /// </summary>
        /// <returns></returns>
        public byte[] ReadBytes()
        {
            byte[] lenBytes = new byte[4];
            int bytesRead = 0;

            // Keep reading until we actually have 4 bytes
            while (bytesRead < 4)
            {
                int n = tcpClient.GetStream().Read(lenBytes, bytesRead, 4 - bytesRead);
                if (n == 0)
                {
                    //client closed
                    this.Close();
                    return null;
                }
                bytesRead += n;
            }


            int num = BitConverter.ToInt32(lenBytes, 0);

            if (num == 0)
            {
                //client closed
                this.Close();
                return null;
            }

            byte[] data = new byte[num];
            bytesRead = 0;
            while (bytesRead < num)
            {
                int n1 = tcpClient.GetStream().Read(data, bytesRead, num - bytesRead);
                if (n1 == 0)
                {
                    //client closed
                    this.Close();
                    return null;
                }
                bytesRead += n1;
            }



            byte[] decrypted = aes.CreateDecryptor().TransformFinalBlock(data, 0, data.Length);
            return decrypted;
        }

        /// <summary>
        /// start reading from the server
        /// </summary>
        public void StartReading()
        {
            new Thread(() => { read(); }).Start();
            HandleServerResponse.Setup(this, TwitterClientMain.GetMainPage());
        }

        /// <summary>
        /// constantly read from the server
        /// </summary>
        private void read()
        {
            string request;
            while (isNotClosed)
            {
                try
                {
                    byte[] data = ReadBytes();
                    if (data != null)
                    {
                        request = Encoding.UTF8.GetString(data);
                        HandleServerResponse.Handle(request);
                    }
                }
                catch (InvalidOperationException e)
                {
                    Close();
                }
                catch (Exception e)
                {
                    if (tcpClient.Connected)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    else
                    {
                        Close();
                    }
                }
            }
        }

        /// <summary>
        /// close the connection
        /// </summary>
        public void Close()
        {
            isNotClosed = false;
            tcpClient.Close();
            TwitterClientMain.SwitchToLoginPage();
        }

    }
}
