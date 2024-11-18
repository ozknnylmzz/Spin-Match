using Cysharp.Threading.Tasks;
using SpinMatch.Boards;
using SpinMatch.Data;
using SpinMatch.Enums;
using SpinMatch.Game;
using SpinMatch.Items;
using SpinMatch.Matchs;
using UnityEngine;

namespace SpinMatch.Level
{
    public class BoardMapGenerator : MonoBehaviour
    {
        [SerializeField] private AllItemsData _allItemsData;

        private ItemGenerator _itemGenerator;
        private MatchDataProvider _matchDataProvider;
        private IBoard _board;

        public void Initialize(IBoard board, ItemGenerator itemGenerator, GameConfig gameConfig)
        {
            _board = board;
            _itemGenerator = itemGenerator;
            _matchDataProvider = gameConfig.MatchDataProvider;
            _itemGenerator.ConfigureRequiredItems();
        }

        public void SetConfigureTypes(int[] possibleConfigureTypes)
        {
            _itemGenerator.SetConfigureTypes(possibleConfigureTypes);
        }

        public void FillBoardWithSpinItems()
        {
            foreach (IGridSlot gridSlot in _board.AllSlots)
            {
                SetSpinItemInBoard(_board, gridSlot, out GridItem item);
                _board.AddSpinItem(item);
            }
        }

        public void FillTopWithSpinItems()
        {
            foreach (IGridSlot topSlot in _board.TopSlots)
            {
                SetItemWithoutMatch(_board, topSlot, out GridItem item);
                _board.AddSpinItem(item);
                _itemGenerator.SetItemOnSlotPosition(item, topSlot);
            }
            
        }

        public async UniTask FillBoardItems()
        {
            int topSlotIndex = 0;
            _board.BoardItems.Clear();
            _board.ClearAllSlot();
            foreach (IGridSlot slot in _board.InBoardSlots)
            {
                SetItemWithoutMatch(_board, slot, out GridItem item);
                _board.AddBoardItem(item);
                _board.AddSpinItem(item);
                _itemGenerator.SetItemOnSlotPosition(item, _board.TopSlots[topSlotIndex]);
                topSlotIndex++;

                
                await UniTask.Yield(); 
            }
           
        }

        public void GenerateItemsPool(ItemType itemType)
        {
            ItemData itemData = _allItemsData.GetItemDataOfType(itemType);
            _itemGenerator.GeneratePool(itemData.ItemPrefab, itemData.ConfigureData.ItemPoolSize);
        }

        private void SetItemWithoutMatch(IBoard board, IGridSlot slot, out GridItem item)
        {
            while (true)
            {
                item = _itemGenerator.CheckRequiredItem()
                    ? _itemGenerator.GetRandomNormalItem()
                    : _itemGenerator.GetRequiredItem();
                _itemGenerator.SetItemOnSlot(item, slot);
                BoardMatchData boardMatchData = _matchDataProvider.GetMatchData(board, slot.GridPosition);

                if (!boardMatchData.MatchExists) return;
                _itemGenerator.AddItem();
                item.Hide();
            }
        }

        private void SetSpinItemInBoard(IBoard board, IGridSlot slot, out GridItem item)
        {
            item = _itemGenerator.GetRandomNormalItem();
            _itemGenerator.SetItemOnSlotPosition(item, slot);
        }

    }
}