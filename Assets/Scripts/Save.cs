using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Save
{
    public static string difficultyLevel = "Normal";

    public static void SetDifficultyLevel(string level)
    {
        difficultyLevel = level;
    }
}
