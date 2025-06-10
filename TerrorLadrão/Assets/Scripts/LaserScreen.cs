using UnityEngine;
using UnityEngine.AI;

public class LaserScreen : MonoBehaviour
{
    [SerializeField] GameObject laserScreen;
    [SerializeField] NavMeshAgent[] agents;
    [Tooltip("The agents that will catch the player")]
    [SerializeField] Light[] lights;
    public static LaserScreen instance;
    void Start()
    {
        if (instance == null) instance = this;
        laserScreen.SetActive(false);
    }
    void Update()
    {
        
    }
    public void ShowLaserScreen(Vector3 playerPos)
    {
        for (int i = 0; i < agents.Length; i++)
        {
            agents[i].gameObject.SetActive(true);
            agents[i].enabled = true;
            agents[i].GetComponent<EnemyAIMovement>().waypoints[i] = null;
            agents[i].SetDestination(playerPos);
        }
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].intensity = 0f;
        }
        laserScreen.SetActive(true);
    }
}
