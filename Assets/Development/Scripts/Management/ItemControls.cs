using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data" , fileName = "ItemData") ]
public class ItemControls : ScriptableObject
{
    public List<GameObject> singleMoveableObjects; 
    public List<GameObject> doubleMouveableObjects; 
    public List<GameObject> moreMoveableObjects; 
}
