using System.Collections.Generic;
using SpinMatch.Boards;
using SpinMatch.Items;

namespace SpinMatch.Strategy
{
    public class BoardClearStrategy
    {
        private readonly BaseFillStrategy _fillStrategy;

        public BoardClearStrategy(BaseFillStrategy fillStrategy)
        {
            _fillStrategy = fillStrategy;
        }

        public void Refill(IEnumerable<IGridSlot> allSlots, IEnumerable<GridItem> allItems)
        {
            _fillStrategy.AddFillJobs(allSlots, allItems);
        }

        public void ClearAllSlots(IEnumerable<IGridSlot> allSlots)
        {
            foreach (IGridSlot slot in allSlots)
            {
                slot.Item.ReturnToPool();
                slot.ClearSlot();
            }
        }
    }
}