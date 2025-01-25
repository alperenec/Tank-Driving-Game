using System;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public GameObject Heart4; // Dördüncü kalp referansı
    public int currentHealth = 3; // Başlangıç sağlığı
    public GameObject[] hearts; // Kalp UI referansları
    public GameObject gameOverText; // Game Over mesajı

    void Start()
    {
        UpdateHeartsUI();
        gameOverText.SetActive(false); // Game Over başlangıçta gizli

        if (Heart4 != null)
        {
            Heart4.SetActive(false); // Heart4 başlangıçta gizlenir
        }
    }

    public void TakeDamage()
    {
        if (currentHealth > 0)
        {
            currentHealth--;
            UpdateHeartsUI();
            Debug.Log("Can azaldı! Kalan can: " + currentHealth);
        }

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    public void AddHealth()
    {
        if (currentHealth == 3 && Heart4 != null) // Eğer sağlık 4'e ulaştıysa
        {
            Heart4.SetActive(true); // Heart4'ü görünür yap
            currentHealth++;
            print("currentHealth == 3 && Heart4 != null "+currentHealth);
        }

        else if(currentHealth < 3 && Heart4 != null) // Maksimum kalp sayısını 4 olarak sınırla
        {
            currentHealth++;
            print("currentHealth < 3"+currentHealth);
        }
        UpdateHeartsUI();
        return;
    }

    void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < currentHealth); // Sadece mevcut sağlığa kadar olan kalpleri aktif yap
            print("UpdateHeartsUI"+currentHealth);

        }
    }

    void GameOver()
    {
        Debug.Log("Oyun bitti! Tüm canlar tükendi.");
        gameOverText.SetActive(true);
        Time.timeScale = 0f; // Oyun durdurulur
    }

    public void ResetHealth()
    {
        currentHealth = 3; // Sağlığı 3'e sıfırla
        UpdateHeartsUI();
    }
}
