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

        private ChipView[,] _prefabs;
        private BoardCell[,] _board;
        private BoardVisuals _visuals;

        public async UniTask Setup(BoardSettings settings, BoardCell[,] boardCells)
        {
            boardTransform.localPosition = new Vector3(0, Screen.height);
            
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
            var pos = new Vector3(-_board.GetLength(0) * _visuals.cellSize / 2 + i * _visuals.cellSize,
                -_board.GetLength(1) * _visuals.cellSize / 2 + j * _visuals.cellSize, 0);

            var chip = Instantiate(chipData.prefab, chipsParent);
            chip.transform.localPosition = pos;
            ((RectTransform)chip.transform).sizeDelta = new Vector2(_visuals.cellSize, _visuals.cellSize);
            chip.Setup(chipData, i, j);

            chipData.OnEffect += () => OnEffect(chip, chipData);
            chipData.OnMove += (from, to) => OnMove(chip, chipData, from, to);

            return chip;
        }

        private async UniTask OnMove(ChipView chip, BoardElement chipData, Vector2Int from, Vector2Int to)
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

            chip.Setup(chipData, to.x, to.y);

            var pos = new Vector3(-_board.GetLength(0) * _visuals.cellSize / 2 + to.x * _visuals.cellSize,
                -_board.GetLength(1) * _visuals.cellSize / 2 + to.y * _visuals.cellSize, 0);
            var distance = Vector2.Distance(from, to);
                Debug.Log($"{distance}, {from}, {to}");

            await chip.transform.DOLocalMove(pos, 0.5f * distance).SetEase(Ease.OutBounce);
        }

        private async UniTask OnEffect(ChipView chip, BoardElement chipData)
        {
            await chip.Destroy();

            chipData.OnEffect -= () => OnEffect(chip, chipData);
            chipData.OnMove -= (from, to) => OnMove(chip, chipData, from, to);
        }
    }
}