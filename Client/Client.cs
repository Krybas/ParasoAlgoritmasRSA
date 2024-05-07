using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;

namespace Client
{
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
        }

        private void Client_Load(object sender, EventArgs e)
        {

        }

        private void buttonClient_Click(object sender, EventArgs e)
        {
            string message = txtClient.Text;
            SendSignedMessageToServer(message);
        }

        private void SendSignedMessageToServer(string message)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                var privateKey = rsa.ExportParameters(true);
                var publicKey = rsa.ExportParameters(false);

                var signature = SignData(Encoding.UTF8.GetBytes(message), privateKey);

                using (var client = new TcpClient("127.0.0.1", 8888))
                using (var stream = client.GetStream())
                {
                    var messageBytes = Encoding.UTF8.GetBytes(message);
                    var signatureBytes = signature;
                    var publicKeyModulusBytes = publicKey.Modulus;
                    var publicKeyExponentBytes = publicKey.Exponent;

                    WriteBytes(stream, BitConverter.GetBytes(messageBytes.Length));
                    WriteBytes(stream, messageBytes);

                    WriteBytes(stream, BitConverter.GetBytes(signatureBytes.Length));
                    WriteBytes(stream, signatureBytes);

                    WriteBytes(stream, BitConverter.GetBytes(publicKeyModulusBytes.Length));
                    WriteBytes(stream, publicKeyModulusBytes);

                    WriteBytes(stream, BitConverter.GetBytes(publicKeyExponentBytes.Length));
                    WriteBytes(stream, publicKeyExponentBytes);
                }
            }
        }

        private byte[] SignData(byte[] data, RSAParameters privateKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(privateKey);
                return rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }

        private void WriteBytes(NetworkStream stream, byte[] data)
        {
            stream.Write(data, 0, data.Length);
        }
    }
}
