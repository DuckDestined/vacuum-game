using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this);
    }
    public void LoadLevelByName(string name)
    {
        SceneManager.LoadScene(name);
    }
}
