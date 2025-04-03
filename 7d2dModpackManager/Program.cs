using Microsoft.VisualBasic;
using System;
using System.IO;
using System.Linq;

namespace _7d2dModpackManager;

public static class Supplimentary {
    public static string gamePath = @"C:\Program Files (x86)\Steam\steamapps\common\7 Days To Die\Mods";
    public static string mpmFolder = $@"{appDataFolder}\7d2d_MPM";
    private static string appDataFolder => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
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
        DirectoryManager.CopyDirectory(Supplimentary.gamePath, Supplimentary.mpmFolder + $@"\{packName}");
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

        DirectoryManager.CopyDirectory(Supplimentary.mpmFolder + $@"\{packName}", Supplimentary.gamePath);
        Console.WriteLine("[ + ] Modpack Loaded");
    }

    static void ClearModsFolder() {
        foreach (var i in Directory.GetDirectories(Supplimentary.gamePath)) {
            Directory.Delete(i, true);
        }

        foreach (var i in Directory.GetFiles(Supplimentary.gamePath)) {
            File.Delete(i);
        }
    }

    static void getInput() {
        Console.WriteLine("---------------------------------------------------");
        Console.Write(" > ");

        string input = Console.ReadLine();

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
            default:
                Console.WriteLine("Command is not allowed");
                break;
        }
    }

    public static void Main() {
        Console.WriteLine("---------------------------------------------------");
        Console.WriteLine("[-w] Watch saved modpacks");
        Console.WriteLine("[-l] Load modpack");
        Console.WriteLine("[-add] Add new modpack from the game mods folder");
        Console.WriteLine("[-clr] Clear current game mods folder");

        while (true) {
            getInput();
        }
    }
}
