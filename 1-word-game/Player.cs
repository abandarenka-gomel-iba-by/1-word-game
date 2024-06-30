namespace _1_word_game
{
    internal class Player
    {
        public string Name { get; set; } = "";
        public string[] Words { get; set; } = [];
        public int TotalScore { get; set; } = 0;
        private string _input = string.Empty;

        public Player() { }

        public Player(string name, string[] words, int score)
        {
            Name = name;
            Words = words;
            TotalScore = score;
        }
        public Player(int num)
        {
            do
            {
                string input = InputOutput.GetInput($"Player {num}, type your name: ");
                if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
                {
                    InputOutput.PrintMessages(messages: ["Incorrect input. Please try again."]);
                }
                else
                {
                    Name = input.Trim();
                }
            } while (string.IsNullOrEmpty(Name));

        }

        public void InputWord()
        {
            _input = InputOutput.GetInput($"Player {Name}, you turn: ");
        }

        public string GetInput()
        {
            return _input; 
        }

        public string[] GetWords() { return Words; }

        public void AddWord()
        {
            var wordsList = Words.ToList();
            wordsList.Add(_input);
            Words = wordsList.ToArray();
            TotalScore++;
            _input = string.Empty;
        }
    }
}
