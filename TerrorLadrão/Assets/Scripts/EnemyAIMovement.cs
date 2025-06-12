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
    [Tooltip("OPCIONAL: transform da cabe�a do inimigo que faz a cabe�a dele olhar para o player")]
    [SerializeField] public Transform[] waypoints;
    [SerializeField] new Light light;

    [Header("Configurable")]
    [SerializeField] float anguloVis�o = 60; //angulo de raio para ter apenas o cone de vis�o
    [SerializeField] float distanciaVis�o = 10; //distancia em que o cone pode pegar
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

        if (distance < distanciaVis�o)
        {
            angle = Vector3.Angle(direction, transform.forward);
            if (angle < anguloVis�o && !isLookingAtPlayer)
            {
                isLookingAtPlayer = true;
                SwitchState(EnemyState.LookingAtPlayer);
            } else if( angle > anguloVis�o && isLookingAtPlayer)
            {
                isLookingAtPlayer = false;
                playerLoseConditionScript.ResetCounting();
                StartCoroutine(WaitState(EnemyState.Patrol));
            }

        }else if(distance >= distanciaVis�o && isLookingAtPlayer)
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

        if (distance < distanciaVis�o)
        {
            if (angle < anguloVis�o)
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
        Gizmos.DrawWireSphere(transform.position, distanciaVis�o);

        Vector3 direcaoEsquerda = Quaternion.Euler(0, -anguloVis�o, 0) * transform.forward;
        Vector3 direcaoDireita = Quaternion.Euler(0, anguloVis�o, 0) * transform.forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, direcaoEsquerda * distanciaVis�o);
        Gizmos.DrawRay(transform.position, direcaoDireita * distanciaVis�o);
    }
}
