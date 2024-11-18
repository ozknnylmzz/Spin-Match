using System;
using DG.Tweening;
using SpinMatch.Game;
using SpinMatch.Inputs;
using SpinMatch.Items;
using SpinMatch.Level;
using SpinMatch.Spin;
using SpinMatch.Strategy;
using UnityEngine;
using UnityEngine.Serialization;

namespace SpinMatch.Boards
{
    public class BoardInitializer : MonoBehaviour
    {
        [SerializeField] private Board _board;
        [FormerlySerializedAs("boardMapGenerator")] [SerializeField] private BoardMapGenerator _boardMapGenerator;
        [SerializeField] private ItemGenerator _itemGenerator;
        [SerializeField] private Match3Game _match3Game;
        [SerializeField] private SpinController _spinController;
        [SerializeField] private BoardInputController _inputController;
        private StrategyConfig _strategyConfig;
        private LevelLoader _levelLoader;
        private GameConfig _gameConfig;

        public void Awake()
        {
            ConstructObjects();
            InitializeGame();
            DOTween.Init().SetCapacity(500, 500);
        }

        private void OnEnable()
        {
            _match3Game.Subscribe();
            _spinController.Subscribe();
        }

        private void OnDisable()
        {
            _match3Game.UnSubscribe();
            _spinController.UnSubscribe();
        }

        private void InitializeGame()
        {
            _gameConfig.Initialize();
            _board.Initialize();
            _strategyConfig.Initialize(_board, _itemGenerator);
            _boardMapGenerator.Initialize(_board, _itemGenerator, _gameConfig);
            _match3Game.Initialize(_strategyConfig, _gameConfig, _board);
            _levelLoader.Initialize(_boardMapGenerator);
            _spinController.Initialize(_board,_boardMapGenerator);
            _inputController.Initialize(_match3Game);
        }

        private void ConstructObjects()
        {
            _gameConfig = new GameConfig();
            _levelLoader = new LevelLoader();
            _strategyConfig = new StrategyConfig();
        }
        
    }
}