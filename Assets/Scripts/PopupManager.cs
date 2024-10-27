using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; }
    public GameObject unlockPopup;
    public Text costText;
    private IChest currentChest;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowImmediateUnlockPopup(IChest chest)
    {
        currentChest = chest;
        int unlockCost = chest.GetUnlockCost();
        costText.text = $"Unlock immediately for {unlockCost} Gems?";
        unlockPopup.SetActive(true);
    }

    public void OnConfirmUnlock()
    {
        int unlockCost = currentChest.GetUnlockCost();
        if (CurrencyManager.Instance.Gems >= unlockCost)
        {
            CurrencyManager.Instance.AddGems(-unlockCost);
            currentChest.CollectReward();
            ChestManager.Instance.OnChestCollected(null);
        }
        unlockPopup.SetActive(false);
    }

    public void OnCancelUnlock()
    {
        unlockPopup.SetActive(false);
    }
}
