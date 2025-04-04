using System;
using System.Threading;
using System.Threading.Tasks;

namespace _7d2dModpackManager.Input;
static class InputProcessor {

    public static event EventHandler<string> UserInputEvent;
    private static void OnUserInputEvent(string input) {
        var handler = UserInputEvent;
        handler?.Invoke(null,input);
    }

    public static void GetInput() {
        Console.Write(" > ");
        string? input = Console.ReadLine();

        input = input.TrimEnd();
        input = input.ToLower();

        OnUserInputEvent(input);
    }
}