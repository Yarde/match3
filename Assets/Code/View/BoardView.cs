using Code.Model;
using Code.Model.Chips;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code.View
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private Image boardFrame;
        [SerializeField] private Image boardBackground;

        [SerializeField] private RectTransform boardTransform;
        [SerializeField] private Transform chipsParent;

        public ChipView[,] Prefabs => _prefabs;
        private ChipView[,] _prefabs;
        private BoardCell[,] _board;
        private BoardSettings _settings;
        private BoardVisuals _visuals;

        private void OnDestroy()
        {
            if (_prefabs == null)
            {
                return;
            }

            foreach (var chip in _prefabs)
            {
                if (chip)
                {
                    chip.OnMoved -= OnChipMoved;
                }
            }
        }

        public async UniTask Setup(BoardSettings settings, BoardCell[,] boardCells)
        {
            boardTransform.localPosition = new Vector3(0, Screen.height);

            _settings = settings;
            _visuals = settings.boardVisuals;
            _board = boardCells;

            background.sprite = _visuals.background;
            boardFrame.sprite = _visuals.boardFrame;
            boardBackground.sprite = _visuals.boardBackground;

            var sizeX = settings.boardSize.x;
            var sizeY = settings.boardSize.y;

            var width = sizeX * _visuals.cellSize + _visuals.boardOffset;
            var height = sizeY * _visuals.cellSize + _visuals.boardOffset;

            boardBackground.pixelsPerUnitMultiplier = 0.05f * sizeX;
            boardTransform.sizeDelta = new Vector2(width, height);

            CreateChips(settings, sizeX, sizeY);

            await boardTransform.DOLocalMove(Vector3.zero, 1f);
        }

        private void CreateChips(BoardSettings settings, int sizeX, int sizeY)
        {
            _prefabs = new ChipView[sizeX, sizeY];
            for (var i = 0; i < settings.boardSize.x; i++)
            {
                for (var j = 0; j < settings.boardSize.y; j++)
                {
                    _prefabs[i, j] = CreateNewChip(i, j, _board[i, j].chip);
                }
            }
        }

        public ChipView CreateNewChip(int i, int j, BoardElement chipData)
        {
            var chip = Instantiate(chipData.prefab, chipsParent);
            chip.Setup(chipData, _visuals.cellSize, _settings.boardSize, i, j);
            chip.OnMoved += OnChipMoved;
            return chip;
        }

        private void OnChipMoved(ChipView chip, Vector2Int from, Vector2Int to)
        {
            if (from.y >= _prefabs.GetLength(1))
            {
                _prefabs[to.x, to.y] = chip;
            }
            else
            {
                _prefabs[to.x, to.y] = _prefabs[from.x, from.y];
                _prefabs[from.x, from.y] = null;
            }
        }
    }
}