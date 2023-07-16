using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateMachineBehaviour
{
    NavMeshAgent agent;
    Transform player;
    float chaseRange;
    float AttackRange;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        AttackRange = animator.GetComponent<Enemy>().enemyStats.attackRange;
        agent.speed = animator.GetComponent<Enemy>().enemyStats.chaseSpeed;
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(player.position);
        float distance = Vector2.Distance(player.position, animator.transform.position);
        /*Vector3 direction = player.position - animator.transform.position;
        animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, Quaternion.LookRotation(direction.normalized), 0.9f);
*/     if (distance <= AttackRange)
        {
            agent.speed = 0;
            animator.SetBool("isAttacking", true);
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
