using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class RMRandomSpawner : MonoBehaviour
{
    public GameObject enemyPrefabs;
    public GameObject[] spawnPoints;
    public GameObject portalPrefab;
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

        RaycastHit hit;
        if (Physics.Raycast(new Vector3(randomVec.x,randomVec.y + 50.0f,randomVec.z),Vector3.down, out hit,150.0f))
        {
            if (hit.collider.gameObject.GetComponent<Terrain>())
            {
                Instantiate(portalPrefab, new Vector3(randomVec.x, hit.point.y + portalPrefab.transform.localScale.y, randomVec.z),
                    spawnPoints[randomIntTwo].transform.rotation);
                Instantiate(enemyPrefabs, new Vector3(randomVec.x,hit.point.y,randomVec.z),spawnPoints[randomIntTwo].transform.rotation );
            }
            else
            {
                currentTime = 0f;
                count--;
            }
        }
        
    }
    
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, radius);
    }
}