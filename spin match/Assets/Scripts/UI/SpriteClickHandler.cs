using System;
using System.Collections;
using System.Collections.Generic;
using SpinMatch.Enums;
using UnityEngine;

namespace SpinMatch.UI
{
    public class SpriteClickHandler : MonoBehaviour
    {
        [SerializeField] private string spriteName;

        private void OnMouseDown()
        {
            HandleClick();
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                HandleClick();
            }
        }

        private void HandleClick()
        {
            if (spriteName == "Spin")
            {
                Debug.Log("Spin sprite clicked!");
                EventManager.Execute(BoardEvents.Spin);
                
            }
            else if (spriteName == "Stop")
            {
                Debug.Log("Stop sprite clicked!");
                // EventManager.Execute(BoardEvents.Stop);

            }
        }
    }
}
