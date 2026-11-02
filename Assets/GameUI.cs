using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Image dashCooldownFill;

    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerMovement playerMovement;

    private float elapsedTime;

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        UpdateTimer();
        UpdateHealth();
        UpdateDashCooldown();
    }

    private void UpdateTimer()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void UpdateHealth()
    {
        healthText.text =
            $"Health: {playerHealth.CurrentHealth}/{playerHealth.MaxHealth}";
    }

    private void UpdateDashCooldown()
    {
        float cooldown = playerMovement.DashCooldown;
        float remaining = playerMovement.DashCooldownRemaining;

        if (cooldown <= 0f)
        {
            dashCooldownFill.fillAmount = 0f;
            return;
        }

        dashCooldownFill.fillAmount = remaining / cooldown;
    }
}