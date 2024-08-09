using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace WebSocketMain.WebSocketMiddleware
{
    public class WebSocketConnectionManager
    {
        private readonly ConcurrentDictionary<Guid, WebSocket> _sockets = new ConcurrentDictionary<Guid, WebSocket>();

        public Guid AddSocket(WebSocket socket)
        {
            var socketId = Guid.NewGuid();
            _sockets.TryAdd(socketId, socket);
            return socketId;
        }

        public WebSocket? GetSocket(Guid socketId)
        {
            _sockets.TryGetValue(socketId, out var socket);
            return socket;
        }

        public IEnumerable<WebSocket> GetAllSockets()
        {
            return _sockets.Values;
        }

        public Guid? GetSocketId(WebSocket socket)
        {
            foreach (var (key, value) in _sockets)
            {
                if (value == socket)
                {
                    return key;
                }
            }
            return null;
        }

        public void RemoveSocket(Guid socketId)
        {
            _sockets.TryRemove(socketId, out _);
        }
    }
}
