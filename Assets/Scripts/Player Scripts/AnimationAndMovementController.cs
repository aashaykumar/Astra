using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AnimationAndMovementController : MonoBehaviour
{
    PlayerInput playerInput;

    [SerializeField]
    private float playerSpeed= 2f;
    [SerializeField]
    private float rotationSpeed = 3f;

    Vector3 targetPosition;
    
    private Camera mainCamera;
    private Coroutine coroutine;
    private InputAction touchPositionAction;
    private InputAction touchPressAction;

    public GameObject arrowObject;
    public Transform arrowPoint;

    bool isMovementPressed;
    bool isAiming;
    float rotationFactorPerFrame = 1f;
    float speed = 1f;

    CharacterController characterController;
    Animator animator;
    [SerializeField] private PlayerStats stats;

    void Awake()
    {
        mainCamera = Camera.main;
        playerInput = GetComponent<PlayerInput>();
        characterController = GetComponent<CharacterController>();
       animator = GetComponent<Animator>();
        touchPositionAction = playerInput.actions["TouchPosition"];
        touchPressAction = playerInput.actions["TouchPress"];
    }

    void onMovementInput(InputAction.CallbackContext context)
    {
        Ray ray = mainCamera.ScreenPointToRay(touchPositionAction.ReadValue<Vector2>());
        if (Physics.Raycast(ray: ray, hitInfo: out RaycastHit hit) && hit.collider)
        {
            if (coroutine != null) StopCoroutine(coroutine);
            if (hit.collider.gameObject.tag == "Enemy")
            {
                handleRotation(hit.point);
                animator.SetBool("isWalking", false);
                animator.SetBool("isShooting", true);
                
            }
            else if (hit.collider.gameObject.tag == "Ground")
            {
                StartCoroutine(PlayerMoveTowards(hit.point));
                targetPosition = hit.point;
            }
        }
    }

    private IEnumerator PlayerMoveTowards(Vector3 target){
        float playerDistanceToFloor = transform.position.y - target.y;
        target.y += playerDistanceToFloor;
        while (Vector3.Distance(transform.position, target) > 1f){
            Vector3 destination = Vector3.MoveTowards(transform.position, target, playerSpeed * Time.deltaTime);
            Vector3 direction = target - transform.position;
            Vector3 movement = direction.normalized * playerSpeed * Time.deltaTime;
            characterController.Move(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized), rotationSpeed * Time.deltaTime);
            animator.SetBool("isWalking", true);
            yield return null;
        }
        if(Vector3.Distance(transform.position, target) <= 1f)
        {
            animator.SetBool("isWalking", false);
            yield return null;
        }
    }
    public void handleRotation(Vector3 target)
    {
        float playerDistanceToFloor = transform.position.y - target.y;
        target.y += playerDistanceToFloor;
        Vector3 direction = target - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized), 1);

    }
    void handleAnimation()
    {
        bool isWalking = animator.GetBool("isWalking");
        bool isRunning = animator.GetBool("isRunning");

        if (isMovementPressed && !isWalking) {
            animator.SetBool("isWalking", true);
        }
        else if (!isMovementPressed && isWalking)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
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
        GameObject arrow = Instantiate(arrowObject, arrowPoint.position, transform.rotation);
        arrow.GetComponent<Rigidbody>().AddForce(transform.forward * 25f, ForceMode.Impulse);
        animator.SetBool("isWalking", false);
        animator.SetBool("isShooting", false);
        stats.UpdateArrowCount();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(targetPosition,1);
    }

}
