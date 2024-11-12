using System.Collections;
using System.Collections.Generic;
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

        public void FillBoardWithItems()
        {
            int topSlotIndex = 0;
            foreach (IGridSlot gridSlot in _board.TopSlots)
            {
                SetItemWithoutMatch(_board, gridSlot,out GridItem item);
                item.SetDestinationSlot(_board.InBoardSlots[topSlotIndex]);
                topSlotIndex++;
            }
        }

        public void FillOutBoardItems()
        {
            foreach (IGridSlot slot in _board.InBoardSlots)
            {
                if (!slot.CanSetItem)
                    continue;
                SetItemWithoutMatch(_board, slot,out GridItem item);
            }
        }

        public void GenerateItemsPool(ItemType itemType)
        {
            ItemData itemData = _allItemsData.GetItemDataOfType(itemType);
            _itemGenerator.GeneratePool(itemData.ItemPrefab, itemData.ConfigureData.ItemPoolSize);
        }

        private void SetItemWithoutMatch(IBoard board, IGridSlot slot,out GridItem item)
        { 
            while (true)
            {
                item = _itemGenerator.CheckRequiredItem() ? _itemGenerator.GetRandomNormalItem() : _itemGenerator.GetRequiredItem();
                 _itemGenerator.SetItemOnSlot(item, slot);

                BoardMatchData boardMatchData = _matchDataProvider.GetMatchData(board, slot.GridPosition);

                if (!boardMatchData.MatchExists) return;
                _itemGenerator.AddItem();
                item.Hide();
            }
        }
    }
}