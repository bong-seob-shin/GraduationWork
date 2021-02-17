using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using System;

public class ManageTerrain : EditorWindow
{
    List<GameObject> terrainObjs = new List<GameObject>();
    private int index = 0;
        
    [MenuItem("Terrain/Manage")]
    static void TerrainManage()
    {
        ManageTerrain window1 = GetWindow<ManageTerrain>();
		
        window1.minSize =  new Vector2( 100f,100f );		
        window1.maxSize =  new Vector2( 200f,500f );	
		
        window1.autoRepaintOnSceneChange = true;
        window1.titleContent = new GUIContent("Manage Terrain");
        window1.Show();
    }


    // void AddTerrain()
    // {
    //     if (Selection.activeGameObject == null)
    //     {
    //         Debug.LogWarning("no terrain was selected");
    //         return;
    //     }
    //
    //     if (Selection.activeGameObject.GetComponent<Terrain>() == null)
    //     {
    //         Debug.LogWarning("this object is not Terrain");
    //         return;
    //     }
    //     
    //     bool isTerrainInList = false;
    //     foreach (var gameObj in terrainObjs)
    //     {
    //         if (Selection.activeGameObject.name == gameObj.name)
    //         {
    //             Debug.LogWarning("already this terrain exit");
    //             isTerrainInList = true;
    //         }
    //     }
    //
    //     if (!isTerrainInList)
    //     {
    //         terrainObjs.Add(Selection.activeGameObject);
    //     }
    //     
    // }

    
    void AddTerrains()
    {
        if (terrainObjs.Count == 0)
        {
            GameObject[] gObjs = Selection.gameObjects;

            foreach (var gObj in gObjs)
            {
                terrainObjs.Add(gObj);
            }
            terrainObjs.Sort(delegate(GameObject p_gobj, GameObject n_gobj)
            {
                if (String.Compare(p_gobj.name, n_gobj.name) == 1)
                {
                    return 1;
                }

                if (String.Compare(p_gobj.name, n_gobj.name) == -1)
                {
                    return -1;
                }
                else return 0;
            });
        }
        
    }
    void SelectTerrain(int index)
    {
        foreach (var gameobj in terrainObjs)
        {
            gameobj.SetActive(false);
        }
        terrainObjs[index].SetActive(true);
    }
    void OnGUI()
    {
        //GUILayout.Space(10);
		
        // if(GUILayout.Button("Add Terrain"))
        // {			
        //     AddTerrain();							
        // }
        
        GUILayout.Space(10);
		
        if(GUILayout.Button("Add Terrains"))
        {			
            AddTerrains();							
        }
        GUILayout.Space(10);

        if(GUILayout.Button("Init Terrain"))
        {			
            terrainObjs.Clear();				
            terrainObjs = new List<GameObject>();
        }
        
        GUILayout.Space(10);

        if(GUILayout.Button("Show All Terrains"))
        {
            Debug.Log(terrainObjs.Count);
            foreach (var gameObj in terrainObjs)
            {
                gameObj.SetActive(true);
            }
        }
        
        GUILayout.Space(10);
        
        if(GUILayout.Button("hide All Terrains"))
        {
            Debug.Log(terrainObjs.Count);
            foreach (var gameObj in terrainObjs)
            {
                gameObj.SetActive(false);
            }
        }
    
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("0"))
        {
            if (terrainObjs[0].activeSelf)
            {
                terrainObjs[0].SetActive(false);
            }
            else if (!terrainObjs[0].activeSelf)
            {
                terrainObjs[0].SetActive(true);
            }

        }
       
        if (GUILayout.Button("1"))
        {
            if (terrainObjs[1].activeSelf)
            {
                terrainObjs[1].SetActive(false);
            }
            else if (!terrainObjs[1].activeSelf)
            {
                terrainObjs[1].SetActive(true);
            }
        }

