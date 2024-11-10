using System.Collections;
using System.Collections.Generic;
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

        private void HandleClick()
        {
            if (spriteName == "Spin")
            {
                Debug.Log("Spin sprite clicked!");
                
            }
            else if (spriteName == "Stop")
            {
                Debug.Log("Stop sprite clicked!");
                
            }
        }
    }
}
