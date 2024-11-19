using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpinMatch.Inputs
{
    public interface IBlockInput 
    {
        public bool IsBlockInput { get; }

        public void SetBlockInput(bool isBlock);
    }
}
