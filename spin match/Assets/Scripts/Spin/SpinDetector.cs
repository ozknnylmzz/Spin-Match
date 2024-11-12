using SpinMatch.Data;
using SpinMatch.Items;
using UnityEngine;

namespace SpinMatch.Spin
{
    public class SpinDetector : MonoBehaviour
    {
        [SerializeField] private Transform _startPoint;

        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Constants.ITEM_TAG_NAME)) 
            {
                NormalItem item=other.GetComponent<NormalItem>();
                item.SetWorldPosition(new Vector2(item.transform.position.x,_startPoint.position.y));
                item.MoveToTarget(new Vector2(item.transform.position.x,transform.position.y),Constants.ITEM_SPIN_SPEED);
            }
        }
    }
}
