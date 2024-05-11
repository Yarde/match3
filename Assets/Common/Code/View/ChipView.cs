using System;
using System.Threading;
using Common.Code.Model.Chips;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Common.Code.View
{
    [RequireComponent(typeof(RectTransform))]
    public class ChipView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private ParticleSystem onEffectParticles;

        private RectTransform _rectTransform;

        private BoardElement _data;
        private float _chipSize;
        private Vector2Int _boardSize;
        private Vector2Int _chipPosition;

        private bool _isDragging;
        private bool _isClicking;
        private Vector2 _clickPosition;

        public event Action<ChipView, Vector2Int, Vector2Int> OnMoved;
        public event Action<Vector2Int, Vector2Int> OnSwapped;
        public event Action<Vector2Int> OnClicked;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void OnDestroy()
        {
            OnMoved = null;
            OnSwapped = null;
            OnClicked = null;
        }

        public void Setup(BoardElement data, float chipSize, Vector2Int boardSize, int i, int j)
        {
            _data = data;
            _chipSize = chipSize;
            _boardSize = boardSize;
            image.sprite = data.sprite;
            text.text = $"{i},{j}";
            name = $"Chip-{i},{j}";
            _chipPosition = new Vector2Int(i, j);

            var pos = new Vector3(-_boardSize.x * chipSize / 2 + i * chipSize,
                -_boardSize.y * chipSize / 2 + j * chipSize, 0);
            transform.localPosition = pos;
            _rectTransform.sizeDelta = new Vector2(chipSize, chipSize);

            data.OnEffect += OnEffect;
            data.OnMove += OnMove;
        }

        private void Refresh(int i, int j)
        {
            text.text = $"{i},{j}";
            name = $"Chip-{i},{j}";
            _chipPosition = new Vector2Int(i, j);
        }

        private async UniTask OnMove(Move move)
        {
            await Move(move, this.GetCancellationTokenOnDestroy());

            OnMoved?.Invoke(this, move.source, move.target);
            Refresh(move.target.x, move.target.y);
        }

        private UniTask OnEffect()
        {
            return new UniTask();
            //return image.DOFade(0f, 0.2f).OnComplete(() => Destroy(gameObject)).WithCancellation(this.GetCancellationTokenOnDestroy());
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_data.isSwappable)
            {
                _isDragging = true;
            }

            if (_data.isClickable)
            {
                _isClicking = true;
            }

            _clickPosition = eventData.position;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_isClicking && Vector2.Distance(_clickPosition, eventData.position) < 10f)
            {
                OnClicked?.Invoke(_chipPosition);
            }

            _isDragging = false;
            _isClicking = false;
        }

        public void Update()
        {
            if (_isDragging && Vector2.Distance(_clickPosition, Input.mousePosition) > 40f)
            {
                _isDragging = false;

                var horizontalMove = _clickPosition.x - Input.mousePosition.x;
                var verticalMove = _clickPosition.y - Input.mousePosition.y;

                var swapDirection = new Vector2Int();
                if (Mathf.Abs(horizontalMove) > Mathf.Abs(verticalMove))
                {
                    swapDirection.x = horizontalMove > 0 ? -1 : 1;
                }
                else
                {
                    swapDirection.y = verticalMove > 0 ? -1 : 1;
                }

                OnSwapped?.Invoke(_chipPosition, _chipPosition + swapDirection);
            }
        }

        private UniTask Move(Move move, CancellationToken token)
        {
            var pos = new Vector3(-_boardSize.x * _chipSize / 2 + move.target.x * _chipSize,
                -_boardSize.y * _chipSize / 2 + move.target.y * _chipSize, 0);
            var distance = Vector2.Distance(move.source, move.target);

            var duration = 0.2f * distance + move.delay;
            return transform.DOLocalMove(pos, duration).SetEase(Ease.OutBounce).WithCancellation(token);
        }
    }
}