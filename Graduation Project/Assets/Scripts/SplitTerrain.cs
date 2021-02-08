/// <summary>
/// Dvornik
/// </summary>
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

/// <summary>
/// Split terrain.
/// </summary>
public class SplitTerrain : EditorWindow {
		
	string num = "4";

	List<TerrainData> terrainData = new List<TerrainData>();
	List<GameObject> terrainGo = new List<GameObject>();
	
	Terrain parentTerrain;
	
	const int terrainsCount = 4;		
	
	// Add submenu
    [MenuItem("Terrain/Split")]
	static void Init()
    {
		// Get existing open window or if none, make a new one:
		SplitTerrain window = GetWindow<SplitTerrain>();
		
       	window.minSize =  new Vector2( 100f,100f );		
		window.maxSize =  new Vector2( 200f,200f );		
		
		window.autoRepaintOnSceneChange = true;
       	window.titleContent = new GUIContent("Split terrain");
       	window.Show();
	}
	
	/// <summary>
	/// Determines whether this instance is power of two the specified x.
	/// </summary>
	/// <returns>
	/// <c>true</c> if this instance is power of two the specified x; otherwise, <c>false</c>.
	/// </returns>
	/// <param name='x'>
	/// If set to <c>true</c> x.
	/// </param>
	bool IsPowerOfTwo(int x)
	{
    	return (x & (x - 1)) == 0;
	}
			
