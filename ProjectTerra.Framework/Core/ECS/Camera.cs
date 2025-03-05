using OpenTK.Mathematics;
using ProjectTerra.Framework.Graphics;
using ProjectTerra.Framework.Maths;
using ProjectTerra.Framework.Core.ECS.Components;

namespace ProjectTerra.Framework.Core.ECS;

public class Camera : BaseObject {
    public Matrix4 ViewMatrix { get; private set; }
    public Matrix4 ProjectionMatrix { get; private set; }

    public Vector3 Position { get; set; }

    public Camera(string name, Vector3 position, float fov, float aspectRatio) 
        : base(name, new Mesh(new Vertex[0], new uint[0]), new Shader(Shader.builtinVertexShader(), Shader.builtinFragmentShader()), null) 
    {
        Position = position;
        ViewMatrix = Matrix4.LookAt(Position, Position + Vector3.UnitZ, Vector3.UnitY);
        ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(fov, aspectRatio, 0.1f, 100f);
    }

    public override void Update() {
        ViewMatrix = Matrix4.LookAt(Position, Position + Vector3.UnitZ, Vector3.UnitY);
    }
}
