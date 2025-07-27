using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Reflection;

public class BasicEnemyScriptTests
{
    private GameObject testEnemyGO;
    private BasicEnemyScript testEnemy;
    private ScriptableEnemy testEnemySO;

    [SetUp]
    public void Setup()
    {
        testEnemyGO = new GameObject("TestEnemy");
        testEnemyGO.AddComponent<SpriteRenderer>();
        testEnemyGO.AddComponent<Rigidbody2D>();
        testEnemy = testEnemyGO.AddComponent<MockBasicEnemyScript>();

        testEnemySO = ScriptableObject.CreateInstance<ScriptableEnemy>();
        testEnemySO.name = "Test Enemy";
        testEnemySO.health = 100;
        testEnemySO.moveSpeed = 5f;
        testEnemySO.currencyValue = 25;
        testEnemySO.damageDealtToServer = 10;

        // Set private fields using reflection
        SetPrivateField(testEnemy, "enemy", testEnemySO);
        SetPrivateField(testEnemy, "slowEffect", new GameObject("SlowEffect"));
        SetPrivateField(testEnemy, "stunEffect", new GameObject("StunEffect"));

        // Invoke Start logic
        testEnemy.InitialiseEnemy();
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(testEnemyGO);
        Object.DestroyImmediate(testEnemySO);
    }

    // UpdateMovementSpeed Tests
    [Test]
    public void UpdateMovementSpeed_Correct()
    {
        testEnemy.UpdateMovementSpeed(0.5f);
        Assert.AreEqual(2.5f, GetPrivate<float>(testEnemy, "moveSpeed"));
        Assert.IsTrue(GetPrivate<bool>(testEnemy, "isSlowed"));
    }

    [Test]
    public void ResetMovementSpeed_Correct()
    {
        testEnemy.UpdateMovementSpeed(0.5f);
        testEnemy.ResetMovementSpeed();
        Assert.AreEqual(5f, GetPrivate<float>(testEnemy, "moveSpeed"));
        Assert.IsFalse(GetPrivate<bool>(testEnemy, "isSlowed"));
    }

    // TakeDamage Tests
    [Test]
    public void TakeDamage_Correct()
    {
        int initialVal = testEnemy.GetHealth();
        int damage = 20;
        testEnemy.TakeDamage(damage);
        Assert.AreEqual(initialVal - damage, testEnemy.GetHealth());
    }

    [Test]
    public void TakeDamage_WithMultiplier_Correct()
    {
        int initialVal = testEnemy.GetHealth();
        int damage = 20;
        float multiplier = 2f;
        testEnemy.SetTakenDamageMultiplier(multiplier);
        testEnemy.TakeDamage(damage);
        Assert.AreEqual(initialVal - (damage * multiplier), testEnemy.GetHealth());
    }

    [Test]
    public void TakeDamage_KillEnemy_Correct()
    {
        // This assumes DestroySelf would normally be called (but skipped in EditorMode)
        testEnemy.TakeDamage(100);
        Assert.LessOrEqual(testEnemy.GetHealth(), 0);
    }

    [Test]
    public void TakeDamage_KillEnemy_ExcessiveDamage()
    {
        // This assumes DestroySelf would normally be called (but skipped in EditorMode)
        testEnemy.TakeDamage(200);
        Assert.LessOrEqual(testEnemy.GetHealth(), 0);
    }

    // Event Tests
    [Test]
    public void OnTakeDamage_InvokesEvent()
    {
        bool invoked = false;
        testEnemy.onTakeDamage.AddListener(() => invoked = true);
        testEnemy.TakeDamage(1);
        Assert.IsTrue(invoked);
    }

    [Test]
    public void OnEnemyDeath_InvokesEvent()
    {
        bool invoked = false;
        BasicEnemyScript script = null;
        testEnemy.onEnemyDeath.AddListener((BasicEnemyScript e) =>
        {
            invoked = true;
            script = e;
        });
        testEnemy.TakeDamage(100);
        Assert.IsTrue(invoked);
        Assert.AreEqual(testEnemy, script);
    }

    [Test]
    public void OnEnemyReveal_InvokesEvent()
    {
        bool invoked = false;
        testEnemy.onEnemyReveal.AddListener(() => invoked = true);
        testEnemy.Hide();
        testEnemy.Reveal();
        Assert.IsTrue(invoked);
    }

    // Utility Functions
    private void SetPrivateField<T>(object obj, string fieldName, T value)
    {
        var field = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
        field?.SetValue(obj, value);
    }

    private T GetPrivate<T>(object obj, string fieldName)
    {
        var field = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
        return (T)field?.GetValue(obj);
    }
}
