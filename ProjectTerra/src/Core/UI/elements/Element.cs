using SDL;
using ProjectTerra;
using OpenTK.Graphics.OpenGL;

namespace ProjectTerra.Core.UI.elements;

public class Element
{
    public (int x, int y) Position { get; set; }
    public (int width, int height) Size { get; set; }
    public Style Style { get; set; }

    public Element(int x, int y, int width, int height) => (Position, Size) = ((x, y), (width, height));

    public void Render() {
        SDL_Rect rect = new SDL_Rect(){x = Position.x, y = Position.y, w = Size.width, h = Size.height};
    }
    
    public Action OnClick { get; set; }
}