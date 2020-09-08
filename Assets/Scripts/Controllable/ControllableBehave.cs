using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class ControllableBehave : MonoBehaviour
{
    //Properties
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
                case MoveStatus.RunForward: return _moveSpeed * 2;
                case MoveStatus.Jump: return 0f;
                default: return 0f;
            }
        }
    }
    public float _moveSpeed = 7f;

    public float jumpForce
    {
        set
        {
            _jumpForce = value;
        }

        get
        {
            switch (moveStatus)
            {
                case MoveStatus.Stay: return _jumpForce;
                case MoveStatus.WalkForward: return _jumpForce;
                case MoveStatus.RunForward: return _jumpForce * 2;
                case MoveStatus.Jump: return 0f;
                default: return 0f;
            }
        }
    }
    public float _jumpForce = 10f;

    public float health
    {
        set
        {
            if (value >= maxHealth)
            {
                _health = maxHealth;
            }
            else if (value < 0f)
            {
                _health = 0f;
            }
            else
            {
                _health = value;
            }

            healthSlider.fillAmount = _health / maxHealth;
        }

        get
        {
            return _health;
        }
    }
    private float _health;

    public float mana
    {
        set
        {
            if (value >= maxMana)
            {
                _mana = maxMana;
            }
            else if (value < 0f)
            {
                _mana = 0f;
            }
            else
            {
                _mana = value;
            }

            manaSlider.fillAmount = _mana / maxMana;
        }

        get
        {
            return _mana;
        }
    }
    private float _mana;

    public float rotateSpeed = 100f;
    public float maxHealth = 100f;
    public float maxMana = 100f;
    public float manaRecoverSpeed = 5f;
    public MoveStatus moveStatus = MoveStatus.Stay;
    public float heightLimit = -50f;
    public List<GameObject> spiritList;
    
    private bool isGround = true;

    //Components
    private Camera controllableView;
    private Animator controllableAnimator;
    private Rigidbody controllableRigidbody;
    private GameController gameController;
    private AudioSource controllableAudioSource;
    private ControllableAudio controllableAudio;
    private ControllableBag controllableBag;

    public Image healthSlider;
    public Image manaSlider;
    public GameObject gameRunningGUI;
    public GameObject playerDiedGUI;

    //Unity Functions
    private void Start()
    {
        controllableView = GetComponentInChildren<Camera>();
        controllableAnimator = GetComponent<Animator>();
        controllableRigidbody = GetComponent<Rigidbody>();
        gameController = FindObjectOfType<GameController>();
        controllableAudioSource = GetComponent<AudioSource>();
        controllableAudio = GetComponent<ControllableAudio>();
        controllableBag = GetComponent<ControllableBag>();

        _health = maxHealth;
        _mana = maxMana;
        spiritList = new List<GameObject>();
    }

    private void Update()
    {
        if (gameController.gameStatus == GameStatus.GameRunning)
        {
            HandleControllableMove();
            HandleControllableAnimate();
            HandleAttack();
            HeightCheck();
            HandlePause();

            mana += manaRecoverSpeed * Time.deltaTime;
        }
    }

    //Functions
    private void HandleControllableMove()
    {
        HandleKeyboardMove();
        HandleMouseRotate();
    }

    private void HandleKeyboardMove()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            controllableRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGround = false;
        }

        var input = new Vector3
            (
            Input.GetAxis(ConstantStrings.AxisNameHorizontal),
            0f,
            Input.GetAxis(ConstantStrings.AxisNameVertical)
            );

        SetMoveStatus(input);

        input.x *= moveSpeed * Time.deltaTime;
        input.z *= moveSpeed * Time.deltaTime;
        transform.Translate(input);
    }

    private void HandleMouseRotate()
    {
        transform.Rotate(Vector3.up, Input.GetAxis(ConstantStrings.AxisNameMouseX) * rotateSpeed * Time.deltaTime);

        float cameraRotateAngle = controllableView.transform.localRotation.eulerAngles.x - Input.GetAxis(ConstantStrings.AxisNameMouseY) * rotateSpeed * Time.deltaTime;
        if (cameraRotateAngle > 40f && cameraRotateAngle < 100f)
            cameraRotateAngle = 40f;
        else if (cameraRotateAngle > 200f && cameraRotateAngle < 270f)
            cameraRotateAngle = 270f;
        controllableView.transform.localRotation = Quaternion.Euler(cameraRotateAngle, 0f, 0f);
    }

    private void SetMoveStatus(Vector3 input)
    {
        if (input != Vector3.zero)
        {
            if (Input.GetKey(KeyCode.R))
            {
                moveStatus = MoveStatus.RunForward;
            }
            else
            {
                moveStatus = MoveStatus.WalkForward;
            }
        }
        else if (!isGround)
        {
            moveStatus = MoveStatus.Jump;
        }
        else
        {
            moveStatus = MoveStatus.Stay;
        }
    }

    private void HandleControllableAnimate()
    {
        if (moveStatus == MoveStatus.Die)
        {
            controllableAnimator.SetInteger("MoveStatus", (int)MoveStatus.Stay);
            controllableAnimator.SetTrigger("Die");
            return;
        }

        controllableAnimator.SetInteger("MoveStatus", (int)moveStatus);
    }

    private void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var currentItem = controllableBag.CurrentItem();
            if (currentItem)
            {
                var itemBasic = currentItem.GetComponent<ItemBasic>();
                if (mana >= itemBasic.manaCost)
                {
                    if (itemBasic.itemUse.Use())
                    {
                        mana -= itemBasic.manaCost;
                    }
                }
                else
                {
#if UNITY_EDITOR
                    Debug.Log("ControllableBehave.HandleAttack(): Mana Not Enough");
#endif
                }
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("currentItem is null");
#endif
            }

            controllableAudioSource.PlayOneShot(controllableAudio.attackAudios[UnityEngine.Random.Range(0, controllableAudio.attackAudios.Count)]);
        }
    }

    private void HandlePause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameController.GamePause(true, true);
        }
    }

    private void HandleDeath()
    {
        moveStatus = MoveStatus.Die;
        HandleControllableAnimate();
        gameController.GameStop();

        gameRunningGUI.SetActive(false);
        playerDiedGUI.SetActive(true);

        gameController.PlayBGM(gameController.bgmList.deathBGM);

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
    }

    private void HeightCheck()
    {
        if (transform.position.y < heightLimit && gameController.gameStatus == GameStatus.GameRunning)
        {
            HandleDeath();
        }
    }

    public void GetInjured(float damage)
    {
        if (health - damage > 0f)
        {
            health -= damage;
            controllableAudioSource.PlayOneShot(controllableAudio.injuredAudios[UnityEngine.Random.Range(0, controllableAudio.injuredAudios.Count)]);
        }
        else
        {
            health = 0f;
            HandleDeath();
        }

#if UNITY_EDITOR
        Debug.Log($"health:{health}\n");
#endif
    }

    public GameObject GetItem(int itemID)
    {
        
        controllableAudioSource.PlayOneShot(controllableAudio.getItemAudios[UnityEngine.Random.Range(0, controllableAudio.getItemAudios.Count)]);
        return controllableBag.AddItem(itemID);
    }

    public void ItemLevelUP(uint offset)
    {
        if (controllableBag.CurrentItem().GetComponent<ItemBasic>().level + offset <= 20u)
        {
            var itemBasic = controllableBag.CurrentItem()?.GetComponent<ItemBasic>();
            itemBasic.level += offset;

#if UNITY_EDITOR
            Debug.Log(itemBasic.name + $"level up: {itemBasic.level}");
#endif
        }
    }
}
