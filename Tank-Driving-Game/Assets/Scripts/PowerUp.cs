using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float speedBoost = 30f; // Anlık hız artış miktarı
    private bool isActivated = false; // Tekrar tetiklenmeyi engellemek için flag

    void OnTriggerEnter(Collider other)
    {
        if (!isActivated && other.CompareTag("Player")) // Sadece ilk temas
        {
            isActivated = true; // Tetiklendi olarak işaretle
            ApplySpeedBoost(other.GetComponent<CarController>()); // Hız artışını uygula
            Destroy(gameObject); // PowerUp objesini yok et
        }
    }

    private void ApplySpeedBoost(CarController carController)
    {
        if (carController != null)
        {
            carController.SetCurrentSpeed(carController.GetCurrentSpeed() + speedBoost); // Anlık hızı artır
            Debug.Log("Anlık hız arttı! Yeni hız: " + carController.GetCurrentSpeed());
        }
    }
}
