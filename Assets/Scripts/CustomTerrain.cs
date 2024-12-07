using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CustomTerrain : MonoBehaviour
{

    public Terrain myTerrain;
    public TerrainData myTerrainData;

    public Vector2 heightRandomRange = new Vector2(0.0f, 0.1f);
    public Texture2D texture;
    public Vector3 textureScale = new Vector3(1.0f, 1.0f, 1.0f);

    public bool inverted = false;

    /// <summary>
    /// Procedimiento que se lanza al cargar el objeto
    /// Asocia los gameObjects a las variables
    /// </summary>
    private void OnEnable()
    {
        Debug.Log("Inicializando terreno");

        myTerrain = this.GetComponent<Terrain>();
        myTerrainData = Terrain.activeTerrain.terrainData;

    }


    /// <summary>
    /// Genera un terreno aleatorio, para ello, crea una matriz de flotantes
    /// que contendrá valores aleatorios entre el valor mínimo y máximo 
    /// que hayamos indicado en heightRandomRange
    /// </summary>
    public void RandomTerrain()
    {
        //Crear una matriz de alturas
        //matriz cargada de valores que ya había en el terreno
        float[,] heightMap = myTerrainData.GetHeights(0, 0, myTerrainData.heightmapResolution, myTerrainData.heightmapResolution);

        //Generación aleatoria con bucles de valores para las alturas
        for (int x = 0; x < myTerrainData.heightmapResolution; x++)
        {
            for (int z = 0; z < myTerrainData.heightmapResolution; z++)
            {
                heightMap[x, z] += (float)Random.Range(heightRandomRange.x, heightRandomRange.y);
            }
        }

        //Asignar alturas a myTerrainData
        myTerrainData.SetHeights(0, 0, heightMap);
    }

    /// <summary>
    /// Resetea el terreno a valores 0 en toda la matriz de alturas
    /// </summary>
    public void ResetTerrain()
    {
        //matriz inicializada a cero
        float[,] heightMap = new float[myTerrainData.heightmapResolution, myTerrainData.heightmapResolution];

        for (int x = 0; x < myTerrainData.heightmapResolution; x++)
        {
            for (int z = 0; z < myTerrainData.heightmapResolution; z++)
            {
                heightMap[x, z] = 0.0f;
            }
        }

        //Asignar alturas a myTerrainData
        myTerrainData.SetHeights(0, 0, heightMap);
    }

    /// <summary>
    /// Este método permite cargar una imagen y usarla como mapa de alturas
    /// Para ello, coge cada pixel de la imagen y lo escala a gris para 
    /// obtener valores entre 0 y 1 (perfecto para nuestra matriz de alturas).
    /// Se ha incluido un vector3 que nos permita escalar cada dimensión de la imagen
    /// con el objetivo de regular la resolución de la textura utilizada
    /// </summary>
    public void LoadTexture()
    {
        //matriz inicializada a cero
        float[,] heightMap = new float[myTerrainData.heightmapResolution, myTerrainData.heightmapResolution];

        //Generación aleatoria con bucles de valores para las alturas
        for (int x = 0; x < myTerrainData.heightmapResolution; x++)
        {
            for (int z = 0; z < myTerrainData.heightmapResolution; z++)
            {
                if (inverted)
                {
                    heightMap[x, z] = (1.0f - texture.GetPixel((int)(z * textureScale.z), (int)(x * textureScale.x)).grayscale) * textureScale.y;
                }
                else
                {
                    heightMap[x, z] = texture.GetPixel((int)(z * textureScale.z), (int)(x * textureScale.x)).grayscale * textureScale.y;
                }
            }
        }
        //Asignar alturas a myTerrainData
        myTerrainData.SetHeights(0, 0, heightMap);
    }
}
