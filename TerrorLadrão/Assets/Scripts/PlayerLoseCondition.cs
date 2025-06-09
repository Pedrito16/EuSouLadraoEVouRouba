using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
public class PlayerLoseCondition : MonoBehaviour
{
    
    [SerializeField] float secondsToLose = 2.5f;
    [SerializeField] TextMeshProUGUI countText;
    public bool isCounting;
    bool canCount;
    [Header("Jumpscare")]
    [SerializeField] GameObject jumpscare;
    [SerializeField] AudioClip batemanScreaming;
    [SerializeField] GameObject shootPng;
    
    [SerializeField] AudioClip shootSound;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Animator playerDieAnimator;
    [SerializeField] Button resetButton;

    [Header("BackGround")]
    [SerializeField] GameObject whiteBackground;
    [SerializeField] GameObject shootBackGround;
    [SerializeField] GameObject redBackGround;
    public static PlayerLoseCondition instance;
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    void Start()
    {
        if (jumpscare != null) jumpscare.SetActive(false);
        if (shootBackGround != null) shootBackGround.SetActive(false);
        if (shootPng != null) shootPng.SetActive(false);
        whiteBackground.SetActive(false);
        redBackGround.SetActive(false);
        resetButton.gameObject.SetActive(false);
        resetButton.onClick.AddListener(ResetScene);
        playerDieAnimator.enabled = false;
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
        //countText.gameObject.SetActive(true);
        while(countingNumber > 0)
        { 
            yield return new WaitForSeconds(0.1f);
            countingNumber -= 0.1f;
            countText.text = countingNumber.ToString("F1");
        }
        //abaixo segue o campo do jumpscare, tudo que acontece quando ele aparece
        isCounting = false;

        //countText.gameObject.SetActive(false);
        StarterAssets.FirstPersonController.instance.canMove = false;
        SoundController.instance.MuteAll();
        if (jumpscare != null) jumpscare.SetActive(true);
        whiteBackground.SetActive(true);
        ShakeEffect.instance.Shake(jumpscare, 3, 1);
        audioSource.clip = batemanScreaming;
        audioSource.Play();

        yield return new WaitForSeconds(3);

        audioSource.clip = null;
        audioSource.Stop();

        if (jumpscare != null) jumpscare.SetActive(false);
        shootBackGround.SetActive(true);

        yield return new WaitForSeconds(1);

        audioSource.clip = shootSound;
        audioSource.Play();
        yield return new WaitForSeconds(1);
        shootPng.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        playerDieAnimator.enabled = true;
        shootPng.SetActive(false);
        audioSource.Stop();
        playerDieAnimator.SetBool("Die", true);
        redBackGround.SetActive(true);
        shootBackGround.SetActive(false);
        whiteBackground.SetActive(false);
        yield return new WaitForSeconds(3);
        resetButton.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
