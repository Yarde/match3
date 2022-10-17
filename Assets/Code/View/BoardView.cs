using Code.Model;
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

        public void Setup(BoardSettings settings, BoardCell[,] boardCells)
        {
            var visuals = settings.boardVisuals;

            background.sprite = visuals.background;
            boardFrame.sprite = visuals.boardFrame;
            boardBackground.sprite = visuals.boardBackground;

            var sizeX = settings.boardSize.x;
            var sizeY = settings.boardSize.y;

            var width = sizeX * visuals.cellSize + visuals.boardOffset;
            var height = sizeY * visuals.cellSize + visuals.boardOffset;

            boardBackground.pixelsPerUnitMultiplier = 0.25f * sizeX;
            boardTransform.sizeDelta = new Vector2(width, height);

            CreateChips(settings, boardCells, sizeX, sizeY, visuals);
        }

        private void CreateChips(BoardSettings settings, BoardCell[,] boardCells, int sizeX, int sizeY, BoardVisuals visuals)
        {
            _prefabs = new ChipView[sizeX, sizeY];
            for (var i = 0; i < settings.boardSize.x; i++)
            {
                for (var j = 0; j < settings.boardSize.y; j++)
                {
                    var chip = Instantiate(boardCells[i, j].chip.prefab, chipsParent);
                    var pos = new Vector3(-sizeX * visuals.cellSize / 2 + i * visuals.cellSize,
                        -sizeY * visuals.cellSize / 2 + j * visuals.cellSize, 0);
                    chip.transform.localPosition = pos;
                    chip.Setup(boardCells[i, j].chip.sprite);
                    _prefabs[i, j] = chip;
                }
            }
        }
    }
}