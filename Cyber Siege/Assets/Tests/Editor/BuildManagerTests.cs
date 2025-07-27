using System;
using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BuildManagerTests
{
    private GameObject testGO;
    private BuildManager buildManager;
    private CurrencyManager currencyManager;
    private Tower testTower;
    private GameObject testTowerGO;
    private Tile testTile;

    [SetUp]
    public void Setup()
    {
        testGO = new GameObject("TestBuildManager");
        buildManager = testGO.AddComponent<BuildManager>();
        currencyManager = testGO.AddComponent<CurrencyManager>();
        CurrencyManager.main = currencyManager;

        // Create Test tower for Build Manager
        testTowerGO = new GameObject("TestTower");
        ScriptableTower testTowerSO = ScriptableObject.CreateInstance<ScriptableTower>();
        testTowerSO.cost = 100;
        testTowerSO.range = 1f;
        testTower = new Tower(testTowerSO, testTowerGO, null);
        buildManager.towers = new Tower[] { testTower };
        testTowerGO.AddComponent<BasicTowerScript>();

        currencyManager.Reset();
        currencyManager.IncreaseCurrency(100);

        testTile = testGO.AddComponent<MockTileScript>();
    }

    [TearDown]
    public void Teardown()
    {
        UnityEngine.Object.DestroyImmediate(testGO);
    }

    // SelectedTower Tests
    [Test]
    public void SetSelectedTower_GetSelectedTower_Correct()
    {
        buildManager.SetSelectedTower(0);
        Assert.AreEqual(testTower, buildManager.GetSelectedTower());
    }

    [Test]
    public void SetSelectedTower_NegativeIndex()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => buildManager.SetSelectedTower(-1));
    }

    [Test]
    public void SetSelectedTower_IndexOutOfRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => buildManager.SetSelectedTower(1));
    }

    [Test]
    public void GetSelectedTowerRange_Correct()
    {
        Assert.AreEqual(1f, buildManager.GetSelectedTower().towerSObj.range);
    }

    [Test]
    public void CanAffortSelectedTower_Correct()
    {
        Assert.IsTrue(buildManager.CanAffordSelectedTower());
    }

    [Test]
    public void BuySelectedTower_Correct()
    {
        int before = currencyManager.GetCurrency();
        buildManager.BuySelectedTower();
        int after = currencyManager.GetCurrency();
        Assert.AreEqual(before - 100, after);
    }

    [Test]
    public void BuySelectedTower_InsufficientCurrency()
    {
        currencyManager.Reset();
        currencyManager.IncreaseCurrency(50);
        int before = currencyManager.GetCurrency();
        buildManager.BuySelectedTower();
        int after = currencyManager.GetCurrency();
        Assert.AreEqual(before, after);
    }

    // EnableGroundBuilding Tests
    [Test]
    public void EnableGroundBuilding_Correct()
    {
        buildManager.EnablePathBuilding();
        buildManager.EnableGroundBuilding();
        // Use reflection to check private flags
        var isGroundBuilding = typeof(BuildManager).GetField("isGroundBuilding", BindingFlags.NonPublic | BindingFlags.Instance);
        var isPathBuilding = typeof(BuildManager).GetField("isPathBuilding", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.IsTrue((bool)isGroundBuilding.GetValue(buildManager));
        Assert.IsFalse((bool)isPathBuilding.GetValue(buildManager));
    }

    // EnablePathBuilding Tests
    [Test]
    public void EnablePathBuilding_DisablesGroundAndEnablesPath()
    {
        buildManager.EnableGroundBuilding();
        buildManager.EnablePathBuilding();
        // Use reflection to check private flags
        var isGroundBuilding = typeof(BuildManager).GetField("isGroundBuilding", BindingFlags.NonPublic | BindingFlags.Instance);
        var isPathBuilding = typeof(BuildManager).GetField("isPathBuilding", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.IsFalse((bool)isGroundBuilding.GetValue(buildManager));
        Assert.IsTrue((bool)isPathBuilding.GetValue(buildManager));
    }

    // DisableBuilding Tests
    [Test]
    public void DisableBuilding_ClearsSelectedTileAndBuildingState()
    {
        buildManager.EnableGroundBuilding();
        buildManager.SetSelectedTilePosition(Vector3.one);
        buildManager.DisableBuilding();
        Assert.IsFalse(buildManager.isTileSelected);
        Assert.IsFalse(buildManager.isBuilding());
    }

    // SelectedTowerToUpgrade Tests
    [Test]
    public void SetSelectedTowerToUpgrade_Correct()
    {
        buildManager.SetSelectedTowerToUpgrade(testTowerGO.GetComponent<BasicTowerScript>());
        Assert.AreEqual(testTowerGO.GetComponent<BasicTowerScript>(), buildManager.GetSelectedTowerToUpgrade());
    }

    // SellSelectedTower Tests
    [Test]
    public void SellSelectedTower_WithNullSelectedTile_Correct()
    {
        buildManager.SetSelectedTile(null);
        Assert.DoesNotThrow(() => buildManager.SellSelectedTower());
    }

    [Test]
    public void SellSelectedTower_Correct()
    {
        currencyManager.Reset();
        ((MockTileScript)testTile).SetMoneySpent(100);
        buildManager.SetSelectedTile(testTile);
        buildManager.SellSelectedTower();
        Assert.IsTrue(((MockTileScript)testTile).destroyTowerCalled);
        Assert.AreEqual(70, currencyManager.GetCurrency());
    }

    // Event Tests
    [Test]
    public void OnTowerSelectedForBuilding_InvokesEvent()
    {
        bool invoked = false;
        buildManager.onTowerSelectedForBuilding.AddListener(() => invoked = true);
        buildManager.SetSelectedTower(0);
        Assert.IsTrue(invoked);
    }

    [Test]
    public void OnStartGroundBuilding_InvokesEvent()
    {
        bool invoked = false;
        buildManager.onStartGroundBuilding.AddListener(() => invoked = true);
        buildManager.EnableGroundBuilding();
        Assert.IsTrue(invoked);
    }

    [Test]
    public void OnStartPathBuilding_InvokesEvent()
    {
        bool invoked = false;
        buildManager.onStartPathBuilding.AddListener(() => invoked = true);
        buildManager.EnablePathBuilding();
        Assert.IsTrue(invoked);
    }

    [Test]
    public void OnStopGroundBuilding_InvokesEvent()
    {
        bool invoked = false;
        buildManager.onStopGroundBuilding.AddListener(() => invoked = true);
        buildManager.EnableGroundBuilding();
        buildManager.DisableBuilding();
        Assert.IsTrue(invoked);
    }

    [Test]
    public void OnStopPathBuilding_InvokesEvent()
    {
        bool invoked = false;
        buildManager.onStopPathBuilding.AddListener(() => invoked = true);
        buildManager.EnablePathBuilding();
        buildManager.DisableBuilding();
        Assert.IsTrue(invoked);
    }

    [Test]
    public void OnTowerSelectedForUpgrading_InvokesEvent()
    {
        bool invoked = false;
        buildManager.onTowerSelectedForUpgrading.AddListener(() => invoked = true);
        buildManager.SetSelectedTowerToUpgrade(testTowerGO.GetComponent<BasicTowerScript>());
        Assert.IsTrue(invoked);
    }
}
