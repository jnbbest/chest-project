using UnityEngine;
using System.Collections.Generic;

public class ChestManager : MonoBehaviour
{
    public static ChestManager Instance { get; private set; }
    public List<ChestConfig> chestConfigs;
    public List<Chest> chests = new List<Chest>();
    private LinkedList<Chest> chestQueue = new LinkedList<Chest>();
    private Chest currentUnlockingChest;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void GenerateChest()
    {
        if (chests.Count < 4) {
            var newChest = CreateRandomChest();
            chests.Add(newChest);
        }
    }

    public void StartUnlockingChest(Chest chest)
    {
        if (currentUnlockingChest == null)
        {
            currentUnlockingChest = chest;
            chest.StartUnlock();
        }
        else
        {
            Chest previousChest = new();
            if (chestQueue.Count > 0)
                previousChest = chestQueue.Last.Value;
            else
                previousChest = currentUnlockingChest;

            chestQueue.AddLast(chest);
            chest.AddtoQueue(previousChest.UnlockEndTime);
        }
    }

    public void OnChestUnlocked(Chest chest)
    {
        if (currentUnlockingChest == chest)
        {
            currentUnlockingChest = null; // Clear current chest

            // Start the next chest in the queue if available
            if (chestQueue.Count > 0)
            {
                var nextChest = chestQueue.First.Value;
                chestQueue.RemoveFirst();
                currentUnlockingChest = nextChest;
                nextChest.StartUnlock();
            }
        }
    }

    public void OnChestCollected(ChestSlot slot)
    {
        if (slot != null) slot.AssignChest(null);
    }

    public Chest CreateRandomChest()
    {
        int randomIndex = Random.Range(0, (chestConfigs.Count-1) * 10);
        ChestConfig selectedConfig = chestConfigs[randomIndex / 10];

        Chest newChest = new Chest
        {
            config = selectedConfig,
            State = ChestState.Locked
        };
        return newChest;
    }
}