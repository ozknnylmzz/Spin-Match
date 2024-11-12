using System.Collections.Generic;
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
        public List<IGridSlot> BottomSlots { get; }
        public List<IGridSlot>InBoardSlots { get; }
        public List<IGridSlot>AllSlots { get; }
        public bool IsPointerOnBoard(Vector3 pointerWorldPos, out GridPosition gridPosition);
        public bool IsPositionInBounds(GridPosition gridPosition);
        public bool IsPositionOnItem(GridPosition gridPosition);
        public Vector3 GridToWorldPosition(GridPosition gridPosition);

    } 
}