using SpinMatch.Boards;

namespace SpinMatch.Matchs
{
    public interface IMatchDataProvider
    {
        public BoardMatchData GetMatchData(IBoard board, GridPosition[] gridPositions);
    }
}