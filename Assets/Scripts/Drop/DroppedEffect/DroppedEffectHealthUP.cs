using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedEffectHealthUP : DroppedEffectBase
{
    public float healthOffset = 10f;

    public override void Effect(GameObject gameObject)
    {
        var controllableBehave = gameObject.GetComponent<ControllableBehave>();
        controllableBehave.health += healthOffset;
        controllableBehave.healthSlider.fillAmount = controllableBehave.health / controllableBehave.maxHealth;
    }
}
