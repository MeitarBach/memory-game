using System;
using System.Collections.Generic;

namespace MemoryGame.Logic
{
    public class Board
    {
        private readonly int r_Height;
        private readonly int r_Width;
        private readonly GameCell[,] r_BoardCells;
        private readonly List<GameCell> r_UnRevealedCells;
        private int m_RemainingCouples;

        public Board(int i_Height, int i_Width)
        {
            r_Height = i_Height;
            r_Width = i_Width;
            m_RemainingCouples = (r_Height * r_Width) / 2;
            r_BoardCells = createBoard();
            shuffleBoard();
            r_UnRevealedCells = createUnRevealedCellsList();
        }

        public int Height
        {
            get
            {
                return r_Height;
            }
        }

        public int Width
        {
            get
            {
                return r_Width;
            }
        }

        public GameCell[,] BoardCells
        {
            get
            {
                return r_BoardCells;
            }
        }

        public int RemainingCouples
        {
            get
            {
                return m_RemainingCouples;
            }

            set
            {
                m_RemainingCouples = value;
            }
        }

        public List<GameCell> UnRevealedCells
        {
            get
            {
                return r_UnRevealedCells;
            }
        }

        private GameCell[,] createBoard()
        {
            GameCell[,] boardCells = new GameCell[r_Height, r_Width];
            int counterOfFilledCells = 0;

            for(int i = 0; i < r_Height; i++)
            {
                for(int j = 0; j < r_Width; j++)
                {
                    char temporaryChar = (char)('A' + (counterOfFilledCells / 2));
                    boardCells[i, j] = new GameCell(temporaryChar);
                    counterOfFilledCells++;
                }
            }
            
            return boardCells;
        }

        private List<GameCell> createUnRevealedCellsList()
        {
            List<GameCell> unRevealedCells = new List<GameCell>();

            foreach(GameCell gameCell in r_BoardCells)
            {
                unRevealedCells.Add(gameCell);
            }

            return unRevealedCells;
        }

        private void shuffleBoard()
        {
            int numberOfCells = r_Height * r_Width;
            Random random = new Random();

            for(int i = 0; i < numberOfCells - 1; i++)
            {
                int firstRow = i / r_Width;
                int firstColumn = i % r_Width;

                // Pick a random cell between i and the end of the array.
                int randomizeSecondIndex = random.Next(i + 1);
                int secondRow = randomizeSecondIndex / r_Width;
                int secondColumn = randomizeSecondIndex % r_Width;

                GameCell temp = r_BoardCells[firstRow, firstColumn];
                r_BoardCells[firstRow, firstColumn] = r_BoardCells[secondRow, secondColumn];
                r_BoardCells[secondRow, secondColumn] = temp;
            }
        }
    }
}