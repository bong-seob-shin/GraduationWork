using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;
using Random = System.Random;

public class RMRandomSpawner : MonoBehaviour
{
    public GameObject enemyPrefabs;
    public GameObject[] spawnPoints;

    public float radius = 2;

    public float spawnTime = 10.0f;
    private float currentTime;
   
    private int randomInt;
    private int randomIntTwo;
    private Vector3 randomVec;
    private Vector3 SpawnVec;

    private int count;
    public int maxCount;
    private bool stopSpawn = false;

    private float height;
    
    private void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("RSpawnPoint");
        currentTime = spawnTime;

        count = 0;
    }

    private void Update()
    {
        if (!stopSpawn)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0.0f)
            {
                randomIntTwo = GetRandom(spawnPoints.Length);
                randomVec = GetRandomVector(spawnPoints[randomIntTwo].transform.position);

                height = Terrain.activeTerrain.SampleHeight(randomVec);
                
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(randomVec.x,height,randomVec.z),Vector3.down, out hit,150.0f))
                {
                    if (hit.transform.CompareTag("Terrain"))
                    {
                        SpawnRandom(height);
                        currentTime = spawnTime;
                        count++;
                    }
                }
            }

            if (count >= maxCount)
            {
                stopSpawn = true;
            }
        }
    }
    
    
    int GetRandom(int count)
    {
        return UnityEngine.Random.Range(0, count);
    }

    Vector3 GetRandomVector(Vector3 vec)
    {
        return (UnityEngine.Random.insideUnitSphere * radius) + vec;
    }
   
    void SpawnRandom(float height)
    {
        Debug.Log(height);
        Instantiate(enemyPrefabs, new Vector3(randomVec.x,height,randomVec.z),spawnPoints[randomIntTwo].transform.rotation );
    }
}