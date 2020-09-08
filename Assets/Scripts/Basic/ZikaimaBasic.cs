using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ZikaimaBasic : MonoBehaviour
{
    private Transform targetTransform;
    private GameController gameController;
    private bool floatTarget = true;       //true:Up, false:Down

    public bool haveTarget
    {
        private set
        {
            _haveTarget = value;
        }

        get
        {
            return targetTransform != null;
        }
    }
    private bool _haveTarget;

    public float rotateSpeed = 20f;
    public float floatSpeed = 0.5f;
    public Transform zikaimaTransform;
    public float attackRadious = 50f;
    public float existTime_secconds = 20f;
    public float attackDeltaTime_secconds = 2f;
    public float floatLimitUp = 2.5f;
    public float floatLimitDown = 0.8f;
    public MasterType masterType = MasterType.Controllable;
    public GameObject bullet;
    public Transform userTransform { set; get; }

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();

        StartCoroutine(AttackTimer());
        Invoke(nameof(Disappear), existTime_secconds);
    }

    private void Update()
    {
        if (gameController.gameStatus == GameStatus.GameRunning)
        {
            Move();
        }
    }

    private void Move()
    {
        if (userTransform)
        {
            transform.position = userTransform.position;
        }
        else
        {
            Disappear();
        }

        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);

        float currentFloatTarget = floatTarget ? floatLimitUp : floatLimitDown;
        if (Mathf.Abs(currentFloatTarget - zikaimaTransform.localPosition.y) < 0.1f)
        {
            floatTarget = !floatTarget;
        }
        else
        {
            float forward = (zikaimaTransform.localPosition.y < currentFloatTarget) ? 1f : -1f;
            zikaimaTransform.Translate(new Vector3(forward, 0f, 0f) * floatSpeed * Time.deltaTime);
        }
    }

    private void SetTarget()
    {
        var TargetList = (
                          from target in FindObjectsOfType<EnemyBehave>()
                          select target.gameObject.transform
                         ).ToList();

        if (TargetList.Count > 0)
        {
            TargetList.Sort((Transform t1, Transform t2) =>
            {
                var t1Distance = Vector3.Distance(t1.position, zikaimaTransform.position);
                var t2Distance = Vector3.Distance(t2.position, zikaimaTransform.position);
                if (t1Distance == t2Distance)
                {
                    return 0;
                }
                else if (t1Distance > t2Distance)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            });

            targetTransform = (Vector3.Distance(userTransform.position, TargetList[0].position) < attackRadious) ? TargetList[0] : null;
        }
        else
        {
            targetTransform = null;
        }
    }

    private void Attack()
    {
        if (gameController.gameStatus == GameStatus.GameRunning)
        {
            SetTarget();

            if (targetTransform)
            {
                var bulletPosition = zikaimaTransform.position;
                var bulletRotation = Quaternion.LookRotation(targetTransform.position + Vector3.up * 0.8f - zikaimaTransform.position);

                Instantiate(bullet, bulletPosition, bulletRotation);
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("ZikaimaBall: did not find target");
#endif
            }
        }
    }

    private IEnumerator AttackTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackDeltaTime_secconds);
            Attack();
        }
    }

    private void Disappear()
    {
        Destroy(gameObject);

        if (masterType == MasterType.Controllable)
        {
            userTransform.gameObject.GetComponent<ControllableBehave>().spiritList.Remove(gameObject);
        }
    }
}

public enum MasterType { Controllable, Enemy};
