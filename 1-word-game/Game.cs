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
        private const string SalutePicture = @"
                                        .''.       
            .''.      .        *''*    :_\/_:     . 
            :_\/_:   _\(/_  .:.*_\/_*   : /\ :  .'.:.'.
        .''.: /\ :   ./)\   ':'* /\ * :  '..'.  -=:o:=-
        :_\/_:'.:::.    ' *''*    * '.\'/.' _\(/_'.':'.'
        : /\ : :::::     *_\/_*     -= o =-  /)\    '  *
        '..'  ':::'     * /\ *     .'/.\'.   '
                                   *..*         :    
        ";
        private const string Rule = "Create words from the original word as much as possible.";
        private const string AvailableCommands = @"Available Commands: /show-words, /score, /total-score";
        private const string DataFileName = "data.json";

        public string originalWord = string.Empty;
        public Player[] players = new Player[2];
        public Player currPlayer;
        public int currPlayerIdx = 0;
        private bool _gameOver = false;
        private string _commandOutput = string.Empty;
        private Storage _storage;
        private Validator _validator;
        private Countdown _countdown;


        public Game()
        {
            players[0] = new Player(1);
            players[1] = new Player(2);
            currPlayer = players[currPlayerIdx];
            _storage = new Storage(DataFileName);
            _validator = new Validator();
            _countdown = new Countdown();
        }

        public void InitOriginalWord()
        {
            do
            {
                InputOutput.PrintMessages(messages: [Name, _validator.InputError()], clearScreen: true);
                string input = InputOutput.GetInput("Type a word between 8 and 30 characters long: ");
                _validator.UpdateInput(input);
                _validator.ValidationInputOriginalWord();

                if (_validator.IsError()) continue;

                originalWord = input.ToLower();
            }
            while (string.IsNullOrEmpty(originalWord));
        }

        public void NextTurnPrint()
        {
            string label = $"Original Word: {getStringUpperWithSpaces(originalWord)}";
            InputOutput.PrintMessages(
                messages: [Name, Rule, label, AvailableCommands, _validator.InputError()],
                clearScreen: true
                );

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

        public void CheckAndAddPlayerWord()
        {
            if (!_countdown.IsRunning())
            {
                _gameOver = true;
                return;
            }
            _validator.UpdateInput(currPlayer.GetInput());
            if (_validator.IsInputCommand())
            {
                DoPlayerCommand();
                return;
            }
            string[] allWords = GetAllCurrentUsersWords();
            _validator.ValidationPlayerWord(originalWord, allWords);

            if (_validator.IsError()) return;

            currPlayer.AddWord();
            SwitchPlayer();
            _countdown.Stop();
        }

        private string[] GetAllCurrentUsersWords()
        {
            string[] player1Words = players[0].GetWords();
            string[] player2Words = players[1].GetWords();
            return player1Words.Concat(player2Words).ToArray();
        }

        public bool IsOver()
        {
            return _gameOver;
        }

        public void RunCountdown()
        {
            if (_countdown != null)
            {
                _countdown.Start();
            }
        }

        public void PrintWinner()
        {
            SwitchPlayer();
            InputOutput.PrintMessages(messages: ["", "Game over.", $"The {currPlayer.Name} wins. Congratulations!", SalutePicture]);
        }

        private void DoPlayerCommand()
        {
            switch (currPlayer.GetInput())
            {
                case "/show-words":
                    string[] allWords = GetAllCurrentUsersWords();
                    _commandOutput = $"All words entered by current users : {String.Join(", ", allWords)}";
                    break;
                case "/score":
                    _commandOutput = $"Score: {players[0].Name} = {players[0].TotalScore}, {players[1].Name} = {players[1].TotalScore}";
                    break;
                case "/total-score":
                    int totalGameScore = 0;
                    foreach (var player in _storage.Players)
                    {
                        totalGameScore += player.TotalScore;
                    }
                    _commandOutput = $"Total Score: {totalGameScore}";
                    break;
                default:
                    _commandOutput = "Unknown command";
                    break;
            }
        }

        public void PrintCommandOutput()
        {
            if (!string.IsNullOrEmpty(_commandOutput))
            {
                InputOutput.PrintUnderCursorAndGoBack(messages: [_commandOutput], lineShift: 3);
            }
            CleartCommandOutput();
        }

        public void CleartCommandOutput()
        {
            _commandOutput = string.Empty;
        }

        private int GetTotalScorePlayerByName(string name)
        {
            Player p = _storage.Players.FirstOrDefault(player => player.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (p == null)
            {
                return 0;
            }
            return p.TotalScore;
        }

        public void SetTotalScoreCurrentPlayers()
        {
            _storage.ReadFromFile();
            int totalScorePlayer1 = GetTotalScorePlayerByName(players[0].Name);
            players[0].TotalScore = totalScorePlayer1;
            int totalScorePlayer2 = GetTotalScorePlayerByName(players[1].Name);
            players[1].TotalScore = totalScorePlayer2;
        }

        public void SaveResults()
        {
            _storage.AddOrUpdatePlayers(players);
            _storage.WriteToFile();
        }
    }
}
