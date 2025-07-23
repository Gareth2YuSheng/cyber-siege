using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CurrencyManagerTests
{
    private GameObject testGO;
    private CurrencyManager currencyManager;

    [SetUp]
    public void Setup()
    {
        testGO = new GameObject("TestCurrencyManager");
        currencyManager = testGO.AddComponent<CurrencyManager>();
    }

    [TearDown]
    public void Teardown()
    {
        UnityEngine.Object.DestroyImmediate(testGO);
    }

    // Reset Tests - Do this first because rest of the test cases rely on this
    [Test]
    public void Reset_Correct()
    {
        currencyManager.Reset();
        Assert.AreEqual(0, currencyManager.GetCurrency());
    }

    // IncreaseCurrency Tests
    [Test]
    public void IncreaseCurrency_Correct()
    {
        int initialVal = currencyManager.GetCurrency();
        currencyManager.IncreaseCurrency(100);
        Assert.AreEqual(initialVal + 100, currencyManager.GetCurrency());
    }

    [Test]
    public void IncreaseCurrency_NegativeCurrency()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => currencyManager.IncreaseCurrency(-100));
    }

    // SpendCurrency Tests
    [Test]
    public void SpendCurrency_WithEnoughFunds()
    {
        int initialVal = currencyManager.GetCurrency();
        currencyManager.IncreaseCurrency(100);
        bool result = currencyManager.SpendCurrency(50);
        Assert.IsTrue(result);
        Assert.AreEqual(initialVal + 50, currencyManager.GetCurrency());
    }

    [Test]
    public void SpendCurrency_WithInsufficientFunds()
    {
        currencyManager.Reset();
        currencyManager.IncreaseCurrency(20);
        bool result = currencyManager.SpendCurrency(100);
        Assert.IsFalse(result);
        Assert.AreEqual(20, currencyManager.GetCurrency());
    }

    [Test]
    public void SpendCurrency_WithZeroFunds()
    {
        currencyManager.Reset();
        bool result = currencyManager.SpendCurrency(100);
        Assert.IsFalse(result);
        Assert.AreEqual(0, currencyManager.GetCurrency());
    }

    [Test]
    public void SpendCurrency_NegativeCurrency()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => currencyManager.SpendCurrency(-100));
    }

    // RMMultiplierStacks Tests
    [Test]
    public void IncreaseRMMultiplierStacks_Correct()
    {
        int initialVal = currencyManager.GetRMMultiplierStacks();
        currencyManager.IncreaseRMMultiplierStacks(2);
        Assert.AreEqual(initialVal + 2, currencyManager.GetRMMultiplierStacks());
    }

    [Test]
    public void IncreaseRMMultiplierStacks_NegativeCurrency()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => currencyManager.IncreaseRMMultiplierStacks(-1));
    }

    [Test]
    public void DecreaseRMMultiplierStacks_Correct()
    {
        currencyManager.Reset();
        currencyManager.IncreaseRMMultiplierStacks(4);
        currencyManager.DecreaseRMMultiplierStacks(2);
        Assert.AreEqual(2, currencyManager.GetRMMultiplierStacks());
    }

    [Test]
    public void DecreaseRMMultiplierStacks_DoesNotGoNegative()
    {
        currencyManager.Reset();
        currencyManager.Reset();
        currencyManager.IncreaseRMMultiplierStacks(4);
        currencyManager.DecreaseRMMultiplierStacks(5);
        Assert.AreEqual(0, currencyManager.GetRMMultiplierStacks());
    }

    [Test]
    public void DecreaseRMMultiplierStacks_DoesNotGoNegativeFromZero()
    {
        currencyManager.Reset();
        currencyManager.DecreaseRMMultiplierStacks(2);
        Assert.AreEqual(0, currencyManager.GetRMMultiplierStacks());
    }

    [Test]
    public void DecreaseRMMultiplierStacks_NegativeCurrency()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => currencyManager.DecreaseRMMultiplierStacks(-1));
    }

    // GainCurrencyFromKillingEnemy Tests
    [Test]
    public void GainCurrencyFromKillingEnemy_Correct()
    {
        int initialVal = currencyManager.GetCurrency();
        currencyManager.GainCurrencyFromKillingEnemy(100);
        Assert.AreEqual(initialVal + 100, currencyManager.GetCurrency());
    }

    [Test]
    public void GainCurrencyFromKillingEnemy_WithMultiplierStack_AppliesMultiplier()
    {
        currencyManager.IncreaseRMMultiplierStacks(2); // 1.1^2 = 1.21
        currencyManager.GainCurrencyFromKillingEnemy(100); // Expect ~121
        Assert.AreEqual(121, currencyManager.GetCurrency());
    }

    [Test]
    public void GainCurrencyFromKillingEnemy_NegativeCurrency()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => currencyManager.GainCurrencyFromKillingEnemy(-100));
    }

    // Decrease Currency Tests
    [Test]
    public void DecreaseCurrency_Correct()
    {
        currencyManager.Reset();
        currencyManager.IncreaseCurrency(100);
        currencyManager.DecreaseCurrency(50);
        Assert.AreEqual(50, currencyManager.GetCurrency());
    }

    public void DecreaseCurrency_FromZero()
    {
        currencyManager.Reset();
        currencyManager.DecreaseCurrency(50);
        Assert.AreEqual(0, currencyManager.GetCurrency());
    }

    [Test]
    public void DecreaseCurrency_NegativeCurrency()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => currencyManager.DecreaseCurrency(-100));
    }

    // Event Tests
    [Test]
    public void OnCurrencyChange_InvokesListeners()
    {
        bool wasCalled = false;
        currencyManager.onCurrencyChange.AddListener(() => wasCalled = true);

        currencyManager.IncreaseCurrency(10);
        Assert.IsTrue(wasCalled);
    }
}
