using _7d2dModpackManager.Input;
using System;
using System.IO;
using System.Linq;

namespace _7d2dModpackManager.Pages; 
class SettingsPage {
    ConsoleVisualizer cv;
    public SettingsPage() {
        PageController.PageSwitched += OnPageSwitched;
        InputProcessor.UserInputEvent += OnUserInput;
        cv = new(8);
        currentState = pageState.MAIN;
    }

    public enum pageState {
        MAIN,
        GAMEPATH,
    }
    pageState currentState;

    private void OnUserInput(object? sender, string e) {
        if (PageController.activePage != PageController.MPMPage.Settings) return;

        if (e == "-b") {
            PageController.SwitchPage(PageController.MPMPage.Main);
            return;
        }

        if (currentState == pageState.MAIN) {
            switch (e) {
                case "0":
                    cv.UserPrompt("\nEnter path to your 7 Days To Die folder: ");
                    currentState = pageState.GAMEPATH;
                    return;
                case "1":
                    userDefault.autoStartGame = !userDefault.autoStartGame;
                    Console.WriteLine();
                    Logger.Log($"Autorun is set to {userDefault.autoStartGame}", Logger.LogType.SUCCESS);
                    userDefault.saveConfig.Invoke();
                    break;
            }
        }

        if (currentState == pageState.GAMEPATH) {
            if (ChangeGamePath(e)) {
                userDefault.saveConfig.Invoke();
            }
        }
    }

    private void OnPageSwitched(object? sender, PageController.MPMPage e) {
        if (e != PageController.MPMPage.Settings) return;
        DisplayPageContent();
    }

    private void DisplayPageContent() {
        cv.TitleDisplay("| Settings |");
        Console.WriteLine();

        cv.CommandDisplay(
            "0", $"Change current game path:\n{String.Concat(Enumerable.Repeat(" ", cv.maxCommandLength + 2))}[{userDefault.gameDir}]");

        cv.CommandDisplay(
            "1", $"Run game when pack is loaded:\n{String.Concat(Enumerable.Repeat(" ", cv.maxCommandLength + 2))}[{userDefault.autoStartGame.ToString()}]");

        cv.CommandDisplay("-b", "Return back to main menu");
    }

    private bool ChangeGamePath(string input) {
        if (!Directory.Exists(input)) {
            Logger.Log("Game directory could not be found", Logger.LogType.EXCEPTION);
            return false;
        }

        foreach (var file in Directory.GetFiles(input)) {
            if (file.Split(@"\").Last() == userDefault.exeName) {
                Supplimentary.SetGameFolder(input);
                Supplimentary.exePath = file;
                Logger.Log("Path changed successfully", Logger.LogType.SUCCESS);
                return true;
            }
        }
        Logger.Log("Can not load game directory", Logger.LogType.EXCEPTION);
        return false;
    }
}
