using System.Collections.Generic;
using DG.Tweening;
using SpinMatch.Items;
using UnityEngine;

namespace SpinMatch.Boards
{
    public interface IBoard : IEnumerable<IGridSlot>
    {
        public int RowCount { get; }
        public int ColumnCount { get; }
        public IGridSlot this[GridPosition gridPosition] { get; }
        public IGridSlot this[int rowIndex, int columnIndex] { get; }
        public List<IGridSlot> TopSlots { get; }
        public List<IGridSlot>InBoardSlots { get; }
        public List<IGridSlot>AllSlots { get; }
        public List<GridItem>AllSpinItems { get; }
        public List<GridItem>BoardItems { get; set; }
        public bool IsPointerOnBoard(Vector3 pointerWorldPos, out GridPosition gridPosition);
        public bool IsPositionInBounds(GridPosition gridPosition);
        public bool IsPositionOnItem(GridPosition gridPosition);
        public Vector3 GridToWorldPosition(GridPosition gridPosition);
        public GridPosition[] AllGridPositions { get; }
        public bool IsPositionOnBoard(GridPosition gridPosition);
        public void ClearAllSlot();
        public void  AddSpinItem(GridItem spinItem);
        public void  AddBoardItem(GridItem boardItem);
        public Tween  MoveToSlotItem();
    } 
}