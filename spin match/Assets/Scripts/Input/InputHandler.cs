using System.Collections;
using System.Collections.Generic;
using SpinMatch.Enums;
using UnityEngine;

namespace SpinMatch.Inputs
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;

        private bool _isDragging;
        private Vector2 _startDragPos;

        private void Update()
        {
           
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 worldPos = GetWorldPosition(Input.mousePosition);
                _startDragPos = worldPos;
                EventManager<Vector2>.Execute(BoardEvents.OnPointerDown, worldPos);
            }

           
            if (Input.GetMouseButton(0))
            {
                _isDragging = true;
                Vector2 worldPos = GetWorldPosition(Input.mousePosition);
                EventManager<Vector2>.Execute(BoardEvents.OnPointerDrag, worldPos);
            }

            
            if (Input.GetMouseButtonUp(0))
            {
                Vector2 worldPos = GetWorldPosition(Input.mousePosition);

                if (_isDragging)
                {
                    _isDragging = false;
                    EventManager<Vector2>.Execute(BoardEvents.OnPointerUp, worldPos);
                }
            }
        }
        
        private Vector2 GetWorldPosition(Vector2 screenPosition)
        {
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPosition);
            return new Vector2(worldPos.x, worldPos.y); // Z eksenini yok sayıyoruz.
        }
    }
}
