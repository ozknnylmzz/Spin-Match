using System.Collections.Generic;
using SpinMatch.Boards;
using SpinMatch.Enums;

namespace SpinMatch.Matchs
{
    public class VerticalMatchDetector : MatchDetector
    {
        private readonly GridPosition[] _lookUpDirections;

        public override MatchDetectorType MatchDetectorType => MatchDetectorType.Vertical;

        public VerticalMatchDetector()
        {
            _lookUpDirections = new[] { GridPosition.Up, GridPosition.Down };
        }

        public override MatchSequence GetMatchSequence(IBoard board, GridPosition initialPosition)
        {
            IGridSlot initialSlot = board[initialPosition];
            List<IGridSlot> matchedGridSlots = new List<IGridSlot> { initialSlot };

            foreach (GridPosition direction in _lookUpDirections)
            {
                GridPosition newPosition = initialPosition + direction;

                while (IsMatchAvailable(board, newPosition, initialSlot, out IGridSlot matchSlot))
                {
                    newPosition += direction;
                    matchedGridSlots.Add(matchSlot);
                }
            }

            if (IsEnoughMatch(matchedGridSlots))
            {
                return new MatchSequence(matchedGridSlots, MatchDetectorType);
            }

            return null;
        }
    } 
}