using SpinMatch.Enums;
using UnityEngine;

namespace SpinMatch.Data
{
    [CreateAssetMenu(menuName = "Board/ConfigureData/ColoredItemConfigureData")]
    public class ColoredItemConfigureData : ConfigureData
    { 
        public override ContentData[] ContentDatas => ColoredItemDatas;

        public ColoredItemData[] ColoredItemDatas;
    }

    [System.Serializable]
    public class ColoredItemData : ContentData
    {
        public ColorType colorType;
        public Sprite Sprite;
    }
}