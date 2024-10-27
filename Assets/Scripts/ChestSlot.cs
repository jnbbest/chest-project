using UnityEngine;
using UnityEngine.UI;

public class ChestSlot : MonoBehaviour
{
    public Text chestStateText;
    public Text timerText;
    public Button chestButton;
    public IChest chest;

    void Update()
    {
        if (chest != null && chest.State == ChestState.Unlocking)
        {
            int remainingMinutes = chest.GetRemainingMinutes();
            timerText.text = $"Unlocking in: {remainingMinutes} min";
            if (remainingMinutes <= 0)
            {
                chest.State = ChestState.UnlockedButNotCollected;
                UpdateUI();
            }
        }
    }

    public void AssignChest(IChest newChest)
    {
        chest = newChest;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (chest == null) return;

        chestStateText.text = chest.State.ToString();
        timerText.gameObject.SetActive(chest.State == ChestState.Unlocking);
        chestButton.interactable = chest.State == ChestState.Locked || chest.State == ChestState.Unlocking;
    }

    public void OnChestClick()
    {
        if (chest == null) return;

        if (chest.State == ChestState.Locked)
        {
            ChestManager.Instance.StartUnlocking(chest);
        }
        else if (chest.State == ChestState.Unlocking)
        {
            PopupManager.Instance.ShowImmediateUnlockPopup(chest);
        }
        else if (chest.State == ChestState.UnlockedButNotCollected)
        {
            chest.CollectReward();
            ChestManager.Instance.OnChestCollected(this);
        }
        
        UpdateUI();
    }
}

