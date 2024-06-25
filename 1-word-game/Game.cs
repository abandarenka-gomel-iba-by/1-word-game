using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using Timer = System.Timers.Timer;

namespace _1_word_game
{
    internal class Game
    {
        private const string Name = @"
         __          __           _        _____                      
         \ \        / /          | |      / ____|                     
          \ \  /\  / /__  _ __ __| |___  | |  __  __ _ _ __ ___   ___ 
           \ \/  \/ / _ \| '__/ _` / __| | | |_ |/ _` | '_ ` _ \ / _ \
            \  /\  / (_) | | | (_| \__ \ | |__| | (_| | | | | | |  __/
             \/  \/ \___/|_|  \__,_|___/  \_____|\__,_|_| |_| |_|\___|
                                                              
        ";
        private const string Rule = "Create words from the original word as much as possible.";
        public string originalWord = string.Empty;
        public Player[] players = new Player[2];
        public Player currPlayer;
        public int currPlayerIdx = 0;
        private bool gameOver = false;
        private string inputError = string.Empty;
        private byte countdown = 0;
        private Timer countdownTimer = new Timer();
        private bool isCountdouwnRunning = false;
        private string commandOutput = string.Empty;
        private Storage Storage;


        public Game()
        {
            players[0] = new Player(1);
            players[1] = new Player(2);
            currPlayer = players[currPlayerIdx];
            countdownTimer.Interval = 1000;
            countdownTimer.Elapsed += CountdownTimerElapsed;
            Storage = new Storage("data.json");
            SetTotalScoreCurrentPlayers();
        }

        public void InitOriginalWord()
        {
            const string Label = "Type a word between 8 and 30 characters long: ";
            do
            {
                NextPrintMessages(Name, inputError, Label);
                string? input = Console.ReadLine();

                if (ValidInputOriginalWord(input))
                {
                    originalWord = input.ToLower();
                }
            }
            while (string.IsNullOrEmpty(originalWord));
        }

        private void NextPrintMessages(params string[] messages)
        {
            Console.Clear();
            for (int i = 0; i < messages.Length; i++)
            {
                if (string.IsNullOrEmpty(messages[i])) { continue; }
                if (i == messages.Length - 1)
                {
                    Console.Write(messages[i]);
                }
                else
                {
                    Console.WriteLine(messages[i]);
                }
            }
        }

        public void NextTurnPrint()
        {
            string label = $"Original Word: {getStringUpperWithSpaces(originalWord)}";
            string availableCommands = @"Available Commands: /show-words, /score, /total-score";
            NextPrintMessages(Name, Rule, label, availableCommands, inputError);
            Console.WriteLine();

            string getStringUpperWithSpaces(string str)
            {
                string result = "";
                foreach (char c in str)
                {
                    result += $" {c}";
                }
                return result.ToUpper();
            }
        }

        public void SwitchPlayer()
        {
            currPlayerIdx = (currPlayerIdx + 1) % 2;
            currPlayer = players[currPlayerIdx];
        }


        public bool CanBeCreated(string? word)
        {
            if (gameOver) { return false; }
            if (string.IsNullOrEmpty(word))
            {
                inputError = "Empty input! Plaese try again.";
                return false;
            }

            string testWord = word.ToLower();
            string[] allWords = GetAllCurrentUsersWords();

            foreach (string w in allWords)
            {
                if (testWord == w)
                {
                    inputError = $"The word '{word}' is already exist! Please try again.";
                    return false;
                }
            }

            foreach (char c in originalWord.ToLower())
            {
                int index = testWord.IndexOf(c);

                if (index >= 0)
                {
                    testWord = testWord.Remove(index, 1);
                }
            }

            if (!string.IsNullOrEmpty(testWord))
            {
                inputError = $"The word '{word}' cannot be create from original word! Please try again.";
                return false;
            }

            inputError = string.Empty;
            return true;
        }

        private string[] GetAllCurrentUsersWords()
        {
            string[] player1Words = players[0].GetWords();
            string[] player2Words = players[1].GetWords();
            return player1Words.Concat(player2Words).ToArray();
        }

        private bool ValidInputOriginalWord(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                inputError = "Input is empty! Please try again";
                return false;
            }
            if (input.Length < 8)
            {
                inputError = "The word is too short! Please try again.";
                return false;
            }
            if (input.Length > 30)
            {
                inputError = "The word is too long! Please try again.";
                return false;
            }
            if (Regex.IsMatch(input, @"[^a-zA-Z]"))
            {
                inputError = "The word should contain only letters! Please try again.";
                return false;
            }
            inputError = string.Empty;
            return true;
        }
        public bool IsOver()
        {
            return gameOver;
        }

        public void RunCountdown()
        {
            if (!isCountdouwnRunning)
            {
                countdown = 30;
                countdownTimer.Stop();
                countdownTimer.Start();
                isCountdouwnRunning = true;
            }
        }

        public void ResetCountdown()
        {
            isCountdouwnRunning = false;
        }

        private void CountdownTimerElapsed(object sender, ElapsedEventArgs e)
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

        public void PrintWinner()
        {
            SwitchPlayer();
            Console.WriteLine();
            Console.WriteLine("Game over.");
            Console.WriteLine($"The {currPlayer.name} wins. Congratulations!");
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

        public bool IsCommand(string input)
        {
            return input.StartsWith("/");
        }

        public void DoCommand(string command)
        {
            switch (command)
            {
                case "/show-words":
                    string[] allWords = GetAllCurrentUsersWords();
                    commandOutput = $"All words entered by current users : {String.Join(", ", allWords)}";
                    break;
                case "/score":
                    commandOutput = $"Score: {players[0].name} = {players[0].totalScore}, {players[1].name} = {players[1].totalScore}";
                    break;
                case "/total-score":
                    int totalGameScore = 0;
                    foreach (var player in Storage.Players)
                    {
                        totalGameScore += player.totalScore;
                    }
                        commandOutput = $"Total Score: {totalGameScore}";
                    break;
                default:
                    commandOutput = "Unknown command";
                    break;
            }
        }

        public void PrintCommandOutput()
        {

            if (!string.IsNullOrEmpty(commandOutput))
            {
                int currentLineCursor = Console.CursorTop;
                int currenPosCursor = Console.CursorLeft;
                Console.SetCursorPosition(0, currentLineCursor + 3);
                Console.WriteLine(commandOutput);
                Console.SetCursorPosition(currenPosCursor, currentLineCursor);
            }
        }

        public void CleartCommandOutput()
        {
            commandOutput = string.Empty;
        }

        private int GetTotalScorePlayerByName(string name)
        {
            Player p = Storage.Players.FirstOrDefault(player => player.name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (p == null)
            {
                return 0;
            }
            return p.totalScore;
        }

        private void SetTotalScoreCurrentPlayers()
        {
            int totalScorePlayer1 = GetTotalScorePlayerByName(players[0].name);
            players[0].totalScore = totalScorePlayer1;
            int totalScorePlayer2 = GetTotalScorePlayerByName(players[1].name);
            players[1].totalScore = totalScorePlayer2;
        }

        public void SaveResults()
        {
            Storage.AddOrUpdatePlayers(players);
            Storage.WriteToFile();
        }
    }
}
