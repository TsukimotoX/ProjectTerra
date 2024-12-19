using SDL;

namespace ProjectTerra.Core;

public unsafe class InputManager
{
    // Dictionaries containing keys and mouse buttons, and also their pressed states.
    private static Dictionary<InputKey, SDL_Scancode> keyMap = new();
    private static Dictionary<InputKey, bool> keyStates = new();
    private static Dictionary<MouseKey, SDL_MouseButtonFlags> mouseMap = new();
    private static Dictionary<MouseKey, bool> mouseStates = new();
    public static (float x, float y) mousePosition; // Mouse position

    // Initializer, constructor
    public InputManager() {
        // Check if we can loop through the key reading. If not, throw an exception
        if (!SDL3.SDL_Init(SDL_InitFlags.SDL_INIT_EVENTS)) throw new Exception($"SDL event system failed to initialize! Error: {SDL.SDL3.SDL_GetError()}");

        // Register every key and mouse button in the dictionaries
        foreach (InputKey key in Enum.GetValues(typeof(InputKey))) {
            keyStates[key] = false;
        }
        foreach (MouseKey key in Enum.GetValues(typeof(MouseKey))) {
            mouseStates[key] = false;
        }
    }

    // Loop that checks for keyboard and mouse interaction. Sends a signal if there is something related to input.
    public void Loop(){
        SDL_Event e;
        while (SDL3.SDL_PollEvent(&e)) {
            switch (e.Type) {
                case SDL_EventType.SDL_EVENT_QUIT:
                    Game.Quit();
                    break;
                case SDL_EventType.SDL_EVENT_KEY_DOWN:
                    keyStates[(InputKey)e.key.scancode] = true;
                    //Console.WriteLine("Key pressed:" + e.key.scancode);
                    break;
                case SDL_EventType.SDL_EVENT_KEY_UP:
                    keyStates[(InputKey)e.key.scancode] = false;
                    break;
                case SDL_EventType.SDL_EVENT_MOUSE_MOTION:
                    mousePosition.x = e.motion.x;
                    mousePosition.y = e.motion.y;
                    //Console.WriteLine("Mouse moved to: " + MouseX + ", " + MouseY);
                    break;
                case SDL_EventType.SDL_EVENT_MOUSE_BUTTON_DOWN:
                    mouseStates[(MouseKey)e.button.button] = true;
                    //Console.WriteLine("Mouse button pressed: " + e.button.button);
                    break;
                case SDL_EventType.SDL_EVENT_MOUSE_BUTTON_UP:
                    mouseStates[(MouseKey)e.button.button] = false;
                    break;
            }
        }
    }

    // Extra useful functions
    public static bool IsKeyDown(InputKey key) => keyStates[key];
    public static bool IsKeyUp(InputKey key) => !keyStates[key];
    public static bool IsMouseDown(MouseKey key) => mouseStates[key];
    public static bool IsMouseUp(MouseKey key) => !mouseStates[key];
    public static bool IsMouseMoved() => mousePosition.x != 0 || mousePosition.y != 0;

    // This function is quirky but it's basically detecting if mouse is over a certain rectangle area.
    public static bool IsMouseOver(float x, float y, float width, float height) => mousePosition.x > x && mousePosition.x < x + width && mousePosition.y > y && mousePosition.y < y + height;
}

public enum InputKey: uint {
    // Common keys
    A = SDL_Scancode.SDL_SCANCODE_A,
    B = SDL_Scancode.SDL_SCANCODE_B,
    C = SDL_Scancode.SDL_SCANCODE_C,
    D = SDL_Scancode.SDL_SCANCODE_D,
    E = SDL_Scancode.SDL_SCANCODE_E,
    F = SDL_Scancode.SDL_SCANCODE_F,
    G = SDL_Scancode.SDL_SCANCODE_G,
    H = SDL_Scancode.SDL_SCANCODE_H,
    I = SDL_Scancode.SDL_SCANCODE_I,
    J = SDL_Scancode.SDL_SCANCODE_J,
    K = SDL_Scancode.SDL_SCANCODE_K,
    L = SDL_Scancode.SDL_SCANCODE_L,
    M = SDL_Scancode.SDL_SCANCODE_M,
    N = SDL_Scancode.SDL_SCANCODE_N,
    O = SDL_Scancode.SDL_SCANCODE_O,
    P = SDL_Scancode.SDL_SCANCODE_P,
    Q = SDL_Scancode.SDL_SCANCODE_Q,
    R = SDL_Scancode.SDL_SCANCODE_R,
    S = SDL_Scancode.SDL_SCANCODE_S,
    T = SDL_Scancode.SDL_SCANCODE_T,
    U = SDL_Scancode.SDL_SCANCODE_U,
    V = SDL_Scancode.SDL_SCANCODE_V,
    W = SDL_Scancode.SDL_SCANCODE_W,
    X = SDL_Scancode.SDL_SCANCODE_X,
    Y = SDL_Scancode.SDL_SCANCODE_Y,
    Z = SDL_Scancode.SDL_SCANCODE_Z,

