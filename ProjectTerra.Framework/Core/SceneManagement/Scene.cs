namespace ProjectTerra.Framework.Core.SceneManagement;

/// <summary>
/// GameScene is a scene class that allows you to work with 3D environment.
/// </summary>
public class GameScene : IScene{
    private HashSet<ECS.Object> _objects = new();
    public string Name { get; set; }

    /// <summary>
    /// GameScene is a scene class that allows you to work with 3D environment.
    /// </summary>
    /// <param name="name"></param>
    public GameScene(string name) => Name = name;

    /// <summary>
    /// Adds object to scene
    /// </summary>
    /// <param name="obj">Object to add. Any element that derives from ECS.Object</param>
    public void AddObject(ECS.Object obj) {
        _objects.Add(obj);
        Console.WriteLine($"Added object {obj.name} to scene {Name}");
    }

    public ECS.Object[] GetObjects() => _objects.ToArray();

    /// <summary>
    /// Removes object from scene
    /// </summary>
    /// <param name="obj">Object to remove, any element that derives from ECS.Object</param>
    public void RemoveObject(ECS.Object obj) => _objects.Remove(obj);

    /// <summary>
    /// ! WARNING ! Removes all objects from scene. Action is irreversible.
    /// </summary>
    public void ClearScene() => _objects.Clear();

    public void Update(){foreach (ECS.Object obj in _objects) {obj.Update();}}

    public void Render(){foreach (ECS.Object obj in _objects) {obj.Render();}}
}

/// <summary>
/// Interface for scenes. Scenes created using this interface can be used in SceneManager.
/// </summary>
public interface IScene{
    string Name { get; set; }
    public void AddObject(ECS.Object obj);
    public void RemoveObject(ECS.Object obj);
    public ECS.Object[] GetObjects();
    public void ClearScene();
    void Update();
    void Render();
}