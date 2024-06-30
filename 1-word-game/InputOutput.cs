namespace _1_word_game
{
    internal class InputOutput
    {
        static public void PrintMessages(string[] messages, bool clearScreen = false)
        {
            if (clearScreen) Console.Clear();

            for (int i = 0; i < messages.Length; i++)
            {
                Console.WriteLine(messages[i]);
            }
        }

        static public string GetInput(string message)
        {
            Console.Write(message);
            string? input = Console.ReadLine();
            if (input == null) return "";
            return input;
        }

        static public void PrintUnderCursorAndGoBack(string[] messages, int lineShift = 1, bool cleanOldLine = false)
        {
            int currentLineCursor = Console.CursorTop;
            int currenPosCursor = Console.CursorLeft;
            Console.SetCursorPosition(0, currentLineCursor + lineShift);

            if (cleanOldLine)
            {
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, currentLineCursor + lineShift);
            }

            PrintMessages(messages);
            Console.SetCursorPosition(currenPosCursor, currentLineCursor);
        }
    }
}
