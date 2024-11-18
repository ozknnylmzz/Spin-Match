using SpinMatch.Data;
using SpinMatch.Enums;

namespace SpinMatch.Level
{
    public class LevelLoader
    {
        public void Initialize(BoardMapGenerator boardMapGenerator)
        {
            LoadLevel(boardMapGenerator);
        }

        private void LoadLevel(BoardMapGenerator boardMapGenerator)
        {
            int[] configureTypes = Constants.CONFIGURETYPES_PIECE_VALUE_7;

            boardMapGenerator.SetConfigureTypes(configureTypes);
            boardMapGenerator.GenerateItemsPool(ItemType.BoardItem);
            boardMapGenerator.FillBoardWithSpinItems();
        }
    }
}