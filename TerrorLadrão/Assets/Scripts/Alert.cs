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
        if (collider != null)
        {
            canvasGroup.alpha = 1f;
            interrogationPoint.SetActive(true);
        }
        else
        {
            canvasGroup.alpha = 0f;
            interrogationPoint.SetActive(false);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
