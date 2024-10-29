using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ChestData
{
    public string configName;
    public ChestState state;
    public long unlockEndTimeTicks;
}

[System.Serializable]
public class PlayerData
{
    public int coins;
    public int gems;
    public ChestData currentUnlockingChest;
    public List<ChestData> chestQueue = new List<ChestData>();
    public List<ChestData> chests = new List<ChestData>();
}

public class SaveSystem
{
    private static string savePath => Application.persistentDataPath + "/playerdata.json";
    private static PlayerData playerData = new PlayerData
    {
        coins = 0,
        gems = 0,
        currentUnlockingChest = null,
        chestQueue = new (),
        chests = new ()
    };
    
    // Save to file
    private static void Save()
    {
        string jsonData = JsonUtility.ToJson(playerData);
        File.WriteAllText(savePath, jsonData);
        Debug.Log("Game Saved: " + jsonData);
    }
    
    public static void SaveCurrency(int coins, int gems)
    {
        playerData.coins = coins;
        playerData.gems = gems;
        Save();
    }

    public static void SaveChestData(Chest currentUnlockingChest, LinkedList<Chest> chestQueue, List<Chest> chests)
    {
        playerData.currentUnlockingChest = currentUnlockingChest != null ? SerializeChest(currentUnlockingChest) : null;
        playerData.chestQueue = SerializeChestList(new List<Chest>(chestQueue));
        playerData.chests = SerializeChestList(chests);
        Save();
    }

    private static void Load()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("Save file not found.");
            return;
        }

        string jsonData = File.ReadAllText(savePath);
        playerData = JsonUtility.FromJson<PlayerData>(jsonData);

        Debug.Log("Game Loaded: " + jsonData);
    }

    public static void LoadCurrency(UnityAction<int, int> coinsAndGems)// (ref int coins, ref int gems)
    {
        Load();
        coinsAndGems(playerData.coins, playerData.gems);
    }

    public static void LoadChestData(ref Chest currentUnlockingChest, ref LinkedList<Chest> chestQueue, ref List<Chest> chests, List<ChestConfig> chestConfigs)
    {
        Load();
        currentUnlockingChest = DeserializeChest(playerData.currentUnlockingChest, chestConfigs);
        chestQueue = new LinkedList<Chest>(DeserializeChestList(playerData.chestQueue, chestConfigs));
        chests = DeserializeChestList(playerData.chests, chestConfigs);
    }

    private static ChestData SerializeChest(Chest chest)
    {
        Debug.Log($" --- chest.config.chestName {chest.config.chestName} chest.State {chest.State} chest.UnlockEndTime.Ticks {chest.UnlockEndTime.Ticks}");
        return new ChestData
        {
            configName = chest.config.chestName,
            state = chest.State,
            unlockEndTimeTicks = chest.UnlockEndTime.Ticks
        };
    }

    private static List<ChestData> SerializeChestList(List<Chest> chestList)
    {
        List<ChestData> serializedChests = new List<ChestData>();
        foreach (var chest in chestList)
        {
            serializedChests.Add(SerializeChest(chest));
            Debug.Log($" -- !! chest.config.chestName {serializedChests[serializedChests.Count-1].configName} chest.State {serializedChests[serializedChests.Count-1].state} chest.UnlockEndTime.Ticks {serializedChests[serializedChests.Count-1].unlockEndTimeTicks}");
        }
        return serializedChests;
    }

    private static Chest DeserializeChest(ChestData chestData, List<ChestConfig> chestConfigs)
    {
        if (chestData == null) return null;

        ChestConfig config = chestConfigs.Find(cfg => cfg.chestName == chestData.configName);
        if (config == null)
        {
            Debug.LogWarning($"Chest config '{chestData.configName}' not found.");
            return null;
        }

        return new Chest
        {
            config = config,
            State = chestData.state,
            UnlockEndTime = new DateTime(chestData.unlockEndTimeTicks)
        };
    }

    private static List<Chest> DeserializeChestList(List<ChestData> chestDataList, List<ChestConfig> chestConfigs)
    {
        List<Chest> deserializedChests = new List<Chest>();
        foreach (var chestData in chestDataList)
        {
            Chest chest = DeserializeChest(chestData, chestConfigs);
            if (chest != null)
            {
                deserializedChests.Add(chest);
            }
        }
        return deserializedChests;
    }
}