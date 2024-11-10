using System.Collections.Generic;
using SpinMatch.Boards;
using SpinMatch.Enums;

namespace SpinMatch.Matchs
{
    public abstract class MatchDetector:IMatchDetector
    {
        public abstract MatchDetectorType MatchDetectorType { get; }

        public abstract MatchSequence GetMatchSequence(IBoard board, GridPosition gridPosition);

        protected virtual int MinMatchAmount { get; } = 3;

        protected bool IsMatchAvailable(IBoard board, GridPosition newPosition, IGridSlot initialSlot, out IGridSlot currentSlot)
        {
            currentSlot = null;

            if (!board.IsPositionOnItem(newPosition))
            {
                return false;
            }

            currentSlot = board[newPosition];

            return IsSameSlot(initialSlot, currentSlot);
        }

        protected bool IsEnoughMatch(IReadOnlyList<IGridSlot> matchedGridSlots)
        {
            return matchedGridSlots.Count >= MinMatchAmount;
        }

        private bool IsSameSlot(IGridSlot initialSlot, IGridSlot currentSlot)
        {
            if (!currentSlot.HasItem )
            {
                return false;
            }

            return initialSlot.ItemId == currentSlot.ItemId;
        }
    
    }
}