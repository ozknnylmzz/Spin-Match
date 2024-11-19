using Cysharp.Threading.Tasks;
using DG.Tweening;
using SpinMatch.Boards;
using SpinMatch.Data;
using SpinMatch.Enums;
using SpinMatch.Inputs;
using SpinMatch.Items;
using SpinMatch.Level;
using UnityEngine;
using UnityEngine.Serialization;

namespace SpinMatch.Spin
{
    public class SpinController : MonoBehaviour
    {
        [FormerlySerializedAs("_endValue")] [SerializeField] private Transform _endSpinItemPointer;
        [SerializeField] private SpriteRenderer spinSprite;
        [SerializeField] private SpriteRenderer stopSprite;
        private bool _isSpinActive = true; 
        private bool _isStopActive = false; 
        private IBoard _board;
        private BoardMapGenerator _boardMapGenerator;
        private Camera _mainCamera;
        private IBlockInput _blockInput;
        public void Initialize(IBoard board, BoardMapGenerator mapGenerator,IBlockInput blockInput)
        {
            _board = board;
            _boardMapGenerator = mapGenerator;
            _mainCamera = Camera.main;
            _blockInput = blockInput;
            _blockInput.SetBlockInput(true);
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

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleClick();
            }
        }
        private void HandleClick()
        {
            Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject == spinSprite.gameObject && _isSpinActive)
                {
                    HandleSpin();
                }
                else if (hit.collider.gameObject == stopSprite.gameObject && _isStopActive)
                {
                    HandleStop();
                }
            }
        }
        
        private void HandleSpin()
        {
            EventManager<bool>.Execute(BoardEvents.OnActiveStopButton, true);
            EventManager<bool>.Execute(BoardEvents.OnActiveSpinButton, false);
            EventManager.Execute(BoardEvents.Spin);

            _isSpinActive = false;
            
        }
        
        private void HandleStop()
        {
            EventManager<bool>.Execute(BoardEvents.OnActiveStopButton, false);
            EventManager<bool>.Execute(BoardEvents.OnActiveSpinButton, true);
            EventManager.Execute(BoardEvents.Stop);

            
            _isStopActive = false;
        }

        private async void OnClickStop()
        {
            _board.ClearAllSlot();
            _isSpinActive = false;
            await _boardMapGenerator.FillBoardItems();
           
            var moveSequence = _board.MoveToSlotItem();
            moveSequence.OnComplete(() =>
            {
                _boardMapGenerator.FillTopWithSpinItems();
                EventManager<bool>.Execute(BoardEvents.OnActiveSpinButton,true);
                _blockInput.SetBlockInput(false);
                _isSpinActive = true;
            });

            moveSequence.Play();
        }

        private void OnClickSpin()
        {
            _board.BoardItems.Clear();
            PlaySpinItems();
            _blockInput.SetBlockInput(true);
        }

        private Sequence _spinSequence;

        private  void PlaySpinItems()
        {
            foreach (GridItem spinItem in _board.AllSpinItems)
            {
                if (spinItem != null)
                {
                    spinItem.MoveToSpinTarget(new Vector2(spinItem.transform.position.x, _endSpinItemPointer.position.y),
                        Constants.ITEM_SPIN_SPEED);
                }
            }
            _isStopActive = true;
        }
    }
}