using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] AudioSource mainSoundTrack;
    [SerializeField] AudioSource[] policeAudio;
    public static SoundController instance;
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    public void MuteAll()
    {
        for(int i = 0; i < policeAudio.Length; i++)
        {
            policeAudio[i].Stop();
        }
        mainSoundTrack.Stop();
    }
}
