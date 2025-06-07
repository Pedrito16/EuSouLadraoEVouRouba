using UnityEngine;
using System.Collections;
using TMPro;
public class PlayerLoseCondition : MonoBehaviour
{
    [SerializeField] GameObject jumpscare;
    [SerializeField] float secondsToLose = 2.5f;
    [SerializeField] TextMeshProUGUI countText;
    public bool isCounting;
    bool canCount;
    public static PlayerLoseCondition instance;
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    void Start()
    {
        if (jumpscare != null) jumpscare.SetActive(false);
        canCount = true;
        countText.gameObject.SetActive(false);
    }
    void Update()
    {
        if (isCounting && canCount)
        {
            print("Contando");
            StartCoroutine(Counting());
            canCount = false;
        }
    }
    IEnumerator Counting()
    {
        float countingNumber = secondsToLose;
        countText.gameObject.SetActive(true);
        while(countingNumber > 0)
        { 
            yield return new WaitForSeconds(0.1f);
            countingNumber -= 0.1f;
            countText.text = countingNumber.ToString("F1");
        }
        //abaixo segue o campo do jumpscare, tudo que acontece quando ele aparece
        countText.gameObject.SetActive(false);
        if (jumpscare != null) jumpscare.SetActive(true);
        isCounting = false;
    }
    public void ResetCounting()
    {
        print("resetando");
        StopAllCoroutines();
        countText.text = secondsToLose.ToString();
        countText.gameObject.SetActive(false);
        isCounting = false;
        canCount = true;
    }
}