    // Number keys
    N1 = SDL_Scancode.SDL_SCANCODE_1,
    N2 = SDL_Scancode.SDL_SCANCODE_2,
    N3 = SDL_Scancode.SDL_SCANCODE_3,
    N4 = SDL_Scancode.SDL_SCANCODE_4,
    N5 = SDL_Scancode.SDL_SCANCODE_5,
    N6 = SDL_Scancode.SDL_SCANCODE_6,
    N7 = SDL_Scancode.SDL_SCANCODE_7,
    N8 = SDL_Scancode.SDL_SCANCODE_8,
    N9 = SDL_Scancode.SDL_SCANCODE_9,
    N0 = SDL_Scancode.SDL_SCANCODE_0,

    Numpad0 = SDL_Scancode.SDL_SCANCODE_KP_0,
    Numpad1 = SDL_Scancode.SDL_SCANCODE_KP_1,
    Numpad2 = SDL_Scancode.SDL_SCANCODE_KP_2,
    Numpad3 = SDL_Scancode.SDL_SCANCODE_KP_3,
    Numpad4 = SDL_Scancode.SDL_SCANCODE_KP_4,
    Numpad5 = SDL_Scancode.SDL_SCANCODE_KP_5,
    Numpad6 = SDL_Scancode.SDL_SCANCODE_KP_6,
    Numpad7 = SDL_Scancode.SDL_SCANCODE_KP_7,
    Numpad8 = SDL_Scancode.SDL_SCANCODE_KP_8,
    Numpad9 = SDL_Scancode.SDL_SCANCODE_KP_9,

    // Special keys
    Escape = SDL_Scancode.SDL_SCANCODE_ESCAPE,
    F1 = SDL_Scancode.SDL_SCANCODE_F1,
    F2 = SDL_Scancode.SDL_SCANCODE_F2,
    F3 = SDL_Scancode.SDL_SCANCODE_F3,
    F4 = SDL_Scancode.SDL_SCANCODE_F4,
    F5 = SDL_Scancode.SDL_SCANCODE_F5,
    F6 = SDL_Scancode.SDL_SCANCODE_F6,
    F7 = SDL_Scancode.SDL_SCANCODE_F7,
    F8 = SDL_Scancode.SDL_SCANCODE_F8,
    F9 = SDL_Scancode.SDL_SCANCODE_F9,
    F10 = SDL_Scancode.SDL_SCANCODE_F10,
    F11 = SDL_Scancode.SDL_SCANCODE_F11,
    F12 = SDL_Scancode.SDL_SCANCODE_F12,
    F13 = SDL_Scancode.SDL_SCANCODE_F13,
    F14 = SDL_Scancode.SDL_SCANCODE_F14,
    F15 = SDL_Scancode.SDL_SCANCODE_F15,
    F16 = SDL_Scancode.SDL_SCANCODE_F16,
    F17 = SDL_Scancode.SDL_SCANCODE_F17,
    F18 = SDL_Scancode.SDL_SCANCODE_F18,
    F19 = SDL_Scancode.SDL_SCANCODE_F19,
    F20 = SDL_Scancode.SDL_SCANCODE_F20,
    F21 = SDL_Scancode.SDL_SCANCODE_F21,
    F22 = SDL_Scancode.SDL_SCANCODE_F22,
    F23 = SDL_Scancode.SDL_SCANCODE_F23,
    F24 = SDL_Scancode.SDL_SCANCODE_F24,

