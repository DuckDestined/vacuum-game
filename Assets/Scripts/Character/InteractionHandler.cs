using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    [Header("Components")]

    [SerializeField] private List<BaseInteractable> _interactablesInReach;

    void Start()
    {
        _interactablesInReach = new List<BaseInteractable>();
    }

    void OnTriggerEnter(Collider other)
    {

        if (!other.gameObject.TryGetComponent(out BaseInteractable interactableComponent))
        {
            Debug.Log("Object in reach is not interactable" + other.gameObject.name, other.gameObject);
            return;
        }

        _interactablesInReach.Add(interactableComponent);
    }

    void OnTriggerExit(Collider other)
    {
        BaseInteractable interactableComponent = other.gameObject.GetComponent<BaseInteractable>();

        if (interactableComponent == null)
        {
            Debug.Log("Object not longer in reach");
        }

        _interactablesInReach.Remove(interactableComponent);
    }

    
    public void Interact()
    {

        if (_interactablesInReach.Count == 0)
        {
            Debug.Log("No Interactables in reach");
            return;
        }

        (BaseInteractable interactable, float distanceFromHandler) closestInteractable = (null, 0f);

        var handlerPosition = this.transform.parent.transform.position;


        foreach(BaseInteractable i in _interactablesInReach)
        {
            var distanceFromHandler = (handlerPosition - i.gameObject.transform.position).magnitude;

            if (closestInteractable.interactable == null || distanceFromHandler < closestInteractable.distanceFromHandler)
            {
                closestInteractable = (i,distanceFromHandler);
                continue;
            }

        }

        closestInteractable.interactable.Interact(this);
        
        if (!closestInteractable.interactable.AllowsMultipleInteractions)
        {
            _interactablesInReach.Remove(closestInteractable.interactable);
        }
    }
}

