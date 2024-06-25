namespace _1_word_game
{
    internal class Player
    {
        public string name { get; set; } = "";
        public string[] words { get; set; } = [];
        public int totalScore { get; set; } = 0;

        public Player() { }

        public Player(string name, string[] words, int score)
        {
            this.name = name;
            this.words = words;
            this.totalScore = totalScore;
        }
        public Player(int num)
        {
            do
            {
                Console.Write($"Player {num}, type your name: ");
                string? input = Console.ReadLine();
                if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Incorrect input. Please try again.");
                }
                else
                {
                    name = input.Trim();
                }
            } while (string.IsNullOrEmpty(name));

        }

        public void ShowWords()
        {
            Console.Write($"Player {name} results:");

            for (int i = 0; i < words.Length; i++)
            {
                if (words[i] == null || words[i].Length == 0) { continue; }
                Console.Write($" {words[i]},");
            }

            Console.WriteLine(); ;
        }

        public string? InputWord()
        {
            Console.Write($"Player {name}, you turn: ");
            return Console.ReadLine();
        }

        public string[] GetWords() { return words; }

        public void AddWord(string newWord)
        {
            var wordsList = words.ToList();
            wordsList.Add(newWord);
            words = wordsList.ToArray();
            totalScore++;
        }
    }
}
