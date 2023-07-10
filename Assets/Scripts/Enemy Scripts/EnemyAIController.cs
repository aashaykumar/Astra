using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : MonoBehaviour
{
    public NavMeshAgent agent;
    public float startWaitTime = 10;
    public float timeToRotate = 2;
    public float speedWalk = 1;
    public float speedRun = 2;
    public Vector3 m_playerPosition;
    List<Transform> wayPoints = new List<Transform>();
    Animator animator;
    float m_WaitTime;
    bool m_PlayerInRange;
    bool m_isPatrol;
    // Start is called before the first frame update
    void Start()
    {
        m_isPatrol = true;
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = false;
        agent.speed = speedWalk;
        GameObject go = GameObject.FindGameObjectWithTag("WayPoints");
        foreach (Transform t in go.transform)
            wayPoints.Add(t);
        agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
        m_playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        animator = GetComponent<Animator>();
        animator.SetBool("isPatrolling", true);
        m_WaitTime = startWaitTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_isPatrol) 
        {
            Chasing();
        }
        else
        {
            Patroling();
        }
    }

    void Move(float speed)
    {
        agent.isStopped = false;
        agent.speed = speed;
    }

    void Stop()
    {
        agent.isStopped = true;
        agent.speed = 0;
        //animator.SetBool("isChasing", false);
        //animator.SetBool("isPatrolling", false);
    }

    void NextPoint()
    {
        agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
        animator.SetBool("isPatrolling", true);
    }

    void Patroling()
    {
        if(m_PlayerInRange)
        {
            Move(speedWalk);
            
        }
        else
        {
            m_PlayerInRange = false;
            //agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
            if(agent.remainingDistance <= agent.stoppingDistance)
            {
                if(m_WaitTime <= 0)
                {
                    NextPoint();
                    Move(speedWalk);
                    m_WaitTime = startWaitTime;
                }
                else
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }

        }
    }

    void Chasing()
    {
        m_PlayerInRange = false;
        Move(speedRun);
        agent.SetDestination(m_playerPosition);
        
        if (agent.remainingDistance <= agent.stoppingDistance) 
        {
            if(m_WaitTime <= 0 && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
            {
                m_isPatrol = true;
                animator.SetBool("isChasing", false);
                m_PlayerInRange = false;
                Move(speedWalk);
                m_WaitTime = startWaitTime;
                agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
            }
            else
            {
                if (Vector3.Distance(transform.position,GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
    
    }
}
