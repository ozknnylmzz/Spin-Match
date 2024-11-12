using System.Collections.Generic;
using SpinMatch.Boards;
using SpinMatch.Enums;
using UnityEngine;

namespace SpinMatch.Items
{
    public class ItemGenerator : MonoBehaviour
    {
        private readonly Dictionary<ItemType, ObjectPool<GridItem>> _itemPools = new();

        private int[] _possibleConfigureTypes;

        private List<int> _requiredItems;

        private int _requiredItemID;

        private bool _requiredItem;
        
        public void GeneratePool(GridItem prefab, int itemPoolSize)
        {
            _itemPools[prefab.ItemType] = new ObjectPool<GridItem>(prefab, itemPoolSize, transform, InitializeItem);
        }

        public void SetConfigureTypes(int[] possibleConfigureTypes)
        {
            _possibleConfigureTypes = possibleConfigureTypes;
        }

        private GridItem GetItemWithId(ItemType itemType, int configureType = 0)
        {
            GridItem item = _itemPools[itemType].GetFromPool();

            ConfigureItem(item, configureType);
            return item;
        }

        public GridItem GetRandomNormalItem()
        {
            return GetItemWithId(ItemType.BoardItem, _possibleConfigureTypes.ChooseRandom());
        }

        public GridItem GetRequiredItem()
        {
            return GetItemWithId(ItemType.BoardItem, GetRandomItemAndRemove());
        }
        
        public void ReturnItemToPool(GridItem item)
        {
            _itemPools[item.ItemType].ReturnToPool(item);
        }

        public void SetItemOnSlot(GridItem item, IGridSlot slot)
        {
            item.SetWorldPosition(slot.WorldPosition);

            slot.SetItem(item);
        }

        public void SetItemPosition(GridItem item,Vector2 worldPosition)
        {
            item.SetWorldPosition(worldPosition);
        }
        
        public void ConfigureRequiredItems()
        {
            _requiredItems = new List<int>();

            for (int i = 0; i <= 6; i++) 
            {
                for (int j = 0; j < 3; j++) 
                {
                    _requiredItems.Add(i);
                }
            }
        }

        public bool CheckRequiredItem()
        {
            return _requiredItem;
        }
        
        private int GetRandomItemAndRemove()
        {
            if (_requiredItems.Count == 0)
            {
                _requiredItem = true;
                return 0; 
            }

            int randomIndex = Random.Range(0, _requiredItems.Count); 
            int item = _requiredItems[randomIndex]; 
            _requiredItems.RemoveAt(randomIndex);
            _requiredItemID = item;
            return item; 
        }
        
        public void AddItem()
        {
            _requiredItems.Add(_requiredItemID);
        }

        private void ConfigureItem(GridItem item, int configureType)
        {
            item.ConfigureItem(configureType);
            item.ResetItem();
        }

        private void InitializeItem(GridItem item)
        {
            item.SetGenerator(this);
            item.Initialize();
        }
    }
}