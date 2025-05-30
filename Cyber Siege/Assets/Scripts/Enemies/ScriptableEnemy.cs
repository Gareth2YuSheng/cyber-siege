using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableEnemy", menuName = "Scriptable Objects/ScriptableEnemy")]
public class ScriptableEnemy : ScriptableObject
{
    public int health;
    public float moveSpeed;
    public int currencyValue;
    public int damageDealtToServer;
}
