using System.Collections.Generic;
using System.Linq;
using SpinMatch.Boards;
using SpinMatch.Enums;
using SpinMatch.Items;
using SpinMatch.Jobs;

namespace SpinMatch.Strategy
{
    public class FallDownFillStrategy : BaseFillStrategy
    {
        private int oldColumnIndex = -1;
        private int extraRowIndex;

        public FallDownFillStrategy(IBoard board, ItemGenerator itemGenerator) : base(board, itemGenerator)
        {
        }

        public override void AddFillJobs(IEnumerable<IGridSlot> allSlots, IEnumerable<GridItem> allItems)
        {
            IEnumerable<int> fallSlotsColumnIndexes = allSlots.Select(slot => slot.GridPosition.ColumnIndex);
            List<ItemFallData> allItemsFallData = new();

            ResetDropSlots();

            foreach (int columnIndex in fallSlotsColumnIndexes)
            {
                IEnumerable<GridItem> itemsToHideOnColumn = GetItemsOnColumn(allItems, columnIndex);
                List<ItemFallData> itemsFallData = GetItemsFallDataPerColumn(columnIndex);

                allItemsFallData.AddRange(itemsFallData);

                if (itemsFallData.Count != 0)
                {
                    JobsExecutor.AddFallJob(new ItemsFallJob(itemsFallData, itemsToHideOnColumn), columnIndex);
                }
            }
        }

        private List<ItemFallData> GetItemsFallDataPerColumn(int columnIndex)
        {
            List<ItemFallData> itemsFallData = new List<ItemFallData>();
            
            DropItemsOnBoard(_board, columnIndex, itemsFallData);

            DropItemsAboveBoard(_board, columnIndex, itemsFallData);

            oldColumnIndex = -1;
            itemsFallData.Reverse();

            return itemsFallData;
        }

        private void DropItemsOnBoard(IBoard board, int columnIndex, List<ItemFallData> itemsFallData)
        {
            for (int rowIndex = 0; rowIndex < board.RowCount; rowIndex++)
            {
                IGridSlot currentSlot = board[rowIndex, columnIndex];

                if (!currentSlot.HasItem)
                {
                    continue;
                }

                if (!CanDropDown(board, currentSlot, out GridPosition destinationPosition))
                {
                    continue;
                }

                GridItem item = currentSlot.Item;
                
                _board.AddBoardItem(item);
                    
                currentSlot.ClearSlot();

                IGridSlot destinationSlot = board[destinationPosition];
                
                destinationSlot.SetItem(item);
                
                _board.AddSpinItem(item);
                
                int pathDistance = rowIndex - destinationPosition.RowIndex;
               
                item.SetState(ItemState.WaitingToFall);

                itemsFallData.Add(new ItemFallData(item, destinationSlot, pathDistance));
            }
        }

        private void DropItemsAboveBoard(IBoard board, int columnIndex, List<ItemFallData> itemsFallData)
        {
            for (int rowIndex = 0; rowIndex < board.RowCount; rowIndex++)
            {
                IGridSlot currentSlot = board[rowIndex, columnIndex];
                if (!currentSlot.CanSetItem)
                {
                    continue;
                }
                GridPosition abovePosition = GetFallPositionAboveBoard(board, columnIndex);

                GridItem item = _itemGenerator.GetRandomNormalItem();
                
                 _board.AddBoardItem(item);

                item.SetWorldPosition(board.GridToWorldPosition(abovePosition));

                currentSlot.SetItem(item);
                
                _board.AddSpinItem(item);

                currentSlot.SetItemDrop(true);

                int pathDistance = abovePosition.RowIndex - rowIndex;

                item.SetState(ItemState.WaitingToFall);

                itemsFallData.Add(new ItemFallData(item, currentSlot, pathDistance));
            }
        }

        private HashSet<GridItem> GetItemsOnColumn(IEnumerable<GridItem> allItems, int columnIndex)
        {
            HashSet<GridItem> matchItemsOnColumn = new();

            foreach (GridItem item in allItems)
            {
                GridPosition itemPosition = item.ItemSlot.GridPosition;

                if (itemPosition.ColumnIndex == columnIndex)
                {
                    matchItemsOnColumn.Add(item);
                }
            }

            return matchItemsOnColumn;
        }

        private void ResetDropSlots()
        {
            foreach (IGridSlot slot in _board)
            {
                slot.SetItemDrop(false);
            }
        }

        private GridPosition GetFallPositionAboveBoard(IBoard board, int columnIndex)
        {
            if (columnIndex == oldColumnIndex)
            {
                extraRowIndex++;
            }
            else
            {
                extraRowIndex = 0;
            }

            oldColumnIndex = columnIndex;

            return new GridPosition(board.RowCount + extraRowIndex, columnIndex);
        }

        private bool CanDropDown(IBoard board, IGridSlot gridSlot, out GridPosition destinationPosition)
        {
            IGridSlot destinationSlot = gridSlot;

            while (board.CanMoveDown(destinationSlot, out GridPosition bottomPosition))
            {
                destinationSlot = board[bottomPosition];
            }

            destinationPosition = destinationSlot.GridPosition;

            return destinationSlot != gridSlot;
        }
    }
}