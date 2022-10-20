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

        public ChipView[,] Prefabs { get; private set; }

        public async UniTask Setup(BoardSettings settings, BoardCell[,] boardCells)
        {
            boardTransform.localPosition = new Vector3(0, Screen.height);

            var visuals = settings.boardVisuals;

            background.sprite = visuals.background;
            boardFrame.sprite = visuals.boardFrame;
            boardBackground.sprite = visuals.boardBackground;

            var width = settings.boardSize.x * visuals.cellSize + visuals.boardOffset;
            var height = settings.boardSize.y * visuals.cellSize + visuals.boardOffset;

            boardBackground.pixelsPerUnitMultiplier = 0.05f * settings.boardSize.x;
            boardTransform.sizeDelta = new Vector2(width, height);

            CreateChips(boardCells, settings, settings.boardSize.x, settings.boardSize.y);

            await boardTransform.DOLocalMove(Vector3.zero, 1f);
        }

        private void CreateChips(BoardCell[,] board, BoardSettings settings, int sizeX, int sizeY)
        {
            Prefabs = new ChipView[sizeX, sizeY];
            for (var i = 0; i < settings.boardSize.x; i++)
            {
                for (var j = 0; j < settings.boardSize.y; j++)
                {
                    Prefabs[i, j] = CreateNewChip(i, j, board[i, j].chip, settings);
                }
            }
        }

        public ChipView CreateNewChip(int i, int j, BoardElement chipData, BoardSettings settings)
        {
            var chip = Instantiate(chipData.prefab, chipsParent);
            chip.Setup(chipData, settings.boardVisuals.cellSize, settings.boardSize, i, j);
            chip.OnMoved += OnChipMoved;
            return chip;
        }

        private void OnChipMoved(ChipView chip, Vector2Int from, Vector2Int to)
        {
            if (from.y >= Prefabs.GetLength(1))
            {
                Prefabs[to.x, to.y] = chip;
            }
            else
            {
                Prefabs[to.x, to.y] = Prefabs[from.x, from.y];
                Prefabs[from.x, from.y] = null;
            }
        }
    }
}