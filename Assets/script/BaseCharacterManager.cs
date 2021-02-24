using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterManager : MonoBehaviour
{
    
    public string displayName;

    

    //[SerializeField]
    public List<ValueList> startEnd_x_y = new List<ValueList>();
}
[System.SerializableAttribute]
public class ValueList
{
    public List<float> List = new List<float>();
    public ValueList(List<float> list)
    {
        List = list;
    }
} 