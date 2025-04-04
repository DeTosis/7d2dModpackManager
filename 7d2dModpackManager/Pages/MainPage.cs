using _7d2dModpackManager.Input;

namespace _7d2dModpackManager.Pages; 
class MainPage {
    ConsoleVisualizer cv = new(10);
    public MainPage() {
        PageController.PageSwitched += OnStartupPage;
        InputProcessor.UserInputEvent += OnUserInput;
    }

    private void OnUserInput(object? sender, string e) {
        if (PageController.activePage != PageController.MPMPage.Main) return;
        switch (e) {
            case "-s":
                PageController.SwitchPage(PageController.MPMPage.Settings);
                break;
            case "-d":
                PageController.SwitchPage(PageController.MPMPage.Display);
                break;
            case "-add":
                PageController.SwitchPage(PageController.MPMPage.Add);
                break;
            case "-rm":
                PageController.SwitchPage(PageController.MPMPage.Remove);
                break;
            default:
                Logger.CommandNotFound();
                break;
        }
    }

    private void OnStartupPage(object? sender, PageController.MPMPage e) {
        if (e == PageController.MPMPage.Main) {
            DrawVisuals();
        }
    }

    private void DrawVisuals() {
        cv.Splitter();

        cv.TitleDisplay("| Main |");

        cv.CommandDisplay("-s", "Open settings");
        cv.CommandDisplay("-d", "Display or load saved modpacks");
        cv.CommandDisplay("-add", "Add new modpack from the game mods folder");
        cv.CommandDisplay("-rm", "Remove existing modpack");
    }
}
