using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(HealthComponent))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private HealthComponent enemyHealthComponent;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private GameObject navPointContainer;
    private List<Transform> navPoints;
    // Start is called before the first frame update
    void Start()
    {
        enemyHealthComponent.HandleHealthUpdated += UpdateHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateHealth(float health)
    {
        healthBar.value = health;
    }
}
