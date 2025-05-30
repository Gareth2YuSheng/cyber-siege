using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableTower", menuName = "Scriptable Objects/ScriptableTower")]
public class ScriptableTower : ScriptableObject
{
    public string towerName;
    public int cost;
    public float range;
    public float rotationSpeed;
    public float bps;
    public int baseUpgradeCost;
    public bool isRotatable;
}
