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
            int topSlotIndex=0;
            
            for (int i = 0; i < _board.RowCount; i++)
            {
                for (int j = 0; j < _board.ColumnCount; j++)
                {
                    IGridSlot gridSlot = _board[i, j];

                    if (!gridSlot.CanSetItem)
                        continue;

                    SetItemWithoutMatch(_board, gridSlot,out GridItem item);
                    topSlotIndex++;
                }
            }
        }

        public void FillOutBoardItems()
        {
            int topSlotIndex=_board.ColumnCount*_board.RowCount;
            
            foreach (IGridSlot slot in _board.TopSlots)
            {
                if (!slot.CanSetItem)
                    continue;
                
                SetItemWithoutMatch(_board, slot,out GridItem item);
                topSlotIndex--;
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
                // item.SetDestinationSlot(slot);
                 _itemGenerator.SetItemOnSlot(item, slot);

                BoardMatchData boardMatchData = _matchDataProvider.GetMatchData(board, slot.GridPosition);

                if (!boardMatchData.MatchExists) return;
                _itemGenerator.AddItem();
                item.Hide();
            }
        }
    }
}