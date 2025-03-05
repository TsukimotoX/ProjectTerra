using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using ProjectTerra.Framework.Graphics;

namespace ProjectTerra.Framework.Core.ECS.Components;

public class Texture : Component {
    public int AtlasHandle { get; private set; }
    public Vector4 UV { get; private set; }

    public Texture(string name) {
        UV = TextureManager.GetTextureCoords(name);
        AtlasHandle = TextureManager.GetAtlasTexture(0); // For now taking only first.
    }

    public void Bind() {
        GL.BindTexture(TextureTarget.Texture2D, AtlasHandle);
    }

    public void Unbind() {
        GL.BindTexture(TextureTarget.Texture2D, 0);
    }
}