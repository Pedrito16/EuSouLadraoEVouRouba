using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public class LaserDie : MonoBehaviour
{
    [SerializeField] NavMeshAgent[] agents;
    [Tooltip("The agents that will catch the player")]
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FirstPersonController player = other.GetComponent<FirstPersonController>();
            for (int i = 0; i < agents.Length; i++)
            {
                agents[i].gameObject.SetActive(true);
                agents[i].enabled = true;
                agents[i].GetComponent<EnemyAIMovement>().waypoints[i] = null;
                agents[i].SetDestination(other.transform.position);
            }
            player.canMove = false;
            player.canUseCamera = true;
        }
    }
}
