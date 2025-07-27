using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class HealthManagerTests
{
    private GameObject testGO;
    private HealthManager healthManager;
    private ServerScript testServer;

    [SetUp]
    public void Setup()
    {
        testGO = new GameObject("TestHealthManager");
        healthManager = testGO.AddComponent<HealthManager>();

        // Mock the Server field for Sprite tests
        testServer = testGO.AddComponent<MockServerScript>();
        var serverField = typeof(HealthManager).GetField("myServer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        serverField.SetValue(healthManager, testServer);
    }

    [TearDown]
    public void Teardown()
    {
        UnityEngine.Object.DestroyImmediate(testGO);
    }

    // InitServerHealth Tests
    [Test]
    public void InitServerHealth_SetsHealthCorrectly()
    {
        healthManager.InitServerHealth(100);
        Assert.AreEqual(100, healthManager.GetServerHealth());
    }

    [Test]
    public void InitServerHealth_NegativeHealth()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => healthManager.InitServerHealth(-50));
    }

    // HealServer Tests
    [Test]
    public void HealServer_Correct()
    {
        healthManager.InitServerHealth(100);
        healthManager.DamageServer(50);
        healthManager.HealServer(20);
        Assert.AreEqual(70, healthManager.GetServerHealth());
    }

    [Test]
    public void HealServer_DoesNotExceedMaxHealth()
    {
        healthManager.InitServerHealth(100);
        healthManager.HealServer(200);
        Assert.AreEqual(100, healthManager.GetServerHealth());
    }

    [Test]
    public void HealServer_NegativeHealth()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => healthManager.HealServer(-30));
    }

    [Test]
    public void HealServer_TriggersHealthySpriteWhenHealedAboveHalfHealth()
    {
        healthManager.InitServerHealth(100);
        healthManager.DamageServer(60);
        ((MockServerScript)testServer).ResetFlags();
        healthManager.HealServer(20);
        Assert.IsTrue(((MockServerScript)testServer).healthySpriteCalled);
    }

    // DamageServer Tests
    [Test]
    public void DamageServer_Correct()
    {
        healthManager.InitServerHealth(100);
        healthManager.DamageServer(30);
        Assert.AreEqual(70, healthManager.GetServerHealth());
    }

    [Test]
    public void DamageServer_NegativeDamage()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => healthManager.DamageServer(-10));
    }

    [Test]
    public void DamageServer_TriggersDamagedSpriteWhenBelowHalfHealth()
    {
        healthManager.InitServerHealth(100);
        healthManager.DamageServer(60);
        Assert.IsTrue(((MockServerScript)testServer).damagedSpriteCalled);
    }

    // onServerDeath Event Tests
    [Test]
    public void DamageServer_KillsServer_TriggersOnDeath()
    {
        healthManager.InitServerHealth(100);
        bool wasCalled = false;
        healthManager.onServerDeath.AddListener(() => wasCalled = true);
        healthManager.DamageServer(100);
        Assert.IsTrue(wasCalled);
    }

    [Test]
    public void DamageServer_WhenDead_DoesNotInvokeAgain()
    {
        healthManager.InitServerHealth(100);
        healthManager.DamageServer(100);

        int callCount = 0;
        healthManager.onServerDeath.AddListener(() => callCount++);
        healthManager.DamageServer(50);
        // Should not call the event after server is already dead
        Assert.AreEqual(0, callCount);
    }

    // onHealthChange Event Tests
    [Test]
    public void OnHealthChange_InvokesOnDamage()
    {
        healthManager.InitServerHealth(100);
        bool wasCalled = false;
        healthManager.onHealthChange.AddListener(() => wasCalled = true);
        healthManager.DamageServer(10);
        Assert.IsTrue(wasCalled);
    }

    [Test]
    public void OnHealthChange_InvokesOnHeal()
    {
        healthManager.InitServerHealth(100);
        healthManager.DamageServer(20);
        bool wasCalled = false;
        healthManager.onHealthChange.AddListener(() => wasCalled = true);
        healthManager.HealServer(10);
        Assert.IsTrue(wasCalled);
    }
}
