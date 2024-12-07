using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


//[ExecuteInEditMode]
[CustomEditor(typeof(CustomTerrain))]


public class CustomTerrainEditor : Editor
{

    //Declaración de variables para el editor
    SerializedProperty heightRandomRange;
    SerializedProperty texture;
    SerializedProperty textureScale;
    SerializedProperty inverted;


    //Foldouts
    bool showRandom = false;
    bool showLoadTexture = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }
        // Update is called once per frame
    void Update()
    {
        
    }

    /// Usamos el OnEnable porque queremos que funcione en 
    /// modo edición, y no sólo cuando pulsemos el play.
    /// Cada vez que activemos el objeto, se ejecutará el método
    private void OnEnable()
    {
        //Cargamos las propiedades que necesitamos para el editor
        heightRandomRange = serializedObject.FindProperty("heightRandomRange");
        texture = serializedObject.FindProperty("texture");
        textureScale = serializedObject.FindProperty("textureScale");
        inverted = serializedObject.FindProperty("inverted");

    }

    ///Este método nos da la posibilidad de diseñar nuestro propio
    ///elemento en el inspector e interaccionar con él
    public override void OnInspectorGUI()
    {
        //Cargamos todos los objetos serializados
        serializedObject.Update();
        //Vinculamos el terreno a un objeto (para acceder a sus métodos)
        CustomTerrain myTerrain = (CustomTerrain)target;


        //Sector Terreno Aleatorio
        showRandom = EditorGUILayout.Foldout(showRandom, "Random");
        if (showRandom)
        {
          
            EditorGUILayout.LabelField("Rango de alturas aleatorias", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(heightRandomRange);
            if (GUILayout.Button("Random Terrain"))
            {
                myTerrain.RandomTerrain();
            }
        }
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        //Sector Cargar textura
        showLoadTexture = EditorGUILayout.Foldout(showLoadTexture, "Load texture");
        if (showLoadTexture)
        {
           // EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.LabelField("Select an image", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(texture);
            EditorGUILayout.LabelField("Set the scale");
            EditorGUILayout.PropertyField(textureScale);
            EditorGUILayout.PropertyField(inverted);

            if (GUILayout.Button("Load texture"))
            {
                myTerrain.LoadTexture();
            }
        }

        //Sector Resetear Terreno
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        if (GUILayout.Button("Reset Terrain"))
        {
            myTerrain.ResetTerrain();
        }

        //Cargamos las modificaciones hechas de nuevo en el terreno
        serializedObject.ApplyModifiedProperties();
        
    }
}
