using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [SerializeField]
    private Settings _defaultSetting;
    private Settings _userSettings;

    void Awake()
    {
        DontDestroyOnLoad(this);
        InitSettings();
    }

    void InitSettings()
    {
        Settings appliedSettings = _defaultSetting;

        if (LoadUserSettings())
        {
            appliedSettings = _userSettings;
        }

        ApplySettings(appliedSettings);
    }


    void ApplySettings(Settings settings)
    {
        
    }

    bool LoadUserSettings()
    {
        return false;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
