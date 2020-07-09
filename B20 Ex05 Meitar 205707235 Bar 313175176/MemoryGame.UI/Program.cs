using System;
using System.Threading;
using System.Windows.Forms;

namespace MemoryGame.UI
{
    public class Program
    {
        public static void Main()
        {
            Application.EnableVisualStyles();
            new MemoryGame().RunGame();
        }
    }
}