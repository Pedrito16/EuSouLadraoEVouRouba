using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    Patrol,
    Searching,
    Attack
}
public class EnemyAIMovement : MonoBehaviour
{
    [SerializeField] float anguloVisão = 60;
    [SerializeField] float distanciaVisão = 10;
    [SerializeField] EnemyState state;
    [SerializeField] Transform[] waypoints;
    [SerializeField] Transform head;
    NavMeshAgent agent;
    Transform target;
    Vector3 direction;
    float angle;
    float distance;
    void Start()
    {
        target = FirstPersonController.instance.transform;
        agent = GetComponent<NavMeshAgent>();
        
    }
    void Update()
    {
        direction = target.position - transform.position;
            
        distance = Vector3.Distance(transform.position, target.position);
        print(direction);
        if (distance < distanciaVisão)
        {
            angle = Vector3.Angle(direction, transform.forward);
            if (angle < anguloVisão)
            {
                agent.SetDestination(target.position);
            }
        }
        switch (state)
        {
            case EnemyState.Idle:
                // Lógica para estado Idle
                break;
        }
    }
    private void LateUpdate()
    {
        if(distance < distanciaVisão)
        {
            head.rotation = Quaternion.LookRotation(direction);
        }
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
