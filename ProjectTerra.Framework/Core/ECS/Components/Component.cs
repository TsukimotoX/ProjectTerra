#pragma warning disable CS8618

namespace ProjectTerra.Framework.Core.ECS.Components;

/// <summary>
/// Component is a base class for all ECS components. Components are like little code pieces that can be attached to an object.
/// You can add as many components as you want to an object, just don't add them more than once per type.
/// </summary>
public abstract class Component{
    protected Object owner;

    public void Attach(Object obj) => owner = obj; // no way around really

    public virtual void Update() {}
    public virtual void Render() {}
}