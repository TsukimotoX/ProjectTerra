using System;
using System.IO;

namespace ProjectTerra.iOS;

[Register("AppDelegate")]
public class AppDelegate : UIApplicationDelegate
{
    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
        CrashLogger.Initialize();
        Game.Initialize();
        return true;
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
