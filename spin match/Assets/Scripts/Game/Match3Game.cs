using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SpinMatch.Boards;
using SpinMatch.Enums;
using SpinMatch.Items;
using SpinMatch.Jobs;
using SpinMatch.Matchs;
using SpinMatch.Strategy;
using UnityEngine;

namespace SpinMatch.Game
{
    public class Match3Game : MonoBehaviour
    {
        private IBoard _board;
        private IMatchDataProvider _matchDataProvider;
        public bool IsSwapAllowed => _isSwapAllowed;
        private bool _isSwapAllowed = true;
        private ItemSwapper _itemSwapper;
        private JobsExecutor _jobsExecutor;
        private MatchClearStrategy _matchClearStrategy;
        private List<Move> _moves;

        public void Initialize(StrategyConfig strategyConfig, GameConfig gameConfig, IBoard board)
        {
            _board = board;
            _itemSwapper = new ItemSwapper();
            _jobsExecutor = new JobsExecutor();
            _moves = new List<Move>();
            _matchClearStrategy = strategyConfig.MatchClearStrategy;
            _matchDataProvider = gameConfig.MatchDataProvider;
        }

        public void Subscribe()
        {
            EventManager.Subscribe(BoardEvents.OnBeforeJobsStart, DisableSwap);
        }

        public void UnSubscribe()
        {
            EventManager.Unsubscribe(BoardEvents.OnBeforeJobsStart, DisableSwap);
        }

        public async void SwapItemsAsync(GridPosition selectedPosition, GridPosition targetPosition)
        {
            IGridSlot selectedSlot = _board[selectedPosition];
            IGridSlot targetSlot = _board[targetPosition];
            await DoNormalSwap(selectedSlot, targetSlot);
        }

        private async UniTask DoNormalSwap(IGridSlot selectedSlot, IGridSlot targetSlot)
        {
            await SwapItemsAnimation(selectedSlot, targetSlot);

            if (IsMatchDetected(out BoardMatchData boardMatchData, selectedSlot.GridPosition, targetSlot.GridPosition))
            {
                _matchClearStrategy.CalculateMatchStrategyJobs(boardMatchData);

                CheckAutoMatch();
                StartJobs();
            }
            else
            {
                SwapItemsBack(selectedSlot, targetSlot);
            }
        }

        private async void StartJobs()
        {
            EventManager.Execute(BoardEvents.OnBeforeJobsStart);
            await _jobsExecutor.ExecuteJobsAsync();
            
            if (NoPossibleMoves())
            {
                
            }

            EventManager.Execute(BoardEvents.OnAfterJobsCompleted);
            EnableSwap();
        }

        private void CheckAutoMatch()
        {
            while (IsMatchDetected(out BoardMatchData boardMatchData, _board.AllGridPositions))
            {
                _matchClearStrategy.CalculateMatchStrategyJobs(boardMatchData);
            }
        }

        public bool IsMatchDetected(out BoardMatchData boardMatchData, params GridPosition[] gridPositions)
        {
            boardMatchData = _matchDataProvider.GetMatchData(_board, gridPositions);

            return boardMatchData.MatchExists;
        }

        private UniTask SwapItemsAnimation(IGridSlot selectedSlot, IGridSlot targetSlot)
        {
            return _itemSwapper.SwapItems(selectedSlot, targetSlot, this);
        }

        private async void SwapItemsBack(IGridSlot selectedSlot, IGridSlot targetSlot)
        {
            await SwapItemsAnimation(selectedSlot, targetSlot);
            EnableSwap();
        }

        public bool IsPointerOnBoard(Vector3 pointerWorldPos, out GridPosition selectedGridPosition)
        {
            return _board.IsPointerOnBoard(pointerWorldPos, out selectedGridPosition);
        }

        private bool NoPossibleMoves()
        {
            FindAllMoves();

            return _moves.Count == 0;
        }

        private void FindAllMoves()
        {
            _moves.Clear();

            for (int i = 0; i < _board.RowCount; i++)
            {
                for (int j = 0; j < _board.ColumnCount; j++)
                {
                    IGridSlot selectedSlot = _board[i, j];


                    foreach (GridPosition direction in GridPosition.SideDirections)
                    {
                        GridPosition targetPosition = selectedSlot.GridPosition + direction;

                        if (!_board.IsPositionOnBoard(targetPosition))
                        {
                            continue;
                        }

                        IGridSlot targetSlot = _board[targetPosition];


                        if (IsSameMove(selectedSlot, targetSlot))
                        {
                            continue;
                        }

                        if (IsMatchDetected(out BoardMatchData boardMatchData, selectedSlot.GridPosition,
                                targetSlot.GridPosition))
                        {
                            _moves.Add(new Move(selectedSlot, targetSlot, boardMatchData));
                        }
                    }
                }
            }
        }

        private bool IsSameMove(IGridSlot selectedSlot, IGridSlot targetSlot)
        {
            return _moves.Any(move => move.SelectedSlot == targetSlot && move.TargetSlot == selectedSlot);
        }


        #region Swap

        private void EnableSwap()
        {
            _isSwapAllowed = true;
        }

        public void DisableSwap()
        {
            _isSwapAllowed = false;
        }

        #endregion
    }
}