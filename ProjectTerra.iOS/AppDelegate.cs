using SDL;
using System.Runtime.InteropServices;

namespace ProjectTerra.iOS;

[Register("AppDelegate")]
public class AppDelegate : UIApplicationDelegate
{
    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
        CrashLogger.Initialize();

        NativeLibrary.SetDllImportResolver(typeof(SDL3).Assembly, (_, assembly, path) =>
        {
            string frameworkPath = "/Frameworks/SDL3.framework/SDL3";
            Console.WriteLine($"Loading SDL3 from: {frameworkPath}");
            return NativeLibrary.Load(frameworkPath, assembly, path);
        });

        if (!SDL3.SDL_Init(SDL_InitFlags.SDL_INIT_VIDEO))
        {
            Console.WriteLine($"Failed to initialize SDL: {SDL3.SDL_GetError()}");
            return false;
        }

        Game.Initialize();
        return true;
    }

    public override void OnResignActivation(UIApplication application)
    {
        Console.WriteLine("Application is going to background.");
    }

    public override void DidEnterBackground(UIApplication application)
    {
        Console.WriteLine("Application entered background.");
    }

    public override void WillEnterForeground(UIApplication application)
    {
        Console.WriteLine("Application will enter foreground.");
    }

    public override void OnActivated(UIApplication application)
    {
        Console.WriteLine("Application is active.");
    }

    public override void WillTerminate(UIApplication application)
    {
        Console.WriteLine("Application is terminating.");
        SDL3.SDL_Quit(); // Корректное завершение SDL
    }
}

public class CrashLogger
{
    private static readonly string DocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    private static readonly string LogsDirectory = Path.Combine(DocumentsPath, "Logs");
    private static string LogFilePath => Path.Combine(LogsDirectory, $"log_{DateTime.Now:yyyyMMddHHmm}.txt");

    public static void Initialize()
    {
        // Создаём папку для логов при инициализации
        if (!Directory.Exists(LogsDirectory))
        {
            Directory.CreateDirectory(LogsDirectory);
        }

        // Подписываемся на обработчики исключений
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;

        // Для проверки добавляем тестовый лог
        WriteLog("CrashLogger initialized.\n");
    }

    private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        string logMessage = $"[UnhandledException] {DateTime.Now}\n{e.ExceptionObject}\n\n";
        WriteLog(logMessage);
    }

    private static void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        string logMessage = $"[UnobservedTaskException] {DateTime.Now}\n{e.Exception}\n\n";
        WriteLog(logMessage);
    }

    private static void WriteLog(string message)
    {
        try
        {
            File.AppendAllText(LogFilePath, message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to write crash log: {ex.Message}");
        }
    }
}
