using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class VacuumCable : MonoBehaviour
{
    [Header("Components & Attachment")]
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Vector3 _playerOffset = new Vector3(0, 0.5f, 0);
    [SerializeField] private LineRenderer _lineRenderer;

    [Header("Settings")]
    [SerializeField] private float _cableWidth = 0.1f;
    [SerializeField] private float _totalCableLength = 5.0f;

    private Vector3 _cableStart;
    private Vector3 _cableEnd;

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
        _cableStart = transform.position;
        _cableEnd = _playerTransform.position;
        DrawCable();
    }


    void DrawCable()
    {
        _lineRenderer.positionCount = 2;
        _lineRenderer.startWidth = _cableWidth;
        _lineRenderer.endWidth = _cableWidth;
        _lineRenderer.SetPositions(new Vector3[] {_cableStart,_cableEnd});
    }

}