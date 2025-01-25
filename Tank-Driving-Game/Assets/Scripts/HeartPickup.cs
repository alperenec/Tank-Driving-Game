using System.Collections;
using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    public float animationDuration = 0.2f; // Büyüyüp küçülme süresi
    private PlayerHealth playerHealth;
    private bool isPickedUp = false; // Kalp alındı mı kontrolü

    [System.Obsolete]
    void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>(); // PlayerHealth script'ine erişim
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isPickedUp && other.CompareTag("Player")) // Sadece ilk tetiklenmede çalışır
        {
            isPickedUp = true; // Tetiklendi olarak işaretle
            if (playerHealth != null)
            {
                playerHealth.AddHealth(); // Sağlığı artır
                StartCoroutine(AnimateHeartPickup()); // Animasyon başlat
            }
        }
    }

    private IEnumerator AnimateHeartPickup()
    {
        // Kalbin büyüyüp küçülme animasyonu
        Vector3 originalScale = transform.localScale;
        Vector3 largerScale = originalScale * 1.5f; // Daha büyük ölçek

        // Büyütme
        float elapsed = 0f;
        while (elapsed < animationDuration / 2)
        {
            transform.localScale = Vector3.Lerp(originalScale, largerScale, elapsed / (animationDuration / 2));
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Küçültme
        elapsed = 0f;
        while (elapsed < animationDuration / 2)
        {
            transform.localScale = Vector3.Lerp(largerScale, originalScale, elapsed / (animationDuration / 2));
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Kalbi yok et
        Destroy(gameObject);
    }
}
