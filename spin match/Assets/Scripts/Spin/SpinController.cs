using DG.Tweening;
using SpinMatch.Boards;
using SpinMatch.Enums;
using UnityEngine;

namespace SpinMatch.Spin
{
    public class SpinController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spinButton;
        [SerializeField] private SpriteRenderer _stopButton;
        
        private IBoard _board;
        
        
        public void Initialize(IBoard board)
        {
            EventManager.Subscribe(BoardEvents.Spin, OnClickSpin);
            _board = board;
        }

        private void OnClickSpin()
        {
            _spinButton.transform.DOScale(Vector3.one * 0.8f, 0.5f)
                .SetLoops(2, LoopType.Yoyo) 
                .SetEase(Ease.InOutQuad);
            //spin mekaniği başlat 
         
        }
    }
}