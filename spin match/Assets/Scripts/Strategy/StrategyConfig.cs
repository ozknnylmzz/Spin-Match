using SpinMatch.Boards;
using SpinMatch.Items;

namespace SpinMatch.Strategy
{
    public class StrategyConfig
    {
        private FallDownFillStrategy FallDownFillStrategy { get; set; }
        private BoardClearStrategy BoardClearStrategy { get; set; }
        public MatchClearStrategy MatchClearStrategy { get; private set; }

        public void Initialize(IBoard board,ItemGenerator itemGenerator)
        {
            FallDownFillStrategy = new FallDownFillStrategy(board, itemGenerator);
            BoardClearStrategy = new BoardClearStrategy(FallDownFillStrategy);
            MatchClearStrategy = new MatchClearStrategy(BoardClearStrategy);
        }
    }
}