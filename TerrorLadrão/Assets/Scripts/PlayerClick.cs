using UnityEngine;

public class PlayerClick : MonoBehaviour
{
    [SerializeField] Transform cameraRoot;
    void Start()
    {
        
    }

    void Update()
    {
        Ray raio = new Ray(cameraRoot.transform.position, cameraRoot.transform.forward);
        RaycastHit hit;
        Debug.DrawRay(cameraRoot.transform.position, cameraRoot.transform.forward);
        if (Physics.Raycast(raio, out hit))
        {
            var button = hit.collider.GetComponent<UnityEngine.UI.Button>();
            if (button != null && Input.GetMouseButtonDown(0))
            {
                print("Clicando");
                button.onClick.Invoke();
            }
        }
    }
}
