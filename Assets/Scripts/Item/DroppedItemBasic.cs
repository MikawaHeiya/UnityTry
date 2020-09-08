using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItemBasic : MonoBehaviour
{
    public int itemID = 0;
    public uint level = 1u;
    public float rotateSpeed = 10f;

    private void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ControllableBehave playerBehave = collision.gameObject.GetComponent<ControllableBehave>();
            var item =  playerBehave.GetItem(itemID);
            item.GetComponent<ItemBasic>().level = level;

#if UNITY_EDITOR
            Debug.Log($"GetItem: Item_{itemID}");
#endif
            Instantiate(Resources.Load($"Prefabs/Particles/Explosion_{Random.Range(0, 10)}"), transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
