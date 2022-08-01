using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private AudioManager audioManager;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        audioManager.Play("on");
    }

}
