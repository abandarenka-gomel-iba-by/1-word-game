using System.Text.RegularExpressions;
using System.Timers;
using Timer = System.Timers.Timer;

const string GAME_NAME = @"
 __          __           _        _____                      
 \ \        / /          | |      / ____|                     
  \ \  /\  / /__  _ __ __| |___  | |  __  __ _ _ __ ___   ___ 
   \ \/  \/ / _ \| '__/ _` / __| | | |_ |/ _` | '_ ` _ \ / _ \
    \  /\  / (_) | | | (_| \__ \ | |__| | (_| | | | | | |  __/
     \/  \/ \___/|_|  \__,_|___/  \_____|\__,_|_| |_| |_|\___|
                                                              
";
const string GAME_RULE = "Create words from the original word as much as possible.";
const byte player1Num = 1;
const byte player2Num = 2;
bool gameOver = false;
Timer countdownTimer;
byte countdown = 0;

string getOriginalWord()
{
    string result = "";
    string originalWordError = "";
    do
    {
        Console.Clear();
        Console.WriteLine(GAME_NAME);

        if (originalWordError.Length > 0)
        {
            Console.WriteLine(originalWordError);
        }
        Console.Write("Type a word between 8 and 30 characters long: ");
        string? input = Console.ReadLine();

        if (input == null)
        {
            originalWordError = "Invalid input value! Please try again.";
            continue;
        }

        originalWordError = inputValidation(input);

        if (originalWordError.Length == 0)
        {
            result = input.ToLower();
        }
    }
    while (result.Length == 0);
    return result;
}


string inputValidation(string str)
{
    if (str.Length == 0)
    {
        return "Input is empty! Please try again";
    }
    if (str.Length < 8)
    {
        return "The word is too short! Please try again.";
    }
    if (str.Length > 30)
    {
        return "The word is too long! Please try again.";
    }
    if (Regex.IsMatch(str, @"[^a-zA-Z]"))
    {
        return "The word should contain only letters! Please try again.";
    }
    return "";
}

void PrintWinner(string userName)
{
    Console.WriteLine();
    Console.WriteLine("Game over.");
    Console.WriteLine($"The {userName} wins. Congratulations!");
    Console.WriteLine(@"
                                           .''.       
               .''.      .        *''*    :_\/_:     . 
              :_\/_:   _\(/_  .:.*_\/_*   : /\ :  .'.:.'.
          .''.: /\ :   ./)\   ':'* /\ * :  '..'.  -=:o:=-
         :_\/_:'.:::.    ' *''*    * '.\'/.' _\(/_'.':'.'
         : /\ : :::::     *_\/_*     -= o =-  /)\    '  *
          '..'  ':::'     * /\ *     .'/.\'.   '
                           *..*         :    
    ");
}

string getStringUpperWithSpaces(string str)
{
    string result = "";
    foreach (char c in str)
    {
        result += $" {c}";
    }
    return result.ToUpper();
}

void PrintResults(string[] words, byte playerNumber)
{
    Console.Write($"Player {playerNumber} results:");

    for (int i = 0; i < words.Length; i++)
    {
        if (words[i] == null || words[i].Length == 0) { continue; }
        Console.Write($" {words[i]},");
    }

    Console.WriteLine();
}
string WordValidation(string originalWord, string[] words, string? word)
{
    if (word == null)
    {
        return "Invalid input value! Please try again.";
    }
    foreach (string w in words)
    {
        if (word.ToLower() == w)
        {
            return $"The word '{word}' is already exist! Please try again.";
        }
    }

    string testWord = word.ToLower();

    foreach (char c in originalWord.ToLower())
    {
        int index = testWord.IndexOf(c);

        if (index >= 0)
        {
            testWord = testWord.Remove(index, 1);
        }
    }

    if (testWord.Length > 0)
    {
        return $"The word '{word}' cannot be create from original word! Please try again.";
    }

    return "";
}

string StartGame(string originalWord)
{
    byte currentPlayerNum = player1Num;
    string[][] words = new string[3][];
    words[player1Num] = new string[100];
    words[player2Num] = new string[100];
    byte round = 0;
    string inputError = "";

    while (!gameOver)
    {
        Console.Clear();
        Console.WriteLine(GAME_NAME);
        Console.WriteLine(GAME_RULE);
        Console.WriteLine($"Original Word: {getStringUpperWithSpaces(originalWord)}");
        PrintResults(words[player1Num], player1Num);
        PrintResults(words[player2Num], player2Num);
        Console.WriteLine();

        if (inputError.Length > 0)
        {
            Console.WriteLine(inputError);
        }
        else
        {
            countdown = 30;
            countdownTimer.Stop();
            countdownTimer.Start();

        }

        Console.Write($"Player {currentPlayerNum} turn: ");
        string? word = Console.ReadLine();

        inputError = WordValidation(originalWord, words[player1Num].Concat(words[player2Num]).ToArray(), word);

        if (inputError.Length > 0 || word == null) { continue; }

        if (!gameOver)
        {
            words[currentPlayerNum][round++] = word.ToLower();
            currentPlayerNum = currentPlayerNum == player1Num ? player2Num : player1Num;
        }
    }

    currentPlayerNum = currentPlayerNum == player1Num ? player2Num : player1Num;
    return $"Player {currentPlayerNum}";
}

void CountdownTimerElapsed(object sender, ElapsedEventArgs e)
{
    countdown--;

    int currentLineCursor = Console.CursorTop;
    int currenPosCursor = Console.CursorLeft;
    Console.SetCursorPosition(0, currentLineCursor + 1);
    Console.Write(new string(' ', Console.WindowWidth));
    Console.SetCursorPosition(0, currentLineCursor + 1);
    Console.WriteLine($"Time left: {countdown} seconds");
    Console.SetCursorPosition(currenPosCursor, currentLineCursor);

    if (countdown <= 0)
    {
        countdownTimer.Stop();
        gameOver = true;
        Console.WriteLine("Time is up! Please press Enter.");
    }
}

countdownTimer = new Timer();
countdownTimer.Interval = 1000;
countdownTimer.Elapsed += CountdownTimerElapsed;

string winner = StartGame(getOriginalWord());

Console.Clear();
Console.WriteLine(GAME_NAME);
PrintWinner(winner);

