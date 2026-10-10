using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class Vaccumable : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    public bool IsConfiguredByLevle = true;
    [SerializeField]
    public float Health;
    [SerializeField]
    private float weight = 1f;

    [Header("Status")]
    [SerializeField]
    public float CurrentHealth;
    [SerializeField]
    public float Resistance;
    public float CurrentLevelHealthPercentage;
    private bool _isDirty = true;



    [Header("Components")]
    [SerializeField] private Material[] _targetMaterials; 
    [SerializeField] private Animator _animator; 
    [SerializeField] private List<Renderer> _renderers;
    [SerializeField] private AudioSource _cleanJingleAudioSource;
    [SerializeField] private GameObject cleanBox;

    void Start()
    {
        CurrentHealth = Health;

        if(_animator == null)
        {
            _animator = GetComponent<Animator>();
        }

        if (_animator == null)
        {
            throw new MissingFieldException("Missing Animator Component!");       
        }

        _renderers = new List<Renderer>();
        var local_renderers = this.GetComponents<Renderer>().ToList();
        _renderers.AddRange(local_renderers);
        var children_renderers = this.GetComponentsInChildren<Renderer>().ToList();
        _renderers.AddRange(children_renderers);
        _targetMaterials = _renderers.Select(r => r.materials).SelectMany(x => x).ToArray();

        _cleanJingleAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        UpdateMaterial();
    }

    void UpdateMaterial()
    {
        if (_targetMaterials != null){
            foreach(Material material in _targetMaterials)
            {
                material.SetFloat("_Health", CurrentHealth/Health); 
            }
        }
    }
    public void Hit(float damage)
    {
        if (!_isDirty)
        {
            return;
        }

        var damageDealt = (damage - Resistance )* Time.deltaTime;
        damageDealt = Mathf.Max(damageDealt, 0);

        CurrentHealth -= damageDealt;
        CurrentHealth = Mathf.Clamp(CurrentHealth,0, Health);


        if (CurrentHealth <= 0.01f)
        {
            _cleanJingleAudioSource.Play();
            cleanBox.SetActive(true);
            _isDirty = false;
        }
    }

    public void SetShakingState(bool state)
    {
        _animator.SetBool("m_isShaking",state);
    }

    public void SetHealthAndResistance(float health, float resistance)
    {
        this.Health = weight * health;
        this.CurrentHealth = Health;
        this.Resistance = resistance;
        UpdateMaterial();
    }

}