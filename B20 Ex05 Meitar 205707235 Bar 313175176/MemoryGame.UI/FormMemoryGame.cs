using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using MemoryGame.Logic;

namespace MemoryGame.UI
{
    public partial class FormMemoryGame : Form
    {
        private const int k_Margin = 20;
        private const int k_ButtonSize = 80;
        private const int k_LabelWidth = 500;
        private const int k_LabelHeight = 32;
        private Board m_Board;
        private Button[,] m_GameCards;
        private Label currentPlayer;
        private Label firstPlayer;
        private Label secondPlayer;
        private Color m_FirstPlayerColor = Color.GreenYellow;
        private Color m_SecondPlayerColor = Color.MediumPurple;
        private Color m_CurrentPlayerColor;
        private Player m_FirstPlayer;
        private Player m_SecondPlayer;
        private Player m_CurrentPlayer;
        private GameCell m_FirstCellChosen;
        private GameCell m_SecondCellChosen;
        private Button m_FirstChosenButton;
        private bool m_IsFirstClick = true;
        private GameManager m_GameManager;

        public FormMemoryGame(FormSettings i_Settings)
        {
            m_Board = createBoard(i_Settings);
            m_GameCards = createCardsBoard();
            createPlayers(i_Settings);
            m_GameManager = new GameManager(m_FirstPlayer, m_SecondPlayer, m_Board);
            m_CurrentPlayer = m_FirstPlayer;
            m_CurrentPlayerColor = m_FirstPlayerColor;
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
                    cardsBoard[i, j].Size = new System.Drawing.Size(k_ButtonSize, k_ButtonSize);
                    cardsBoard[i, j].Location = new Point(x, y);
                    cardsBoard[i, j].BackColor = Color.DarkGray;
                    cardsBoard[i, j].Click += gameCard_Click;
                    cardsBoard[i, j].Text = m_Board.BoardCells[i, j].ToString();
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
            currentPlayer.Text = string.Format("Current Player: {0}", m_CurrentPlayer.Name);
            currentPlayer.BackColor = m_FirstPlayerColor;
            currentPlayer.Size = labelSize;
            int currentPlayerY = this.Location.Y + k_Margin + (m_Board.Height * (k_Margin + k_ButtonSize));
            currentPlayer.Location = new Point(labelsX, currentPlayerY);
            this.Controls.Add(currentPlayer);

            firstPlayer = new Label();
            firstPlayer.Text = string.Format("{0}: {1} Pairs", m_FirstPlayer.Name, m_FirstPlayer.Score);
            firstPlayer.BackColor = m_FirstPlayerColor;
            firstPlayer.Size = labelSize;
            firstPlayer.Location = new Point(labelsX, currentPlayerY + currentPlayer.Height + k_Margin);
            this.Controls.Add(firstPlayer);

            secondPlayer = new Label();
            secondPlayer.Text = string.Format("{0}: {1} Pairs", m_SecondPlayer.Name, m_SecondPlayer.Score);
            secondPlayer.BackColor = m_SecondPlayerColor;
            secondPlayer.Size = labelSize;
            secondPlayer.Location = new Point(labelsX, firstPlayer.Location.Y + currentPlayer.Height + k_Margin);
            this.Controls.Add(secondPlayer);
        }

        private void updateLabels()
        {
            currentPlayer.Text = string.Format("Current Player: {0}", m_CurrentPlayer.Name);
            firstPlayer.Text = string.Format("{0}: {1} Pairs", m_FirstPlayer.Name, m_FirstPlayer.Score);
            secondPlayer.Text = string.Format("{0}: {1} Pairs", m_SecondPlayer.Name, m_SecondPlayer.Score);
        }

        private void createPlayers(FormSettings i_Settings)
        {
            m_FirstPlayer = new Player(i_Settings.FirstPlayerName, ePlayerType.Human);
            ePlayerType oponnentType = i_Settings.IsOponnentHuman() ? ePlayerType.Human : ePlayerType.Computer;
            m_SecondPlayer = new Player(i_Settings.SecondPlayerName, oponnentType);
        }

