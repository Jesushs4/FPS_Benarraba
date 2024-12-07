using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[CreateAssetMenu(fileName = "Conversations", menuName = "New conversations")]
public class ConversationScriptableObject : ScriptableObject
{
    public GameObject prefab;
    public Dictionary<string, List<string> > hintsDict = new Dictionary<string, List<string> >();
}
