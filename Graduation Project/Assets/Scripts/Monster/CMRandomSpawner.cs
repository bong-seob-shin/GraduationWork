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

    public float CspawnTime = 10.0f;
    private float CcurrentTime;
   
    private int CCrandomInt;
    private int CrandomIntTwo;
    private Vector3 CrandomVec;
    private Vector3 CSpawnVec;

    public int Ccount;
    private bool CstopSpawn = false;

    public Terrain Cterrain;

    private void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("CSpawnPoint");
        CcurrentTime = CspawnTime;
    }

    private void Update()
    {
        if (!CstopSpawn)
        {
            CcurrentTime -= Time.deltaTime;
            if (CcurrentTime <= 0.0f)
            {
                SpawnRandom();
                CcurrentTime = CspawnTime;
                Ccount++;
            }

            if (Ccount >= 20)
            {
                CstopSpawn = true;
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
        CrandomIntTwo = GetRandom(spawnPoints.Length);
        CrandomVec = GetRandomVector(spawnPoints[CrandomIntTwo].transform.position);

        CrandomVec.y = transform.position.y + 50.0f;
       
        RaycastHit Chit;
        if (Physics.Raycast(new Vector3(CrandomVec.x,CrandomVec.y,CrandomVec.z),Vector3.down, out Chit,150.0f))
        {
            if (Chit.collider.gameObject.GetComponent<Terrain>())
            {
                TerrainData data = Cterrain.terrainData;
                float height = data.GetHeight((int) CrandomVec.x, (int) CrandomVec.y);
                Debug.Log(height);
                Instantiate(enemyPrefabs, new Vector3(CrandomVec.x,height,CrandomVec.z), spawnPoints[CrandomIntTwo].transform.rotation);
            }
        }
    }
}