using System.Collections.Generic;
using SpinMatch.Boards;
using SpinMatch.Enums;

namespace SpinMatch.Matchs
{
    public class MatchSequence
    {
        public IReadOnlyList<IGridSlot> MatchedGridSlots { get; }
        public MatchDetectorType MatchDetectorType { get; }

        public MatchSequence(IReadOnlyList<IGridSlot> matchedGridSlots,MatchDetectorType matchDetectorType)
        {
            MatchedGridSlots = matchedGridSlots;
            MatchDetectorType = matchDetectorType;
        }
    } 
}