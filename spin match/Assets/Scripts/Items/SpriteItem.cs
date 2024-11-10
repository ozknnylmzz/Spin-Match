using UnityEngine;

namespace SpinMatch.Items
{
    public abstract class SpriteItem : GridItem
    {
        [SerializeField] protected SpriteRenderer _spriteRenderer;
        private int _initialSortingOrder;

        protected void SetSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }

        public override void Initialize()
        {
            base.Initialize();
            _initialSortingOrder = _spriteRenderer.sortingOrder;
        }

        public override void ResetItem()
        {
            base.ResetItem();
            ResetLayer();
        }

        private void ResetLayer()
        {
            _spriteRenderer.sortingOrder = _initialSortingOrder;
        }
    }
}