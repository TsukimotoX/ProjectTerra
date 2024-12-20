using System;
using System.IO;
using Xamarin.Essentials;

namespace ProjectTerra.iOS;

[Register("AppDelegate")]
public class AppDelegate : UIApplicationDelegate
{
    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
        Game.Initialize();

        return true;
    }
}

public class CrashLogger
{
    private static string LogFilePath => Path.Combine(FileSystem.AppDataDirectory, $"log_{DateTime.Now:yyyyMMddHHmm}.txt");

    public static void Initialize()
    {
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
    }

    private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        string logMessage = $"[UnhandledException] {DateTime.Now}\n{e.ExceptionObject}\n\n";
        WriteLog(logMessage);
    }

    private static void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
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