    PrtScr = SDL_Scancode.SDL_SCANCODE_PRINTSCREEN,
    SysRq = SDL_Scancode.SDL_SCANCODE_SYSREQ,
    Pause = SDL_Scancode.SDL_SCANCODE_PAUSE,
    Insert = SDL_Scancode.SDL_SCANCODE_INSERT,
    Delete = SDL_Scancode.SDL_SCANCODE_DELETE,
    Home = SDL_Scancode.SDL_SCANCODE_HOME,
    End = SDL_Scancode.SDL_SCANCODE_END,
    PageUp = SDL_Scancode.SDL_SCANCODE_PAGEUP,
    PageDown = SDL_Scancode.SDL_SCANCODE_PAGEDOWN,
    Backquote = SDL_Scancode.SDL_SCANCODE_GRAVE,
    Minus = SDL_Scancode.SDL_SCANCODE_MINUS,
    Equals = SDL_Scancode.SDL_SCANCODE_EQUALS,
    LeftBracket = SDL_Scancode.SDL_SCANCODE_LEFTBRACKET,
    RightBracket = SDL_Scancode.SDL_SCANCODE_RIGHTBRACKET,
    Backslash = SDL_Scancode.SDL_SCANCODE_BACKSLASH,
    Semicolon = SDL_Scancode.SDL_SCANCODE_SEMICOLON,
    Apostrophe = SDL_Scancode.SDL_SCANCODE_APOSTROPHE,
    Comma = SDL_Scancode.SDL_SCANCODE_COMMA,
    Period = SDL_Scancode.SDL_SCANCODE_PERIOD,
    Slash = SDL_Scancode.SDL_SCANCODE_SLASH,
    CapsLock = SDL_Scancode.SDL_SCANCODE_CAPSLOCK,
    NumLock = SDL_Scancode.SDL_SCANCODE_NUMLOCKCLEAR,
    ScrollLock = SDL_Scancode.SDL_SCANCODE_SCROLLLOCK,
    LeftShift = SDL_Scancode.SDL_SCANCODE_LSHIFT,
    RightShift = SDL_Scancode.SDL_SCANCODE_RSHIFT,
    LeftControl = SDL_Scancode.SDL_SCANCODE_LCTRL,
    RightControl = SDL_Scancode.SDL_SCANCODE_RCTRL,
    LeftAlt = SDL_Scancode.SDL_SCANCODE_LALT,
    RightAlt = SDL_Scancode.SDL_SCANCODE_RALT,
    Menu = SDL_Scancode.SDL_SCANCODE_MENU,
    NumpadDecimal = SDL_Scancode.SDL_SCANCODE_KP_DECIMAL,
    NumpadDivide = SDL_Scancode.SDL_SCANCODE_KP_DIVIDE,
    NumpadMultiply = SDL_Scancode.SDL_SCANCODE_KP_MULTIPLY,
    NumpadSubtract = SDL_Scancode.SDL_SCANCODE_KP_MINUS,
    NumpadAdd = SDL_Scancode.SDL_SCANCODE_KP_PLUS,
    NumpadEnter = SDL_Scancode.SDL_SCANCODE_KP_ENTER,
    Space = SDL_Scancode.SDL_SCANCODE_SPACE,
    Enter = SDL_Scancode.SDL_SCANCODE_RETURN,
    Backspace = SDL_Scancode.SDL_SCANCODE_BACKSPACE,
    Tab = SDL_Scancode.SDL_SCANCODE_TAB,
    LeftArrow = SDL_Scancode.SDL_SCANCODE_LEFT,
    RightArrow = SDL_Scancode.SDL_SCANCODE_RIGHT,
    UpArrow = SDL_Scancode.SDL_SCANCODE_UP,
    DownArrow = SDL_Scancode.SDL_SCANCODE_DOWN,

    // System and unused keys. WARNING! AVOID USING THEM FOR BINDING KEYS IN GAME!
    Power = SDL_Scancode.SDL_SCANCODE_POWER, // Power button
    Sleep = SDL_Scancode.SDL_SCANCODE_SLEEP, // Sleep button
    Application = SDL_Scancode.SDL_SCANCODE_APPLICATION, // Application menu button
    Help = SDL_Scancode.SDL_SCANCODE_HELP, // Help button
    Clear = SDL_Scancode.SDL_SCANCODE_CLEAR, // Clear button
    Mode = SDL_Scancode.SDL_SCANCODE_MODE, // Mode switch button
    // Multimedia buttons
    MediaPlayPause = SDL_Scancode.SDL_SCANCODE_MUTE, // Play/Pause button (video/audio)
    MediaStop = SDL_Scancode.SDL_SCANCODE_STOP, // Stop button
    VolumeDown = SDL_Scancode.SDL_SCANCODE_VOLUMEDOWN, // Decrease volume button
    VolumeUp = SDL_Scancode.SDL_SCANCODE_VOLUMEUP, // Increase volume button
    MediaRewind = SDL_Scancode.SDL_SCANCODE_MEDIA_REWIND, // Rewind button
    MediaFastForward = SDL_Scancode.SDL_SCANCODE_MEDIA_FAST_FORWARD, // Fast forward button
    MediaEject = SDL_Scancode.SDL_SCANCODE_MEDIA_EJECT, // Eject disk button
    MediaPlay = SDL_Scancode.SDL_SCANCODE_MEDIA_PLAY, // Play button
    MediaPause = SDL_Scancode.SDL_SCANCODE_MEDIA_PAUSE, // Pause button
    MediaRecord = SDL_Scancode.SDL_SCANCODE_MEDIA_RECORD, // Record button
    MediaSelect = SDL_Scancode.SDL_SCANCODE_MEDIA_SELECT, // Media select button

    // TODO: Gamepad buttons
}

public enum MouseKey: uint {
    Left = SDL_MouseButtonFlags.SDL_BUTTON_LMASK,
    Right = SDL_MouseButtonFlags.SDL_BUTTON_RMASK,
    Middle = SDL_MouseButtonFlags.SDL_BUTTON_MMASK,
    X1 = SDL_MouseButtonFlags.SDL_BUTTON_X1MASK,
    X2 = SDL_MouseButtonFlags.SDL_BUTTON_X2MASK,
}