using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class FurarScript : MonoBehaviour
{
    [Header("Mesh")]
    [SerializeField] MeshFilter domeFilter;
    [SerializeField] Mesh originalDome;
    [SerializeField] Mesh domeWithCut;
    [Header("Canvas-related")]
    [SerializeField] GameObject canva;
    [SerializeField] TextMeshProUGUI pierceText;
    [SerializeField] string piercePiercingText = "Furando";
    [Space]
    [SerializeField] TextMeshProUGUI secondsText;
    [SerializeField] float secondsToPierce;
    [Space]
    [SerializeField] Button confirmButton;
    [SerializeField] AudioSource sawSource;
    bool endedCutting;
    void Start()
    {
        domeFilter.mesh = originalDome;
        confirmButton.onClick.AddListener(StartPierce);
    }
    public void StartPierce()
    {
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
        sawSource.Stop();
        canva.SetActive(false);
        domeFilter.mesh = domeWithCut;
    }
}
