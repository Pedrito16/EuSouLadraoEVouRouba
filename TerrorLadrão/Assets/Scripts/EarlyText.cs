using UnityEngine;
using TMPro;
using System.Collections;
public class EarlyText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] string[] textString;
    [SerializeField] float timeToAppear;
    [SerializeField] AudioSource writingSource;
    [SerializeField] AudioClip writingSound;
    void Start()
    {
        writingSource = GetComponentInChildren<AudioSource>();
        StartCoroutine(TextWrite(textString));
    }
    void Update()
    {
        
    }
    IEnumerator TextWrite(string[] stringArray) 
    {
        writingSource.Play();
        for (int i = 0; i < stringArray.Length; i++)
        {
            char[] caracteres = textString[i].ToCharArray();
            for (int j = 0; j < caracteres.Length; j++)
            {
                text.text += caracteres[j];
                yield return new WaitForSeconds(0.05f);
            }
            writingSource.Stop();
            yield return new WaitForSeconds(1f);
            writingSource.Play();
            yield return new WaitForSeconds(1f);
            text.text = "";
        }
        text.gameObject.SetActive(false);
        writingSource.Stop();
    }
}
