using System.Collections.Generic;
using UnityEditor.MPE;
using UnityEngine;

[ExecuteAlways]
#nullable enable
public class VacuumCable : BaseInteractable
{
    [Header("Components & Attachment")]
    [SerializeField] private Transform _boundObjectTransform;
    [SerializeField] private Vector3 _playerOffset = new Vector3(0, 0.5f, 0);
    [SerializeField] private LineRenderer _lineRenderer;

    [Header("Settings")]
    [SerializeField] private float _cableWidth = 0.1f;
    [SerializeField] private float _totalCableLength = 5.0f;

    private Vector3 _cableStart;
    private Vector3 _cableEnd;
    [SerializeField] private BindableObjectBase? _boundTo;


    void Awake()
    {
        InitCable();
    }
 
    private void InitCable()
    {
        if (_lineRenderer == null)
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }
    }

    void Update()
    {
        if (_boundTo == null)
        {
            return;
        }

        DrawCable();
    }


    void DrawCable()
    {  
        _cableStart = transform.position;
        _cableEnd = _boundObjectTransform.position;
        _lineRenderer.positionCount = 2;
        _lineRenderer.startWidth = _cableWidth;
        _lineRenderer.endWidth = _cableWidth;
        _lineRenderer.SetPositions(new Vector3[] {_cableStart,_cableEnd});
    }

    void ClearCable()
    {
        _lineRenderer.positionCount = 0;
    }

    public override void Interact(InteractionHandler interactionHandler)
    {
        var bindableObject = interactionHandler.gameObject.GetComponentInHierarchy<BindableObjectBase>();
        
        if(bindableObject == null)
        {
            Debug.Log("InteractionHandler has no bindable object.");
            return;
        }

        ToggleBind(bindableObject);
    }

    public void ToggleBind( BindableObjectBase bindableObject )
    {
        if(_boundTo == null)
        {
            _boundObjectTransform = bindableObject.transform;
            _boundTo = bindableObject;
            DrawCable();
        }
        else
        {
            _boundTo = null;
            ClearCable();
        }

        bindableObject.ToggleBind();
    }
}