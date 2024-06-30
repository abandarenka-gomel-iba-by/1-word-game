using _1_word_game;

Game game = new();

game.SetTotalScoreCurrentPlayers();
game.InitOriginalWord();

while (!game.IsOver())
{
    game.NextTurnPrint();
    game.PrintCommandOutput();
    game.RunCountdown();
    game.currPlayer.InputWord();
    game.CheckAndAddPlayerWord();
}

game.SaveResults();
game.PrintWinner();
