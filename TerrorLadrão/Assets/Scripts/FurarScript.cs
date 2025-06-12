using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class FurarScript : MonoBehaviour
{
    [Header("Canvas-related")]
    [SerializeField] GameObject ui;
    [SerializeField] TextMeshProUGUI pierceText;
    [SerializeField] string piercePiercingText = "Furando";
    [Space]
    [SerializeField] TextMeshProUGUI secondsText;
    [SerializeField] float secondsToPierce;
    [Space]
    [SerializeField] Button confirmButton;
    [SerializeField] float seeRange = 10f;

    [Header("Audio")]
    [SerializeField] AudioSource sawSource;
    [Header("Misc")]
    [SerializeField] UnityEvent afterPierce;
    [SerializeField] UnityEvent beforePierce;
    MeshCollider meshCollider;
    bool endedCutting;
    bool isCutting;
    void Start()
    {
        beforePierce.Invoke();
        confirmButton.onClick.AddListener(StartPierce);
    }
    void Update()
    {
        if (endedCutting) return;
        if (isCutting) return;
        float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        if (distance <= seeRange)
        {
            ui.SetActive(true);
        }
        else
        {
            ui.SetActive(false);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, seeRange);
    }
    public void StartPierce()
    {
        isCutting = true;
        StartCoroutine(dotAnimation());
        StartCoroutine(Counting());
        sawSource.Play();
        confirmButton.gameObject.SetActive(false);
    }
    IEnumerator dotAnimation()
    {
        while (!endedCutting)
        {
            string furando = piercePiercingText;
            for (int i = 0; i < 3; i++)
            {
                furando += ".";
                pierceText.text = furando;
                yield return new WaitForSeconds(0.5f);
            }
            pierceText.text = piercePiercingText;
        }
    }
    IEnumerator Counting()
    {
        float countingNumber = secondsToPierce;
        while (countingNumber > 0)
        {
            yield return new WaitForSeconds(0.1f);
            countingNumber -= 0.1f;
            secondsText.text = countingNumber.ToString("F1");
        }
        EndCutting();
    }
    void EndCutting()
    {
        endedCutting = true;
        sawSource.Stop();
        ui.SetActive(false);
        afterPierce.Invoke();
    }
    public void SkipScene(float seconds)
    {
        StartCoroutine(WinScene(seconds));
    }
    IEnumerator WinScene(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("WinScene");
    }
}
