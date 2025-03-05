using SDL;
using OpenTK.Graphics.OpenGL;

namespace ProjectTerra.Framework.Platform;

public unsafe class Window: IWindow {
    private SDL_Window* _handle;
    private string coh = "[ProjectTerra.Framework.Platform:Window]";
    
    public (int width, int height) Size { get; set; } = (1280, 720);
    public (int x, int y) Position { get; set; } = (100, 100);
    public string Title { get; set; } = "ProjectTerra";

    public Window(){
        if (!SDL3.SDL_Init(SDL_InitFlags.SDL_INIT_VIDEO | SDL_InitFlags.SDL_INIT_EVENTS)) throw new Exception($"{coh} Failed to initialize SDL: {SDL3.SDL_GetError()}");

        _handle = SDL3.SDL_CreateWindow(Title, Size.width, Size.height, /*SDL_WindowFlags.SDL_WINDOW_FULLSCREEN | */ SDL_WindowFlags.SDL_WINDOW_OPENGL);
        
        if (_handle == null) throw new Exception($"{coh} Failed to create window:" + SDL3.SDL_GetError());
        
        Console.WriteLine($"{coh} Window successfully initialized!");
    }

    ~Window() {
        SDL3.SDL_DestroyWindow(_handle);
        SDL3.SDL_Quit();
    }

    public void Update() => SDL3.SDL_GL_SwapWindow(_handle);
    //public void Update() => Console.WriteLine("fumo");

    public void Resize(int width, int height) {
        Size = (width, height);
        SDL3.SDL_SetWindowSize(_handle, width, height);
        GL.Viewport(0, 0, width, height);
        Console.WriteLine($"{coh} Window resized to {width}x{height}");
    }

    public void Move(int x, int y) {
        Position = (x, y);
        SDL3.SDL_SetWindowPosition(_handle, x, y);
        Console.WriteLine($"{coh} Window moved to {x},{y}");
    }

    public void SetTitle(string title) {
        Title = title;
        SDL3.SDL_SetWindowTitle(_handle, title);
        Console.WriteLine($"{coh} Window title set to {title}");
    }

    public void SetFullscreen(bool fullscreen) {
        SDL3.SDL_SetWindowFullscreen(_handle, fullscreen ? true : false);
        Console.WriteLine($"{coh} Window fullscreen set to {fullscreen}");
    }

    public SDL_Window* getWindow() => _handle;
}

public unsafe interface IWindow {
    (int width, int height) Size { get; }
    (int x, int y) Position { get; }
    string Title { get; }

    void Update();

    SDL_Window* getWindow();
}