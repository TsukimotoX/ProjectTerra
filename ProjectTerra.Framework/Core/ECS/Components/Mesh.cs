using ProjectTerra.Framework.Graphics;
using ProjectTerra.Framework.Maths;

namespace ProjectTerra.Framework.Core.ECS.Components;

/// <summary>
/// Mesh is a component that allows you to create 3d shapes and render them on screen.
/// You can add a mesh to an object in order to make it visible as this shape.
/// For example, a cube, or a sphere, or a plane... Whatever you want!
/// </summary>
public class Mesh : Component {
    public Vertex[] Vertices { get; private set; }
    public uint[] Triangles { get; private set; }
    private int _meshId = -1;
    private bool _isUploaded;
    public bool isEnabled = true;

    /// <summary>
    /// Mesh is a component that allows you to create 3d shapes and render them on screen.
    /// You can add a mesh to an object in order to make it visible as this shape.
    /// For example, a cube, or a sphere, or a plane... Whatever you want!
    /// </summary>
    /// <param name="vertices">A list of vertices to put together. Those are "dots" in 3d space</param>
    /// <param name="triangles">A list of triangles. Each triangle is made of 3 vertices. Pro tip: Label your vertices to make them easier to piece together.</param>
    public Mesh(Vertex[] vertices, uint[] triangles) {
        Vertices = vertices;
        Triangles = triangles;
    }

    /// <summary>
    /// Uploads mesh to the render queue.
    /// </summary>
    public void Upload(){
        if (_isUploaded) return;
        _meshId = Renderer.Instance.BakeMesh(this);
        _isUploaded = true;
    }

    public override void Render() {
        base.Render();
        if (!isEnabled) return;
        if (!_isUploaded) Upload();
    }

    /// <summary>
    /// Deletes mesh from the render queue
    /// </summary>
    public void Delete(){
        if (!_isUploaded) return;
        Renderer.Instance.GetBuffer().Remove(_meshId);
    }
}

/// <summary>
/// MeshData is a data class that contains all the information needed to render a mesh.
/// </summary>
public class MeshData {
    public int VBO { get; }
    public int VAO { get; }
    public int EBO { get; }
    public int IndicesCount { get; } 
    public MeshData(int vbo, int vao, int ebo, int indicescount){
        VBO = vbo;
        VAO = vao;
        EBO = ebo;
        IndicesCount = indicescount;
    }
}