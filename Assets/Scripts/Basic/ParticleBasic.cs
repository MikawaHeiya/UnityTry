using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBasic : MonoBehaviour
{
    public float stayTime_secconds = 2f;

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(DestorySelf), stayTime_secconds);
    }

    void DestorySelf() => Destroy(gameObject);
}
