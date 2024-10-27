using UnityEngine;
using System.Collections.Generic;

public class ChestManager : MonoBehaviour
{
    public static ChestManager Instance { get; private set; }
    public List<IChest> chests = new List<IChest>();
    private Queue<IChest> chestQueue = new Queue<IChest>();
    public List<ChestConfig> chestConfigs;

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

    public void StartUnlocking(IChest chest)
    {
        if (chests.Exists(c => c.State == ChestState.Unlocking)) chestQueue.Enqueue(chest);
        else chest.StartUnlock();
    }

    public void OnChestCollected(ChestSlot slot)
    {
        if (slot != null) slot.AssignChest(null); // Clear slot if chest was collected

        if (chestQueue.Count > 0)
        {
            IChest nextChest = chestQueue.Dequeue();
            nextChest.StartUnlock();
        }
    }

    public IChest CreateRandomChest()
    {
        int randomIndex = Random.Range(0, chestConfigs.Count);
        ChestConfig selectedConfig = chestConfigs[randomIndex];

        Chest newChest = new Chest
        {
            config = selectedConfig,
            State = ChestState.Locked
        };
        return newChest;
    }

}