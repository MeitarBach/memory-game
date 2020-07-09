using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using MemoryGame.Logic;

namespace MemoryGame.UI
{
    public partial class FormMemoryGame : Form
    {
        // Form controls constants
        private const int k_Margin = 20;
        private const int k_ButtonSize = 80;

        // Form constant colors
        private readonly Color r_FirstPlayerColor = Color.GreenYellow;
        private readonly Color r_SecondPlayerColor = Color.MediumPurple;
        private readonly Color r_CoveredButtonColor = Color.DarkGray;

        // Data structures
        private readonly Button[,] r_GameCardsButtons;
        private Dictionary<char, Image> m_LetterToImageMap;

        // Game logic entities
        private Board m_Board;
        private Player m_FirstPlayer;
        private Player m_SecondPlayer;
        private GameManager m_GameManager;
        private GameCell m_FirstCellChosen;
        private GameCell m_SecondCellChosen;

        // Current turn pointers
        private Color m_CurrentPlayerColor;
        private Player m_CurrentPlayer;
        private bool m_IsFirstClick = true;

        // Form labels
        private Label currentPlayer;
        private Label firstPlayer;
        private Label secondPlayer;

        public FormMemoryGame(FormSettings i_Settings)
        {
            m_Board = createLogicBoard(i_Settings);
            r_GameCardsButtons = createGameCardButtons();
            createPlayers(i_Settings);
            createDictionary();
            m_GameManager = new GameManager(m_FirstPlayer, m_SecondPlayer, m_Board);
            m_CurrentPlayer = m_FirstPlayer;
            m_CurrentPlayerColor = r_FirstPlayerColor;
            createLabels(i_Settings);
            this.CenterToScreen();
            InitializeComponent();
        }

        private Board createLogicBoard(FormSettings i_Settings)
        {
            i_Settings.GetBoardSize(out int boardHeight, out int boardWidth);

            return new Board(boardHeight, boardWidth);
        }

