using System;
using System.Text;
using System.IO;
using System.Text.Json;

namespace _1_word_game
{
    internal class Storage
    {
        public Player[] Players { get; set; } = Array.Empty<Player>();
        public string filePath { get; set; }

        public Storage(string filePath)
        {
            this.filePath = filePath;
            ReadFromFile();
        }

        public void ReadFromFile()
        {
            if (!File.Exists(filePath))
            {
                Players = Array.Empty<Player>();
                WriteToFile();
            }
            else
            {
                string jsonString = File.ReadAllText(filePath);

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
            File.WriteAllText(filePath, jsonString);
        }

        public void AddOrUpdatePlayers(Player[] newPlayers)
        {
            foreach (var newPlayer in newPlayers)
            {
                int index = Array.FindIndex(Players, player => player.name.Equals(newPlayer.name, StringComparison.OrdinalIgnoreCase));

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
