using System.Collections;
using UnityEngine;

public class EndlessLevelManager : LevelManager
{
    protected override IEnumerator StartLevel()
    {
        yield break;
    }
}
