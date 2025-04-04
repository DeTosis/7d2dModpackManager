using _7d2dModpackManager.Input;
using _7d2dModpackManager.Pages;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace _7d2dModpackManager;

public static class Supplimentary {
    public static string gameFolder { get; private set; } = @"C:\Program Files (x86)\Steam\steamapps\common\7 Days To Die";
    public static string modFolder = @"\Mods";
    public static string exePath { get; set; } = "";
    public const string exeName = "7DaysToDie_EAC.exe";
    public static string mpmFolder { get; private set; } = $@"{appDataFolder}\7d2d_MPM";
    private static string appDataFolder => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

    public static void SetMPMFolder(string path) {
        mpmFolder = path + @"\7d2d_MPM";
    }

    public static void SetGameFolder(string path) {
        if (path[path.Length - 1] == '\\') {
            gameFolder = path.TrimEnd('\\');
        } else {
            gameFolder = path;
        }
    }
}

internal class Program {
    static void DisplayDirectories() {
        if (Directory.GetDirectories(Supplimentary.mpmFolder).Length == 0) {
            Console.WriteLine("No modpacks awailable");
            return;
        }

        int modPackCounter = 0;
        foreach (var mp in Directory.GetDirectories(Supplimentary.mpmFolder)) {
            Console.WriteLine($"[{modPackCounter}] " + mp.Split(@"\").Last());
            modPackCounter++;
        }

        Console.WriteLine($"Total modpacks loaded {modPackCounter}");
    }

    static void SaveModpack() {
        Console.WriteLine("Enter a modpack name:");
        Console.Write(" > ");

        string packName = Console.ReadLine();
        DirectoryManager.CopyDirectory(
            Supplimentary.gameFolder + Supplimentary.modFolder, Supplimentary.mpmFolder + $@"\{packName}");
        Console.WriteLine("[ + ] Saved Sucessfully");
    }

    static void LoadModPack() {
        Console.WriteLine("Enter a modpack name:");
        Console.Write(" > ");

        string packName = Console.ReadLine();
        if (String.IsNullOrEmpty(packName)) {
        if (!Directory.Exists(Supplimentary.mpmFolder + $@"\{packName}")) {
                Console.WriteLine("[ - ] Pack does not exist");
                return;
            }
        }

        DirectoryManager.CopyDirectory(Supplimentary.mpmFolder + $@"\{packName}", Supplimentary.gameFolder + Supplimentary.modFolder);
        Console.WriteLine("[ + ] Modpack Loaded");
        if (startGame) {
            {
                Process p = new();
                p.StartInfo = new ProcessStartInfo($@"{Supplimentary.exePath}");
                p.Start();

                Console.WriteLine("[ I ] Awaiting game (up to 30s)");

                int seconds = 0;
                while (seconds < 30) {
                    Thread.Sleep(1000);
                    var game = Process.GetProcessesByName("7daystodie");

                    if (game.Length > 0) {
                        Console.WriteLine("[ + ] Game started");
                        return;
                    }
                    seconds++;
                }
                Console.WriteLine("[ - ] Can not load game status");
            }
        }
    }

    static bool startGame = true;

    public static void Main() {
        Logger.LogEvent += Log;

        Init init = new();
        MainPage startup = new();

        SettingsPage settingsPage = new();
        DisplayModpacks displayModpacks = new();
        AddPackPage addPackPage = new();

        PageController.SwitchPage(PageController.MPMPage.Init);
        init.CheckInitStatus();
        Thread.Sleep(1000);

        PageController.SwitchPage(PageController.MPMPage.Main);

        while (true) {
            InputProcessor.GetInput();
        }
    }

    private static void Log(object? sender, string e) {
        Console.WriteLine(e);
    }
}
