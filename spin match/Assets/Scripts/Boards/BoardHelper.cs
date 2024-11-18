using System.Collections.Generic;
using SpinMatch.Items;

namespace SpinMatch.Boards
{
    public static class BoardHelper
    {
        public static HashSet<GridItem> GetItemsOfSlots(IEnumerable<IGridSlot> slotsToChooseFrom)
        {
            HashSet<GridItem> items = new();

            foreach (IGridSlot slot in slotsToChooseFrom)
            {
                items.Add(slot.Item);
            }

            return items;
        }
    }
}