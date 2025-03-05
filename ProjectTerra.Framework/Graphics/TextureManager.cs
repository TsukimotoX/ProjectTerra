using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using StbImageSharp;

namespace ProjectTerra.Framework.Graphics;

public class TextureManager{
    private static Dictionary<string, Vector4> _textureCoords = new();
    private static List<TextureAtlas> _atlases = new();
    private static int _maxTextureSize, _maxTextureUnits;
    private static string coh = "[ProjectTerra.Framework.Graphics:TextureManager]";

    static TextureManager(){
        GL.GetInteger(GetPName.MaxTextureSize, out _maxTextureSize);
        GL.GetInteger(GetPName.MaxTextureImageUnits, out _maxTextureUnits);
        Console.WriteLine($"{coh} Max texture size: {_maxTextureSize}x{_maxTextureSize}");
        Console.WriteLine($"{coh} Max texture units: {_maxTextureUnits}");
    }

    public static void LoadTexture(string name, Stream stream){
        if (_textureCoords.ContainsKey(name)) return; // Already loaded
        
        var image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
        if (image == null || image.Width == 0 || image.Height == 0) throw new Exception($"{coh} Failed to load texture: {name}");
        
        foreach (var atlas in _atlases) {
            if (atlas.AddImage(name, image.Data, image.Width, image.Height, out var coords)) {
                _textureCoords[name] = coords;
                return;
            }
        }

        if (_atlases.Count < _maxTextureUnits) {
            var atlas = new TextureAtlas();
            if (atlas.AddImage(name, image.Data, image.Width, image.Height, out var coords)) {
                _textureCoords[name] = coords;
                _atlases.Add(atlas);
            } else throw new Exception($"{coh} Unable to add {name} to new atlas.");
        } else throw new Exception("{coh} Texture atlas limit reached.");
    }

    public static Vector4 GetTextureCoords(string name){
        if (!_textureCoords.ContainsKey(name)) throw new Exception($"{coh} Texture {name} not found.");
        return _textureCoords[name];
    }

    public static int GetAtlasTexture(int index) => _atlases[index].atlasHandle;
}

public class TextureAtlas {
    public int atlasHandle { get; private set; }
    private int _size;
    private int _offsetX = 0, _offsetY = 0, _rowOffset = 0;

    public TextureAtlas(){
        GL.GetInteger(GetPName.MaxTextureSize, out _size);

        atlasHandle = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, atlasHandle);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, _size, _size, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
    }

    public bool AddImage(string name, byte[] data, int width, int height, out Vector4 coords){
        if (_offsetX + width > _size){
            _offsetX = 0;
            _offsetY += _rowOffset;
            _rowOffset = 0;
        }

        if (_offsetY + height > _size){
            coords = default;
            return false;
        }

        GL.TexSubImage2D(TextureTarget.Texture2D, 0, _offsetX, _offsetY, width, height, PixelFormat.Rgba, PixelType.UnsignedByte, data);
        coords = new Vector4(
            _offsetX / (float)_size,
            _offsetY / (float)_size,
            (_offsetX + width) / (float)_size,
            (_offsetY + height) / (float)_size
        );

        _offsetX += width;
        _rowOffset = Math.Max(_rowOffset, height);
        return true;
    }
}