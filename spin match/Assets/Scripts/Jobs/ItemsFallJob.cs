using Cysharp.Threading.Tasks;
using DG.Tweening;
using SpinMatch.Enums;
using SpinMatch.Items;
using System.Collections.Generic;
using UnityEngine;

namespace SpinMatch.Jobs
{
    public class ItemsFallJob : Job
    {
        private const float IntervalDuration = 0.04f;

        private readonly IEnumerable<ItemFallData> _itemsFallData;
        private readonly IEnumerable<GridItem> _hideItemsOnColumn;

        private ItemsFallJob _nextItemsFallJob;

        public ItemsFallJob(IEnumerable<ItemFallData> itemsFallData, IEnumerable<GridItem> hideItemsOnColumn)
        {
            _itemsFallData = itemsFallData;
            _hideItemsOnColumn = hideItemsOnColumn;
        }

        public override async UniTask ExecuteAsync()
        {
            await UniTask.WhenAll(_hideItemsOnColumn.Select(item => UniTask.WaitUntil(() => item.ItemState == ItemState.Hide )));

            Sequence sequence = DOTween.Sequence();

            foreach (ItemFallData itemFallData in _itemsFallData)
            {
                itemFallData.Item.SetState(ItemState.Fall);
                _ = sequence
                    .Join(GetFallTween(itemFallData))
                    .PrependInterval(IntervalDuration);
            }

            await sequence;

            JobCompleted = true;

            if (_nextItemsFallJob != null)
            {
                await _nextItemsFallJob.ExecuteAsync();
            }
        }

        private Tween GetFallTween(ItemFallData itemFallData)
        {
            Transform item = itemFallData.Item.transform;
            float pathDistance = itemFallData.PathDistance;

            float fallDuration = pathDistance <= 4
                ? 0.08f * pathDistance + 0.08f
                : pathDistance * 0.1f;

            return item.DOMove(itemFallData.DestinationSlot.WorldPosition, fallDuration)
                        .SetEase(Ease.InSine)
                        .OnComplete(() =>
                        {
                            if (itemFallData.Item.DestinationSlot == itemFallData.DestinationSlot)
                            {
                                itemFallData.DestinationSlot.SetItemDrop(false);
                                GetBounceTween(item, itemFallData);
                            }
                        });
        }

        private void GetBounceTween(Transform item, ItemFallData itemFallData)
        {
            Sequence seq = DOTween.Sequence();

            float itemYPos = item.position.y;

            seq.Append(item.DOMoveY(itemYPos - 0.015f, 0.04f)
                           .SetEase(Ease.OutQuad))
               .Append(item.DOMoveY(itemYPos + 0.04f, 0.08f)
                           .SetEase(Ease.OutQuad))
               .Append(item.DOMoveY(itemYPos, 0.06f)
                           .SetEase(Ease.OutQuad))
               .OnComplete(() =>
               {
                   itemFallData.Item.SetState(ItemState.Rest);
                   itemFallData.Item.ResetPathDistance();
               });
        }

        public void SetNextFallJob(ItemsFallJob nextItemsFallJob)
        {
            _nextItemsFallJob = nextItemsFallJob;
        }
    }
}