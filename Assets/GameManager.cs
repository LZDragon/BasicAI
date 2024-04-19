using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject navPointContainer;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int numberOfEnemies = 4;
    [SerializeField] private Player player;
    private Transform[] navPoints;

    // Start is called before the first frame update
    void Start()
    {
        PopulateNavPointsList();
        GenerateEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 randomNavPosition = navPoints[Random.Range(0, navPoints.Length - 1)].position;
            GameObject generatedObject = Instantiate(enemyPrefab, randomNavPosition, Quaternion.identity);
            Enemy generatedEnemy = generatedObject.GetComponent<Enemy>();
            generatedEnemy.NavPoints = navPoints;
            generatedEnemy.SetPlayer(player);
        }
    }

    private void PopulateNavPointsList()
    {
        navPoints = navPointContainer.GetComponentsInChildren<Transform>();
        if (navPoints.Length < 1)
        {
            Debug.LogError("Not enough NavPoints in NavPointsContainer");
        }
    }
}
