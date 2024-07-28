using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    public GameObject droppedLoot;
    public List<Loot> lootList = new List<Loot>();

    GameObject GetLoot()
    {
        int roll = Random.Range(1, 101);
        List<Loot> rolledItems = new List<Loot>();
        foreach (Loot item in lootList)
        {
            if (roll <= item.dropChance)
            {
                rolledItems.Add(item);
            }
        }
        if (rolledItems.Count > 0)
        {
            droppedLoot = rolledItems[Random.Range(0, rolledItems.Count)].loot;
            return droppedLoot;
        }
        return null;
    }

    public void InstantiateLoot(Vector3 spawnPosition)
    {
        droppedLoot = GetLoot();
        if (droppedLoot != null)
        {
            Instantiate(droppedLoot, spawnPosition, Quaternion.identity);
        }
    }
}
