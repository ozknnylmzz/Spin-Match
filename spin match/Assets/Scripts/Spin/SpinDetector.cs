using System;
using SpinMatch.Data;
using SpinMatch.Enums;
using SpinMatch.Items;
using UnityEngine;

namespace SpinMatch.Spin
{
    public class SpinDetector : MonoBehaviour
    {
        [SerializeField] private Transform _startPoint;
        private bool isStop;

        private void OnEnable()
        {
            EventManager.Subscribe(BoardEvents.Stop, OnStop);
            EventManager.Subscribe(BoardEvents.Spin, OnSpin);
        }

        private void OnSpin()
        {
            isStop = false;
        }

        private void OnStop()
        {
            isStop = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Constants.ITEM_TAG_NAME)) 
            {
                NormalItem item=other.GetComponent<NormalItem>();
                item.SetWorldPosition(new Vector2(item.transform.position.x,_startPoint.position.y));

                if (isStop)
                {
                    item.MoveToDestinationSlot();
                }
                else
                {
                    item.MoveToSpinTarget(new Vector2(item.transform.position.x,transform.position.y),Constants.ITEM_SPIN_SPEED);
                }
            }
        }
    }
}
