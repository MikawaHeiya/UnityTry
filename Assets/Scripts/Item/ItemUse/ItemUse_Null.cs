using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUse_Null : ItemUseBasic
{
    public override bool Use()
    {
#if UNITY_EDITOR
        Debug.Log("ItemUse_Null::Use() Invoke");
#endif
        return false;
    }
}
