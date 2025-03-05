using DiscordRPC;

namespace ProjectTerra.Framework.Utils;

public static class DiscordRPCManager {
    private static DiscordRpcClient? _client;

    public static void Initialize(string clientid="1342530650720309369"){
        _client = new DiscordRpcClient(clientid);
        _client.Initialize();
        _client.SetPresence(new RichPresence(){Details = "Project Terra",State = "Playtesting the game",Timestamps = Timestamps.Now,});
    }

    public static void Shutdown(){
        if (_client == null) return;
        _client.Dispose();
        _client = null;
    }
}