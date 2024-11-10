using SpinMatch.Boards;
using UnityEngine;

namespace SpinMatch.data
{
    [CreateAssetMenu(menuName = "Board/BoardConfigData", order = 1)]
    public class BoardConfigData : ScriptableObject
    {
        public int RowCount;
        public int ColumnCount;
        public float CellSpacing;
        public GridSlot Grid;
    }
}
