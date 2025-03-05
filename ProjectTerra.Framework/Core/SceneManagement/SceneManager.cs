namespace ProjectTerra.Framework.Core.SceneManagement;

/// <summary>
/// SceneManager is a class that handles scene management, like switching or managing scenes.
/// </summary>
public class SceneManager{
    // Singleton
    private static SceneManager? _instance;
    /// <summary>
    /// Single instance of SceneManager. SceneManager can only be created once per program.
    /// </summary>
    public static SceneManager Instance => _instance ??= new SceneManager();
    private string coh = "[ProjectTerra.Framework.Core.SceneManagement:SceneManager]";

    // Manager properties
    private Dictionary<string, IScene> _scenes = new();
    private IScene _currentScene = new GameScene("default"); // By default, the scene is game scene. If you want to switch, you would wanna name this manually.

    /// <summary>
    /// Registers a new scene in the manager.
    /// </summary>
    /// <param name="name">Name of the scene to refer to.</param>
    /// <param name="scene">The scene to be registered.</param>
    public void RegisterScene(string name, IScene scene) => _scenes[name] = scene;

    /// <summary>
    /// Sets the current scene by Scene object.
    /// </summary>
    /// <param name="scene">Scene object to switch to</param>
    public void SetScene(IScene scene)
    {
        if (_currentScene == scene) return;
        Console.WriteLine($"{coh} Switching scene: {_currentScene.Name} → {scene.Name}");
        _currentScene = scene;
    }

    /// <summary>
    /// Sets the current scene by scene name, if it exists in the manager.
    /// </summary>
    /// <param name="name">Name of the scene</param>
    public void SetScene(string name){
        if (_scenes.TryGetValue(name, out var scene)) SetScene(scene);
        else Console.WriteLine($"{coh} Scene '{name}' not found.");
    }

    /// <summary>
    /// Gets the current scene.
    /// </summary>
    public IScene GetScene() => _currentScene;

    public void Update() => _currentScene.Update();
    public void Render() => _currentScene.Render();
}