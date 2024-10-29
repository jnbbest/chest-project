using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public ChestSlot[] chestSlots;
    public Text coinsText;
    public Text gemsText;

    private CurrencyManager currencyManager;

    private void Start()
    {
        currencyManager = GetComponent<CurrencyManager>();
    }

    public void GenerateRandomChest()
    {
        if (ChestManager.Instance.chests.Count < chestSlots.Length)
        {
            Chest newChest = ChestManager.Instance.CreateRandomChest();
            foreach (ChestSlot slot in chestSlots)
            {
                if (slot.chest == null)
                {
                    slot.AssignChest(newChest);
                    break;
                }
            }
        }
    }

    public void UpdateCurrencyUI()
    {
        coinsText.text = "Coins: " + currencyManager.Coins.ToString();
        gemsText.text = "Gems: " + currencyManager.Gems.ToString();
    }
}
