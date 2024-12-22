using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private GameObject batPrefab; // Spawn edilecek yarasa
    [SerializeField] private Transform[] spawnPoints; // Spawn noktalarý
    [SerializeField] private int maxBats = 5; // Maksimum yarasa sayýsý
    private int currentBatCount = 0; // Þu anda sahnedeki yarasa sayýsý

    public void SpawnBats()
    {
        if (currentBatCount >= maxBats)
        {
            return; // Eðer maksimum sayýdaysak yeni yarasa spawn etme
        }

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            // Rastgele bir spawn noktasý seç
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Yarasa oluþtur
            Instantiate(batPrefab, randomSpawnPoint.position, Quaternion.identity);

            // Yarasalarý say
            currentBatCount++;

            // Eðer maksimum sayýya ulaþýrsak döngüyü kýr
            if (currentBatCount >= maxBats)
                break;
        }
    }
}
