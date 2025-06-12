using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public class LaserDie : MonoBehaviour
{
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
            LaserScreen.instance.ShowLaserScreen(other.transform.position); 
            player.canMove = false;
            player.canUseCamera = true;
        }
    }
}
