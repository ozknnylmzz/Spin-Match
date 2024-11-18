using System.Collections;
using System.Collections.Generic;
using SpinMatch.Boards;
using UnityEngine;

namespace SpinMatch.Game
{
    public class Move 
    {
        public IGridSlot SelectedSlot { get; private set; }
        public IGridSlot TargetSlot { get; private set; }
        public BoardMatchData BoardMatchData { get; private set; }

        public Move(IGridSlot selectedSlot, IGridSlot targetSlot, BoardMatchData boardMatchData)
        {
            SelectedSlot = selectedSlot;
            TargetSlot = targetSlot;
            BoardMatchData = boardMatchData;
        }

        public Move(IGridSlot selectedSlot, IGridSlot targetSlot)
        {
            SelectedSlot = selectedSlot;
            TargetSlot = targetSlot;
        }

        public Move(IGridSlot selectedSlot)
        {
            SelectedSlot = selectedSlot;
        }
    }
}
