using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebSocketMain.WebSocketMiddleware;

namespace WebSocketMain.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ChatWebSocketHandler _webSocketHandler;

        public ChatController(ChatWebSocketHandler webSocketHandler)
        {
            _webSocketHandler = webSocketHandler;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await _webSocketHandler.HandleWebSocketAsync(webSocket,HttpContext);
            }
            else
            {
                return BadRequest("WebSocket is not supported.");
            }

            return Ok();
        }
    }
}
