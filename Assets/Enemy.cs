using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(HealthComponent))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private HealthComponent enemyHealthComponent;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Player player;
    private bool isDead = false;


    public bool IsDead()
    {
        return isDead;
    }
    public void SetPlayer(Player playerRef)
    {
        player = playerRef;
    }

    [SerializeField] private float enemyVisibilityRange = 10f;
    private Transform[] navPoints;

    public Transform[] NavPoints
    {
        set => navPoints = value;
    }

    private bool isPlayerInRange;



    private bool isGameRunning;
    // Start is called before the first frame update
    void Start()
    {
        enemyHealthComponent.HandleHealthUpdated += UpdateHealth;
        enemyHealthComponent.HandleOnKilled += OnKilled;
        isGameRunning = true;
        healthBar.maxValue = enemyHealthComponent.Health;
        healthBar.value = enemyHealthComponent.Health;
        StartCoroutine(UpdateDestination());
        AssignHealthBarRotationConstraint();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AssignHealthBarRotationConstraint()
    {
        ConstraintSource rotationConstraintSource = new ConstraintSource();
        rotationConstraintSource.sourceTransform = Camera.main.transform;
        rotationConstraintSource.weight = 1;
        GetComponentInChildren<RotationConstraint>().SetSource(0,rotationConstraintSource);
    }

    private void UpdateHealth(float health)
    {
        healthBar.value = health;
    }

    private void OnKilled()
    {
        isDead = true;
        gameObject.SetActive(false);
    }

    private bool CheckPlayerInRange()
    {
        float distanceSquared = Mathf.Pow((transform.position.x - player.transform.position.x), 2) +
                                    Mathf.Pow((transform.position.z - player.transform.position.z), 2);
        if ( distanceSquared < Mathf.Pow(enemyVisibilityRange,2))
        {
            return true;
        }
        return false;
    }

    private IEnumerator UpdateDestination()
    {
        while (isGameRunning)
        {
            if (CheckPlayerInRange())
            {
                navMeshAgent.destination = player.transform.position;
                
            }
            else if(navMeshAgent.nextPosition == navMeshAgent.destination || !navMeshAgent.hasPath || navMeshAgent.isStopped)
            {
                navMeshAgent.destination = navPoints[Random.Range(0, navPoints.Length - 1)].position;
            }


            yield return new WaitForSeconds(2);
        }
    }
}
