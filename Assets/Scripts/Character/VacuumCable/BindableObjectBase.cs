using UnityEngine;

public abstract class BindableObjectBase : MonoBehaviour
{
    protected bool _isBound;

    public void ToggleBind()
        {
            _isBound = !_isBound;
        }
}
