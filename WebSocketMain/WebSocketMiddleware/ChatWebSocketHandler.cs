using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;

namespace WebSocketMain.WebSocketMiddleware
{
    public class ChatWebSocketHandler
    {
        private readonly WebSocketConnectionManager _connectionManager;
        private readonly ILogger<ChatWebSocketHandler> _logger;

        public ChatWebSocketHandler(WebSocketConnectionManager connectionManager, ILogger<ChatWebSocketHandler> logger)
        {
            _connectionManager = connectionManager;
            _logger = logger;
        }

        public async Task HandleWebSocketAsync(WebSocket webSocket,HttpContext context)
        {
            var connectionId = context.Request.Query["id"];
            var stringBuilder = new StringBuilder();
            _connectionManager.AddSocket(webSocket, connectionId);
            await BroadcastMessageAsync($"{connectionId} is connected");
            try
            {
                while (webSocket.State == WebSocketState.Open)
                {
                    var buffer = WebSocket.CreateServerBuffer(1024 * 4);
                    var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var message = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                        var messageParts = message.Split(':', 3);
                        if (messageParts.Length  == 3)
                        {
                            var recipientId = messageParts[0].ToString();
                            var receiverId = messageParts[1];
                            if (!string.IsNullOrEmpty(message))
                            {
                                var getRecepientSocketId= _connectionManager.GetSocket(recipientId);
                                if (getRecepientSocketId!= null && getRecepientSocketId.State == WebSocketState.Open)
                                {
                                    _logger.LogInformation($"Received message from ID {connectionId}: {messageParts[2]}");
                                    await getRecepientSocketId.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(messageParts[2].ToString())), result.MessageType, result.EndOfMessage, CancellationToken.None);
                                    
                                    stringBuilder.Clear();
                                }
                                else
                                {
                                    _logger.LogInformation($"Received message from ID {connectionId}: {messageParts[2]}");
                                    await getRecepientSocketId.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(messageParts[2].ToString())), result.MessageType, result.EndOfMessage, CancellationToken.None);
                                    await BroadcastMessageAsync($"{receiverId} : offline");
                                }
                                
                            }
                        }
                        

                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by the server", CancellationToken.None);
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex.Message, ex);
            }
            finally
            {
                if(webSocket != null)
                {
                    webSocket.Dispose();
                }
            }
            
        }

        private async Task BroadcastMessageAsync(string message)
        {
            foreach (var socket in _connectionManager.GetAllSockets())
            {
                if (socket.State == WebSocketState.Open)
                {
                    var bytes = Encoding.UTF8.GetBytes(message);
                    var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);
                    await socket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }
}
