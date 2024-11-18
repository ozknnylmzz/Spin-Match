using Cysharp.Threading.Tasks;
using DG.Tweening;
using SpinMatch.Enums;
using SpinMatch.Items;
using System.Collections.Generic;
using UnityEngine;

namespace SpinMatch.Jobs
{
    public class ItemsHideJob : Job
    {
        private const float ScaleDuration = 0.13f;

        private readonly IEnumerable<GridItem> _items;

        public ItemsHideJob(IEnumerable<GridItem> items)
        {
            _items = items;

            ItemStateManager.AddJobToItems(items, this);
        }

        public override async UniTask ExecuteAsync()
        {
            Debug.Log("ItemsHideJob  Start"); 
            
            // await UniTask.WhenAll(_items.Select(item => UniTask.WaitUntil(() => item.ItemState == ItemState.Rest)));
            Debug.Log("ItemsHideJob  End");
            

            Sequence sequence = DOTween.Sequence();

            foreach (GridItem item in _items)
            {

                _ = sequence.Join(item.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), ScaleDuration)
                            .SetEase(Ease.InOutSine));
            }

            await sequence;
            
            foreach (GridItem item in _items)
            {
                item.Kill(isSpecialKill: false);
            }

            await UniTask.Delay(64);

            JobCompleted = true;
        }
    }
}