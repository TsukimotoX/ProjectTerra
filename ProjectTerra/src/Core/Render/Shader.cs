#if ANDROID || IOS 
using OpenTK.Graphics.ES30;
#else
using OpenTK.Graphics.OpenGL4;
#endif

using System.Reflection;

namespace ProjectTerra.Core.Render;

public class Shader{
    private int _shaderProgram;
    private int vertexShader, fragmentShader;

    public Shader(string shaderName){
        bool isGLES = OperatingSystem.IsAndroid() || OperatingSystem.IsIOS();
        string platformSpecificShaderPath = isGLES 
            ? $"{shaderName.Replace(".glsl", "_es.glsl")}" 
            : shaderName;
        
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(platformSpecificShaderPath);
        if (stream == null) throw new Exception($"Resource {platformSpecificShaderPath} not found in assembly.");
        using var reader = new StreamReader(stream);
        string shaderSource = reader.ReadToEnd().Trim();
        if (string.IsNullOrWhiteSpace(shaderSource)) throw new Exception($"Shader file {shaderName} is empty or not readable.");
        if (!shaderSource.Contains("<split>")) throw new Exception($"Shader file {shaderName} does not contain '<split>' separator.");
        Console.WriteLine($"Shader content:\n{shaderSource}");

        string[] shaderParts = shaderSource.Split(new[] { "<split>" }, StringSplitOptions.None);
        if (shaderParts.Length != 2) throw new Exception($"Shader file {platformSpecificShaderPath} must contain exactly one '<split>' tag.");

        string vertexShaderSource = shaderParts[0].Trim();
        string fragmentShaderSource = shaderParts[1].Trim();

        vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexShaderSource);
        GL.CompileShader(vertexShader);
        GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int success);
        if (success == 0) Console.WriteLine(GL.GetShaderInfoLog(vertexShader));

        fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, fragmentShaderSource);
        GL.CompileShader(fragmentShader);
        GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out success);
        if (success == 0) Console.WriteLine(GL.GetShaderInfoLog(fragmentShader));

        _shaderProgram = GL.CreateProgram();
        GL.AttachShader(_shaderProgram, vertexShader);
        GL.AttachShader(_shaderProgram, fragmentShader);
        GL.LinkProgram(_shaderProgram);
        GL.GetProgram(_shaderProgram, GetProgramParameterName.LinkStatus, out success);
        if (success == 0) Console.WriteLine(GL.GetProgramInfoLog(_shaderProgram));
    }

    ~Shader(){
        GL.DetachShader(_shaderProgram, vertexShader);
        GL.DetachShader(_shaderProgram, fragmentShader);
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);
        
        GL.DeleteProgram(_shaderProgram);
        GC.SuppressFinalize(this);
    }

    public void Use()
    {
        GL.UseProgram(_shaderProgram);
    }

    public int GetAttribLocation(string attribName)
    {
        return GL.GetAttribLocation(_shaderProgram, attribName);
    }
}

public class Buffer{
    private int _VBO; // Vertex Buffer Object
    private int _VAO; // Vertex Array Object
    private int _EBO; // Element Buffer Object
    private float[] vertices = [];
    private uint[] indices = [];

    public Buffer(){
        _VBO = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _VBO);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        _VAO = GL.GenVertexArray();
        GL.BindVertexArray(_VAO);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        _EBO = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _EBO);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
    }
    ~Buffer(){
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.DeleteBuffer(_VBO);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        GL.DeleteBuffer(_EBO);
    }

    public void Update(float[] vertices, uint[] indices){
        this.vertices = vertices;
        this.indices = indices;
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
    }
}