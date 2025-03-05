using ProjectTerra.Framework.Core.ECS.Components;
using ProjectTerra.Framework.Core.ECS;
using ProjectTerra.Framework.Core.SceneManagement;
using ProjectTerra.Framework.Platform;
using ProjectTerra.Framework.Graphics;
using ProjectTerra.Framework.Maths;
using OpenTK.Mathematics;

namespace ProjectTerra.Tests;

public class Test {

    public static void Main(string[] args) {
        var host = new Host();
        TextureManager.LoadTexture("stone", new FileStream("./ProjectTerra.Game/stone.png", FileMode.Open));

        SceneManager.Instance.GetScene().AddObject(
            new BaseObject(
                "test", 
                new Mesh(
                    new Vertex[] {
                        new Vertex((0, 0, 0), (1, 0, 0, 1), (0, 0)),
                        new Vertex((1, 0, 0), (0, 1, 0, 1), (1, 0)),
                        new Vertex((1, 1, 0), (0, 0, 1, 1), (1, 1)),
                        new Vertex((0, 1, 0), (1, 1, 0, 1), (0, 1)),
                    },
                    new uint[] {
                        0, 1, 2,
                        2, 3, 0
                    }
                ), 
                new Shader(Shader.builtinVertexShader(),Shader.builtinFragmentShader()),
                new Texture("stone")));
        
        SceneManager.Instance.GetScene().AddObject(
            new Camera("camera", new Vector3(0, 0, 10), MathHelper.DegreesToRadians(45f), 16f / 9f)
        );
        
        Console.WriteLine("Test scene loaded.");
        Console.WriteLine(TextureManager.GetTextureCoords("stone"));
        host.Run();
    }
}