	void SplitIt()
	{
		
		if ( Selection.activeGameObject == null )
		{
			Debug.LogWarning("No terrain was selected");
			return;
		}

        var objects = Selection.objects;
        var currentObjectIndex = 0;

        foreach(var obj in objects)
        {
            currentObjectIndex++;

            var tObj = obj as GameObject;

            if (tObj == null)
                continue;

            parentTerrain = tObj.GetComponent<Terrain>() as Terrain;

            if (parentTerrain == null)
            {
                Debug.LogWarning("Current selection is not a terrain");
                return;
            }

            var progressCaption = "Spliting terrain " + parentTerrain.name + " (" + currentObjectIndex.ToString() + " of " + objects.Length.ToString() + ")";

            //Split terrain 
            for (int i = 0; i < terrainsCount; i++)
            {
                EditorUtility.DisplayProgressBar(progressCaption, "Process " + i, (float)i / terrainsCount);

                TerrainData td = new TerrainData();
                GameObject tgo = Terrain.CreateTerrainGameObject(td);

                tgo.name = parentTerrain.name + " " + i;

                terrainData.Add(td);
                terrainGo.Add(tgo);

                Terrain genTer = tgo.GetComponent(typeof(Terrain)) as Terrain;
                genTer.terrainData = td;

                AssetDatabase.CreateAsset(td, "Assets/" + genTer.name + ".asset");


                // Assign splatmaps
                genTer.terrainData.splatPrototypes = parentTerrain.terrainData.splatPrototypes;

                // Assign detail prototypes
                genTer.terrainData.detailPrototypes = parentTerrain.terrainData.detailPrototypes;

                // Assign tree information
                genTer.terrainData.treePrototypes = parentTerrain.terrainData.treePrototypes;


                // Copy parent terrain propeties
                #region parent properties
                genTer.basemapDistance = parentTerrain.basemapDistance;
                genTer.castShadows = parentTerrain.castShadows;
                genTer.detailObjectDensity = parentTerrain.detailObjectDensity;
                genTer.detailObjectDistance = parentTerrain.detailObjectDistance;
                genTer.heightmapMaximumLOD = parentTerrain.heightmapMaximumLOD;
                genTer.heightmapPixelError = parentTerrain.heightmapPixelError;
                genTer.treeBillboardDistance = parentTerrain.treeBillboardDistance;
                genTer.treeCrossFadeLength = parentTerrain.treeCrossFadeLength;
                genTer.treeDistance = parentTerrain.treeDistance;
                genTer.treeMaximumFullLODCount = parentTerrain.treeMaximumFullLODCount;

                #endregion

                //Start processing it			

                // Translate peace to position
                #region translate peace to right position 

                Vector3 parentPosition = parentTerrain.GetPosition();

                int terraPeaces = (int)Mathf.Sqrt(terrainsCount);

                float spaceShiftX = parentTerrain.terrainData.size.z / terraPeaces;
                float spaceShiftY = parentTerrain.terrainData.size.x / terraPeaces;

                float xWShift = (i % terraPeaces) * spaceShiftX;
                float zWShift = (i / terraPeaces) * spaceShiftY;

                tgo.transform.position = new Vector3(tgo.transform.position.x + zWShift,
                                                      tgo.transform.position.y,
                                                      tgo.transform.position.z + xWShift);

                // Shift last position
                tgo.transform.position = new Vector3(tgo.transform.position.x + parentPosition.x,
                                                      tgo.transform.position.y + parentPosition.y,
                                                      tgo.transform.position.z + parentPosition.z
                                                     );



                #endregion

                // Split height
                #region split height

                Debug.Log("Split height");

                //Copy heightmap											
                td.heightmapResolution = parentTerrain.terrainData.heightmapResolution / terraPeaces;

                //Keep y same
                td.size = new Vector3(parentTerrain.terrainData.size.x / terraPeaces,
                                       parentTerrain.terrainData.size.y,
                                       parentTerrain.terrainData.size.z / terraPeaces
                                      );

                float[,] parentHeight = parentTerrain.terrainData.GetHeights(0, 0, parentTerrain.terrainData.heightmapResolution, parentTerrain.terrainData.heightmapResolution);
                
               
                float[,] peaceHeight = new float[parentTerrain.terrainData.heightmapResolution / terraPeaces + 1,
                                                  parentTerrain.terrainData.heightmapResolution / terraPeaces + 1
                                                ];

                // Shift calc
                int heightShift = parentTerrain.terrainData.heightmapResolution / terraPeaces;

                int startX = 0;
                int startY = 0;

                int endX = 0;
                int endY = 0;

                
                    startX = startY = 0;
                    endX = endY = parentTerrain.terrainData.heightmapResolution / terraPeaces + 1;
             

               
                // iterate
                for (int x = startX; x < endX; x++)
                {

                    EditorUtility.DisplayProgressBar(progressCaption, "Split height", (float)x / (endX - startX));

                    for (int y = startY; y < endY; y++)
                    {

                        int xShift = 0;
                        int yShift = 0;

                        xShift = (heightShift/(terraPeaces-1)) * (i % terraPeaces);
                        yShift = (heightShift/(terraPeaces-1)) * (i / terraPeaces);
                        
                    
                        

                        float ph = parentHeight[x + xShift, y + yShift];
						
                        peaceHeight[x, y] = ph;

                    }

                }

                EditorUtility.ClearProgressBar();

                // Set heightmap to child
                genTer.terrainData.SetHeights(0, 0, peaceHeight);
                #endregion

                // Split splat map
                #region split splat map	

                td.alphamapResolution = parentTerrain.terrainData.alphamapResolution / terraPeaces;

                float[,,] parentSplat = parentTerrain.terrainData.GetAlphamaps(0, 0, parentTerrain.terrainData.alphamapResolution, parentTerrain.terrainData.alphamapResolution);

                float[,,] peaceSplat = new float[parentTerrain.terrainData.alphamapResolution / terraPeaces,
                                                  parentTerrain.terrainData.alphamapResolution / terraPeaces,
                                                  parentTerrain.terrainData.alphamapLayers
                                                ];

                // Shift calc
                int splatShift = parentTerrain.terrainData.alphamapResolution / terraPeaces;

              
                    startX = startY = 0;
                    endX = endY = parentTerrain.terrainData.alphamapResolution / terraPeaces;
               
                // iterate
                for (int s = 0; s < parentTerrain.terrainData.alphamapLayers; s++)
                {
                    for (int x = startX; x < endX; x++)
                    {

                        EditorUtility.DisplayProgressBar(progressCaption, "Processing splat...", (float)x / (endX - startX));

                        for (int y = startY; y < endY; y++)
                        {

                            int xShift = 0;
                            int yShift = 0;

                            xShift = (heightShift/(terraPeaces-1)) * (i % terraPeaces);
                            yShift = (heightShift/(terraPeaces-1)) * (i / terraPeaces);
                       

                            float ph = parentSplat[x + xShift, y + yShift, s];
                            peaceSplat[x, y, s] = ph;

                        }


                    }
                }

                EditorUtility.ClearProgressBar();

                // Set heightmap to child
                genTer.terrainData.SetAlphamaps(0, 0, peaceSplat);
                #endregion

                // Split detail map
                #region split detail map	

                td.SetDetailResolution(parentTerrain.terrainData.detailResolution / terraPeaces, 8);

                for (int detLay = 0; detLay < parentTerrain.terrainData.detailPrototypes.Length; detLay++)
                {
                    int[,] parentDetail = parentTerrain.terrainData.GetDetailLayer(0, 0, parentTerrain.terrainData.detailResolution, parentTerrain.terrainData.detailResolution, detLay);

                    int[,] peaceDetail = new int[parentTerrain.terrainData.detailResolution / terraPeaces,
                                                  parentTerrain.terrainData.detailResolution / terraPeaces
                                                ];

                    // Shift calc
                    int detailShift = parentTerrain.terrainData.detailResolution / terraPeaces;

                 
                        startX = startY = 0;
                        endX = endY = parentTerrain.terrainData.detailResolution / terraPeaces;
                 
                    // iterate				
                    for (int x = startX; x < endX; x++)
                    {

                        EditorUtility.DisplayProgressBar(progressCaption, "Processing detail...", (float)x / (endX - startX));

                        for (int y = startY; y < endY; y++)
                        {

                            int xShift = 0;
                            int yShift = 0;

                            xShift = (heightShift/(terraPeaces-1)) * (i % terraPeaces);
                            yShift = (heightShift/(terraPeaces-1)) * (i / terraPeaces);
                         

                            int ph = parentDetail[x + xShift, y + yShift];
                            peaceDetail[x, y] = ph;

                        }

                    }
                    EditorUtility.ClearProgressBar();

                    // Set heightmap to child
                    genTer.terrainData.SetDetailLayer(0, 0, detLay, peaceDetail);

                }
                #endregion

            

                AssetDatabase.SaveAssets();



            }

            EditorUtility.ClearProgressBar();


        }



    }
	
	void OnGUI()
    {
					
		if(GUILayout.Button("Split selected terrains"))
        {			
			SplitIt();							
		}
			
													
	}
	
		
	
}