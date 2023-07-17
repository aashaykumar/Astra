using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AnimationAndMovementController : MonoBehaviour
{
    PlayerInput playerInput;


    [SerializeField]
    private float playerSpeed = 2f;
    [SerializeField]
    private float rotationSpeed = 3f;
    [SerializeField]
    private float arrowForce = 10f;

    Vector3 targetPosition;

    private Camera mainCamera;
    private Coroutine coroutine;
    private InputAction touchPositionAction;
    private InputAction touchPressAction;

    public GameObject arrowObject;
    public Transform arrowPoint;

    bool isMovementPressed = false;
    bool isAiming;
    float rotationFactorPerFrame = 1f;
    float speed = 1f;

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
                if (hit.collider.gameObject.tag == "Enemy")
                {
                    if (!hit.collider.GetComponent<Enemy>().isDead)
                    {
                        isMovementPressed = false;
                        handleRotation(hit.point);
                        //animator.SetBool("isWalking", false);
                        animator.SetBool("isShooting", true);
                    }
                }
                else if (hit.collider.gameObject.tag == "Ground")
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
    }

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

    public void Shoot()
    {
        GameObject obj = ObjectPoolingManager.spawnObject(arrowObject, arrowPoint.position, transform.rotation, ObjectPoolingManager.poolType.PlayerArrow);
        //GameObject arrow = Instantiate(arrowObject, arrowPoint.position, transform.rotation);
        obj.GetComponent<Rigidbody>().AddForce(transform.forward * arrowForce, ForceMode.VelocityChange);
        animator.SetBool("isWalking", false);
        animator.SetBool("isShooting", false);
        stats.UpdateArrowCount();
    }

    /*private void OnDrawGizmos()
     {
         Gizmos.color = Color.yellow;
         Gizmos.DrawSphere(targetPosition, 1);
     }*/

}
