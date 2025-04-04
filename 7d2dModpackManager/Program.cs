using Microsoft.VisualBasic;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Channels;

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

public static class DirectoryManager {
    public static void CopyDirectory(string src, string dst) {
        var dir = new DirectoryInfo(src);
        DirectoryInfo[] dirs = dir.GetDirectories();


        Directory.CreateDirectory(dst);

        foreach (FileInfo file in dir.GetFiles()) {
            string targetFilePath = Path.Combine(dst, file.Name);
            file.CopyTo(targetFilePath);
        }

        foreach (DirectoryInfo subDir in dirs) {
            string newDestinationDir = Path.Combine(dst, subDir.Name);
            CopyDirectory(subDir.FullName, newDestinationDir);
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

        ClearModsFolder();

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

    static void ClearModsFolder() {
        foreach (var i in Directory.GetDirectories(Supplimentary.gameFolder + Supplimentary.modFolder)) {
            Directory.Delete(i, true);
        }

        foreach (var i in Directory.GetFiles(Supplimentary.gameFolder + Supplimentary.modFolder)) {
            File.Delete(i);
        }
    }

    static void ChangePath(string src) {
        Console.WriteLine($"Enter a new path for {src}");
        string newP = Console.ReadLine();
        newP = newP.TrimEnd();

        if (!Directory.Exists(newP)) {
            Console.WriteLine("[ - ] Cannot find location specified");
            return;
        }

        if (src == Supplimentary.mpmFolder) {
            if (Directory.GetDirectories(newP).Length > 0) {
                Console.WriteLine("[ - ] Destination directory must be empty");
                return;
            }

            Supplimentary.SetMPMFolder(newP);
            Console.WriteLine("[ + ] Path changed successfully");
            return;
        }

        if (src == Supplimentary.gameFolder) {
            foreach (var file in Directory.GetFiles(newP)) {
                if (file.Split(@"\").Last() == Supplimentary.exeName) {
                    Supplimentary.SetGameFolder(newP);
                    Supplimentary.exePath = file;
                    Console.WriteLine("[ + ] Path changed successfully");
                    return;
                }
            }
            Console.WriteLine("[ - ] Can not load game directory");
        }
    }

    static bool startGame = true;
    private static void OpenSettings() {
        Console.WriteLine($"[0] Game Path: {Supplimentary.gameFolder}");
        Console.WriteLine($"[1] MPM Path: {Supplimentary.mpmFolder}");
        Console.WriteLine($"[2] Start game on modpack load: {startGame.ToString()}\n");
        Console.WriteLine("Choose what do you want to modify: ");
        Console.Write(" > ");

        string input = Console.ReadLine();
        input = input.TrimEnd();

        switch (input) {
            case "0":
                ChangePath(Supplimentary.gameFolder + Supplimentary.modFolder);
                break;
            case "1":
                ChangePath(Supplimentary.mpmFolder);
                break;
            case "2":
                startGame = !startGame;
                Console.WriteLine($"[ + ] Start game on modpack load: {startGame.ToString()}");
                break;
            default:
                Console.WriteLine("[ - ] Invalid command");
                break;
        }
    }

    static void getInput() {
        Console.WriteLine("---------------------------------------------------");
        Console.Write(" > ");

        string input = Console.ReadLine();
        input = input.TrimEnd();

        switch (input) {
            case "-w":
                DisplayDirectories();
                break;
            case "-add":
                SaveModpack();
                break;
            case "-l":
                LoadModPack();
                break;
            case "-rm":
                break;
            case "-clr":
                ClearModsFolder();
                break;
            case "-s":
                OpenSettings();
                break;
            default:
                Console.WriteLine("Command is not allowed");
                break;
        }
    }

    public static void Main() {
        if (!_INIT_()) {
            Console.WriteLine("[ - ] Initialization failed");
            return;
        } else {
            Console.WriteLine("[ + ] Initialized correctly");
        }


        Console.WriteLine("---------------------------------------------------");
        Console.WriteLine("[-w]   Watch saved modpacks");
        Console.WriteLine("[-l]   Load modpack");
        Console.WriteLine("[-add] Add new modpack from the game mods folder");
        Console.WriteLine("[-clr] Clear current game mods folder");
        Console.WriteLine("[-s]   Open settings");

        while (true) {
            getInput();
        }
    }

    private static bool _INIT_() {
        var game = Process.GetProcessesByName("7daystodie");
        if (game.Length > 0) {
            Console.WriteLine("[ - ] Close game to use mpm");
            return false;
        }

        if (!Directory.Exists(Supplimentary.mpmFolder)) {
            Directory.CreateDirectory(Supplimentary.mpmFolder);
        }

        if (!Directory.Exists(Supplimentary.gameFolder + Supplimentary.modFolder)) {
            Console.WriteLine("[ - ] Can not find game folder, please specify it: \n");
            OpenSettings();
            return false;
        }

        foreach (var file in Directory.GetFiles(Supplimentary.gameFolder)) {
            if (file.Split(@"\").Last() == Supplimentary.exeName) {
                Supplimentary.exePath = file;
                Console.WriteLine("[ + ] EXE file found");
                return true;
            }
        }
        Console.WriteLine("[ - ] Could not found game exe");

        return false;
    }
}
