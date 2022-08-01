using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    private const float PLAYER_DISTANCE_SPAWN_LEVEL_PART = 50f;
    
    [SerializeField] private Transform levelPart_Start;
    [SerializeField] private List<Transform> levelPartList;
    [SerializeField] private Transform player;

    private Vector3 lastEndPosition;

    private void Awake()
    {
        lastEndPosition = levelPart_Start.Find("EndPosition").position;
        
        /*for (int i = 0; i < 5; i++)
        {
            SpawnLevelPart();
        }*/
        
        //Debug.Log(lastEndPosition);
    }

    private void Update()
    {
        if (Vector3.Distance(player.position, lastEndPosition) < PLAYER_DISTANCE_SPAWN_LEVEL_PART)
        {
            //Debug.Log("SPAWNLANDI");
            // Son spawnlanan bölümün sonuna yeterince yaklaştığında yenisi spawnlanır
            SpawnLevelPart();
        }
        //Debug.Log("aradaki fark = " + Vector3.Distance(player.GetPosition(), lastEndPosition));
        //Debug.Log("Oyuncu konumu= " + player.GetPosition() + " | Son konum= "+ lastEndPosition);
    }

    private void SpawnLevelPart()
    {
        Transform chosenLevelPart = levelPartList[Random.Range(0, levelPartList.Count)];
        Transform lastLevelPartTransform = SpawnLevelPart(chosenLevelPart, lastEndPosition);
        lastEndPosition = lastLevelPartTransform.Find("EndPosition").position;
    }
    
    private Transform SpawnLevelPart(Transform levelPart, Vector3 spawnPosition)
    {
        Transform levelPartTransform = Instantiate(levelPart, spawnPosition, Quaternion.identity);
        return levelPartTransform;
    }
}
