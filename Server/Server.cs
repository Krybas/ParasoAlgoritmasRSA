using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace Server
{
    public partial class Server : Form
    {
        public Server()
        {
            InitializeComponent();
        }
        private void Server_Load(object sender, EventArgs e)
        {

        }

        private TcpListener _server;

        private void button1_Click(object sender, EventArgs e)
        {
            StartServer();
        }

        private void StartServer()
        {
            _server = new TcpListener(IPAddress.Any, 8888);
            _server.Start();

            var listeningThread = new Thread(ListenForClients);
            listeningThread.IsBackground = true;
            listeningThread.Start();

            AddMessageToTextBox("Server started...");
        }

        private void ListenForClients()
        {
            while (true)
            {
                var client = _server.AcceptTcpClient();
                var clientThread = new Thread(HandleClient);
                clientThread.IsBackground = true;
                clientThread.Start(client);
            }
        }

        private void HandleClient(object tcpClient)
        {
            using (var client = (TcpClient)tcpClient)
            using (var stream = client.GetStream())
            {
                var messageLengthBytes = ReadBytes(stream, 4);
                var messageLength = BitConverter.ToInt32(messageLengthBytes, 0);

                var messageBytes = ReadBytes(stream, messageLength);
                var message = Encoding.UTF8.GetString(messageBytes);

                var signatureLengthBytes = ReadBytes(stream, 4);
                var signatureLength = BitConverter.ToInt32(signatureLengthBytes, 0);

                var signatureBytes = ReadBytes(stream, signatureLength);

                var publicKeyModulusLengthBytes = ReadBytes(stream, 4);
                var publicKeyModulusLength = BitConverter.ToInt32(publicKeyModulusLengthBytes, 0);

                var publicKeyModulusBytes = ReadBytes(stream, publicKeyModulusLength);

                var publicKeyExponentLengthBytes = ReadBytes(stream, 4);
                var publicKeyExponentLength = BitConverter.ToInt32(publicKeyExponentLengthBytes, 0);

                var publicKeyExponentBytes = ReadBytes(stream, publicKeyExponentLength);

                var publicKey = new RSAParameters
                {
                    Modulus = publicKeyModulusBytes,
                    Exponent = publicKeyExponentBytes
                };

                var isValidSignature = VerifySignature(Encoding.UTF8.GetBytes(message), signatureBytes, publicKey);

                if (isValidSignature)
                {
                    AddMessageToTextBox($"Message: {message}");
                    AddMessageToTextBox("Signature is valid.");
                }
                else
                {
                    AddMessageToTextBox("Invalid signature!");
                }
            }
        }

        private byte[] ReadBytes(NetworkStream stream, int length)
        {
            var buffer = new byte[length];
            var bytesRead = 0;

            while (bytesRead < length)
            {
                var chunk = stream.Read(buffer, bytesRead, length - bytesRead);
                if (chunk == 0)
                {
                    throw new EndOfStreamException();
                }

                bytesRead += chunk;
            }

            return buffer;
        }

        private bool VerifySignature(byte[] data, byte[] signature, RSAParameters publicKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                try
                {
                    rsa.ImportParameters(publicKey);
                    return rsa.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                }
                catch (CryptographicException)
                {
                    return false;
                }
            }
        }

        private void AddMessageToTextBox(string message)
        {
            if (txtServer.InvokeRequired)
            {
                txtServer.Invoke(new MethodInvoker(delegate
                {
                    txtServer.AppendText(message + Environment.NewLine);
                }));
            }
            else
            {
                txtServer.AppendText(message + Environment.NewLine);
            }
        }
    }
}
