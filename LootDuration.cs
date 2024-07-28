using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LootDuration : MonoBehaviour
{
    private float timer;
    public float lootDuration;
    // Start is called before the first frame update
    void Awake()
    {
        timer = 0f;
        lootDuration = 15f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < lootDuration)
        {
            timer += Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
