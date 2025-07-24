using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;

public class ServerManagerTests
{
    private GameObject testGO;
    private ServerManager serverManager;

    [SetUp]
    public void Setup()
    {
        testGO = new GameObject("TestServerManager");
        serverManager = testGO.AddComponent<ServerManager>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(testGO);
    }

    // AttachSpyware Tests
    [Test]
    public void AttachSpyware_AttachNormalSpyware_Correct()
    {
        // Setup
        GameObject testSpywareGO = new GameObject("TestSpyware");
        MockSpywareEnemyScript testSpywareScript = testSpywareGO.AddComponent<MockSpywareEnemyScript>();
        testSpywareScript.SetEnemyName("Spyware");
        // Call Func
        serverManager.AttachSpyware(testSpywareScript);
        // Use Reflection to access the Queue
        var queueField = typeof(ServerManager).GetField("attachedSpywareEnemies", BindingFlags.NonPublic | BindingFlags.Instance);
        var queue = queueField.GetValue(serverManager) as Queue<SpywareEnemyScript>;
        // Test
        Assert.IsNotNull(queue);
        Assert.AreEqual(1, queue.Count);
        Assert.AreEqual(testSpywareScript, queue.Peek());
        Assert.AreEqual(0, serverManager.GetCryptojackingCount());
        Assert.IsTrue(serverManager.HasSpywareAttached());
    }

    [Test]
    public void AttachSpyware_AttachCryptojacking_Correct()
    {
        // Setup
        GameObject testSpywareGO = new GameObject("TestSpyware");
        MockSpywareEnemyScript testSpyware = testSpywareGO.AddComponent<MockSpywareEnemyScript>();
        testSpyware.SetEnemyName("Cryptojacking");
        // Call Func
        serverManager.AttachSpyware(testSpyware);
        // Use Reflection to access the Queue
        var queueField = typeof(ServerManager).GetField("attachedSpywareEnemies", BindingFlags.NonPublic | BindingFlags.Instance);
        var queue = queueField.GetValue(serverManager) as Queue<SpywareEnemyScript>;
        // Test
        Assert.IsNotNull(queue);
        Assert.AreEqual(1, queue.Count);
        Assert.AreEqual(testSpyware, queue.Peek());
        Assert.AreEqual(1, serverManager.GetCryptojackingCount());
        Assert.IsTrue(serverManager.HasSpywareAttached());
    }

    // PurgeFirstSpyware Tests
    [Test]
    public void PurgeFirstSpyware_RemovesNormalSpyware_Correct()
    {
        // Setup
        GameObject testSpywareGO = new GameObject("TestSpyware");
        MockSpywareEnemyScript testSpyware = testSpywareGO.AddComponent<MockSpywareEnemyScript>();
        testSpyware.SetEnemyName("Cryptojacking");
        serverManager.AttachSpyware(testSpyware);
        // Call Func
        serverManager.PurgeFirstSpyware();
        // Use Reflection to access the Queue
        var queueField = typeof(ServerManager).GetField("attachedSpywareEnemies", BindingFlags.NonPublic | BindingFlags.Instance);
        var queue = queueField.GetValue(serverManager) as Queue<SpywareEnemyScript>;
        // Test
        Assert.IsNotNull(queue);
        Assert.AreEqual(0, queue.Count);
        Assert.IsTrue(testSpyware.destroyCalled);
        Assert.AreEqual(0, serverManager.GetCryptojackingCount()); //Still include to check any edge cases
        Assert.IsFalse(serverManager.HasSpywareAttached());
    }

    [Test]
    public void PurgeFirstSpyware_RemovesCryptojacking_Correct()
    {
        // Setup
        GameObject testSpywareGO = new GameObject("TestSpyware");
        MockSpywareEnemyScript testSpyware = testSpywareGO.AddComponent<MockSpywareEnemyScript>();
        testSpyware.SetEnemyName("Cryptojacking");
        serverManager.AttachSpyware(testSpyware);
        // Call Func
        serverManager.PurgeFirstSpyware();
        // Use Reflection to access the Queue
        var queueField = typeof(ServerManager).GetField("attachedSpywareEnemies", BindingFlags.NonPublic | BindingFlags.Instance);
        var queue = queueField.GetValue(serverManager) as Queue<SpywareEnemyScript>;
        // Test
        Assert.IsNotNull(queue);
        Assert.AreEqual(0, queue.Count);
        Assert.IsTrue(testSpyware.destroyCalled);
        Assert.AreEqual(0, serverManager.GetCryptojackingCount());
        Assert.IsFalse(serverManager.HasSpywareAttached());
    }

    // Test Queue is working properly
    [Test]
    public void Queue_WorkingCorrectly()
    {
        // Setup
        GameObject testSpywareGO1 = new GameObject("TestSpyware1");
        MockSpywareEnemyScript testSpyware1 = testSpywareGO1.AddComponent<MockSpywareEnemyScript>();
        testSpyware1.SetEnemyName("Cryptojacking");
        serverManager.AttachSpyware(testSpyware1);
        GameObject testSpywareGO2 = new GameObject("TestSpyware2");
        MockSpywareEnemyScript testSpyware2 = testSpywareGO2.AddComponent<MockSpywareEnemyScript>();
        testSpyware2.SetEnemyName("Spyware");
        serverManager.AttachSpyware(testSpyware2);
        GameObject testSpywareGO3 = new GameObject("TestSpyware3");
        MockSpywareEnemyScript testSpyware3 = testSpywareGO3.AddComponent<MockSpywareEnemyScript>();
        testSpyware3.SetEnemyName("Cryptojacking");
        serverManager.AttachSpyware(testSpyware3);
        // Call Func
        serverManager.PurgeFirstSpyware();
        // Use Reflection to access the Queue
        var queueField = typeof(ServerManager).GetField("attachedSpywareEnemies", BindingFlags.NonPublic | BindingFlags.Instance);
        var queue = queueField.GetValue(serverManager) as Queue<SpywareEnemyScript>;
        // Test
        Assert.IsNotNull(queue);
        Assert.AreEqual(2, queue.Count);
        Assert.IsTrue(testSpyware1.destroyCalled);
        Assert.AreEqual(1, serverManager.GetCryptojackingCount());
        Assert.IsTrue(serverManager.HasSpywareAttached());
    }
}
