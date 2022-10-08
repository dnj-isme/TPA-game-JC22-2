using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private bool spawned = true;
    private float lastDespawned = 0;
    [SerializeField, Range(1, 5)] private int floatCount = 3;
    [SerializeField, Range(0, 25)] private float spawnRange = 15;
    private ObjectPool instance;
    [SerializeField, Range(2, 10)] private float spawnCooldown = 5f;

    private void Start()
    {
        instance = ObjectPool.Instance;
    }
    private void Update()
    {
        if (instance == null) return;
        if (transform.childCount < floatCount)
        {
            if(spawned)
            {
                spawned = false;
                lastDespawned = Time.time;
            }

            if(!spawned && Time.time - lastDespawned >= spawnCooldown)
            {
                GameObject enemy = instance.GetPooledObject();
                if (enemy != null)
                {
                    Vector3 randomPosition = new Vector3(Random.value * spawnRange, 0, Random.value * spawnRange);
                    enemy.GetComponent<EnemyStats>().ResetStat().RefreshSlider();
                    enemy.GetComponent<EnemyInteraction>().RemoveTarget();

                    enemy.transform.parent = transform;
                    enemy.transform.position = transform.position + randomPosition;
                    enemy.SetActive(true);
                    spawned = true;
                }
            }
        }
    }
}
