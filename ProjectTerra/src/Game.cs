using ProjectTerra.Core;
using ProjectTerra.Core.Render;
using SDL;

#if ANDROID || IOS 
using OpenTK.Graphics.ES30;
#else
using OpenTK.Graphics.OpenGL4;
#endif

namespace ProjectTerra;

public static unsafe class Game {
    public static bool _isRunning = true;
    public static SDL_Window* window;
    public static Renderer renderer = null;
    private static InputManager _inputManager = new();
    public static (int w, int h) windowSize;

    public static void Initialize(){
        if (!SDL3.SDL_Init(SDL_InitFlags.SDL_INIT_VIDEO)) throw new Exception("SDL failed to initialize!");
        
        #if ANDROID || IOS
            var p = SDL3.SDL_GetDisplayBounds(0, out var rect);
            if (p != 0) throw new Exception("Failed to get display bounds!");
            windowSize = (rect.w, rect.h);
        #else
            windowSize = (800, 600);
        #endif

        window = SDL3.SDL_CreateWindow("Project Terra", windowSize.w, windowSize.h, SDL_WindowFlags.SDL_WINDOW_OPENGL | SDL_WindowFlags.SDL_WINDOW_RESIZABLE);
        if (window == null) throw new Exception("SDL failed to create window!");

        renderer = new Renderer(window);
        if (!SDL3.SDL_GL_MakeCurrent(window, renderer.glContext)) throw new Exception($"Failed to make OpenGL context current: {SDL3.SDL_GetError()}");

        GL.Viewport(0, 0, windowSize.w, windowSize.h);

        _Run();
    }

    private static void Start(){
        float[] vertices = {
            0.5f,  0.5f, 0.0f,  //0
            0.5f, -0.5f, 0.0f,  // 1
            -0.5f, -0.5f, 0.0f,  // 2
            -0.5f,  0.5f, 0.0f   // 3
        };
        uint[] triangles = {
            0, 1, 2,
            0, 2, 3
        };

        renderer.UpdateBuffer(vertices, triangles);
        renderer.AddRenderAction("Test", () => GL.DrawElements(PrimitiveType.Triangles, triangles.Length, DrawElementsType.UnsignedInt, 0));
    }

    // like a life hint :)
    private static void _Run() {
        Start();

        string version = GL.GetString(StringName.Version);
        string glsrenderer = GL.GetString(StringName.Renderer);
        

        while (_isRunning) {
            var glerror = GL.GetError();
            if (glerror != ErrorCode.NoError) Console.WriteLine($"OpenGL Error: {glerror}");
            var sdlerror = SDL3.SDL_GetError();
            if (sdlerror != "" || sdlerror != null) Console.WriteLine($"SDL Error: {sdlerror}");

            _inputManager.Loop();
            renderer.Render();

            SDL3.SDL_Delay(16);
        }

        SDL3.SDL_Quit();
    }

    public static void Quit() => _isRunning = false;
}
