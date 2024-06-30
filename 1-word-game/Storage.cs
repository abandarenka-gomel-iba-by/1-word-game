using System.Text.Json;

namespace _1_word_game
{
    internal class Storage
    {
        public Player[] Players { get; set; } = Array.Empty<Player>();
        public string FilePath { get; set; }

        public Storage(string filePath)
        {
            FilePath = filePath;
        }

        public void ReadFromFile()
        {
            if (!File.Exists(FilePath))
            {
                Players = Array.Empty<Player>();
                WriteToFile();
            }
            else
            {
                string jsonString = File.ReadAllText(FilePath);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    IncludeFields = true
                };

                Players = JsonSerializer.Deserialize<Player[]>(jsonString, options) ?? Array.Empty<Player>();
            }
        }

        public void WriteToFile()
        {
            string jsonString = JsonSerializer.Serialize(Players);
            File.WriteAllText(FilePath, jsonString);
        }

        public void AddOrUpdatePlayers(Player[] newPlayers)
        {
            foreach (var newPlayer in newPlayers)
            {
                int index = Array.FindIndex(Players, player => player.Name.Equals(newPlayer.Name, StringComparison.OrdinalIgnoreCase));

                if (index >= 0)
                {
                    Players[index] = newPlayer;
                }
                else
                {
                    var playersList = Players.ToList();
                    playersList.Add(newPlayer);
                    Players = playersList.ToArray();
                }
            }
        }
    }
}
