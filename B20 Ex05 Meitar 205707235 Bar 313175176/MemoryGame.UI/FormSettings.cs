using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MemoryGame.UI
{
    public partial class FormSettings : Form
    {
        private readonly string[] r_BoardSizes = {"4x4", "4x5", "4x6", "5x4", "5x6", "6x4", "6x5", "6x6"};
        private int m_BoardSizeIndex = 0;

        public FormSettings()
        {
            InitializeComponent();
        }

        public void GetBoardSize(out int o_Height, out int o_Width)
        {
            int.TryParse(boardSizeButton.Text[0].ToString(), out o_Height);
            int.TryParse(boardSizeButton.Text[2].ToString(), out o_Width);
        }

        public string FirstPlayerName
        {
            get
            {
                return firstPlayerTextBox.Text;
            }
        }
        
        public string SecondPlayerName
        {
            get
            {
                return secondPlayerTextBox.Text;
            }
        }

        private void oponnentButton_Click(object sender, EventArgs e)
        {
            secondPlayerTextBox.Enabled = !secondPlayerTextBox.Enabled;
            (sender as Button).Text = secondPlayerTextBox.Enabled ? "Against Computer" : "Against a Friend";
            secondPlayerTextBox.Text = secondPlayerTextBox.Enabled ? string.Empty : "- computer -";

        }

        private void boardSizeButton_Click(object sender, EventArgs e)
        {
            m_BoardSizeIndex++;
            m_BoardSizeIndex %= r_BoardSizes.Length;
            (sender as Button).Text = r_BoardSizes[m_BoardSizeIndex];
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
