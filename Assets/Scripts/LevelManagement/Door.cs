using Unity.Cinemachine;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Room _room;

    void Start()
    {
        if (_room == null)
        {
          throw new MissingComponentException("Door needs a room.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (_room == null)
        {
          throw new MissingComponentException("Door needs a room.");
        }
    }

}
