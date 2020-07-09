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
            selectedCell = ComputerRandomMove(i_Board);

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
    }
}
