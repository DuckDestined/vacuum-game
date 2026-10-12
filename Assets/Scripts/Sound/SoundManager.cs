using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;

    public static SoundManager Instance;


    void Awake()
    {
        DontDestroyOnLoad(this);

        if(Instance != null)
        {
            Destroy(this);
        }

        Instance = this;
    }

    public void SetLevel(string channelName)
    {
        
    }
}

