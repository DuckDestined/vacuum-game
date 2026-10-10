using System;
using UnityEngine;

public abstract class BaseInteractable : MonoBehaviour
{
    public bool AllowsMultipleInteractions;
    public abstract void Interact(InteractionHandler interactionHandler);

}