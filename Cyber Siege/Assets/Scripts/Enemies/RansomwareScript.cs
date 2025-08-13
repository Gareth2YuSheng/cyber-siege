using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RansomwareScript : BasicEnemyScript
{
    [Header("References")]
    // [SerializeField] private Transform enemyRangeTransform;
    [SerializeField] private LayerMask towerMask;
    [SerializeField] private GameObject attackPrefab;

    [Header("Attributes")]
    [SerializeField] private float range;
    [SerializeField] private float cooldown = 5f;

    protected Transform towerTarget;
    public bool hasAttemptedRemoval = false;
    private float timeUntilDisableTowers;

    // protected override void Start()
    // {
    //     base.Start();
    //     // UpdateEnemyRangeTransform();
    // }

    protected override void Update()
    {
        base.Update();

        timeUntilDisableTowers += Time.deltaTime;
        if (timeUntilDisableTowers >= cooldown)
        {
            Attack();
            timeUntilDisableTowers = 0f;
        }
    }

    protected virtual void OnMouseDown()
    {
        if (!hasAttemptedRemoval)
        {
            EnemyManager.main.SetSelectedRansomware(this);
        }
    }

    // private void UpdateEnemyRangeTransform()
    // {
    //     // Range (Radius) is to be multiplied by 2 as X, Y and Z are length variables.
    //     enemyRangeTransform.localScale = new Vector3(range / transform.localScale.x * 2, range / transform.localScale.y * 2, range / transform.localScale.z * 2);
    // }

    public void PromptPurchased()
    {
        hasAttemptedRemoval = true;
    }

    private void Attack()
    {
        Debug.Log("Ransomware Scanning For Towers");
        GameObject shockwave = Instantiate(attackPrefab, transform.position, Quaternion.identity);
        // Set the parent of the shockwave so it follows the ransomware
        shockwave.transform.SetParent(transform);
        RansomwareAttackScript shockwaveScript = shockwave.GetComponent<RansomwareAttackScript>();
        shockwaveScript.SetRansomwareScript(this);
        StartCoroutine(shockwaveScript.ExpandAndFade(range, 0.7f));
    }
}