        if (GUILayout.Button("2"))
        {
            if (terrainObjs[2].activeSelf)
            {
                terrainObjs[2].SetActive(false);
            }
            else if (!terrainObjs[2].activeSelf)
            {
                terrainObjs[2].SetActive(true);
            }
        }

        if (GUILayout.Button("3"))
        {
            if (terrainObjs[3].activeSelf)
            {
                terrainObjs[3].SetActive(false);
            }
            else if (!terrainObjs[3].activeSelf)
            {
                terrainObjs[3].SetActive(true);
            }
        }
        GUILayout.EndHorizontal();
        

        GUILayout.Space(10);
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("4"))
        {
            if (terrainObjs[4].activeSelf)
            {
                terrainObjs[4].SetActive(false);
            }
            else if (!terrainObjs[4].activeSelf)
            {
                terrainObjs[4].SetActive(true);
            }
        }

        if (GUILayout.Button("5"))
        {
            if (terrainObjs[5].activeSelf)
            {
                terrainObjs[5].SetActive(false);
            }
            else if (!terrainObjs[5].activeSelf)
            {
                terrainObjs[5].SetActive(true);
            }
        }

        if (GUILayout.Button("6"))
        { 
            if (terrainObjs[6].activeSelf)
            {
                terrainObjs[6].SetActive(false);
            }
            else if (!terrainObjs[6].activeSelf)
            {
                terrainObjs[6].SetActive(true);
            }
        }

        if (GUILayout.Button("7"))
        {
            if (terrainObjs[7].activeSelf)
            {
                terrainObjs[7].SetActive(false);
            }
            else if (!terrainObjs[7].activeSelf)
            {
                terrainObjs[7].SetActive(true);
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("8"))
        {
            if (terrainObjs[8].activeSelf)
            {
                terrainObjs[8].SetActive(false);
            }
            else if (!terrainObjs[8].activeSelf)
            {
                terrainObjs[8].SetActive(true);
            }
        }
       
        if (GUILayout.Button("9"))
        {
            if (terrainObjs[9].activeSelf)
            {
                terrainObjs[9].SetActive(false);
            }
            else if (!terrainObjs[9].activeSelf)
            {
                terrainObjs[9].SetActive(true);
            }
        }

        if (GUILayout.Button("10"))
        {
            if (terrainObjs[10].activeSelf)
            {
                terrainObjs[10].SetActive(false);
            }
            else if (!terrainObjs[10].activeSelf)
            {
                terrainObjs[10].SetActive(true);
            }
        }

        if (GUILayout.Button("11"))
        {
            if (terrainObjs[11].activeSelf)
            {
                terrainObjs[11].SetActive(false);
            }
            else if (!terrainObjs[11].activeSelf)
            {
                terrainObjs[11].SetActive(true);
            }
        }
        GUILayout.EndHorizontal();
        

        GUILayout.Space(10);
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("12"))
        {
            if (terrainObjs[12].activeSelf)
            {
                terrainObjs[12].SetActive(false);
            }
            else if (!terrainObjs[12].activeSelf)
            {
                terrainObjs[12].SetActive(true);
            }
        }

        if (GUILayout.Button("13"))
        {
            if (terrainObjs[13].activeSelf)
            {
                terrainObjs[13].SetActive(false);
            }
            else if (!terrainObjs[13].activeSelf)
            {
                terrainObjs[13].SetActive(true);
            }
        }

        if (GUILayout.Button("14"))
        {
            if (terrainObjs[14].activeSelf)
            {
                terrainObjs[14].SetActive(false);
            }
            else if (!terrainObjs[14].activeSelf)
            {
                terrainObjs[14].SetActive(true);
            }
        }

        if (GUILayout.Button("15"))
        {
            if (terrainObjs[15].activeSelf)
            {
                terrainObjs[15].SetActive(false);
            }
            else if (!terrainObjs[15].activeSelf)
            {
                terrainObjs[15].SetActive(true);
            }
        }
        GUILayout.EndHorizontal();
    }
}

