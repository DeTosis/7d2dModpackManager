using System;

namespace _7d2dModpackManager; 
static class Logger {
    public static event EventHandler<string> LogEvent;
    private static void OnLogEvent(string logMsg) {
        var handler = LogEvent;
        handler?.Invoke(null, logMsg);
    }

    public enum LogType {
        DEBUG,
        WARNING,
        SUCCESS,
        EXCEPTION,
        INFO,
    }

    public static void Log(string logMsg, LogType logType) {
        switch (logType) {
            case LogType.DEBUG:
                OnLogEvent($"[ * ] {logMsg}");
                break;
            case LogType.WARNING:
                OnLogEvent($"[ ! ] {logMsg}");
                break;
            case LogType.SUCCESS:
                OnLogEvent($"[ + ] {logMsg}");
                break;
            case LogType.EXCEPTION:
                OnLogEvent($"[ - ] {logMsg}");
                break;
            case LogType.INFO:
                OnLogEvent($"[ I ] {logMsg}");
                break;
        }
    }

    public static void CommandNotFound() {
        Logger.Log("Command not found", LogType.EXCEPTION);
    }
}
