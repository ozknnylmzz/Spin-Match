using System.Collections;
using System.Collections.Generic;
using SpinMatch.data;
using SpinMatch.Data;
using UnityEngine;

namespace SpinMatch.Boards
{
    public class Board : MonoBehaviour, IBoard
    {
        [SerializeField] private BoardConfigData _boardConfigData;

        private const float _cellSize = 1f;
        private Vector3 _originPos;

        private IGridSlot[,] _gridSlots;
        private GridPosition[] _allGridPositions;
        private List<IGridSlot> _topSlots = new(); 
        private List<IGridSlot> _bottomSlots = new(); 
        public IGridSlot this[GridPosition gridPosition] => _gridSlots[gridPosition.RowIndex, gridPosition.ColumnIndex];
        public IGridSlot this[int rowIndex, int columnIndex] => _gridSlots[rowIndex, columnIndex];

        public GridPosition[] AllGridPositions => _allGridPositions;
        // public IGridSlot[] AllGridSlots  => _allGridSlots;
        public int RowCount => _boardConfigData.RowCount;
        public int ColumnCount => _boardConfigData.ColumnCount;

        public void Initialize()
        {
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            _gridSlots = new IGridSlot[RowCount, ColumnCount];
            _allGridPositions = new GridPosition[RowCount * ColumnCount];

            _originPos = GetOriginPosition(RowCount, ColumnCount);

            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    int iteration = i * ColumnCount + j;
                    Vector3 slotPosition = GridToWorldPosition(i, j);

                    GridSlot gridSlot =
                        Instantiate(_boardConfigData.Grid, slotPosition, Quaternion.identity, transform);
                    gridSlot.name = "(" + i + " , " + j + ")";

                    GridPosition gridPosition = new GridPosition(i, j);
                    gridSlot.SetPosition(gridPosition, slotPosition);

                    _gridSlots[i, j] = gridSlot;
                    _allGridPositions[iteration] = gridPosition;
                }
            }
            
            GenerateSlotsOutBoard( _topSlots, Constants.SLOT_TYPE_TOP);
            GenerateSlotsOutBoard( _bottomSlots, Constants.SLOT_TYPE_BOTTOM);
        }

        private void GenerateSlotsOutBoard(List<IGridSlot> slotList, string slotType)
        {
            for (int i = 0; i < ColumnCount; i++)
            {
                for (int j = 0; j < RowCount; j++)
                {
                    Vector3 slotPosition=slotType==Constants.SLOT_TYPE_TOP
                        ?GridToWorldPosition(i + RowCount, j)
                        : GridToWorldPosition(i - RowCount, j); 
                 
                    GridSlot sideSlot = Instantiate(_boardConfigData.Grid, slotPosition, Quaternion.identity, transform);
                    sideSlot.name = $"{slotType} {i}";
                    slotList.Add(sideSlot); 
                }
            }
        }
        
        public bool IsPointerOnBoard(Vector3 pointerWorldPos, out GridPosition gridPosition)
        {
            gridPosition = WorldToGridPosition(pointerWorldPos);

            return IsPositionOnBoard(gridPosition);
        }

        public bool IsPositionInBounds(GridPosition gridPosition)
        {
            return gridPosition.IsPositionInBounds(RowCount, ColumnCount);
        }

        private bool IsPositionOnBoard(GridPosition gridPosition)
        {
            return IsPositionInBounds(gridPosition);
        }

        public bool IsPositionOnItem(GridPosition gridPosition)
        {
            return IsPositionInBounds(gridPosition) && this[gridPosition].CanContainItem;
        }

        private GridPosition WorldToGridPosition(Vector3 pointerWorldPos)
        {
            Vector2 gridPos = (pointerWorldPos - _originPos) / (_cellSize + _boardConfigData.CellSpacing);
            int rowIndex = Mathf.FloorToInt(gridPos.y + _cellSize / 2);
            int columnIndex = Mathf.FloorToInt(gridPos.x + _cellSize / 2);

            return new GridPosition(rowIndex, columnIndex);
        }

        public Vector3 GridToWorldPosition(GridPosition gridPosition)
        {
            return GridToWorldPosition(gridPosition.RowIndex, gridPosition.ColumnIndex);
        }
        
        private Vector3 GridToWorldPosition(int rowIndex, int columnIndex)
        {
            return new Vector3(columnIndex, rowIndex) * (_cellSize + _boardConfigData.CellSpacing) + _originPos;
        }

        private Vector3 GetOriginPosition(int rowCount, int columnCount)
        {
            float gridWidth = columnCount * _cellSize + (columnCount - 1) * _boardConfigData.CellSpacing;
            float gridHeight = rowCount * _cellSize + (rowCount - 1) * _boardConfigData.CellSpacing;

            float originX = -gridWidth / 2.0f + _cellSize / 2;
            float originY = -gridHeight / 2.0f + _cellSize / 2;

            return new Vector3(originX, originY);
        }

        public IEnumerator<IGridSlot> GetEnumerator()
        {
            foreach (IGridSlot slot in _gridSlots)
            {
                yield return slot;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}