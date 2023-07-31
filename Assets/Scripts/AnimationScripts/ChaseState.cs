using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateMachineBehaviour
{
    NavMeshAgent agent;
    float timer;
    Transform player;
    float chaseRange;
    float AttackRange;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0f;
        agent = animator.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        AttackRange = animator.GetComponent<Enemy>().enemyStats.attackRange;
        agent.speed = animator.GetComponent<Enemy>().enemyStats.chaseSpeed;
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if (timer > 3)
        {
            agent.SetDestination(player.position);
            timer = 0f;
        }
        //Vector2 Playerpos =new Vector2(player.position.x, player.position.z);
        //Vector2 Enemypos =new Vector2(animator.transform.position.x, animator.transform.position.z);
        float distance = Vector3.Distance(player.position, animator.transform.position);
        Vector3 direction = player.position - animator.transform.position;
        animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, Quaternion.LookRotation(direction.normalized), 0.9f);
        if (distance <= AttackRange)
        {
            agent.speed = 0;
            animator.SetBool("isAttacking", true);
            agent.SetDestination(agent.transform.position);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
