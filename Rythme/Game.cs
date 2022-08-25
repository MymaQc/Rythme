using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using WindowsInput.Native;
using WindowsInput;

namespace Rythme {
    
    public class Track {
        
        public Track(string etrack1, string etrack2, string etrack3, string etrack4) {
            Etrack1 = etrack1;
            Etrack2 = etrack2;
            Etrack3 = etrack3;
            Etrack4 = etrack4;
        }

        public string Etrack1 { get; }
        public string Etrack2 { get; }
        public string Etrack3 { get; }
        public string Etrack4 { get; }
        
    }

    internal static class Game {
        
        private static ConsoleKeyInfo currentKeyInfo;

        private static void Main(params string[] colorParam) {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            int width = 40;
            string color = colorParam.Length > 0 ? colorParam[0] : "White";

            Console.CursorVisible = false;

            string[] files = Directory.GetFiles("../../../json/");
            List<string> fileNames = new List<string>();

            foreach (string filePath in files) {
                string fileName = "";

                for (int u = 14; u < filePath.Length - 5; u++) {
                    fileName += filePath[u];
                }

                fileNames.Add(fileName);
            }

            Console.WriteLine(fileNames);

            line(ref width);
            string tempText = "Rythm game";
            centerWrite(ref tempText, ref width);
            
            tempText = "Press a to start";
            leftWrite(ref tempText, ref width, true);
            tempText = "Press s to enter settings";
            leftWrite(ref tempText, ref width, true);
            bLine(ref width);         
            
            while (true) {
                ConsoleKeyInfo currentKey = Console.ReadKey();

                switch (currentKey.KeyChar) {
                    case 's':
                    case 'S':
                        Console.Clear();
                        settings(ref width, ref color);
                        break;
                    case 'a':
                    case 'A':
                        Console.Clear();
                        levelSelect(ref width, ref color);
                        break;
                }
            }
        }

        private static void play(ref int width, ref string colorParam) {
            string color = colorParam;
            int[] currentLine = { 0, 0, 0, 0 };         
            int score = 0;
            int missed = 0;
            int combo = 0;

            string track1;
            string track2;
            string track3;
            string track4;
            
            using (StreamReader r = new StreamReader("../../../json/test.json")) {
                string json = r.ReadToEnd();
                Track track = JsonConvert.DeserializeObject<Track>(json);

                track1 = track.Etrack1;
                track2 = track.Etrack2;
                track3 = track.Etrack3;
                track4 = track.Etrack4;
            }
            
            Task.Run(keyTest);
            
            Console.Clear();

            for (int i = 0; i < track1.Length; i++) {
                char currentKey = currentKeyInfo.KeyChar;
                int bufferedWidth = width + 5;

                if (i + width >= track1.Length) {
                    i = 0;
                }

                Console.SetCursorPosition(0, 0);

                line(ref bufferedWidth);

                ConsoleColor consoleColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color, true);
                Console.ForegroundColor = consoleColor;

                if (currentKey == 'd' || currentKey == 'D') {
                    Console.ForegroundColor = color == "Blue" ? ConsoleColor.Yellow : ConsoleColor.Blue;
                    simulateTypingText("z");
                }

                Console.SetCursorPosition(0, 0);

                Console.Write("| D|");

                for (int u = 0; u < width; u++) {    
                    if (track1[u + i] == 'X') {
                        Console.Write("X");
                        currentLine[0] = 1;
                    } else {
                        Console.Write("-");
                        currentLine[0] = 0;
                    }
                }

                consoleColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color, true);
                Console.ForegroundColor = consoleColor;

                if (currentKey == 'f' || currentKey == 'F') {
                    Console.ForegroundColor = color == "Green" ? ConsoleColor.Yellow : ConsoleColor.DarkGreen;
                    simulateTypingText("z");
                }
                
                Console.Write("|");
                Console.WriteLine();
                Console.Write("| F|");

                for (int u = 0; u < width; u++) {
                    if (track2[u + i] == 'X') {
                        Console.Write("X");
                        currentLine[1] = 1;
                    } else {
                        Console.Write("-");
                        currentLine[1] = 0;
                    }
                }

                consoleColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colorParam, true);
                Console.ForegroundColor = consoleColor;

                if (currentKey == 'j' || currentKey == 'J') {
                    Console.ForegroundColor = color == "DarkMagenta" ? ConsoleColor.Yellow : ConsoleColor.Magenta;
                    simulateTypingText("z");
                }
                
