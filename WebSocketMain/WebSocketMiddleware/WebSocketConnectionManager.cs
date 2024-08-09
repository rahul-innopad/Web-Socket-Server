using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace WebSocketMain.WebSocketMiddleware
{
    public class WebSocketConnectionManager
    {
        private readonly ConcurrentDictionary<Guid, WebSocket> _sockets = new ConcurrentDictionary<Guid, WebSocket>();

        public Guid AddSocket(WebSocket socket,string socketId)
        {
            var id = Guid.Parse(socketId);
            _sockets.TryAdd(id, socket);
            return id;
        }

        public WebSocket? GetSocket(string socketId)
        {
            var socketIdInString = Guid.Parse(socketId);
            _sockets.TryGetValue(socketIdInString, out var socket);
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
