using _7d2dModpackManager.Input;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace _7d2dModpackManager.Pages;

public static class userDefault {
    public static Func<bool> saveConfig;
    public static bool autoStartGame = false;

    public static string gameDir = @"C:\Program Files (x86)\Steam\steamapps\common\7 Days To Die";
    public static string modsFolder = @"\Mods";
    public static string exePath = "";
    public static string exeName = @"7DaysToDie_EAC.exe";

    public static string MPMPath = $@"{AppData}\MPMData";
    public static string MPMcfg = @"\meta.json";
    static string AppData => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
}

class Init {
    public class Settings {
        public UserPrefs userPrefs;
        public Settings(string gameDir, bool autoStartGame) {
            userPrefs = new(gameDir, autoStartGame);
        }

        public class UserPrefs {
            public string gameDir;
            public bool autoStartGame;

            public UserPrefs(string gameDir, bool autoStartGame) {
                this.gameDir = gameDir;
                this.autoStartGame = autoStartGame;
            }
        }
    }

    bool configLoaded = false;
    bool initComplete = false;

    public Init() {
        PageController.PageSwitched += OnInitPage;
    }

    private void OnInitPage(object? sender, PageController.MPMPage e) {
        if (e == PageController.MPMPage.Init) {
            _INIT_();
        }
    }

    public bool CheckInitStatus() {
        if (initComplete) {
            Logger.Log("Initialization successfull", Logger.LogType.SUCCESS);
        } else {
            Logger.Log("Initialization failed", Logger.LogType.EXCEPTION);
        }
        return initComplete;
    }

    private void DrawTitle() {
        ConsoleVisualizer cv = new();
        cv.Splitter();
        cv.TitleDisplay("| INITIALIZATION |");
    }

    private void _INIT_() {
        DrawTitle();

        userDefault.saveConfig = SaveModpackConfig;
        
        if (!SetupMPM()) return;
        if (!SetupGameFolder()) return;
        if (!SaveModpackConfig()) return;
        if (!LocateGameExe()) return;

        initComplete = true;
    }

    private bool LoadMPMConfig(string cfgPath) {
        try {
            string input = File.ReadAllText(cfgPath);
            if (String.IsNullOrEmpty(input)) {
                throw new Exception("Config file is empty or incompatable");
            }

            Settings sett = JsonConvert.DeserializeObject<Settings>(input);

            userDefault.gameDir = sett.userPrefs.gameDir;
            userDefault.autoStartGame = sett.userPrefs.autoStartGame;

        } catch (Exception e){
            Logger.Log($"EXC: {e.Message}", Logger.LogType.EXCEPTION);
            return false;
        }
        return true;
    }

    public bool SaveModpackConfig() {
        Settings sett = new(userDefault.gameDir, userDefault.autoStartGame);
        string output = JsonConvert.SerializeObject(sett, Formatting.Indented);

        try {
            File.WriteAllText(userDefault.MPMPath + userDefault.MPMcfg, output);
            if (configLoaded) {
                Logger.Log("Config saved", Logger.LogType.SUCCESS);
            } else {
                Logger.Log("Config generated", Logger.LogType.SUCCESS);
            }
        } catch (Exception e) {
            Logger.Log($"EXC: {e.Message}", Logger.LogType.EXCEPTION);
            return false;
        }
        return true;
    }

    private bool SetupMPM() {
        if (!Directory.Exists(userDefault.MPMPath)) {
            Directory.CreateDirectory(userDefault.MPMPath);
            Logger.Log("Root directory created", Logger.LogType.INFO);
        } else {
            Logger.Log("Root directory found", Logger.LogType.SUCCESS);
        }

        if (!File.Exists(userDefault.MPMPath + "\\" + userDefault.MPMcfg)) {
            File.Create(userDefault.MPMPath + "\\" + userDefault.MPMcfg).Close();
            Logger.Log("Config file created", Logger.LogType.INFO);
        } else {
            Logger.Log("Config file found", Logger.LogType.SUCCESS);
            if (LoadMPMConfig(userDefault.MPMPath + "\\" + userDefault.MPMcfg)) {
                configLoaded = true;
                Logger.Log("Config file loaded", Logger.LogType.SUCCESS);
            } else {
                configLoaded = false;
                Logger.Log("Can not load config file", Logger.LogType.WARNING);
            }
        }
        return true;
    }

    private bool SetupGameFolder() {
        if (!Directory.Exists(userDefault.gameDir)) {
            Logger.Log("Could not find game directory", Logger.LogType.EXCEPTION);
            return false;
        } else {
            Logger.Log("Game directory found", Logger.LogType.SUCCESS);
        }

        if (!Directory.Exists(userDefault.gameDir + userDefault.modsFolder)) {
            Directory.CreateDirectory(userDefault.gameDir + userDefault.modsFolder);
            Logger.Log("Mods directory created", Logger.LogType.INFO);
        } else {
            Logger.Log("Mods directory found", Logger.LogType.SUCCESS);
        }
        return true;
    }

    private bool LocateGameExe() {
        foreach (var item in Directory.GetFiles(userDefault.gameDir)) {
            if (item.Split(@"\").Last() == userDefault.exeName) {
                userDefault.exePath = item;
                Logger.Log("Game .exe found", Logger.LogType.SUCCESS);
                return true;
            }
        }
        Logger.Log("Game .exe can not be found", Logger.LogType.EXCEPTION);
        return false;
    }
}
