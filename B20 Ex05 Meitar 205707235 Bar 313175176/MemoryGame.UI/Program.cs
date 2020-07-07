using System;
using System.Threading;

namespace MemoryGame.UI
{
    public class Program
    {
        public static void Main()
        {
            //GameUI memoryGame = new GameUI();
            //memoryGame.RunGames();
            //MessageDisplayer.DisplayMessage(MessageDisplayer.GoodBye);
            //Thread.Sleep(2500);
            FormSettings settings = new FormSettings();
            settings.ShowDialog();
            FormMemoryGame memoryGame = new FormMemoryGame(settings);
            memoryGame.ShowDialog();
        }
    }
}