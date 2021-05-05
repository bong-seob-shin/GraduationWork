using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class RandomSpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject[] spawnPoints;
   
    public float radius = 2;

    public float spawnTime = 10.0f;
    private float currentTime;
   
    private int randomInt;
    private int randomIntTwo;
    private Vector3 randomVec;

    public int count;
    private bool stopSpawn = false;

    private void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        currentTime = spawnTime;
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

            if (count >= 20)
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
        randomInt = GetRandom(enemyPrefabs.Length);
        randomIntTwo = GetRandom(spawnPoints.Length);
        randomVec = GetRandomVector(spawnPoints[randomIntTwo].transform.position);
        Instantiate(enemyPrefabs[randomInt], new Vector3(randomVec.x,transform.position.y ,randomVec.z), spawnPoints[randomIntTwo].transform.rotation);
    }
}