using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedBasic : MonoBehaviour
{
    public string dropName = "Empty";
    public float rotateSpeed = 30f;
    public float playerAttachRadious = 5f;
    public int dropPercentage = 1; //percentage == 1 / dropPercentage
    public DroppedEffectBase droppedEffect;

    private Rigidbody rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);

        PlayerAttach();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            droppedEffect.Effect(collision.gameObject);

#if UNITY_EDITOR
            Debug.Log($"GetDrop: {dropName}");
#endif
            Instantiate(Resources.Load($"Prefabs/Particles/Explosion_{Random.Range(0, 10)}"), transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    
    private void PlayerAttach()
    {
        var playerPosition = FindObjectOfType<ControllableBehave>().transform.position;
        if (Vector3.Distance(playerPosition, transform.position) <= playerAttachRadious)
        {
            rigidBody.AddForce((playerPosition - transform.position).normalized * 0.1f, ForceMode.Impulse);
        }
    }
}
