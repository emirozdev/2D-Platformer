using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private GameObject batPrefab; // Spawn edilecek yarasa
    [SerializeField] private Transform[] spawnPoints; // Spawn noktalar�
    [SerializeField] private int maxBats = 5; // Maksimum yarasa say�s�
    private int currentBatCount = 0; // �u anda sahnedeki yarasa say�s�

    public void SpawnBats()
    {
        if (currentBatCount >= maxBats)
        {
            return; // E�er maksimum say�daysak yeni yarasa spawn etme
        }

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            // Rastgele bir spawn noktas� se�
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Yarasa olu�tur
            Instantiate(batPrefab, randomSpawnPoint.position, Quaternion.identity);

            // Yarasalar� say
            currentBatCount++;

            // E�er maksimum say�ya ula��rsak d�ng�y� k�r
            if (currentBatCount >= maxBats)
                break;
        }
    }
}
