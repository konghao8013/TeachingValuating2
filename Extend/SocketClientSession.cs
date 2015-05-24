using System;
using System.Net;
using System.Net.Sockets;
using System.Linq;
namespace SilverSocketClient
{
    public class SocketClientSession
    {
        readonly int port;
        readonly string _ip;
        /// <summary>
        /// 设置缓冲区大小
        /// </summary>
        int length = 1024;
        Socket socket;
        public SocketClientSession(string ip, int port)
        {
            this.port = port;
            _ip = ip;
        }
        public void Start()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            SocketAsyncEventArgs socketArgs = new SocketAsyncEventArgs();
            socketArgs.RemoteEndPoint = new DnsEndPoint(_ip, port);
            socketArgs.Completed += socketArgs_Completed;
            socket.ConnectAsync(socketArgs);

        }

        void socketArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == 0)
            {

                var bs = new byte[length];
                e.SetBuffer(bs, 0, bs.Length);
                e.Completed -= socketArgs_Completed;
                e.Completed += e_Completed;
                socket.ReceiveAsync(e);
            }
            else
            {
                if (Error != null)
                {
                    Error(this);
                }
            }
        }

        void e_Completed(object sender, SocketAsyncEventArgs e)
        {
            if (Receive != null && e.SocketError == 0)
            {
                var bs = e.Buffer.Where(a => a > 0).ToArray();
                Receive(bs);
                socket.ReceiveAsync(e);
            }
            else if (e.SocketError != 0 && Error != null)
            {
                Error(this);
            }
        }
        /// <summary>
        /// 向服务端发送消息
        /// </summary>
        /// <param name="bytes"></param>
        public void Send(byte[] bytes)
        {
            var arg = new SocketAsyncEventArgs();
            arg.Completed += arg_Completed;
            arg.SetBuffer(bytes, 0, bytes.Length);
            socket.SendAsync(arg);
        }

        void arg_Completed(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError != 0 && Error != null)
            {
                Error(this);
            }
        }

        public event Action<SocketClientSession> Error;
        public event Action<byte[]> Receive;

    }
}
