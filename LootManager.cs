using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class Loot : ScriptableObject
{
    public GameObject loot;
    public string lootName;
    public int dropChance;

    public Loot (GameObject loot, string lootName, int dropChance)
    {
        this.loot = loot;
        this.lootName = lootName;
        this.dropChance = dropChance;
    }
}
