using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class CleanBoxEffect : MonoBehaviour
{
    [SerializeField] float _effectLength    = 1f; 
    [SerializeField] float _progress        = 0f;
    [SerializeField] bool _isStarted         = false;


    private Renderer _renderer;
    [SerializeField] private Material _material;

    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void Awake()
    {
        _renderer = GetComponent<Renderer>();

        if (_renderer == null)
        {
            throw new MissingComponentException("Box needs a renderer");
        }

        _material = _renderer.material;

        if(_material == null)
        {
            throw new MissingComponentException("Box needs a material");
            
        }

        StartEffect();
    }

    void Update()
    {
        if (!_isStarted)
        {
            return;
        }

        _progress += Time.deltaTime;
        var _progressPercent = _progress/_effectLength;

        if(_progressPercent <= 0.9f)
        {
            _material.SetFloat("_progress",_progressPercent);
        } else
        {
            gameObject.SetActive(false);
        }
    }
    private void StartEffect()
    {
       _isStarted = true;
       _progress = 0f;
    }
    }