                Console.Write("|");
                Console.WriteLine();
                Console.Write("| J|");

                for (int u = 0; u < width; u++) {
                    if (track3[u + i] == 'X') {
                        Console.Write("X");
                        currentLine[2] = 1;
                    } else {
                        Console.Write("-");
                        currentLine[2] = 0;
                    }
                }

                consoleColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colorParam, true);
                Console.ForegroundColor = consoleColor;

                if (currentKey == 'k' || currentKey == 'K') {
                    Console.ForegroundColor = color == "Yellow" ? ConsoleColor.Red : ConsoleColor.DarkYellow;
                    simulateTypingText("z");
                }
                
                Console.Write("|");
                Console.WriteLine();
                Console.Write("| K|");

                for (int u = 0; u < width; u++) {
                    if (track4[u + i] == 'X') {
                        Console.Write("X");
                        currentLine[3] = 1;
                    } else {
                        Console.Write("-");
                        currentLine[3] = 0;
                    }

                }
                
                Console.Write("|");
                Console.WriteLine();
                consoleColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colorParam, true);
                Console.ForegroundColor = consoleColor;
                bLine(ref bufferedWidth);

                switch (track1[i]) {
                    case 'X' when currentKey == 'd':
                        score += 10;
                        combo++;
                        break;
                    case 'x' when currentKey == 'd':
                        score += 5;
                        combo++;
                        break;
                    case 'X' when currentKey != 'd':
                        missed++;
                        combo = 0;
                        break;
                    case '-' when currentKey == 'd':
                        missed++;
                        combo = 0;
                        break;
                }

                switch (track2[i]) {
                    case 'X' when currentKey == 'f':
                        score += 10;
                        combo++;
                        break;
                    case 'x' when currentKey == 'f':
                        score += 5;
                        combo++;
                        break;
                    case 'X' when currentKey != 'f':
                        missed++;
                        combo = 0;
                        break;
                    case '-' when currentKey == 'd':
                        missed++;
                        combo = 0;
                        break;
                }

                switch (track3[i]) {
                    case 'X' when currentKey == 'j':
                        score += 10;
                        combo++;
                        break;
                    case 'x' when currentKey == 'j':
                        score += 5;
                        combo++;
                        break;
                    case 'X' when currentKey != 'j':
                        missed++;
                        combo = 0;
                        break;
                    case '-' when currentKey == 'd':
                        missed++;
                        combo = 0;
                        break;
                }

                switch (track4[i]) {
                    case 'X' when currentKey == 'k':
                        score += 10;
                        break;
                    case 'x' when currentKey == 'k':
                        score += 5;
                        break;
                    case 'X' when currentKey != 'k':
                        missed++;
                        combo = 0;
                        break;
                    case '-' when currentKey == 'd':
                        missed++;
                        combo = 0;
                        break;
                }           

                Console.WriteLine();
                
                Console.Write(currentLine[0]);
                Console.Write(currentLine[1]);
                Console.Write(currentLine[2]);
                Console.WriteLine(currentLine[3]);
                Console.WriteLine("Score: {0}", score); 
                Console.WriteLine("Combo: {0}", combo);
                Console.WriteLine("Missed: {0}", missed);               
                Thread.Sleep(100);
            }
        }

        private static void keyTest() {
            while (true) {
                currentKeyInfo = Console.ReadKey(true);
                if (currentKeyInfo.Key == ConsoleKey.Enter) {
                    break;
                }
            }

            Task.Run(keyTest);
        }

        private static void levelSelect(ref int width, ref string colorParam) {
            string color = colorParam;
            
            line(ref width);
            string tempText = "Level selection";
            centerWrite(ref tempText, ref width);
            tempText = "Test level          (t)";
            leftWrite(ref tempText, ref width, true);
            tempText = "Go back to the menu (e)";
            leftWrite(ref tempText, ref width, true);
            bLine(ref width);

            while (true) {
                ConsoleKeyInfo choiceKey = Console.ReadKey();

                switch (choiceKey.KeyChar) {
                    case 'e':
                    case 'E':
                        Console.Clear();
                        Main(color);
                        break;
                    case 't':
                    case 'T':
                        Console.Clear();
                        difficultySelect(ref width, ref color);
                        break;
                }
            }
        }

        private static void difficultySelect(ref int width, ref string colorParam) {
            string tempText;
            string color = colorParam;
            
            Console.Clear();
            line(ref width);
            tempText = "Difficulty";
            centerWrite(ref tempText, ref width);
            tempText = "Easy      (e)";
            leftWrite(ref tempText, ref width, true);
            tempText = "Medium    (m)";
            leftWrite(ref tempText, ref width, true);
            tempText = "Hard      (h)";
            leftWrite(ref tempText, ref width, true);
            tempText = "Go back   (B)";
            leftWrite(ref tempText, ref width, true);
            bLine(ref width);
            
            while (true) {
                ConsoleKeyInfo choiceKey = Console.ReadKey();

                switch (choiceKey.KeyChar) {
                    case 'b':
                    case 'B':
                        Console.Clear();
                        Main(color);
                        break;
                    case 'e':
                    case 'E':
                        Console.Clear();
                        play(ref width, ref color);
                        break;
                    case 'm':
                    case 'M':
                        Console.Clear();
                        play(ref width, ref color);
                        break;
                    case 'h':
                    case 'H':
                        Console.Clear();
                        play(ref width, ref color);
                        break;
                }
            }
        }

        private static void settings(ref int width, ref string colorParam) {
            string tempText;
            string color = colorParam;

            line(ref width);
            tempText = "Settings";
            centerWrite(ref tempText, ref width);

            tempText = "Press c for colors option";
            leftWrite(ref tempText, ref width, true);
            tempText = "Press m for the credits";
            leftWrite(ref tempText, ref width, true);
            tempText = "Press e to return to menu";
            leftWrite(ref tempText, ref width, true);
            bLine(ref width);

            while (true) {
                ConsoleKeyInfo currentKey = Console.ReadKey();

                switch (currentKey.KeyChar) {
                    case 'e':
                    case 'E':
                        Console.Clear();
                        Main(color);
                        break;
                    case 'c':
                    case 'C':
                        Console.Clear();
                        mcolor(ref width, ref color);
                        break;
                    case 'm':
                    case 'M':
                        Console.Clear();
                        credits(ref width, ref color);
                        break;
                }
            }
        }

        private static void credits(ref int width, ref string colorParam) {
            string tempText;
            string color = colorParam;
            string text;

            line(ref width);
            tempText = "Credits";
            centerWrite(ref tempText, ref width);
            Console.Write("|           ");
            text = "MymaQc";
            animateWrite(ref text, ref width);
            Console.Write("            |");
            Console.WriteLine();
            Console.Write("|             ");
            text = "Jorik Dupre";          
            animateWrite(ref text, ref width);
            Console.Write("              |");
            Console.WriteLine();
            tempText = "Press e to go back";
            centerWrite(ref tempText, ref width);
            bLine(ref width);

            while (true) {
                ConsoleKeyInfo currentKey = Console.ReadKey();

                if (currentKey.KeyChar != 'e' && currentKey.KeyChar != 'E') {
                    continue;
                }
                Console.Clear();
                settings(ref width, ref color);
            }
        }

        private static void mcolor(ref int width, ref string colorParam) {
            string tempText;
            if (colorParam == null) {
                throw new ArgumentNullException(nameof(colorParam));
            }

            line(ref width);
            tempText = "Color options";
            centerWrite(ref tempText, ref width);
            tempText = "White   (0)";
            leftWrite(ref tempText, ref width, true);
            tempText = "Blue    (1)";
            leftWrite(ref tempText, ref width, true);
            tempText = "Red     (2)";
            leftWrite(ref tempText, ref width, true);
            tempText = "Magenta (3)";
            leftWrite(ref tempText, ref width, true);
            tempText = "Yellow  (4)";
            leftWrite(ref tempText, ref width, true);
            tempText = "Green   (5)";
            leftWrite(ref tempText, ref width, true);
            bLine(ref width);

            while (true) {
                ConsoleKeyInfo currentKey = Console.ReadKey();

                switch (currentKey.KeyChar) {
                    case '0': {
                        colorParam = "White";

                        ConsoleColor consoleColor = ConsoleColor.White;
                        try {
                            consoleColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colorParam, true);
                        } catch (Exception) {
                            //Invalid color
                        }

                        Console.ForegroundColor = consoleColor;
                        Console.Clear();
                        Main(colorParam);
                        break;
                    } case '1': {
                        colorParam = "Blue";

                        ConsoleColor consoleColor = ConsoleColor.White;
                        try {
                            consoleColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colorParam, true);
                        } catch (Exception) {
                            //Invalid color
                        }

                        Console.ForegroundColor = consoleColor;
                        Console.Clear();
                        Main(colorParam);
                        break;
                    }
                    case '2': {
                        colorParam = "DarkRed";

                        ConsoleColor consoleColor = ConsoleColor.White;
                        try {
                            consoleColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colorParam, true);
                        } catch (Exception) {
                            //Invalid color
                        }

                        Console.ForegroundColor = consoleColor;
                        Console.Clear();
                        Main(colorParam);
                        break;
                    }
                    case '3': {
                        colorParam = "DarkMagenta";

                        ConsoleColor consoleColor = ConsoleColor.White;
                        try {
                            consoleColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colorParam, true);
                        } catch (Exception) {
                            //Invalid color
                        }

                        Console.ForegroundColor = consoleColor;
                        Console.Clear();
                        Main(colorParam);
                        break;
                    }
                    case '4': {
                        colorParam = "Yellow";

                        ConsoleColor consoleColor = ConsoleColor.White;
                        try {
                            consoleColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colorParam, true);
                        } catch (Exception) {
                            //Invalid color
                        }

                        Console.ForegroundColor = consoleColor;
                        Console.Clear();
                        Main(colorParam);
                        break;
                    }
                    case '5': {
                        colorParam = "Green";

                        ConsoleColor consoleColor = ConsoleColor.White;
                        try {
                            consoleColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colorParam, true);
                        } catch (Exception) {
                            //Invalid color
                        }

                        Console.ForegroundColor = consoleColor;
                        Console.Clear();
                        Main(colorParam);
                        break;
                    }
                }
            }
        }

        private static void centerWrite(ref string textParam, ref int widthParam) {
            string text = textParam;
            int width = widthParam;
            int spaceCount = (width - text.Length) / 2;
            bool extraSpace = (text.Length % 2) > 0;

            Console.Write("|");

            for (int i = 1; i < spaceCount; i++) {
                Console.Write(" ");
            }

            Console.Write(text);

            for (int i = 1; i < spaceCount; i++) {
                Console.Write(" ");
            }
            
            if (extraSpace) {
                Console.Write(" ");
            }
            
            Console.WriteLine("|");
        }

        private static void leftWrite(ref string textParam, ref int widthParam, bool jumpLineParam) {
            string text = textParam;
            int width = widthParam;

            Console.Write("| " + text);

            int characterCount = text.Length + 3;

            for (int i = 0; i < width; i++) {
                if (characterCount != width) {
                    Console.Write(" ");
                    characterCount += 1;
                } else {
                    if (jumpLineParam) {
                        Console.WriteLine("|");
                    } else {
                        Console.Write("|");
                    }

                    break;
                }
            }
        }

        private static void line(ref int widthParam) {
            int width = widthParam;

            for (int i = 0; i < width; i++) {
                if (i == width - 1) {
                    Console.WriteLine("_");
                } else {
                    Console.Write("_");
                }
            }
        }

        private static void bLine(ref int widthParam) {
            int width = widthParam;

            for (int i = 0; i < width; i++) {
                if (i == width - 1) {
                    Console.WriteLine("‾");
                } else {
                    Console.Write("‾");
                }
            }
        }

        private static void animateWrite(ref string textParam, ref int widthParam){
            string text = textParam;
            int width = widthParam;
            int characterCount = 0;
            const char period = '.';
            const char coma = ',';
            const char space = ' ';

            foreach (char t in text) {
                if (characterCount >= width && t.Equals(space)) {
                    Console.WriteLine(t);
                    characterCount = 0;
                } else switch (t) {
                    case coma:
                        Console.Write(t);
                        characterCount += 1;
                        Thread.Sleep(250);
                        break;
                    case space:
                        Console.Write(t);
                        characterCount += 1;
                        Thread.Sleep(125);
                        break;
                    case period:
                        Console.Write(t);
                        characterCount += 1;
                        Thread.Sleep(300);
                        break;
                    default:
                        Console.Write(t);
                        characterCount += 1;
                        Thread.Sleep(75);
                        break;
                }
            }
        }

        private static void simulateTypingText(string Text, int typingDelay = 0, int startDelay = 0) {
            InputSimulator sim = new InputSimulator();

            sim.Keyboard.Sleep(startDelay);

            string[] lines = Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            foreach (string line in lines) {
                char[] words = line.ToCharArray();
                
                foreach (char word in words) {
                    sim.Keyboard.TextEntry(word).Sleep(typingDelay);
                }
                
                sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                sim.Keyboard.KeyPress(VirtualKeyCode.HOME);
            }
        }
        
    }
    
}