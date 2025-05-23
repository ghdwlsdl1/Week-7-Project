using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("이동 및 점프")]
    public float moveSpeed = 5f;           // 이동 속도
    public float baseMoveSpeed;            // 이동 속도 저장
    public float jumpPower = 80f;          // 점프 힘
    public float baseJumpPower;            // 점프 힘 저장
    public LayerMask groundLayerMask;      // 바닥 판정용 레이어
    private Vector2 curMovementInput;      // 현재 입력 방향
    private float runMultiplier = 1.5f;    // 달리기 배율
    public float staminaRunCost = 20f;    // 초당 소모량
    private bool isRunning;                // 달리는 중
    public float currentSpeedBoost = 1f;   // 음식 효과 배율


    private bool isInventoryOpen = false;
    public bool canLook => !isInventoryOpen;

    private Vector2 mouseDelta;

    private Rigidbody _rigidbody;          // 물리 이동용 리지드바디
    private void Awake()
    {
        baseMoveSpeed = moveSpeed;
        baseJumpPower = jumpPower;
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        var stamina = CharacterManager.Instance.Player.condition.stamina;

        if (isRunning)
        {
            stamina.Subtract(staminaRunCost * Time.fixedDeltaTime);

            if (stamina.curValue <= 0f)
            {
                isRunning = false;
            }
        }

        Move();
    }

    //---------------------------------------------------------------------

    // 이동 입력 처리
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            curMovementInput = context.ReadValue<Vector2>();
        else if (context.phase == InputActionPhase.Canceled)
            curMovementInput = Vector2.zero;
    }
    // 이동처리
    void Move()
    {
        if (CharacterManager.Instance.Player.viewSwitcher.IsThirdPerson())
            MoveThirdPerson();
        else
            MoveFirstPerson();
    }
    // 3인칭
    void MoveThirdPerson()
    {
        Transform cam = CharacterManager.Instance.Player.viewSwitcher.mainCameraTransform;
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 dir = camForward * curMovementInput.y + camRight * curMovementInput.x;

        float speedMultiplier = isRunning ? runMultiplier : 1f;
        speedMultiplier *= currentSpeedBoost;

        dir *= moveSpeed * speedMultiplier;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;

        if (dir != Vector3.zero)
            transform.forward = new Vector3(dir.x, 0, dir.z);
    }
    // 1인칭
    void MoveFirstPerson()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;

        float speedMultiplier = isRunning ? runMultiplier : 1f;
        speedMultiplier *= currentSpeedBoost;

        dir *= moveSpeed * speedMultiplier;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;

    }

    //---------------------------------------------------------------------

    // 마우스 입력 처리
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta()
    {
        return mouseDelta;
    }

    //---------------------------------------------------------------------

    // 점프 입력 처리
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
            _rigidbody.AddForce(Vector2.up * jumpPower , ForceMode.Impulse);
    }

    // 바닥 체크 (4방향 Ray로 바닥 판정)
    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }
        return false;
    }
    //---------------------------------------------------------------------
    // 인벤토리 입력 처리
    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            InventoryUI.Instance.ToggleInventory();
            isInventoryOpen = !isInventoryOpen;
        }
    }

    //---------------------------------------------------------------------
    // 달리기 입력 처리
    public void OnRun(InputAction.CallbackContext context)
    {
        var condition = CharacterManager.Instance.Player.condition;
        float currentStamina = condition.stamina.curValue;

        if (context.phase == InputActionPhase.Performed && currentStamina >= 20f)
        {
            isRunning = true;

        }
        else if (context.phase == InputActionPhase.Canceled || currentStamina <= 0f)
        {
            isRunning = false;

        }
    }
    //---------------------------------------------------------------------
    // 시점전환 입력 처리
    public void OnChangeView(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            CharacterManager.Instance.Player.viewSwitcher.SwitchView();
        }
    }
}
