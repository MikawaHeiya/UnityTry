using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelect : MonoBehaviour
{
    private ControllableBag controllableBag;

    private void Start()
    {
        controllableBag = FindObjectOfType<ControllableBag>();
    }

    private void OnMouseDown()
    {
        controllableBag.SetCurrentItem(gameObject);

#if UNITY_EDITOR
        Debug.Log($"Item Select");
#endif
    }
}
