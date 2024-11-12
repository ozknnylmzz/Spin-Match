using DG.Tweening;
using SpinMatch.Boards;
using SpinMatch.Data;
using SpinMatch.Enums;
using UnityEngine;

namespace SpinMatch.Spin
{
    public class SpinController : MonoBehaviour
    {
        [SerializeField] private Transform _endValue;
        
        private IBoard _board;
       
        public void Initialize(IBoard board)
        {
            EventManager.Subscribe(BoardEvents.Spin, OnClickSpin);
            _board = board;
        }

        private void OnClickSpin()
        {
            PlaySpinItems();
        }

        private Sequence _spinSequence;

        private void PlaySpinItems()
        {
            foreach (IGridSlot slot in _board.AllSlots)
            {
                if (slot.Item != null)
                {
                    slot.Item.MoveToSpinTarget(new Vector2(slot.Item.transform.position.x,_endValue.position.y),Constants.ITEM_SPIN_SPEED);
                }
            }
        }
        
      
    }
}