        private Button[,] createGameCardButtons()
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
                    cardsBoard[i, j].BackColor = r_CoveredButtonColor;
                    cardsBoard[i, j].Click += gameCard_Click;
                    cardsBoard[i, j].Text = m_Board.BoardCells[i, j].ToString();
                    m_Board.BoardCells[i, j].CellChangedRevealedState += gameCell_CellChangedRevealedState;
                    this.Controls.Add(cardsBoard[i, j]);
                }
            }

            return cardsBoard;
        }

        private void createPlayers(FormSettings i_Settings)
        {
            m_FirstPlayer = new Player(i_Settings.FirstPlayerName, ePlayerType.Human);
            ePlayerType opponentType = i_Settings.IsOpponentHuman() ? ePlayerType.Human : ePlayerType.Computer;
            m_SecondPlayer = new Player(i_Settings.SecondPlayerName, opponentType);
        }

        private void createDictionary()
        {
            m_LetterToImageMap = new Dictionary<char, Image>();
            int numOfLettersInBoard = (m_Board.Height * m_Board.Width) / 2;

            for (int i = 0; i < numOfLettersInBoard; i++)
            {
                char charToMapImage = (char)('A' + i);
                WebRequest request = WebRequest.Create("https://picsum.photos/80");
                WebResponse resonse = request.GetResponse();
                Stream streamImage = resonse.GetResponseStream();
                Image image = Image.FromStream(streamImage);
                m_LetterToImageMap.Add(charToMapImage, image);
            }
        }

        private void createLabels(FormSettings i_Settings)
        {
            int labelsX = this.Location.X + k_Margin;

            currentPlayer = new Label();
            currentPlayer.Text = string.Format("Current Player: {0}", m_CurrentPlayer.Name);
            currentPlayer.BackColor = r_FirstPlayerColor;
            int currentPlayerY = this.Location.Y + k_Margin + (m_Board.Height * (k_Margin + k_ButtonSize));
            currentPlayer.Location = new Point(labelsX, currentPlayerY);
            currentPlayer.AutoSize = true;
            this.Controls.Add(currentPlayer);

            firstPlayer = new Label();
            firstPlayer.Text = string.Format("{0}: {1} Pairs", m_FirstPlayer.Name, m_FirstPlayer.Score);
            firstPlayer.BackColor = r_FirstPlayerColor;
            firstPlayer.Location = new Point(labelsX, currentPlayerY + currentPlayer.Height + k_Margin);
            firstPlayer.AutoSize = true;
            this.Controls.Add(firstPlayer);

            secondPlayer = new Label();
            secondPlayer.Text = string.Format("{0}: {1} Pairs", m_SecondPlayer.Name, m_SecondPlayer.Score);
            secondPlayer.BackColor = r_SecondPlayerColor;
            secondPlayer.Location = new Point(labelsX, firstPlayer.Location.Y + currentPlayer.Height + k_Margin);
            secondPlayer.AutoSize = true;
            this.Controls.Add(secondPlayer);
        }

        private void updateLabels()
        {
            currentPlayer.BackColor = m_CurrentPlayerColor;
            currentPlayer.Text = string.Format("Current Player: {0}", m_CurrentPlayer.Name);
            firstPlayer.Text = string.Format("{0}: {1} Pairs", m_FirstPlayer.Name, m_FirstPlayer.Score);
            secondPlayer.Text = string.Format("{0}: {1} Pairs", m_SecondPlayer.Name, m_SecondPlayer.Score);
            this.Refresh();
        }

        private void gameCard_Click(object sender, EventArgs e)
        {
            if (m_IsFirstClick)
            {
                m_FirstCellChosen = findChosenGameCell(sender as Button);
            }
            else
            {
                m_SecondCellChosen = findChosenGameCell(sender as Button);
                if(m_SecondCellChosen != null)
                {
                    executeMove();
                    computerMove();
                    resetIfGameOver();
                }
            }

            toggleHalfTurn();
        }

        private void gameCell_CellChangedRevealedState()
        {
            for (int i = 0; i < m_Board.Height; i++)
            {
                for (int j = 0; j < m_Board.Width; j++)
                {
                    if (m_Board.BoardCells[i, j].IsRevealed)
                    {
                        m_LetterToImageMap.TryGetValue(m_Board.BoardCells[i, j].Letter, out Image image);
                        r_GameCardsButtons[i, j].Image = image;
                        //// Change Card's border color if it hadn't been discovered yet
                        if (m_Board.UnRevealedCells.Contains(m_Board.BoardCells[i, j]))
                        {
                            r_GameCardsButtons[i, j].FlatStyle = FlatStyle.Flat;
                            r_GameCardsButtons[i, j].FlatAppearance.BorderColor = m_CurrentPlayerColor;
                            r_GameCardsButtons[i, j].FlatAppearance.BorderSize = 3;
                        }
                    }
                    else // Cover unrevealed cards
                    {
                        r_GameCardsButtons[i, j].Image = null;
                        r_GameCardsButtons[i, j].FlatStyle = FlatStyle.Standard;
                    }
                }

                this.Refresh(); // Refresh form to see changes
            }
        }

        private void executeMove()
        {
            m_CurrentPlayer = m_GameManager.ExecuteMove(m_CurrentPlayer, m_FirstCellChosen, m_SecondCellChosen);
            m_CurrentPlayerColor = m_CurrentPlayer == m_FirstPlayer ? r_FirstPlayerColor : r_SecondPlayerColor;
        }

        private void computerMove()
        {
            while (m_CurrentPlayer.Type == ePlayerType.Computer && !m_GameManager.IsGameOver())
            {
                updateLabels();
                m_FirstCellChosen = m_CurrentPlayer.PlayerMove(m_Board);
                Thread.Sleep(1000);
                m_SecondCellChosen = m_CurrentPlayer.ComputerAiMove(m_Board, m_FirstCellChosen);
                executeMove();
                updateLabels();
            }
        }

        private void toggleHalfTurn()
        {
            if ((m_IsFirstClick && m_FirstCellChosen != null) || (!m_IsFirstClick && m_SecondCellChosen != null))
            {
                updateLabels();
                m_IsFirstClick = !m_IsFirstClick;
            }
        }

        private GameCell findChosenGameCell(Button i_SelectedCard)
        {
            GameCell chosendCard = null;
            bool foundGameCell = false;

            for (int i = 0; i < m_Board.Height; i++)
            {
                for (int j = 0; j < m_Board.Width; j++)
                {
                    foundGameCell = i_SelectedCard == r_GameCardsButtons[i, j];
                    if (foundGameCell)
                    {
                        if(!m_Board.BoardCells[i, j].IsRevealed)
                        {
                            m_Board.BoardCells[i, j].IsRevealed = true;
                            chosendCard = m_Board.BoardCells[i, j];
                        }

                        break;
                    }
                }

                if(foundGameCell)
                {
                    break;
                }
            }

            return chosendCard;
        }

        private void resetIfGameOver()
        {
            if (m_GameManager.IsGameOver())
            {
                updateLabels();
                DialogResult playAnotherGame = MessageBox.Show(gameOverMessage(), "Game Over", MessageBoxButtons.YesNo);
                if (playAnotherGame == DialogResult.Yes)
                {
                    resetGame();
                    computerMove();
                }
                else
                {
                    this.Close();
                }
            }
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
            updateLabels();
            resetButtons();
            createDictionary();
            //// Winner starts next game - Last player starts if it's a draw
            if (m_FirstPlayer.Score > m_CurrentPlayer.Score)
            {
                m_CurrentPlayer = m_FirstPlayer;
                m_CurrentPlayerColor = r_FirstPlayerColor;
            }
        }

        private void resetButtons()
        {
            for (int i = 0; i < m_Board.Height; i++)
            {
                for (int j = 0; j < m_Board.Width; j++)
                {
                    r_GameCardsButtons[i, j].Image = null;
                    r_GameCardsButtons[i, j].FlatStyle = FlatStyle.Standard;
                    m_Board.BoardCells[i, j].CellChangedRevealedState += gameCell_CellChangedRevealedState;
                }
            }
        }

        private string gameOverMessage()
        {
            string finalMsg;

            if (m_FirstPlayer.Score == m_SecondPlayer.Score)
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
    }
}
