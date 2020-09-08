using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUse_XLine : ItemUseBasic
{
    public GameObject bullet;

    private ItemBasic itemBasic;
    private ControllableBehave userBehave;
    private Transform userTransform;
    private uint instantiateNumber = 7;
    private float attackRadious = 8f;

    private void Start()
    {
        itemBasic = GetComponent<ItemBasic>();
        userBehave = FindObjectOfType<ControllableBehave>();
        userTransform = userBehave.gameObject.transform;
    }

    private void Initial()
    {
        if (!userBehave || !userTransform)
        {
            userBehave = FindObjectOfType<ControllableBehave>();
            userTransform = userBehave.gameObject.transform;
        }
    }

    public override bool Use()
    {
        Initial();

        uint number = instantiateNumber;
        bullet.GetComponent<XLine>().existTime = itemBasic.level + 3f; 
        while (number-- > 0)
        {
            var offset = Quaternion.AngleAxis(userTransform.rotation.eulerAngles.y, Vector3.up) * Vector3.forward +
                Quaternion.AngleAxis(Random.Range(180f, 360f), Quaternion.AngleAxis(userTransform.rotation.eulerAngles.y, Vector3.up) * Vector3.forward) *
                Quaternion.AngleAxis(userTransform.rotation.eulerAngles.y, Vector3.up) * Vector3.left * Random.Range(0f, attackRadious);
            var bulletPosition = new Vector3(userTransform.position.x, userTransform.position.y + 0.8f, userTransform.position.z) + offset;
            Instantiate(bullet, bulletPosition, userBehave.gameObject.GetComponentInChildren<Camera>().gameObject.transform.rotation);
        }

        return true;
    }
}
