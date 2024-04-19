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
    [SerializeField] private Player player;

    public void SetPlayer(Player playerRef)
    {
        player = playerRef;
    }

    [SerializeField] private float enemyVisibilityRange = 3f;
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
        isGameRunning = true;
        StartCoroutine(UpdateDestination());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateHealth(float health)
    {
        healthBar.value = health;
    }

    private bool CheckPlayerInRange()
    {
        float distance = Mathf.Sqrt(Mathf.Pow((transform.position.x - player.transform.position.x), 2) +
                                    Mathf.Pow((transform.position.z - player.transform.position.z), 2));
        if ( distance < enemyVisibilityRange)
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
            else if(navMeshAgent.nextPosition == navMeshAgent.destination || !navMeshAgent.hasPath )
            {
                navMeshAgent.destination = navPoints[Random.Range(0, navPoints.Length - 1)].position;
            }

            yield return new WaitForSeconds(3);
        }
    }
}
