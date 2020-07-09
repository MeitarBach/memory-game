using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
