using SDL;
using OpenTK.Graphics.OpenGL;
using ProjectTerra.Framework.Platform;
using OpenTK;
using ProjectTerra.Framework.Core.ECS.Components;
using ProjectTerra.Framework.Core.ECS;
using ProjectTerra.Framework.Core.SceneManagement;
using OpenTK.Mathematics;

namespace ProjectTerra.Framework.Graphics;

public unsafe class Renderer {
    private IWindow? _window;
    private SDL_GLContextState* _glContext;
    private string coh = "[ProjectTerra.Framework.Graphics:Renderer]";
    public static HashSet<int> _renderQueue = new();
    // Also look into:
    // - Batch rendering
    // - Occlusion culling
    // - More ways to opitimize your bullshit code

    // Additional stuff in order to use OpenGL features seamlessly
    public ShaderBuffer shaderBuffer = new();
    private Dictionary<string, Shader> _shaders = new();
    private Shader? _currentShader;

    // Singleton
    private static Renderer? _instance;
    public static Renderer Instance => _instance ??= new Renderer();

    /// <summary>
    /// Initializes the renderer of the game. It can only be created once per program.
    /// </summary>
    /// <param name="window">A window to render on</param>
    /// <exception cref="Exception">If it fails to initialize the OpenGL context.</exception>
    public void Initialize(IWindow window){
        _window = window;
        // Create OpenGL context
        _glContext = SDL3.SDL_GL_CreateContext(_window.getWindow());
        if (_glContext == null) throw new Exception($"{coh} Failed to create OpenGL context: {SDL3.SDL_GetError()}");

        // Loading OpenGL data
        GL.LoadBindings(new OpenGLBindings());
        GL.Viewport(0, 0, _window.Size.width, _window.Size.height);
        GL.ClearColor(0.53f, 0.81f, 0.92f, 1.0f);
        GL.Enable(EnableCap.Blend);
        GL.Enable(EnableCap.DepthTest);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        // Set default shader on startup
        AddShader("default", new Shader(Shader.builtinVertexShader(), Shader.builtinFragmentShader()));
        UseShader("default");
    }

    /// <summary>
    /// Renders everything that is queued in render queue.
    /// </summary>
    public void Render(){
        if (_glContext == null) return;

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        var scene = SceneManager.Instance.GetScene();
        Camera? camera = null;

        foreach (var obj in scene.GetObjects()) { // Предполагаем, что у сцены есть список объектов
            if (obj is Camera cam) {
                camera = cam;
                break;
            }
        }

        if (camera != null) {
            Matrix4 view = camera.ViewMatrix;
            Matrix4 projection = camera.ProjectionMatrix;

            GL.UniformMatrix4(_currentShader.GetUniformLocation("view"), false, ref view);
            GL.UniformMatrix4(_currentShader.GetUniformLocation("projection"), false, ref projection);
        }


        foreach (var meshId in _renderQueue){
            if (!IsMeshVisible(meshId)) continue;
            shaderBuffer.Render(meshId);
        }

        _renderQueue.Clear();
    }

    // Render queue management
    
    public void QueueMesh(int meshId) => _renderQueue.Add(meshId);
    public int BakeMesh(Mesh mesh) => shaderBuffer.Bake(mesh.Vertices, mesh.Triangles);
    private bool IsMeshVisible(int meshId) => true; // Make this into occlusion culling function.

    // Shader management
    public void AddShader(string name, Shader shader) {if (!_shaders.ContainsKey(name)) _shaders[name] = shader;}
    public Shader GetShader(string name) {
        if (_shaders.ContainsKey(name)) return _shaders[name];
        Console.WriteLine($"{coh} Shader '{name}' not found! Using default.");
        return _shaders["default"];
    }
    public void UseShader(string name) {
        if (_shaders.ContainsKey(name)) {_currentShader = _shaders[name]; _currentShader.Use();}
        else{
            Console.WriteLine($"{coh} Shader '{name}' not found! Using default.");
            _currentShader = _shaders["default"];
            _currentShader.Use();
        }
    }
    public Shader? GetCurrentShader() => _currentShader;
    public Shader[] GetShaderList() => _shaders.Values.ToArray();

    public ShaderBuffer GetBuffer() => shaderBuffer;
}

public class OpenGLBindings : IBindingsContext {
    public IntPtr GetProcAddress(string processName) => SDL3.SDL_GL_GetProcAddress(processName);
}
