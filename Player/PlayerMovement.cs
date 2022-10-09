using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;  
    private Animator animator;
    private PlayerStats playerStats;
    private float agilityMultiplier;
    private CinemachineFreeLook cinemachine;

    [SerializeField] private KeyCode hideMouseBind;
    [SerializeField] private SFXManager sFXManager;
    [SerializeField] private Transform cameraTransform;

    [Header("Movement Property")]
    [SerializeField, Range(1, 20)] private float movementSpeed = 5f;
    [SerializeField, Range(0.1f, 0.5f)] private float maxRotateSpeed = 0.3f;
    [SerializeField, Range(0.1f, 20)] private float fallSpeed = 10f;
    [SerializeField, Range(0, 100)] private float attackMovementPenalty = 70;

    private float reference;
    private Vector3 direction;
    private Vector3 movement;

    [Header("Sliding Property")]
    [SerializeField] private KeyCode slideKey = KeyCode.Mouse1;
    [SerializeField, Range(1, 25)] private float slideSpeed = 8f;
    [SerializeField, Range(0, 1)] private float startSlide = 0.2f;
    [SerializeField, Range(0.1f, 3)] private float slideDuration = 2f;
    
    private float lastSlide;

    [Header("Rolling Property")]
    [SerializeField] private KeyCode rollKey = KeyCode.Mouse3;
    [SerializeField, Range(1, 30)] private float rollSpeed = 5f;
    [SerializeField, Range(0, 1)] private float startRoll = 0.2f;
    [SerializeField, Range(0.1f, 3)] private float rollDuration = 1.3f;

    private float lastRoll;

    [Header("Displayed for debug purpose")]
    [SerializeField] private bool slide = false;
    [SerializeField] private bool roll = false;
    [SerializeField] private bool attack = false;

    // Start is called before the first frame update
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
        cinemachine = GameObject.FindGameObjectWithTag("Cinemachine").GetComponent<CinemachineFreeLook>();
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetKeyDown(hideMouseBind))
        {
            Cursor.visible = !Cursor.visible;
            Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
            HandleAnimation();
        }
        if (Cursor.visible)
        {
            cinemachine.m_XAxis.m_InputAxisName = "";
            cinemachine.m_YAxis.m_InputAxisName = "";
            return;
        }
        else
        {
            cinemachine.m_XAxis.m_InputAxisName = "Mouse X";
            cinemachine.m_YAxis.m_InputAxisName = "Mouse Y";
        }
        attack = animator.GetCurrentAnimatorStateInfo(0).IsName("Punch") || animator.GetCurrentAnimatorStateInfo(0).IsName("Sword Hit") || animator.GetBool("AttackAgain");
        agilityMultiplier = playerStats.Agility / 10f;

        if (Time.time - lastSlide >= slideDuration) slide = false;
        if (Time.time - lastRoll >= rollDuration) roll = false;

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        direction = new Vector3(x, 0, z).normalized;

        if (Input.GetKeyDown(slideKey) && !slide && !roll && !attack)
        {
            lastSlide = Time.time;
            slide = true;
        }

        if (Input.GetKeyDown(rollKey) && !slide && !roll && !attack)
        {
            lastRoll = Time.time;
            roll = true;
        }

        sFXManager.Walking = direction.magnitude > 0.1f && !attack && !roll && !slide;

        HandleAnimation();
    }

    public GameObject fadeOut;

    private void FixedUpdate()
    {
        if (!playerStats.Alive)
        {
            controller.Move(transform.position);
            GameSceneManager manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameSceneManager>();
            manager.LoadGameOverScene(2);
            fadeOut.SetActive(true);
            manager.PlayFadeAnimation(fadeOut);
        }
        //kalau misalkan ada input gerak dan mousenya tidak mati
        if (direction.magnitude >= 0.1f && !Cursor.visible)
        { 
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref reference, 0.1f);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            float ySpeed = movement.y;
            movement = (Quaternion.Euler(0, angle, 0) * Vector3.forward).normalized;

            if (slide && Time.time - lastSlide >= startSlide) movement *= slideSpeed;
            else if (roll && Time.time - lastRoll >= startRoll) movement *= rollSpeed;
            else movement *= movementSpeed;

            movement *= playerStats.SpeedMultiplier;

            if (attack) movement *= agilityMultiplier * (100 - attackMovementPenalty) / 100f;

            movement.y = ySpeed;
        }
        else movement.x = movement.z = 0;

        movement.y -= controller.isGrounded ? 0 : fallSpeed;

        controller.Move(Time.fixedDeltaTime * movement);
    }

    private void HandleAnimation()
    {
        animator.SetFloat("DirectionX", direction.x);
        animator.SetFloat("DirectionY", direction.z);
        animator.SetBool("IsWalking", direction.magnitude > 0.1f);
        animator.SetBool("Slide", slide);
        animator.SetBool("Roll", roll);
    }
}
    