using SpinMatch.Boards;
using SpinMatch.Enums;

namespace SpinMatch.Matchs
{
    public interface IMatchDetector
    {
        public MatchSequence GetMatchSequence(IBoard board, GridPosition gridPosition);
        public MatchDetectorType MatchDetectorType { get; }
    } 
}