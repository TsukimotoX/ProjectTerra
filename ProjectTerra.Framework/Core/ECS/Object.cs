#pragma warning disable CS8603

using OpenTK.Mathematics;
using ProjectTerra.Framework.Core.ECS.Components;
using ProjectTerra.Framework.Graphics;

namespace ProjectTerra.Framework.Core.ECS;

/// <summary>
/// BaseObject is a base 3d object that can be used in SceneManager.
/// It has Mesh, Shader, Texture set up for you.
/// </summary>
public class BaseObject : Object { 
    public Mesh mesh { get; set; }
    public Shader shader { get; set; }
    public Texture? texture { get; set; }

    /// <summary>
    /// BaseObject is a base 3d object that can be used in SceneManager.
    /// It has Mesh, Shader, Texture set up for you.
    /// </summary>
    public BaseObject(string name, Mesh mesh, Shader shader, Texture? texture) : base(name) {
        this.mesh = mesh;
        this.shader = shader;
        this.texture = texture ?? null;
        Console.WriteLine($"New object is created: {name}");
    }

    public override void Update() {}

    public override void Render() {
        if (!enabled) return;
        Console.WriteLine($"Rendering {name}!");
        shader.Use();
        if (texture != null) {
            Console.WriteLine($"Binding texture {texture.AtlasHandle}");
            texture.Bind();
        }
        mesh.Render();
    }
}

/// <summary>
/// Object is a base class for all ECS objects. It can be a 3d object, UI element, or whatever you desire to make out of it.
/// Not recommended to use in production; its just a base class.
/// </summary>
public class Object {
    public string name { get; set; }
    public Vector3 position { get; set; } = Vector3.Zero;
    public Vector3 rotation { get; set; } = Vector3.Zero;
    public Vector3 scale { get; set; } = Vector3.One;
    public bool enabled = true;

    private List<Component> components = new();

    public Object(string name) => this.name = name;

    public virtual void Update() {foreach (var component in components) component.Update();}
    public virtual void Render() {foreach (var component in components) component.Render();}

    /// <summary>
    /// Adds component to object.
    /// </summary>
    /// <param name="component">Any element that derives from ECS.Component</param>
    public void AddComponent(Component component) {
        components.Add(component);
        component.Attach(this);
    }

    /// <summary>
    /// Returns component of type T. If not found, returns null.
    /// T is any element that derives from ECS.Component
    /// </summary>
    /// <typeparam name="T">Any element that derives from ECS.Component</typeparam>
    /// <returns>The component of type T</returns>
    public T GetComponent<T>() where T : Component => components.OfType<T>().FirstOrDefault();
}