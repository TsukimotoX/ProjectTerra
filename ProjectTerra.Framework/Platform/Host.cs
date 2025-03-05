using SDL;
using OpenTK.Graphics.OpenGL;
using ProjectTerra.Framework.Graphics;
using ProjectTerra.Framework.Input;
using ProjectTerra.Framework.Core.SceneManagement;

namespace ProjectTerra.Framework.Platform;

public unsafe class Host {
    private Window _window;
    private Renderer _renderer = Renderer.Instance;
    private InputManager _inputManager = InputManager.Instance;
    private SceneManager _sceneManager = SceneManager.Instance;
    public static string platform = CheckPlatform();
    private string coh = "[ProjectTerra.Framework.Platform:Host]";

    /// <summary>
    /// Initializes a new instance of Host. Hosts are cross-platform, so you should only have one of these. It contains everything you need: Window, Renderer,
    /// InputManager, SceneManager, etc.
    /// </summary>
    public Host(){
        _window = new Window();
        _renderer.Initialize(_window);
    }

    /// <summary>
    /// Runs the game loop. Also the entrance point of everything running inside the framework.
    /// </summary>
    public void Run(){
        if (platform == "Unknown") throw new Exception($"{coh} Uh oh, ProjectTerra doesn't support your platform! Make sure you're playing on Android, iOS, Linux, MacOS, or Windows!");
        Console.WriteLine($"{coh} Running on platform: {platform}");

        while(true){
            var error = GL.GetError();
            if (error != ErrorCode.NoError) Console.WriteLine($"{coh} OpenGL Error: {error}");
            var sdlerror = SDL3.SDL_GetError();
            if (sdlerror != null && sdlerror != "") Console.WriteLine($"{coh} SDL Error: [{sdlerror}]");

            _window.Update();
            _renderer.Render();
            _inputManager.Update();
            _sceneManager.Update();
            _sceneManager.Render();

            SDL_Event e;
            while (SDL3.SDL_PollEvent(&e)){
                switch (e.Type){
                    case SDL_EventType.SDL_EVENT_QUIT:
                        Quit();
                        break;
                }
            }

            Thread.Sleep(16);
        }
    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    public static void Quit() => Environment.Exit(0);

    /// <summary>
    /// Checks the current platform.
    /// </summary>
    /// <returns>Platform string: `Windows`, `Linux`, `MacOS`, `Android`, `IOS`, or `Unknown`.</returns>
    public static string CheckPlatform(){
        return OperatingSystem.IsWindows() ? "Windows" :
            OperatingSystem.IsLinux() ? "Linux" :
            OperatingSystem.IsMacOS() ? "MacOS" :
            OperatingSystem.IsAndroid() ? "Android" :
            OperatingSystem.IsIOS() ? "IOS" :
            "Unknown";
    }
}