        private void gameCard_Click(object sender, EventArgs e)
        {
            if (m_IsFirstClick)
            {
                m_FirstCellChosen = findChosenGameCell(sender as Button);
                m_FirstChosenButton = sender as Button;
                m_FirstChosenButton.Enabled = false;
            }
            else
            {
                m_SecondCellChosen = findChosenGameCell(sender as Button);
                (sender as Button).Enabled = false;

                m_CurrentPlayer = m_GameManager.ExecuteMove(m_CurrentPlayer, m_FirstCellChosen, m_SecondCellChosen);
                m_CurrentPlayerColor = m_CurrentPlayer == m_FirstPlayer ? m_FirstPlayerColor : m_SecondPlayerColor;
                if (m_FirstCellChosen.Letter != m_SecondCellChosen.Letter)
                {
                    coverButtons(sender as Button);
                    currentPlayer.BackColor = m_CurrentPlayerColor;
                }

                updateButtonsText();
                updateLabels();
                if(m_GameManager.IsGameOver())
                {
                    DialogResult playAnotherGame = MessageBox.Show(gameOverMessage(), "Game Over", MessageBoxButtons.YesNo);
                    if(playAnotherGame == DialogResult.Yes)
                    {
                        resetGame();
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }

            m_IsFirstClick = !m_IsFirstClick;
        }

        private string gameOverMessage()
        {
            string finalMsg;

            if(m_FirstPlayer.Score == m_SecondPlayer.Score)
            {
                finalMsg = "It's a draw, maybe next time we'll have a winner :-(";
            }
            else
            {
                string winner = m_FirstPlayer.Score > m_SecondPlayer.Score ? m_FirstPlayer.Name : m_SecondPlayer.Name;
                finalMsg = string.Format("Congratulations {0}!! You are the Winner!!", winner);
            }

            return string.Format(
@"Final Score:
{0}
{1}
{2}
Play another game?", firstPlayer.Text, secondPlayer.Text, finalMsg);
        }

        private GameCell findChosenGameCell(Button i_SelectedCard)
        {
            GameCell chosendCard = null;

            for (int i = 0; i < m_Board.Height; i++)
            {
                for (int j = 0; j < m_Board.Width; j++)
                {
                    if (i_SelectedCard == m_GameCards[i, j])
                    {
                        m_Board.BoardCells[i, j].IsRevealed = true;
                        i_SelectedCard.Text = m_Board.BoardCells[i, j].ToString();
                        i_SelectedCard.BackColor = m_CurrentPlayerColor;
                        chosendCard = m_Board.BoardCells[i, j];
                        break;
                    }
                }
            }

            return chosendCard;
        }

        private void updateButtonsText()
        {
            for (int i = 0; i < m_Board.Height; i++)
            {
                for (int j = 0; j < m_Board.Width; j++)
                {
                    m_GameCards[i, j].Text = m_Board.BoardCells[i, j].ToString();
                }
            }
        }

        private void coverButtons(Button i_ButtonToCover)
        {
            i_ButtonToCover.Enabled = true;
            m_FirstChosenButton.Enabled = true;
            i_ButtonToCover.BackColor = Color.DarkGray;
            m_FirstChosenButton.BackColor = Color.DarkGray;
        }

        private void resetGame()
        {
            m_FirstPlayer.Score = 0;
            m_SecondPlayer.Score = 0;
            if (m_SecondPlayer.Type == ePlayerType.Computer)
            {
                m_SecondPlayer.ResetComputerMemory();
            }
            
            m_Board = new Board(m_Board.Height, m_Board.Width);
            m_GameManager = new GameManager(m_FirstPlayer, m_SecondPlayer, m_Board);
            resetButtons();
            updateLabels();
            if(m_FirstPlayer.Score > m_CurrentPlayer.Score)
            {
                m_CurrentPlayer = m_FirstPlayer;
                m_CurrentPlayerColor = m_FirstPlayerColor;
            }
        }

        private void resetButtons()
        {
            for (int i = 0; i < m_Board.Height; i++)
            {
                for (int j = 0; j < m_Board.Width; j++)
                {
                    m_GameCards[i, j].BackColor = Color.DarkGray;
                    m_GameCards[i, j].Text = m_Board.BoardCells[i, j].ToString();
                    m_GameCards[i, j].Enabled = true;
                }
            }
        }

        //internal void RunGames()
        //{
        //    bool playAnotherGame;

        //    do
        //    {
        //        //resetGame(firstPlayer, secondPlayer);
        //        playAnotherGame = runSingleGame();
        //    }
        //    while (playAnotherGame);
        //}

        //private bool runSingleGame()
        //{
        //    GameManager gameManager = new GameManager(m_FirstPlayer, m_SecondPlayer, m_Board);
        //    bool gameStillActive = true;
        //    //m_CurrentPlayer = m_FirstPlayer;

        //    while (gameStillActive)
        //    {
        //        GameCell firstCell = null;
        //        GameCell secondCell = null;

        //        //// Get a move from the player / computer - if player quits returns false
        //        gameStillActive = getPlayerMove(currentPlayer, i_Board, boardPainter, ref firstCell, ref secondCell);
        //        if (!gameStillActive)
        //        {
        //            break;
        //        }

        //        if (i_SecondPlayer.Type == ePlayerType.Computer)
        //        {
        //            i_SecondPlayer.ComputerRememberCell(firstCell);
        //            i_SecondPlayer.ComputerRememberCell(secondCell);
        //        }

        //        currentPlayer = gameManager.ExecuteMove(currentPlayer, firstCell, secondCell);
        //        if (gameManager.IsGameOver())
        //        {
        //            break;
        //        }
        //    }

        //    if (gameStillActive)
        //    { // Finished game without quitting
        //        announceWinner(i_FirstPlayer, i_SecondPlayer);
        //        gameStillActive = stillWantToPlay();
        //    }

        //    return gameStillActive;
        //}
    }
}
