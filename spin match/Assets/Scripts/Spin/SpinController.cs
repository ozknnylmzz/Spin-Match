using Cysharp.Threading.Tasks;
using DG.Tweening;
using SpinMatch.Boards;
using SpinMatch.Data;
using SpinMatch.Enums;
using SpinMatch.Items;
using SpinMatch.Level;
using UnityEngine;

namespace SpinMatch.Spin
{
    public class SpinController : MonoBehaviour
    {
        [SerializeField] private Transform _endValue;

        private IBoard _board;
        private BoardMapGenerator _boardMapGenerator;

        public void Initialize(IBoard board, BoardMapGenerator mapGenerator)
        {
            _board = board;
            _boardMapGenerator = mapGenerator;
        }

        public void Subscribe()
        {
            EventManager.Subscribe(BoardEvents.Spin, OnClickSpin);
            EventManager.Subscribe(BoardEvents.Stop, OnClickStop);
        }

        public void UnSubscribe()
        {
            EventManager.Unsubscribe(BoardEvents.Spin, OnClickSpin);
            EventManager.Unsubscribe(BoardEvents.Stop, OnClickStop);
        }

        private async void OnClickStop()
        {
           
            _board.ClearAllSlot();
          
            await _boardMapGenerator.FillBoardItems();
           
            var moveSequence = _board.MoveToSlotItem();
            moveSequence.OnComplete(() =>
            {
                _boardMapGenerator.FillTopWithSpinItems();
            });

            moveSequence.Play();
        }

        private void OnClickSpin()
        {
            _board.BoardItems.Clear();
            PlaySpinItems();
        }

        private Sequence _spinSequence;

        private void PlaySpinItems()
        {
            foreach (GridItem spinItem in _board.AllSpinItems)
            {
                if (spinItem != null)
                {
                    spinItem.MoveToSpinTarget(new Vector2(spinItem.transform.position.x, _endValue.position.y),
                        Constants.ITEM_SPIN_SPEED);
                }
            }
        }
    }
}