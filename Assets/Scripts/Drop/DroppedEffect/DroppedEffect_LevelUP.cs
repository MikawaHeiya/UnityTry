using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedEffect_LevelUP : DroppedEffectBase
{
    public override void Effect(GameObject gameObject)
    {
        gameObject?.GetComponent<ControllableBehave>().ItemLevelUP(1u);
    }
}
