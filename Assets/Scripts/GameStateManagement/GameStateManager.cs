using Unity.VectorGraphics;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

    public void EndGame()
    {
     SceneManager.LoadScene(1);
    }

    public void LoadLevelById(int id)
    {
     SceneManager.LoadScene(id);
    }

    public void ReturnToHub()
    {
        LoadLevelByName("Hub Scene"); 
    }

    public void LoadLevelByName(string name)
    {
        SceneManager.LoadScene(name);
    }


}
