using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CollectN.Core
{
    public class GraphiteTcpClient : IDisposable
    {
        public string Hostname { get; private set; }
        public int Port { get; private set; }
        public string KeyPrefix { get; private set; }

        private readonly TcpClient _tcpClient;

        public GraphiteTcpClient(string hostname, int port = 2003, string keyPrefix = null)
        {
            Hostname = hostname;
            Port = port;
            KeyPrefix = keyPrefix;

            _tcpClient = new TcpClient(Hostname, Port);
        }

        public void Send(string path, int value)
        {
            Send(path, value, DateTime.Now);
        }

        public void Send(string path, int value, DateTime timeStamp)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(KeyPrefix))
                {
                    path = KeyPrefix + "." + path;
                }

                var message = new PlaintextMessage(path, value, timeStamp).ToByteArray();

                _tcpClient.GetStream().Write(message, 0, message.Length);
            }
            catch
            {
                throw;
            }
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            if (_tcpClient != null)
            {
                _tcpClient.Close();
            }
        }

        #endregion
    }

    public class PlaintextMessage
    {
        public string Path { get; private set; }
        public int Value { get; private set; }
        public long Timestamp { get; private set; }

        public PlaintextMessage(string path, int value, DateTime timestamp)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            Path = path;
            Value = value;
            Timestamp = timestamp.ToUnixTime();
        }

        public byte[] ToByteArray()
        {
            var line = string.Format("{0} {1} {2}\n", Path, Value, Timestamp);

            return Encoding.UTF8.GetBytes(line);
        }
    }

    public static class DateTimeExtensions
    {
        private static readonly DateTime EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime().ToUniversalTime();

        public static long ToUnixTime(this DateTime self)
        {
            return (long)(self.ToUniversalTime() - EPOCH).TotalSeconds;
        }
    }
}
