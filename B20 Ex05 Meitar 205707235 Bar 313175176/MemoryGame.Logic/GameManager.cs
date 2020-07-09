using System.Threading;

namespace MemoryGame.Logic
{
    public class GameManager
    {
        private Player m_FirstPlayer;
        private Player m_SecondPlayer;
        private Board m_Board;

        public GameManager(Player i_FirstPlayer, Player i_SecondPlayer, Board i_Board)
        {
            m_FirstPlayer = i_FirstPlayer;
            m_SecondPlayer = i_SecondPlayer;
            m_Board = i_Board;
        }

        public Player ExecuteMove(Player i_CurrentPlayer, GameCell i_FirstCell, GameCell i_SecondCell)
        {
            Player nextPlayer = i_CurrentPlayer;

            m_Board.UnRevealedCells.Remove(i_FirstCell);
            m_Board.UnRevealedCells.Remove(i_SecondCell);
            if (i_FirstCell.Letter == i_SecondCell.Letter)
            {
                i_CurrentPlayer.Score++;
                m_Board.RemainingCouples--;
                if(i_CurrentPlayer.Type == ePlayerType.Computer)
                {
                    Thread.Sleep(2000);
                }
            }
            else
            {
                Thread.Sleep(2000);
                coverCells(i_FirstCell, i_SecondCell);
                nextPlayer = togglePlayer(i_CurrentPlayer);
            }

            if (m_SecondPlayer.Type == ePlayerType.Computer)
            {
                m_SecondPlayer.ComputerRememberCell(i_FirstCell);
                m_SecondPlayer.ComputerRememberCell(i_SecondCell);
            }

            return nextPlayer;
        }

        public bool IsGameOver()
        {
            return m_Board.RemainingCouples == 0;
        }

        private Player togglePlayer(Player i_Player)
        {
            Player newPlayer = m_FirstPlayer;

            if(i_Player == m_FirstPlayer)
            {
                newPlayer = m_SecondPlayer;
            }

            return newPlayer;
        }

        private void coverCells(GameCell i_CellOne, GameCell i_CellTwo)
        {
            m_Board.UnRevealedCells.Add(i_CellOne);
            m_Board.UnRevealedCells.Add(i_CellTwo);
            i_CellOne.IsRevealed = false;
            i_CellTwo.IsRevealed = false;
        }
    }
}
