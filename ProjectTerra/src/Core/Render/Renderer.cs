using SDL;
using OpenTK;

#if ANDROID || IOS 
using OpenTK.Graphics.ES30;
#else
using OpenTK.Graphics.OpenGL4;
#endif

namespace ProjectTerra.Core.Render;

public unsafe class Renderer
{
    private SDL_Window* _window;
    public SDL_GLContextState* glContext;
    private Dictionary<string, Action> _renderActions = new Dictionary<string, Action>();
    private static Shader shader = new Shader("ProjectTerra.src.Core.Render.Shader.basic.glsl");
    private static Buffer buffer = new Buffer();

    public Renderer(SDL_Window* window) {
        _window = window;

        #if ANDROID || IOS
            SDL3.SDL_GL_SetAttribute(SDL_GLAttr.SDL_GL_CONTEXT_PROFILE_MASK, (int)SDL_GLProfile.SDL_GL_CONTEXT_PROFILE_ES);
            SDL3.SDL_GL_SetAttribute(SDL_GLAttr.SDL_GL_CONTEXT_MAJOR_VERSION, 3);
            SDL3.SDL_GL_SetAttribute(SDL_GLAttr.SDL_GL_CONTEXT_MINOR_VERSION, 0);
        #else
            SDL3.SDL_GL_SetAttribute(SDL_GLAttr.SDL_GL_CONTEXT_MAJOR_VERSION, 3);
            SDL3.SDL_GL_SetAttribute(SDL_GLAttr.SDL_GL_CONTEXT_MINOR_VERSION, 3);
            SDL3.SDL_GL_SetAttribute(SDL_GLAttr.SDL_GL_CONTEXT_PROFILE_MASK, (int)SDL_GLProfile.SDL_GL_CONTEXT_PROFILE_CORE);
        #endif

        int profileMask = 0, major = 0, minor = 0;
        Console.WriteLine($"SDL_GL_CONTEXT_PROFILE_MASK: {SDL3.SDL_GL_GetAttribute(SDL_GLAttr.SDL_GL_CONTEXT_PROFILE_MASK, ref profileMask)}");
        Console.WriteLine($"SDL_GL_CONTEXT_MAJOR_VERSION: {SDL3.SDL_GL_GetAttribute(SDL_GLAttr.SDL_GL_CONTEXT_MAJOR_VERSION, ref major)}");
        Console.WriteLine($"SDL_GL_CONTEXT_MINOR_VERSION: {SDL3.SDL_GL_GetAttribute(SDL_GLAttr.SDL_GL_CONTEXT_MINOR_VERSION, ref minor)}");

        Console.WriteLine($"Profile Mask: {profileMask}, Version: {major}.{minor}");

        glContext = SDL3.SDL_GL_CreateContext(_window);
        if (glContext == null) throw new Exception("Failed to create OpenGL context");

        GL.LoadBindings(new OpenGLBindings());

        GL.ClearColor(1.0f, 0.0f, 0.0f, 1.0f);
        GL.Enable(EnableCap.DepthTest);

        shader.Use();
    }

    public void Render(){
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        GL.Viewport(0, 0, Game.windowSize.w, Game.windowSize.h);
        foreach (var action in _renderActions.Values) {action.Invoke();}
        SDL3.SDL_GL_SwapWindow(_window);
    }

    public void UpdateBuffer(float[] vertices, uint[] indices) => buffer.Update(vertices, indices);

    public void AddRenderAction(string name, Action action) => _renderActions.Add(name, action);
    public void RemoveRenderAction(string name) => _renderActions.Remove(name);

    public SDL_GLContextState* GetRenderer() => glContext;
}

public class OpenGLBindings: IBindingsContext
{
    public nint GetProcAddress(string name) => SDL3.SDL_GL_GetProcAddress(name);
}