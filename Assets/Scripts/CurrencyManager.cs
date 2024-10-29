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

        LoadCurrency();
    }

    public void LoadCurrency()
    {
        SaveSystem.LoadCurrency((coins, gems) => {Coins = coins; Gems = gems;});
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        uiManager.UpdateCurrencyUI(); 
        SaveSystem.SaveCurrency(Coins,Gems);
    }

    public void AddGems(int amount)
    {
        Gems += amount;
        uiManager.UpdateCurrencyUI();
        SaveSystem.SaveCurrency(Coins,Gems);
    }

    public bool SpendGems(int amount)
    {
        if (Gems >= amount)
        {
            Gems -= amount;
            uiManager.UpdateCurrencyUI();
            return true; 
        }
        SaveSystem.SaveCurrency(Coins,Gems);
        return false; 
    }
}