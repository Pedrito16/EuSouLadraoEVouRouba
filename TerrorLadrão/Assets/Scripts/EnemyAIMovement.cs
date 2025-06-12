using StarterAssets;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public enum EnemyState
{
    LookingAtPlayer,
    Patrol,
    Wait,
}
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAIMovement : MonoBehaviour
{
    [Header("Optional")]
    [SerializeField] Transform head;
    [Tooltip("OPCIONAL: transform da cabeça do inimigo que faz a cabeça dele olhar para o player")]
    [SerializeField] public Transform[] waypoints;
    [SerializeField] new Light light;

    [Header("Configurable")]
    [SerializeField] float anguloVisão = 60; //angulo de raio para ter apenas o cone de visão
    [SerializeField] float distanciaVisão = 10; //distancia em que o cone pode pegar
    [SerializeField] float waitingTime = 0.85f;
    [SerializeField] bool canMove = true;
    [Header("Read-Only")]
    [SerializeField] int walkingToWaypoint;
    [SerializeField] EnemyState state;

    //variaveis privadas
    PlayerLoseCondition playerLoseConditionScript;
    NavMeshAgent agent;
    Transform target;
    Vector3 direction;
    float angle;
    float distance;
    bool isLookingAtPlayer;
    Color originalLightColor;
    float originalIntensity = 50f;
    Animator animator;
    void Start()
    {
        playerLoseConditionScript = PlayerLoseCondition.instance;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        target = FirstPersonController.instance.transform;

        originalLightColor = light.color;
        light.intensity = originalIntensity;
        if (waypoints[walkingToWaypoint] != null) agent.SetDestination(waypoints[walkingToWaypoint].position);
        if (canMove)
        {
            agent.enabled = true;
            SwitchState(EnemyState.Patrol);
        }
        else
        {
            state = EnemyState.Wait;
            animator.SetBool("IsPatrolling", false);
        }
    }
    void Update()
    {
        direction = target.position - transform.position;
        distance = Vector3.Distance(transform.position, target.position);

        if (distance < distanciaVisão)
        {
            angle = Vector3.Angle(direction, transform.forward);
            if (angle < anguloVisão && !isLookingAtPlayer)
            {
                isLookingAtPlayer = true;
                SwitchState(EnemyState.LookingAtPlayer);
            } else if( angle > anguloVisão && isLookingAtPlayer)
            {
                isLookingAtPlayer = false;
                playerLoseConditionScript.ResetCounting();
                StartCoroutine(WaitState(EnemyState.Patrol));
            }

        }else if(distance >= distanciaVisão && isLookingAtPlayer)
        {
            isLookingAtPlayer = false;
            playerLoseConditionScript.ResetCounting();
            StartCoroutine(WaitState(EnemyState.Patrol));
        }
        switch (state)
        {
            case EnemyState.Patrol:
                if (agent.remainingDistance <= 0.5f && waypoints[walkingToWaypoint] != null)
                {
                    walkingToWaypoint++;
                    walkingToWaypoint = CheckIfShouldResetPatrol(walkingToWaypoint);
                    agent.SetDestination(waypoints[walkingToWaypoint].position);
                }
                    break;
        }
    }
    private void LateUpdate()
    {
        if (head == null) return;

        if (distance < distanciaVisão)
        {
            if (angle < anguloVisão)
            {
                head.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
    void SwitchState(EnemyState newState)
    {
        switch (newState)
        {
            case EnemyState.LookingAtPlayer:
                if(canMove) agent.enabled = false;
                playerLoseConditionScript.isCounting = true;
                light.color = new Color(248, 98, 15);
                light.intensity = 0.001f;
                animator.SetBool("IsPatrolling", false);
                break;
            case EnemyState.Patrol:
                animator.SetBool("IsPatrolling", true);
                break;
            case EnemyState.Wait:
                StartCoroutine(WaitState(EnemyState.Patrol));
                break;
        }
        state = newState;
    }
    IEnumerator WaitState(EnemyState stateToGoTo)
    {        
        yield return new WaitForSeconds(waitingTime);
        head.rotation = Quaternion.identity;
        if(canMove) agent.enabled = true;
        light.color = originalLightColor;
        light.intensity = originalIntensity;
        if (stateToGoTo == EnemyState.Patrol) animator.SetBool("IsPatrolling", true);
        state = stateToGoTo;
    }
    int CheckIfShouldResetPatrol(int value)
    {
        value = Mathf.Clamp(value, 0, waypoints.Length);
        if (value < waypoints.Length)
        {
            return value;
        }
        else if (value >= waypoints.Length)
        {
            value = 0;
            return value;
        }
        else return 0;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaVisão);

        Vector3 direcaoEsquerda = Quaternion.Euler(0, -anguloVisão, 0) * transform.forward;
        Vector3 direcaoDireita = Quaternion.Euler(0, anguloVisão, 0) * transform.forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, direcaoEsquerda * distanciaVisão);
        Gizmos.DrawRay(transform.position, direcaoDireita * distanciaVisão);
    }
}
