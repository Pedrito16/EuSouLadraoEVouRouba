using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIMovement : MonoBehaviour
{
    [SerializeField] float anguloVisão = 60;
    [SerializeField] float distanciaVisão = 10;
    NavMeshAgent agent;
    Transform target;
    Vector3 direction;
    float angle;
    void Start()
    {
        target = FirstPersonController.instance.transform;
        agent = GetComponent<NavMeshAgent>();
        
    }
    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if(distance < distanciaVisão)
        {
            direction = target.position - transform.position;
            angle = Vector3.Angle(direction, transform.forward);
            if (angle < anguloVisão / 2)
            {
                agent.SetDestination(target.position);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaVisão);

        Vector3 direcaoEsquerda = Quaternion.Euler(0, -anguloVisão / 2, 0) * transform.forward;
        Vector3 direcaoDireita = Quaternion.Euler(0, anguloVisão / 2, 0) * transform.forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, direcaoEsquerda * distanciaVisão);
        Gizmos.DrawRay(transform.position, direcaoDireita * distanciaVisão);
    }
}
