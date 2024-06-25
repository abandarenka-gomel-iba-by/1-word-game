using _1_word_game;

Game game = new ();

game.InitOriginalWord();

while (!game.IsOver())
{
    game.NextTurnPrint();
    game.RunCountdown();
    game.PrintCommandOutput();

    string? word = game.currPlayer.InputWord();

    if (game.IsCommand(word))
    {
        game.DoCommand(word);
    }
    else if (game.CanBeCreated(word))
    {
        game.currPlayer.AddWord(word);
        game.SwitchPlayer();
        game.ResetCountdown();
        game.CleartCommandOutput();
    }
}

game.SaveResults();
game.PrintWinner();
