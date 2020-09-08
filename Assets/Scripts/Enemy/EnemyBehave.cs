using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehave : MonoBehaviour
{
    public float moveSpeed
    {
        set
        {
            _moveSpeed = value;
        }

        get
        {
            switch (moveStatus)
            {
                case MoveStatus.Stay: return 0f;
                case MoveStatus.WalkForward: return _moveSpeed;
                case MoveStatus.RunForward: return 0f;
                default: return 0f;
            }
        }
    }
    private float _moveSpeed = 3f;

    public float damage
    {
        set
        {
            _damage = value;
        }

        get
        {
            switch (gameController.gameDifficulty)
            {
                case GameDifficulty.Easy:
                    return _damage;
                case GameDifficulty.Normal:
                    return _damage * 2f;
                case GameDifficulty.Hard:
                    return _damage * 3f;
                case GameDifficulty.Lunatic:
                    return _damage * 4f;
                default:
                    return 0f;
            }
        }
    }
    private float _damage = 5f;

    public float enemyView
    {
        set
        {
            _enemyView = value;
        }

        get
        {
            switch (gameController.gameDifficulty)
            {
                case GameDifficulty.Easy:
                    return _enemyView;
                case GameDifficulty.Normal:
                    return _enemyView * 1.5f;
                case GameDifficulty.Hard:
                    return _enemyView * 2f;
                case GameDifficulty.Lunatic:
                    return _enemyView * 3f;
                default:
                    return 0f;
            }
        }
    }
    private float _enemyView = 30f;

    public MoveStatus moveStatus = MoveStatus.Stay;
    public bool isDie = false;
    public List<GameObject> dropList;

    private Transform targetTransform;
    private Animator enemyAnimator;
    private GameController gameController;
    private ControllableBehave controllableBehave;
    private uint scoreOffset = 1u;
    private Vector3 lastTargetPosition;

    private void Start()
    {
        targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
        gameController = FindObjectOfType<GameController>();
        controllableBehave = FindObjectOfType<ControllableBehave>();
        enemyAnimator = GetComponent<Animator>();

        lastTargetPosition = transform.position;

        HandleAnimate();
    }

    private void Update()
    {
        if (gameController.gameStatus == GameStatus.GameRunning && !isDie)
        {
            HandleEnemyMove();
            HandleAnimate();
        }
    }

    private void HandleAnimate()
    {
        if (isDie)
            enemyAnimator.SetTrigger("Die");

        enemyAnimator.SetInteger("MoveStatus", (int)moveStatus);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet") && !isDie)
        {
            GetInjured();
        }
        else if (other.CompareTag("Player") && !isDie)
        {
            var controllableBehave = other.gameObject.GetComponent<ControllableBehave>();
            controllableBehave.GetInjured(damage);
        }
    }

    private IEnumerator DestorySelf()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private void HandleEnemyMove()
    {
        if (targetTransform)
        {
            float viewAngleOffset = Mathf.Abs(Vector3.Angle(transform.rotation * Vector3.forward, targetTransform.position - transform.position));
            if (viewAngleOffset <= enemyView || Vector3.Distance(transform.position, targetTransform.position) <= enemyView * 0.25f)
            {
                lastTargetPosition = targetTransform.position;
            }

            SetMoveStatus();
            if (moveStatus != MoveStatus.Stay)
            {
                RushTo(lastTargetPosition);
            }
        }
    }

    private void RushTo(Vector3 destination)
    {
            transform.LookAt(destination);
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    private void SetMoveStatus()
    {
        if (Vector3.Distance(transform.position, lastTargetPosition) < 0.6f)
        {
            moveStatus = MoveStatus.Stay;
        }
        else
        {
            moveStatus = MoveStatus.WalkForward;
        }
    }

    public void GetInjured()
    {
        if (!isDie)
        {
            moveStatus = 0;
            isDie = true;
            HandleAnimate();
            gameController.AddScore(scoreOffset);

            if (UnityEngine.Random.Range(0, 1) == 0)
            {
                DropItem();
            }

            StartCoroutine(DestorySelf());
        }
    }

    private void DropItem()
    {
        var drop = dropList[UnityEngine.Random.Range(0, dropList.Count)];
        if (UnityEngine.Random.Range(0, drop.GetComponent<DroppedBasic>().dropPercentage) == 0)
        {
            var item = Instantiate(dropList[UnityEngine.Random.Range(0, dropList.Count)], transform.position, Quaternion.identity);
            item.GetComponent<Rigidbody>().AddForce(Vector3.up * 0.1f, ForceMode.Impulse);
        }
    }
}
