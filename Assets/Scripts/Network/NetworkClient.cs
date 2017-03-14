using System;
using System.Linq;
using System.Linq.Expressions;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Shared.DataPackages.Client;
using Shared.DataPackages.Server;
using UnityEngine;

namespace Client.Network
{
    public sealed class NetworkClient : MonoBehaviour
    {
        [SerializeField] private Game _game;
        [SerializeField] private string _host;
        [SerializeField] private int _port;

        private TcpClient _tcpClient;

        private NetworkStream _stream;

        private bool _isNotMoreToRead;
        private readonly byte[] _buffer = new byte[1024];
        private int _offset = 0;
        private int _count = 1024;
        private readonly ServerPackageFactory _factory = new ServerPackageFactory();

        private void OnDisable()
        {
            if (_tcpClient != null && _tcpClient.Connected)
                Disconnect();
        }

        public void Connect()
        {
            _tcpClient = new TcpClient();
            _tcpClient.BeginConnect(IPAddress.Parse(_host), _port, ConnectionCallback, _tcpClient);
        }

        private void ConnectionCallback(IAsyncResult ar)
        {
            Debug.Log("Connected");
            _tcpClient.EndConnect(ar);
            _stream = _tcpClient.GetStream();

            var pingPackage = new PingPackage {Value = 10};
            SendPackage(pingPackage);

            _isNotMoreToRead = true;
            ReceivePackage();
        }

        public void Disconnect()
        {
            SendPackage(new ExitFromRoomPackage());
            _isNotMoreToRead = false;
//            _tcpClient.Close();
        }

        public void SendPackage(ClientPackage package)
        {
            var data = package.ToByteArray();
            try
            {
                _stream.BeginWrite(data, 0, data.Length, SendCallback, package);
            }
            catch (NullReferenceException e)
            {
                
            }
            catch (IOException e)
            {
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            var package = (ClientPackage) ar .AsyncState;

            if (package.Type == ClientPackageType.ExitFromRoom)
            {
                _isNotMoreToRead = false;
                _tcpClient.Close();
            }
        }

        public void ReceivePackage()
        {

            AsyncCallback callback = null;
            callback = ar =>
            {
                int bytesRead = _stream.EndRead(ar);

                var data = new byte[bytesRead];
                Array.Copy(_buffer, data, bytesRead);

                if (_isNotMoreToRead)
                {
                    _stream.BeginRead(_buffer, _offset, _count, callback, null);
                }

                using (var stream = new MemoryStream(data))
                {
                    ServerPackage package = null;
                    do
                    {
                        package = _factory.GetNextPackage(stream);
                        if (package != null)
                        {
                            _game.SetPackage(package);
                        }
                    } while (package != null);
                }
            };

            _stream.BeginRead(_buffer, _offset, _count, callback, null);
        }
    }
}
