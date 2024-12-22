using UnityEngine;
using UnityEngine.UI;

public class MarketUI : MonoBehaviour
{
    [SerializeField] private Button buyHeartButton; // Market sahnesindeki buton
    private GameSession gameSession;

    private void Start()
    {
        // Sahne yüklendiðinde GameSession'ý bul ve butonu baðla
        gameSession = FindObjectOfType<GameSession>();
        if (gameSession != null && buyHeartButton != null)
        {
            buyHeartButton.onClick.RemoveAllListeners(); // Eski referanslarý temizle
            buyHeartButton.onClick.AddListener(gameSession.buyHeart); // Yeni referansý ata
        }
        else
        {
            Debug.LogError("GameSession or Button not found!");
        }
    }
}
