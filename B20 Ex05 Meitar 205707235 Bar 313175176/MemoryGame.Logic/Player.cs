using System;
using System.Collections.Generic;
using System.Threading;

namespace MemoryGame.Logic
{
    public enum ePlayerType
    {
        Human = 1,
        Computer = 2
    }

    public class Player
    {
        private readonly Random r_Random = new Random();
        private readonly string r_Name;
        private readonly ePlayerType r_Type;
        private ushort m_Score;
        private Dictionary<char, GameCell> m_ComputerMemory = null;

        public Player(string i_PlayerName, ePlayerType i_PlayerType)
        {
            r_Name = i_PlayerName;
            r_Type = i_PlayerType;
            m_Score = 0;

            if(i_PlayerType == ePlayerType.Computer)
            {
                m_ComputerMemory = new Dictionary<char, GameCell>();
            }
        }

        public ePlayerType Type
        {
            get
            {
                return r_Type;
            }
        }

        public string Name
        {
            get
            {
                return r_Name;
            }
        }

        public ushort Score
        {
            get
            {
                return m_Score;
            }

            set
            {
                m_Score = value;
            }
        }

        public GameCell PlayerMove(Board i_Board) // Returns null if player quits
        {
            GameCell selectedCell = null;

            if (Type == ePlayerType.Human)
            {
                const bool v_IvalidInput = true;
                while (v_IvalidInput)
                {
                    string inputMoveFromUser = Console.ReadLine();
                    if (isLeaving(inputMoveFromUser))
                    {
                        break;
                    }

                    if (validateMove(inputMoveFromUser, i_Board))
                    {
                        selectedCell = i_Board.BoardCells[inputMoveFromUser[1] - '1', inputMoveFromUser[0] - 'A'];
                        break;
                    }
                }
            }
            else // Computer First Move - Always random, Second move is intelligent
            {
                selectedCell = ComputerRandomMove(i_Board);
            }

            if (selectedCell != null) // User Didn't Quit
            {
                selectedCell.IsRevealed = true;
                i_Board.UnRevealedCells.Remove(selectedCell);
            }

            return selectedCell;
        }

        public GameCell ComputerRandomMove(Board i_Board)
        {
            int gameCellIndex = r_Random.Next(i_Board.UnRevealedCells.Count);
            return i_Board.UnRevealedCells[gameCellIndex];
        }

        public GameCell ComputerAiMove(Board i_Board, GameCell i_FirstRevealedCell) // Second half of move is intelligent
        {
            GameCell selectedCell;

            if(m_ComputerMemory.TryGetValue(i_FirstRevealedCell.Letter, out selectedCell)) // Letter found in memory
            {
                //// Make sure it's not the same cell
                selectedCell = selectedCell != i_FirstRevealedCell ? selectedCell : ComputerRandomMove(i_Board);
            }
            else
            {
                Thread.Sleep(500); // wait a little before playing - for UX
                selectedCell = ComputerRandomMove(i_Board);
            }

            selectedCell.IsRevealed = true;
            i_Board.UnRevealedCells.Remove(selectedCell);

            return selectedCell;
        }

        public void ComputerRememberCell(GameCell i_GameCell) // computer remembers up to 1/2 of the board
        {
            if(!m_ComputerMemory.ContainsKey(i_GameCell.Letter))
            {
                m_ComputerMemory.Add(i_GameCell.Letter, i_GameCell);
            }
        }

        public void ResetComputerMemory()
        {
            m_ComputerMemory = new Dictionary<char, GameCell>();
        }

        private bool validateMove(string i_MoveInput, Board i_Board)
        {
            bool validMove = i_MoveInput.Length == 2 && char.IsUpper(i_MoveInput[0]) && char.IsDigit(i_MoveInput[1]);

            if(validMove) // valid syntax
            {
                int lineNum = i_MoveInput[1] - '1'; 
                int colNum = i_MoveInput[0] - 'A';
                validMove = lineNum >= 0 && lineNum < i_Board.Height && colNum >= 0 && colNum < i_Board.Width;
                if(!validMove) // Out of range
                {
                    //MessageDisplayer.DisplayMessage(MessageDisplayer.InvalidMoveOutOfRange);
                }
                else
                {
                    GameCell inputCell = i_Board.BoardCells[lineNum, colNum];
                    validMove = !inputCell.IsRevealed; // this should be false for a valid move!
                    if(!validMove) // Cell is Revealed
                    {
                        //MessageDisplayer.DisplayMessage(MessageDisplayer.InvalidMoveCellRevealed);
                    }
                }
            }
            else // invalid syntax
            {
                //MessageDisplayer.DisplayMessage(MessageDisplayer.InvalidMoveSyntaxError);
            }

            return validMove;
        }

        private bool isLeaving(string i_MoveInput)
        {
            return (i_MoveInput.Length == 1) && (i_MoveInput[0] == 'Q');
        }
    }
}
