using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSO", menuName = "Scriptable Objects/LevelSO")]
public class LevelSO : ScriptableObject
{
    // Доступно только в редакторе. В билде выдает ошибку.
    // public SceneAsset scene;
    public Texture2D image;
}
