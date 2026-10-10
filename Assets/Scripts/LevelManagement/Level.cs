using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Level : MonoBehaviour
{
    // Settings
    [Header("Settings")]
    [SerializeField] private int _levelReward = 100;
    [SerializeField] private float _levelDifficulty = 100f;
    [SerializeField] private float _levelDefense = 10f;
    [SerializeField] private float _patienceDecreaseRate = 0.5f;
    [SerializeField] private float _currentPatience = 15f;

    // Components
    [Header("Components")]
    [SerializeField] private Vaccumable[] _vacuumables;
    [SerializeField] private TMP_Text _health_text;
    [SerializeField] private TMP_Text _patience_text;

    // Private Fields 
    public float _levelHealth = 0;
    public float _currentLevelHealth = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if( _vacuumables == null || _vacuumables.Length == 0) {
            SetVacuumablesList();   
        }

        // Set the values of all vacuumables in a level and calculate the total Level health on level and 
        ConfigureVacuumables();
        CalculateTotalLevelHealth();
        UpdateUI();
    }
    // Update is called once per frame
    void Update()
    {
        CalculateCurrentLevelHealth();
        UpdateUI();

        if (_currentLevelHealth <= 0.01f)
        {
            GameStateManager.Instance.ReturnToHub();
        }

        if (_currentPatience <= 0.01f)
        {
            GameStateManager.Instance.EndGame();
        }
    }
    
    private void CalculateCurrentLevelHealth()
    {
        _currentLevelHealth = 0;
        foreach (Vaccumable vaccumable in _vacuumables)
        {
            _currentLevelHealth += vaccumable.CurrentHealth;
        }
    }

    private void CalculateTotalLevelHealth()
    {
        _levelHealth = 0;
        foreach (Vaccumable vaccumable in _vacuumables)
        {
            _levelHealth += vaccumable.Health;
        }

        _currentLevelHealth = _levelHealth;
    }
    
    private void UpdateUI()
   {
        var _currentLevelHealthPercentage = _currentLevelHealth / _levelHealth;
        _health_text.text = _currentLevelHealthPercentage*100 + "%";
        _currentPatience -= _patienceDecreaseRate * Time.deltaTime;
        _patience_text.text = _currentPatience + "%";
   }

    private void SetVacuumablesList()

    {
        _vacuumables = GetComponentsInChildren<Vaccumable>();
    }

    private void ConfigureVacuumables()
    {
        if (_vacuumables.Length <= 0)
        {
            return;
        }

        foreach(Vaccumable vaccumable in _vacuumables.Where(v => v.IsConfiguredByLevle))
        {
            vaccumable.SetHealthAndResistance(_levelDifficulty,_levelDefense);
        }
    }
}
