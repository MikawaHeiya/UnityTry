using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BulletBehave : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float existTime_secconds = 10f;

    public ParticleSystem movingParticle;
    public ParticleSystem disappearParticle;
    public ParticleSystem bombParticle;

    private GameController gameController;

    private bool isMoving = true;

    private void Awake()
    {
        Invoke(nameof(Disappear), existTime_secconds);
    }

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    void Update()
    {
        if (gameController.gameStatus == GameStatus.GameRunning)
        {
            HandleBulletMove();
        }
    }

    private void HandleBulletMove()
    {
        if (isMoving)
        {
            transform.Translate(new Vector3(0f, 0f, 1f) * moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        isMoving = false;
        movingParticle.Stop();

        Instantiate(bombParticle, transform.position, bombParticle.transform.rotation);

        Destroy(gameObject);
    }

    private void Disappear()
    {
        isMoving = false;
        movingParticle.Stop();

        Instantiate(disappearParticle, transform.position, disappearParticle.transform.rotation);

        Destroy(gameObject);
    }
}
