using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Save
{
    public enum DifficultyLevel
    {
        Easy,
        Normal,
        Hard,
    }
    public static DifficultyLevel difficultyLevel = DifficultyLevel.Normal;

    public static void SetDifficultyLevel(DifficultyLevel level)
    {
        difficultyLevel = level;
    }
}
