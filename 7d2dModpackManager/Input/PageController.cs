using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace _7d2dModpackManager.Input; 

static class PageController {
    public enum MPMPage {
        None,
        Init,
        Main,
        Settings,
        Add,
        Remove,
        Display,
    }

    private static MPMPage ActivePage = MPMPage.None;
    public static MPMPage activePage {
        get { 
            return ActivePage; 
        }
        private set {
            Console.Clear();
            ActivePage = value;
            OnPageSwitched();
        }
    }

    public static event EventHandler<MPMPage>? PageSwitched;
    private static void OnPageSwitched() {
        var handler = PageSwitched;
        handler?.Invoke(null, ActivePage);
    }

    public static void SwitchPage(MPMPage targetPage) {
        activePage = targetPage;
    }
}
