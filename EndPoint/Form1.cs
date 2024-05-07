using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace EndPoint
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            string message = txtClient.Text;
            SendSignedMessageToServer(message);
        }
        static void SendSignedMessageToServer(string message)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    byte[] signature = SignData(Encoding.UTF8.GetBytes(message), rsa.ExportParameters(true));
                    string publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());

                    // Prisijungiame prie serverio
                    using (TcpClient client = new TcpClient("127.0.0.1", 8888))
                    using (NetworkStream stream = client.GetStream())
                    {
                        byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                        byte[] publicKeyBytes = Encoding.UTF8.GetBytes(publicKey);
                        stream.Write(messageBytes, 0, messageBytes.Length);
                        stream.Write(signature, 0, signature.Length);
                        stream.Write(publicKeyBytes, 0, publicKeyBytes.Length);
                    }
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }

        static byte[] SignData(byte[] data, RSAParameters privateKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(privateKey);
                return rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }
    }
}
