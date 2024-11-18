using Cysharp.Threading.Tasks;
using DG.Tweening;
using SpinMatch.Boards;
using SpinMatch.Enums;
using SpinMatch.Game;
using UnityEngine;

namespace SpinMatch.Items
{
    public class ItemSwapper
    {
        private const float SwapDuration = 0.13f;

        public async UniTask SwapItems(IGridSlot selectedSlot, IGridSlot targetSlot, Match3Game match3Game)
        {
            GridItem selectedItem = selectedSlot.Item;
            GridItem targetItem = targetSlot.Item;

            selectedSlot.SetItem(targetItem);
            targetSlot.SetItem(selectedItem);

            Vector3 selectedPosition = selectedSlot.WorldPosition;
            Vector3 targetPosition = targetSlot.WorldPosition;


            if (match3Game.IsMatchDetected(out BoardMatchData boardMatchData, selectedSlot.GridPosition, targetSlot.GridPosition) )
            {
                EventManager.Execute(BoardEvents.OnBeforeJobsStart);
            }

            await  DOTween.Sequence()
                .Join(selectedItem.transform.DOMove(targetPosition, SwapDuration))
                .Join(targetItem.transform.DOMove(selectedPosition, SwapDuration))
                .SetEase(Ease.Linear);
        }

    }
}