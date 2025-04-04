using _7d2dModpackManager.Input;
using System.IO;
using System;

namespace _7d2dModpackManager.Pages; 
class AddPackPage {
    public AddPackPage() {
        PageController.PageSwitched += OnStartupPage;
        InputProcessor.UserInputEvent += OnUserInput;
    }

    private void OnUserInput(object? sender, string e) {
        if (PageController.activePage != PageController.MPMPage.Add) return;

        if (e == "-b") {
            PageController.SwitchPage(PageController.MPMPage.Main);
            return;
        }
    }

    private void OnStartupPage(object? sender, PageController.MPMPage e) {
        if (PageController.activePage != PageController.MPMPage.Add) return;

        ConsoleVisualizer cv = new(16);
        cv.Splitter();
        cv.TitleDisplay("| Create a mod pack |");



        Console.WriteLine();
        cv.CommandDisplay("-b", "Return back to main menu");
    }
}
