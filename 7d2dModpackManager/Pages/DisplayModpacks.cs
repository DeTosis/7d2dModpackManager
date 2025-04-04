using _7d2dModpackManager.Input;
using System;
using System.IO;
using System.Linq;

namespace _7d2dModpackManager.Pages; 
class DisplayModpacks {
    public DisplayModpacks() {
        PageController.PageSwitched += OnStartupPage;
        InputProcessor.UserInputEvent += OnUserInput;
    }

    private void OnUserInput(object? sender, string e) {
        if (PageController.activePage != PageController.MPMPage.Display) return;

        if (e == "-b") {
            PageController.SwitchPage(PageController.MPMPage.Main);
            return;
        } 
    }

    private void OnStartupPage(object? sender, PageController.MPMPage e) {
        if (PageController.activePage != PageController.MPMPage.Display) return;

        ConsoleVisualizer cv = new(16);
        cv.Splitter();
        cv.TitleDisplay("| Registered packs |");

        int packCount = 0;
        foreach (var item in Directory.GetDirectories(userDefault.MPMPath)) {
            packCount++;
            cv.CommandDisplay($"[{packCount}]",item.Split(@"\").Last());
        }
        cv.CommandDisplay("Pack count", packCount.ToString());

        Console.WriteLine();
        cv.CommandDisplay("-b", "Return back to main menu");
    }
}
