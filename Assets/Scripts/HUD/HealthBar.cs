using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField] private Slider slider;
    private TestPlayer player;

    private float currentHealth;
    private float desiredHealth;
    private float smoothedHealth;
    private bool desiredHealthChanged = false;
    [SerializeField] private float lerpValue;

    private bool isDead = false;

    private void Start() {
        slider = GetComponent<Slider>();
        player = FindObjectOfType<TestPlayer>();

        currentHealth = player.GetCurrentHealth();
        desiredHealth = currentHealth;

        InitializeSliderValues();

    }

    private void Update() {
        if(currentHealth != desiredHealth && !isDead){
            smoothedHealth = Mathf.Lerp(desiredHealth, currentHealth, lerpValue);
            slider.value = smoothedHealth;
            currentHealth = smoothedHealth;
        }
    }
    
    public void SetHealth(float health)
    {
        desiredHealth = health;
        desiredHealthChanged = true;
        if(health == 0){
            player.Dead();
        }
    }

    public void DecreaseHealth(float amount){
        desiredHealth -= amount;
        desiredHealthChanged = true;
        if(slider.value <= 0){
            player.Dead();
        }
    }
    
    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void IncreaseHealth(float increaseAmount){
            desiredHealth += increaseAmount;
            desiredHealthChanged = true;
    }

    private void InitializeSliderValues(){
        slider.maxValue = player.GetMaxHealth();
        slider.value = player.GetCurrentHealth();
    }
}
