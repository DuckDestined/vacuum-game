using Unity.Cinemachine;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] GameObject  _roomCamera;

    void Start()
    {
        _roomCamera.SetActive(false);
    }

    public void OnEnterRoom()
    {
        _roomCamera.SetActive(true);
        Debug.Log("Entered Room");
    }

  public void OnRoomExit()
    {
        _roomCamera.SetActive(false);
        Debug.Log("Exited Room");
    }


    void OnTriggerEnter(Collider other)
    {
        if(!isColliderPlayer(other))
        {
            return;
        }

        OnEnterRoom();
    }

    void OnTriggerExit(Collider other)
    {
        if(!isColliderPlayer(other))
        {
            return;
        }
        
        OnRoomExit();
    }

    private bool isColliderPlayer(Collider other)
    {
        return other.gameObject.CompareTag("Player");
    }
}
