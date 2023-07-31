using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AnimationAndMovementController : MonoBehaviour
{
    PlayerInput playerInput;

    [SerializeField]
    private float playerSpeed = 5f;
    [SerializeField]
    private float rotationSpeed = 3f;

    private float arrowForce = 15f;

    Vector3 targetPosition;

    private Camera mainCamera;
    private Coroutine coroutine;
    private InputAction touchPositionAction;
    private InputAction touchPressAction;

    public GameObject arrowObject;
    public Transform arrowPoint;

    bool isMovementPressed = false;
    float shootDelay = 1.5f;
    CharacterController characterController;
    Animator animator;
    [SerializeField] private PlayerStats stats;
    [SerializeField] private GameObject gameController;
    private GameManagerScript gameManagerScript;

    void Awake()
    {
        mainCamera = Camera.main;
        playerInput = GetComponent<PlayerInput>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        gameManagerScript = gameController.GetComponentInChildren<GameManagerScript>();
        touchPositionAction = playerInput.actions["TouchPosition"];
        touchPressAction = playerInput.actions["TouchPress"];
    }

    void onMovementInput(InputAction.CallbackContext context)
    {
        if (gameManagerScript.currentState == GameState.Playing)
        {
            Ray ray = mainCamera.ScreenPointToRay(touchPositionAction.ReadValue<Vector2>());
            if (Physics.Raycast(ray: ray, hitInfo: out RaycastHit hit) && hit.collider)
            {
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                    animator.SetBool("isWalking", false);
                }
                /*if (hit.collider.gameObject.tag == "Enemy")
                {
                    if (!hit.collider.GetComponent<Enemy>().isDead)
                    {
                        isMovementPressed = false;
                        handleRotation(hit.point);
                        //animator.SetBool("isWalking", false);
                        animator.SetBool("isShooting", true);
                    }
                }*/
               if (hit.collider.gameObject.tag == "Ground")
                {
                    targetPosition = hit.point;
                    isMovementPressed = true;
                }
            }
        }
    }
    private void Update()
    {
        if (isMovementPressed)
        {
            PlayerMoveTowards(targetPosition);
        }

        if (shootDelay <= 0)
        {
            Vector3 enemyPos = ObjectPoolingManager.GetNearestEnemy(transform.position);
            if ( enemyPos != Vector3.zero)
            {
                isMovementPressed = false;
                handleRotation(enemyPos);
                animator.SetBool("isShooting", true);
            }
            shootDelay = 1.5f;
        }
        else
            shootDelay -= Time.deltaTime;
    }

    //Player movement
    public void PlayerMoveTowards(Vector3 target)
    {
        float playerDistanceToFloor = transform.position.y - target.y;
        target.y += playerDistanceToFloor;
        if (Vector3.Distance(transform.position, target) > 0.1f && isMovementPressed)
        {
            Vector3 direction = target - transform.position;
            Vector3 movement = direction.normalized * playerSpeed * Time.deltaTime;
            characterController.Move(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized), rotationSpeed * Time.deltaTime);
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
            isMovementPressed = false;
        }
    }

    //Player rotation update on movement
    public void handleRotation(Vector3 target)
    {
        float playerDistanceToFloor = transform.position.y - target.y;
        target.y += playerDistanceToFloor;
        Vector3 direction = target - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized), 1f);
    }

    private void OnEnable()
    {
        touchPressAction.performed += onMovementInput;
    }

    private void OnDisable()
    {
        touchPressAction.performed -= onMovementInput;
    }

    // Player shoot arrow
    public void Shoot()
    {
        GameObject obj = ObjectPoolingManager.spawnObject(arrowObject, arrowPoint.position, transform.rotation, ObjectPoolingManager.poolType.PlayerArrow);
        obj.GetComponent<Rigidbody>().AddForce(transform.forward * arrowForce, ForceMode.VelocityChange);
        animator.SetBool("isWalking", false);
        animator.SetBool("isShooting", false);
        stats.UpdateArrowCount();
    }
}
