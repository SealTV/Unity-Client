using System.IO;
using System.Net;
using System.Net.Sockets;
using Shared.Packages.Client;
using UnityEngine;

namespace Client
{
    public class Test : MonoBehaviour
    {
        [SerializeField]
        private PingPackage _pingPackage;
        [SerializeField]
        private bool _isConnected;
        [SerializeField]
        private int value;



        private Socket s;

        private void Start()
        {
            _pingPackage = new PingPackage {Value = 10};
            Debug.Log(_pingPackage.Value);
            Debug.Log(_pingPackage.Type);
        }

        private void Update()
        {
            _isConnected = s != null && s.Connected;

            if (Input.GetKeyUp(KeyCode.A))
            {
                Connect();
            }

            if (Input.GetKeyUp(KeyCode.T) && _isConnected)
            {
                SendPacket();
            }

        }

        private NetworkStream _stream;

        private void Connect()
        {
            if (s != null)
                s.Close();

            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            s.Connect(IPAddress.Parse("127.0.0.1"), 3990);
            Debug.Log("Connected new!");

            _stream = new NetworkStream(s, FileAccess.Write);
           
        }

        private void SendPacket()
        {
            PingPackage pingPackage = new PingPackage
            {
                Value = value
            };

            var arr = pingPackage.ToByteArray();
            _stream.Write(arr, 0, arr.Length);
            _stream.BeginWrite(arr, 0, arr.Length,
                ar =>
                {
                    _stream.EndWrite(ar);
                    Debug.Log("Date Sendet!");
                },
                _stream);
        }
    }
}
