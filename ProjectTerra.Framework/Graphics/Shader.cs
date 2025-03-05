#pragma warning disable CS0618

using OpenTK.Graphics.OpenGL;
using ProjectTerra.Framework.Platform;

namespace ProjectTerra.Framework.Graphics;

public class Shader {
    private int programID; // GL program ID for shader
    private string vertexSource, fragmentSource; // Shader source code
    private string coh = "[ProjectTerra.Framework.Graphics:Shader]";

    public Shader(string vertexSource, string fragmentSource) {
        this.vertexSource = AdjustPlatform(vertexSource, ShaderType.VertexShader);
        this.fragmentSource = AdjustPlatform(fragmentSource, ShaderType.FragmentShader);

        Compile();
    }

    private void Compile() {
        int vertexShader = CompileShader(vertexSource, ShaderType.VertexShader);
        int fragmentShader = CompileShader(fragmentSource, ShaderType.FragmentShader);

        programID = GL.CreateProgram();
        GL.AttachShader(programID, vertexShader);
        GL.AttachShader(programID, fragmentShader);
        GL.LinkProgram(programID);

        GL.GetProgram(programID, ProgramParameter.LinkStatus, out int linkStatus);
        if (linkStatus == 0) {
            string infoLog = GL.GetShaderInfoLog(programID);
            throw new Exception($"Shader linking failed: {infoLog}");
        }

        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);
    }

    public void Use() {
        GL.UseProgram(programID);
        Console.WriteLine($"{coh} Using shader: {programID}");
    } 

    public int GetProgram() => programID;
    public int GetUniformLocation(string name) => GL.GetUniformLocation(programID, name);

    private string AdjustPlatform(string source, ShaderType shaderType) {
        bool isMobile = Host.CheckPlatform() == "Android" || Host.CheckPlatform() == "IOS";

        string version = isMobile ? "#version 300 es\n" : "#version 330\n";
        string precision = isMobile && shaderType == ShaderType.FragmentShader ? "precision mediump float;" : "";

        var lines = source.Split('\n');
        source = string.Join("\n", lines.Where(line => !line.StartsWith("#version") && !line.StartsWith("precision")));
        //Console.WriteLine(source);
        return $"{version}\n{precision}\n{source}";
    }

    private int CompileShader(string source, ShaderType shaderType) {
        int shader = GL.CreateShader(shaderType);
        GL.ShaderSource(shader, source);
        GL.CompileShader(shader);

        // Check if shader is valid to work with
        GL.GetShader(shader, ShaderParameter.CompileStatus, out int compileStatus);
        if (compileStatus == 0) {
            string infoLog = GL.GetShaderInfoLog(shader);
            throw new Exception($"Error compiling shader: {infoLog}");
        }
    
        return shader;
    }

    public static string builtinVertexShader() => @"#version 330 core
        layout(location = 0) in vec3 aPos;
        layout(location = 1) in vec4 aColor;
        layout(location = 2) in vec2 aTexCoord;
        out vec4 vertexColor;
        out vec2 TexCoord;
        uniform mat4 model;
        uniform mat4 view;
        uniform mat4 projection;
        void main()
        {
            gl_Position = projection * view * model * vec4(aPos, 1.0);
            vertexColor = aColor;
            TexCoord = aTexCoord;
        }";

    public static string builtinFragmentShader() => @"#version 330 core
        in vec4 vertexColor;
        in vec2 TexCoord;
        out vec4 FragColor;
        uniform sampler2D texture0;
        uniform int useTexture;
        void main()
        {
            if (useTexture == 0) {
                FragColor = vertexColor;
                return;
            } else {
                FragColor = texture(texture0, TexCoord);
            }
        }";
}