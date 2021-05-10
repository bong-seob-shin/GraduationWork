using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class CMRandomSpawner : MonoBehaviour
{
    public GameObject enemyPrefabs;
    public GameObject[] spawnPoints;
    public float radius = 2;

    public float spawnTime = 10.0f;
    private float currentTime;
   
    private int CrandomInt;
    private int randomIntTwo;
    private Vector3 randomVec;
    private Vector3 SpawnVec;

    private int count;
    public int maxCount;
    private bool stopSpawn = false;

    private void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("CSpawnPoint");
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
                SpawnRandom();
                currentTime = spawnTime;
                count++;
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
   
    void SpawnRandom()
    {
        randomIntTwo = GetRandom(spawnPoints.Length);
        randomVec = GetRandomVector(spawnPoints[randomIntTwo].transform.position);

        randomVec.y = randomVec.y + 50.0f;
        
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(randomVec.x,randomVec.y,randomVec.z),Vector3.down, out hit,150.0f))
        {
            if (hit.collider.gameObject.GetComponent<Terrain>())
            {
                TerrainData data = Terrain.activeTerrain.terrainData;
                float height = data.GetHeight((int) randomVec.x, (int) randomVec.y);
                if (randomVec.y >= height)
                {
                    Instantiate(enemyPrefabs, new Vector3(randomVec.x, randomVec.y, randomVec.z),
                        spawnPoints[randomIntTwo].transform.rotation);
                    
                }
                else
                {
                    count--;
                }

            }
        }
        
        
    }
}