using System;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Scriptable Objects/Settings")]
public class Settings : ScriptableObject
{
    public enum DisplayMode
    {
        Windowed,
        Fullscreen,
        FullscreenWindow,
    };

    public DisplayMode displayMode; 
    [Range(0,1)]
    [DefaultValue(0.75f)]   
    public float masterVolume; 
    [Range(0,1)]
    [DefaultValue(0.75f)]  
    public float musicVolume;
    [Range(0,1)]  
    [DefaultValue(0.75f)]    
    public float effectsVolume;
}
