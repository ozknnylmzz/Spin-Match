using System.Collections;
using System.Collections.Generic;
using SpinMatch.Boards;
using SpinMatch.Enums;
using SpinMatch.Game;
using UnityEngine;

namespace SpinMatch.Inputs
{
    public class BoardInputController : MonoBehaviour,IBlockInput
    {
        [SerializeField] private Camera mainCamera;
        private Match3Game _match3Game;
        private GridPosition _selectedGridPosition;
        private bool _isDragMode;
        public bool IsBlockInput { get; private set; }
    

        public void Initialize(Match3Game match3Game)
        {
            _match3Game = match3Game;
        }

        private void Update()
        {
            if (IsBlockInput)
            {
                return;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                if (!_match3Game.IsSwapAllowed)
                {
                    return;
                }
                _isDragMode = false;
                
                Vector2 pointerWorldPos = GetWorldPosition(Input.mousePosition);
                
                if (_match3Game.IsPointerOnBoard(pointerWorldPos, out _selectedGridPosition))
                {
                    _isDragMode = true;
                }
            }

           
            if (Input.GetMouseButton(0))
            {
                if (!_isDragMode)
                    return;
                Vector2 pointerWorldPos = GetWorldPosition(Input.mousePosition);
                if (!_match3Game.IsPointerOnBoard(pointerWorldPos, out GridPosition targetGridPosition))
                {
                    _isDragMode = false;
                    return;
                }

                if (!IsSideGrid(targetGridPosition))
                {
                    return;
                }

                _isDragMode = false;

                SwapAsync((_selectedGridPosition, targetGridPosition));
                
            }

            
            if (Input.GetMouseButtonUp(0))
            {
                _isDragMode = false;
                Vector2 pointerWorldPos = GetWorldPosition(Input.mousePosition);

                if (!_match3Game.IsSwapAllowed)
                {
                    return;
                }

                if (!_match3Game.IsPointerOnBoard(pointerWorldPos, out _selectedGridPosition))
                {
                    return;
                }

                _isDragMode = false;
            }
        }
        
        private Vector2 GetWorldPosition(Vector2 screenPosition)
        {
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPosition);
            return new Vector2(worldPos.x, worldPos.y); 
        }
        
        private bool IsSideGrid(GridPosition gridPosition)
        {
            bool isSideGrid = gridPosition.Equals(_selectedGridPosition + GridPosition.Up) ||
                              gridPosition.Equals(_selectedGridPosition + GridPosition.Down) ||
                              gridPosition.Equals(_selectedGridPosition + GridPosition.Left) ||
                              gridPosition.Equals(_selectedGridPosition + GridPosition.Right);

            return isSideGrid;
        }
        
        private void SwapAsync((GridPosition selectedGridPosition, GridPosition targetGridPosition) swapInput)
        {
            _match3Game.DisableSwap();
            _match3Game.SwapItemsAsync(swapInput.selectedGridPosition, swapInput.targetGridPosition);
        }
        
        public void SetBlockInput(bool isBlock)
        {
            IsBlockInput = isBlock;
        }

    }
}
