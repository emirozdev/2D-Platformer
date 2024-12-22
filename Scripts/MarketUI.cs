using UnityEngine;
using UnityEngine.UI;

public class MarketUI : MonoBehaviour
{
    [SerializeField] private Button buyHeartButton; // Market sahnesindeki buton
    private GameSession gameSession;

    private void Start()
    {
        // Sahne y�klendi�inde GameSession'� bul ve butonu ba�la
        gameSession = FindObjectOfType<GameSession>();
        if (gameSession != null && buyHeartButton != null)
        {
            buyHeartButton.onClick.RemoveAllListeners(); // Eski referanslar� temizle
            buyHeartButton.onClick.AddListener(gameSession.buyHeart); // Yeni referans� ata
        }
        else
        {
            Debug.LogError("GameSession or Button not found!");
        }
    }
}
