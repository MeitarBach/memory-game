using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using MemoryGame.Logic;

namespace MemoryGame.UI
{
    public partial class FormMemoryGame : Form
    {
        private const int k_Margin = 12;
        private const int k_ButtonSize = 160;
        private const int k_LabelWidth = 250;
        private const int k_LabelHeight = 32;
        private Board m_Board;
        private Button[,] m_GameCards;
        private Label currentPlayer;
        private Label firstPlayer;
        private Label secondPlayer;

        public FormMemoryGame(FormSettings i_Settings)
        {
            m_Board = createBoard(i_Settings);
            m_GameCards = createCardsBoard();
            this.Size = new Size(k_Margin + m_Board.Width * (k_ButtonSize + k_Margin),(4 + m_Board.Height) * k_Margin + (k_ButtonSize * m_Board.Height));
            createLabels(i_Settings);
            InitializeComponent();
        }

        private Board createBoard(FormSettings i_Settings)
        {
            int boardHeight;
            int boardWidth;

            i_Settings.GetBoardSize(out boardHeight, out boardWidth);

            return new Board(boardHeight, boardWidth);
        }

        private Button[,] createCardsBoard()
        {
            Button[,] cardsBoard = new Button[m_Board.Height, m_Board.Width];

            for(int i = 0; i < m_Board.Height; i++)
            {
                int y = this.Location.Y + k_Margin + (i * (k_ButtonSize + k_Margin));

                for(int j = 0; j < m_Board.Width; j++)
                {
                    int x = this.Location.X + k_Margin + (j * (k_ButtonSize + k_Margin));
                    cardsBoard[i, j] = new Button();
                    cardsBoard[i, j].Size = new Size(k_ButtonSize, k_ButtonSize);
                    cardsBoard[i, j].Location = new Point(x, y);
                    this.Controls.Add(cardsBoard[i, j]);
                }
            }

            return cardsBoard;
        }

        private void createLabels(FormSettings i_Settings)
        {
            int labelsX = this.Location.X + k_Margin;
            Size labelSize = new Size(k_LabelWidth, k_LabelHeight);

            currentPlayer = new Label();
            currentPlayer.Text = string.Format("0", i_Settings.FirstPlayerName);
            currentPlayer.Size = labelSize;
            int currentPlayerY = this.Location.Y + k_Margin + (m_Board.Height * (k_Margin + k_ButtonSize));
            currentPlayer.Location = new Point(labelsX, currentPlayerY);
            this.Controls.Add(currentPlayer);

            firstPlayer = new Label();
            firstPlayer.Text = string.Format("1", i_Settings.FirstPlayerName);
            firstPlayer.Size = labelSize;
            firstPlayer.Location = new Point(labelsX, currentPlayerY + currentPlayer.Height + k_Margin);
            this.Controls.Add(firstPlayer);

            secondPlayer = new Label();
            secondPlayer.Text = string.Format("2", i_Settings.SecondPlayerName);
            secondPlayer.Size = labelSize;
            secondPlayer.Location = new Point(labelsX, firstPlayer.Location.Y + currentPlayer.Height + k_Margin);
            this.Controls.Add(secondPlayer);
        }
    }
}
