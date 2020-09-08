using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUse_ZikaimaBall : ItemUseBasic
{
    public GameObject zikaima;
    private ControllableBehave userBehave;
    private ItemBasic itemBasic;

    private void Start()
    {
        userBehave = FindObjectOfType<ControllableBehave>();
        itemBasic = GetComponent<ItemBasic>();
    }

    public override bool Use()
    {
        if (userBehave.spiritList.Count > itemBasic.level)
        {
#if UNITY_EDITOR
            Debug.Log("ItemUse_Zikaima: zikaima upper limit reached");
#endif
            return false;
        }

        zikaima.GetComponent<ZikaimaBasic>().existTime_secconds = itemBasic.level * 2f + 20f;
        var zikaimaBall = Instantiate(zikaima, userBehave.gameObject.transform.position, zikaima.transform.rotation);
        var zikaimaBasic = zikaimaBall.GetComponent<ZikaimaBasic>();
        zikaimaBasic.userTransform = userBehave.transform;
        userBehave.spiritList.Add(zikaimaBall);

        return true;
    }
}
