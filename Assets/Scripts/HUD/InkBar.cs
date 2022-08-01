using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InkBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    Draw draw;
    float timer = 0;
    float lastInkUsageTime = 0;
    bool doesHaveInk = true;
    [SerializeField] float regenAmount = 13f;
    [SerializeField] float defaultDecreaseAmount = 7f;
    [SerializeField] float inkRegenerationCooldown = .3f;
    
    private void Start() {
        draw = FindObjectOfType<Draw>();
    }

    private void Update() {
        timer += Time.deltaTime;

        if(timer - lastInkUsageTime >= inkRegenerationCooldown){
            IncreaseInk(regenAmount * Time.deltaTime);
        }
    }

    public void SetInk(float ink)
    {
        slider.value = ink;
    }

    public void IncreaseInk(float increaseAmount){
        slider.value += increaseAmount;
        doesHaveInk = true;
    }

    public void DecreaseInk(){
        slider.value -= defaultDecreaseAmount * Time.deltaTime;
        lastInkUsageTime = Time.time;
        timer = lastInkUsageTime;

        if(slider.value <= 0){
            doesHaveInk = false;
        }
    }

    public void DecreaseInk(float decreaseAmount){
        slider.value -= decreaseAmount * Time.deltaTime;

        if(slider.value <= 0){
            doesHaveInk = false;
        }
    }
    
    public void SetMaxInk(int ink)
    {
        slider.maxValue = ink;
        slider.value = ink;
    }
    
    public float GetCurrentInk(){
        return slider.value;
    }
    public bool DoeshaveInk(){
        return doesHaveInk;
    }
}
