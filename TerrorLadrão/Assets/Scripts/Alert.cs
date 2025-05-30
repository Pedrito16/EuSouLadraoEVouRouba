using UnityEngine;

public class Alert : MonoBehaviour
{
    [SerializeField] GameObject interrogationPoint;
    [SerializeField] LayerMask enemyFilter;
    CanvasGroup canvasGroup;
    [SerializeField] float radius;
    void Start()
    {
        canvasGroup = interrogationPoint.GetComponent<CanvasGroup>();
        interrogationPoint.SetActive(false);
    }
    void Update()
    {
        Collider[] collider = Physics.OverlapSphere(transform.position, radius, enemyFilter);   
        if (collider.Length > 0)
        {
                interrogationPoint.SetActive(true);
        }
        else
        {
                interrogationPoint.SetActive(false);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
