using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListOfSkins", menuName = "Scriptable Objects/ListOfSkins")]
public class ListOfSkins : ScriptableObject
{
    public List<SkinSO> Skins;

}
