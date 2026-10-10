using UnityEngine;

public static class NewMonoBehaviourScript
{
    /// <summary>
    /// Searches for a component on the object itself, its children, or its parents.
    /// </summary>
    public static T GetComponentInHierarchy<T>(this GameObject gameObject, bool includeInactive = false) where T : Component
    {
        // 1. Search the object itself and its children
        T component = gameObject.GetComponentInChildren<T>(includeInactive);
        
        // 2. If not found, look up into the parents
        if (component == null)
        {
            component = gameObject.GetComponentInParent<T>(includeInactive);
        }
        
        return component;
    }
}
