using System;
using System.Linq;

namespace _7d2dModpackManager; 
class ConsoleVisualizer {
    int consoleWidth;
    public int maxCommandLength = 8;

    public ConsoleVisualizer() { consoleWidth = Console.WindowWidth; }
    public ConsoleVisualizer(int maxCommandLength) {
        this.maxCommandLength = maxCommandLength;
        consoleWidth = Console.WindowWidth;
    }

    public void TitleDisplay(string title) {
        Console.WriteLine(
            String.Concat(Enumerable.Repeat(" ", (consoleWidth / 2) - title.Length / 2)) + title);
    }

    public void Splitter() {
        Console.WriteLine();
        //Console.WriteLine(String.Concat(Enumerable.Repeat("-", consoleWidth)));
    }

    public void CommandDisplay(string cmd, string desc) {
        string offset = String.Concat(Enumerable.Repeat(" ", maxCommandLength - cmd.Length));
        Console.WriteLine($"[{cmd}]{offset}{desc}");
    }

    public void UserPrompt(string prompt) {
        Console.WriteLine(prompt);
    }
}
