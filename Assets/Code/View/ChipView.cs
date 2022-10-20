using System;
using Code.Model.Chips;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace Code.View
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

        public void Setup(BoardElement data, float chipSize, Vector2Int boardSize, int i, int j)
        {
            _data = data;
            _chipSize = chipSize;
            _boardSize = boardSize;
            image.sprite = data.sprite;
            text.text = $"{i},{j}";
            _chipPosition = new Vector2Int(i, j);

            var pos = new Vector3(-_boardSize.x * chipSize / 2 + i * chipSize,
                -_boardSize.y * chipSize / 2 + j * chipSize, 0);
            transform.localPosition = pos;
            _rectTransform.sizeDelta = new Vector2(chipSize, chipSize);

            data.OnEffect += OnEffect;
            data.OnMove += OnMove;
        }

        private async UniTask OnMove(Vector2Int from, Vector2Int to)
        {
            await Move(from, to);
            
            OnMoved?.Invoke(this, from, to);
            Setup(_data, _chipSize, _boardSize, to.x, to.y);
        }

        private async UniTask OnEffect()
        {
            _data.OnEffect -= OnEffect;
            _data.OnMove -= OnMove;

            if (onEffectParticles)
            {
                onEffectParticles.Play();
                await UniTask.Delay((int)(onEffectParticles.main.duration * 1000));
            }

            Destroy(gameObject);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log($"OnPointerDown, {eventData.position}, swappable {_data.isSwappable}, clickable {_data.isClickable}, pos: {text.text}");
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
                Debug.Log(
                    $"OnPointerMove, from:{_clickPosition}, to:{eventData.position}, isSelected:{_isDragging}, distance:{Vector2.Distance(_clickPosition, eventData.position)}");

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
                
                Debug.Log($"Swap: {text.text}, to: {_chipPosition + swapDirection}, distance: {Vector2.Distance(_clickPosition, Input.mousePosition)}");

                OnSwapped?.Invoke(_chipPosition, _chipPosition + swapDirection);
            }
        }

        private async UniTask Move(Vector2Int from, Vector2Int to)
        {
            var pos = new Vector3(-_boardSize.x * _chipSize / 2 + to.x * _chipSize,
                -_boardSize.y * _chipSize / 2 + to.y * _chipSize, 0);
            var distance = Vector2.Distance(from, to);

            await transform.DOLocalMove(pos, 0.2f * distance + 0.05f * from.y).SetEase(Ease.OutBounce);
        }
    }
}