using OpenTK.Graphics.OpenGL;
using ProjectTerra.Framework.Maths;
using ProjectTerra.Framework.Core.ECS.Components;

namespace ProjectTerra.Framework.Graphics;

public class ShaderBuffer{
    private int __nextMeshId = 0;
    private Dictionary<int, MeshData> _meshQueue = new();
    private string coh = "[ProjectTerra.Framework.Graphics:ShaderBuffer]";

    public int Bake(Vertex[] vertices, uint[] triangles){
        int vbo = GL.GenBuffer();
        int vao = GL.GenVertexArray();
        int ebo = GL.GenBuffer();

        GL.BindVertexArray(vao);

        float[] vertexData = vertices.SelectMany(v => v.ToArray()).ToArray();

        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertexData.Length * sizeof(float), vertexData, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, triangles.Length * sizeof(int), triangles, BufferUsageHint.StaticDraw);

        // Position of vertex
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vertex.Size, 0); // 3 for x, y, z
        GL.EnableVertexAttribArray(0);

        // Color of vertex ( in case texture doesn't load in )
        GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, Vertex.Size, sizeof(float) * 3); // 4 for r, g, b, a
        GL.EnableVertexAttribArray(1);

        // Texture of vertex
        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, Vertex.Size, sizeof(float) * 7); // 2 for u, v
        GL.EnableVertexAttribArray(2);

        GL.BindVertexArray(0);

        int meshId = __nextMeshId++;
        _meshQueue[meshId] = new MeshData(vbo, vao, ebo, triangles.Length);
        return meshId;
    }

    public void Render(int meshId){
        if (!_meshQueue.ContainsKey(meshId)) return;
        
        MeshData mesh = _meshQueue[meshId];
        Console.WriteLine($"{coh} Rendering mesh with VAO: {mesh.VAO}");
        GL.BindVertexArray(mesh.VAO);
        int vao, vbo;
        GL.GetInteger(GetPName.VertexArrayBinding, out vao);
        GL.GetInteger(GetPName.ArrayBufferBinding, out vbo);
        Console.WriteLine($"VAO: {vao}, VBO: {vbo}");
        Console.WriteLine($"Drawing {mesh.IndicesCount} triangles...");
        GL.DrawElements(PrimitiveType.Triangles, mesh.IndicesCount, DrawElementsType.UnsignedInt, 0);
    }

    public void Remove(int meshId){
        if (_meshQueue.ContainsKey(meshId)){
            MeshData mesh = _meshQueue[meshId];
            GL.DeleteVertexArray(mesh.VAO);
            GL.DeleteBuffer(mesh.VBO);
            GL.DeleteBuffer(mesh.EBO);
            _meshQueue.Remove(meshId);
        }
    }
}