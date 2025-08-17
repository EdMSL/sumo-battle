using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListOfLevels", menuName = "Scriptable Objects/ListOfLevels")]
public class ListOfLevels : ScriptableObject
{
    public List<LevelSO> Levels;
}
