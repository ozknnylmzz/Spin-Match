using DG.Tweening;
using SpinMatch.Boards;
using SpinMatch.Enums;
using UnityEngine;

namespace SpinMatch.Items
{
    public abstract class GridItem : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        public abstract ItemType ItemType { get; }
        public ItemState ItemState { get; private set; } = ItemState.Rest;
        public int ConfigureType { get; private set; } = 0;
        public abstract void ConfigureItem(int configureType);

        public ColorType ColorType { get; private set; } = ColorType.None;

        public IGridSlot ItemSlot { get; private set; }

        public IGridSlot DestinationSlot { get; private set; }
        
        public int ItemStateDelay { get; private set; } = 0;

        private ItemGenerator _generator;
        
        public int PathDistance { get; private set; } = 0;
        public virtual void Initialize() {}
        
        private void SetScale(float scale)
        {
            transform.SetScale(scale);
        }

        public void SetSlot(IGridSlot slot)
        {
            ItemSlot = slot;
        }

        public void SetGenerator(ItemGenerator generator)
        {
            _generator = generator;
        }

        public void ReturnToPool()
        {
            _generator.ReturnItemToPool(this);
        }

        public virtual void ResetItem()
        {
            SetScale(1);
            SetState(ItemState.Rest);
            SetItemStateDelay(0);
        }
        
        public void SetDestinationSlot(IGridSlot destinationSlot)
        {
            DestinationSlot = destinationSlot;
        }
        
        public void SetState(ItemState state)
        {
            if (ItemState == ItemState.Hide) return;
            ItemState = state;
        }
        public void ResetPathDistance()
        {
            PathDistance = 0;
        }

        protected void SetConfigureType(int configureType)
        {
            ConfigureType = configureType;
        }

        protected void SetColorType(ColorType colorType)
        {
            ColorType = colorType;
        }

        public void SetPathDistance(int pathDistance)
        {
            PathDistance = pathDistance;
        }

        public void MoveToDestinationSlot()
        {
            _rigidbody2D.velocity =Vector2.zero;
            ReturnToPool();
            ResetItem();
            Kill();
            // if (DestinationSlot==null)
            // {
            //     
            //     ReturnToPool();
            //     ResetItem();
            //     Kill();
            //     return null;
            // }
            // else
            // {
            //     SetSlot(DestinationSlot);
            //   return  transform.DOMoveY(DestinationSlot.WorldPosition.y, 1);
            // }
        }

        public void MoveToSpinTarget(Vector2 targetPosition, float speed)
        {
            if (_rigidbody2D == null)
            {
                Debug.LogError("Rigidbody2D is not assigned!");
                return;
            }

            Vector2 direction = (targetPosition - _rigidbody2D.position).normalized;
            _rigidbody2D.velocity = direction * speed;
        }


        /// <summary>
        ///   <para>shouldPlayExplosion: explosion particle oynasin mi</para>
        ///   <para>isSpecialKill: 4lu tas mi 8li tas mi</para>
        /// </summary>
        public virtual void Kill(bool shouldPlayExplosion = true, bool isSpecialKill = false)
        {
            this.Hide();
        }
        
        private void SetItemStateDelay(int value)
        {
            ItemStateDelay = value;
        }

    
    }
}