using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VaccumControlling : MonoBehaviour
{
    [SerializeField]
    private float power;


    private List<Vaccumable> hitVaccumableObjects;


    void Start()
    {
        hitVaccumableObjects = new List<Vaccumable>();
    }


    void Update()
    {
        foreach(Vaccumable hitObject in hitVaccumableObjects)
        {
            hitObject.Hit(power);
        }
    }

    public void TurnOffVaccum()
    {   
        foreach(Vaccumable vaccumable in hitVaccumableObjects)
        {
            vaccumable.SetShakingState(false);
        }
        
        hitVaccumableObjects = new List<Vaccumable>();
    }

    void OnTriggerEnter(Collider other)
    {
        
        var vaccumable = other.gameObject.GetComponent<Vaccumable>();

        if (vaccumable == null)
        {
            return;
        }

        hitVaccumableObjects.Add(vaccumable);
        vaccumable.SetShakingState(true);
    }

    void OnTriggerExit(Collider other)
    {
        var vaccumable = other.gameObject.GetComponent<Vaccumable>();

        if (vaccumable == null)
        {
            return;
        }

        hitVaccumableObjects.Remove(vaccumable);
        vaccumable.SetShakingState(false);

    }
}
