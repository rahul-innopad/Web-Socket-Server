using WebSocketMain.WebSocketMiddleware;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSingleton<ChatWebSocketHandler>();
        builder.Services.AddSingleton<WebSocketConnectionManager>();

        var app = builder.Build();

        app.UseWebSockets();
        //app.MapWebSocketManager("/chat", app.ApplicationServices.GetService<ChatWebSocketHandler>());

        app.Map("/chat", async context =>
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                var handler = context.RequestServices.GetRequiredService<ChatWebSocketHandler>();
                await handler.HandleWebSocketAsync(webSocket, context);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        });

        app.Run();
    }
}