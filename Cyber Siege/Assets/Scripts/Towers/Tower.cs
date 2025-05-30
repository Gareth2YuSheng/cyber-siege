using System;
using UnityEngine;

[Serializable]
public class Tower
{
    public ScriptableTower towerSObj;
    public GameObject prefab;
    public Sprite sprite;

    public Tower(ScriptableTower _towerSObj, GameObject _prefab, Sprite _sprite)
    {
        towerSObj = _towerSObj;
        prefab = _prefab;
        sprite = _sprite;
    }
}
