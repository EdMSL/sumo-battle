using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SkinSO", menuName = "Scriptable Objects/SkinSO")]
public class SkinSO : ScriptableObject
{
    public Material material;
    public Texture2D image;
}
