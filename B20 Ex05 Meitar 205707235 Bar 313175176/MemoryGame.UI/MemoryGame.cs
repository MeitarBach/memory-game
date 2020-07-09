namespace MemoryGame.UI
{
    internal class MemoryGame
    {
        internal void RunGame()
        {
            FormSettings settings = new FormSettings();
            settings.ShowDialog();
            FormMemoryGame memoryGame = new FormMemoryGame(settings);
            memoryGame.ShowDialog();
        }
    }
}
