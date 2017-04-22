using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreHolder {

    public static int highscore = 0;

    public static bool HasHighScore()
    {
        return highscore > 0;
    }
}
