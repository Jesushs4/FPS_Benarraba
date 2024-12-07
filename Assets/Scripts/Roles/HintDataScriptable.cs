using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "HintsList", menuName = "New conversations")]
public class HintDataScriptable : ScriptableObject
{
    public GameObject prefabHint;
    public List<string> hintList = new List<string>();
}
