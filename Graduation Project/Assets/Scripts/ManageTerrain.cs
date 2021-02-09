using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

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


    void AddTerrain()
    {
        if (Selection.activeGameObject == null)
        {
            Debug.LogWarning("no terrain was selected");
            return;
        }

        if (Selection.activeGameObject.GetComponent<Terrain>() == null)
        {
            Debug.LogWarning("this object is not Terrain");
            return;
        }
        
        bool isTerrainInList = false;
        foreach (var gameObj in terrainObjs)
        {
            if (Selection.activeGameObject.name == gameObj.name)
            {
                Debug.LogWarning("already this terrain exit");
                isTerrainInList = true;
            }
        }

        if (!isTerrainInList)
        {
            terrainObjs.Add(Selection.activeGameObject);
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
        GUILayout.Space(10);
		
        if(GUILayout.Button("Add Terrain"))
        {			
            AddTerrain();							
        }
			
        GUILayout.Space(10);

        if(GUILayout.Button("Init Terrain"))
        {			
            terrainObjs.Clear();				
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
        if (terrainObjs.Count != 0)
        {
            GUILayout.Box("now Terrain :" + terrainObjs[index].name,GUILayout.Width(200));
        }
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Previos"))
        {
            if (index > 0)
            {
                index--;
            }
            SelectTerrain(index);
            Repaint();
        }
        
        GUILayout.Space(100);
        
        if (GUILayout.Button("Next"))
        {
            if (index < terrainObjs.Count-1)
            {
                index++;
            }
            SelectTerrain(index);
            Repaint();
        }



    }
}

