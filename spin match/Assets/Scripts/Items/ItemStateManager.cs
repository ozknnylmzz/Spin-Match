using Cysharp.Threading.Tasks;
using SpinMatch.Enums;
using System.Collections.Generic;
using SpinMatch.Jobs;

namespace SpinMatch.Items
{
    public static class ItemStateManager
    {
        private static readonly Dictionary<GridItem, List<Job>> _itemJobPairs = new();

        public static void AddJobToItems(IEnumerable<GridItem> items, Job job)
        {
            foreach (GridItem item in items)
            {
                AddJobToItems(item, job);
            }
        }

        private static void AddJobToItems(GridItem item, Job job)
        {
            if (_itemJobPairs.ContainsKey(item))
            {
                _itemJobPairs[item].Add(job);
            }
            else
            {
                _itemJobPairs[item] = new() { job };
            }
        }

        public static void SetAllItemsState()
        {
            foreach (var itemJobPair in _itemJobPairs)
            {
                SetItemState(itemJobPair);
            }
            
            _itemJobPairs.Clear();
        }

        private static async void SetItemState(KeyValuePair<GridItem, List<Job>> itemJobPair)
        {
            await UniTask.WhenAll(itemJobPair.Value.Select(job => UniTask.WaitUntil(() => job.JobCompleted)));

            if (itemJobPair.Key.ItemStateDelay > 0)
            {
                await UniTask.Delay(itemJobPair.Key.ItemStateDelay);
            }

            itemJobPair.Key.SetState(ItemState.Hide);
        }

    }
}