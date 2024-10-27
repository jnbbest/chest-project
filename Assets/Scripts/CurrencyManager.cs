using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public UIManager uiManager;
    public static CurrencyManager Instance { get; private set; }
    public int Coins { get; private set; }
    public int Gems { get; private set; }

    void Awake()
    {
        uiManager = GetComponent<UIManager>();
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        uiManager.UpdateCurrencyUI();
    }

    public void AddGems(int amount)
    {
        Gems += amount;
        uiManager.UpdateCurrencyUI();
    }

    public bool SpendGems(int amount)
    {
        if (Gems >= amount)
        {
            Gems -= amount;
            uiManager.UpdateCurrencyUI();
            return true; 
        }
        return false; 
    }
}