using DG.Tweening;
using SpinMatch.Game;
using SpinMatch.Items;
using SpinMatch.Level;
using SpinMatch.Spin;
using UnityEngine;
using UnityEngine.Serialization;

namespace SpinMatch.Boards
{
    public class BoardInitializer : MonoBehaviour
    {
        [SerializeField] private Board _board;
        [SerializeField] private BoardMapGenerator boardMapGenerator;
        [SerializeField] private ItemGenerator _itemGenerator;
        [SerializeField] private SpinController _spinController;
        
        private LevelLoader _levelLoader;
        private GameConfig _gameConfig;

        public void Awake()
        {
            ConstructObjects();
            InitializeGame();
            DOTween.Init().SetCapacity(500, 500);
        }

        private void InitializeGame()
        {
            _gameConfig.Initialize();
            _board.Initialize();
            boardMapGenerator.Initialize(_board, _itemGenerator, _gameConfig);
            _levelLoader.Initialize(boardMapGenerator);
            _spinController.Initialize(_board);
        }

        private void ConstructObjects()
        {
            _gameConfig = new GameConfig();
            _levelLoader = new LevelLoader();
        }
        
    }
}