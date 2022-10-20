using System;
using Code.Model.Chips;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.View
{
    public class ChipView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private ParticleSystem onEffectParticles;

        private BoardElement _data;
        private bool _isDragging;
        private bool _isClicking;
        private Vector2 _clickPosition;

        public void Setup(BoardElement data, int i, int j)
        {
            _data = data;
            image.sprite = data.sprite;
            text.text = $"{i},{j}";
        }

        public async UniTask Destroy()
        {
            if (onEffectParticles)
            {
                onEffectParticles.Play();
                await UniTask.Delay((int)(onEffectParticles.main.duration * 1000));
            }
            
            Destroy(gameObject);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log($"OnPointerDown, swappable {_data.isSwappable}, clickable {_data.isClickable}, pos: {text.text}");
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
            if (_isClicking && Vector2.Distance(_clickPosition, eventData.position) < 0.1f)
            {
                Debug.Log($"OnPointerMove, from:{_clickPosition}, to:{eventData.position}, isSelected:{_isDragging}, distance:{Vector2.Distance(_clickPosition, eventData.position)}");

                // on click
            }

            _isDragging = false;
            _isClicking = false;
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (_isDragging && Vector2.Distance(_clickPosition, eventData.position) > 0.1f)
            {
                Debug.Log($"Swap: {text.text}, distance: {Vector2.Distance(_clickPosition, eventData.position)}");
                
                _isDragging = false;
                
                // on move
            }
        }
    }
}