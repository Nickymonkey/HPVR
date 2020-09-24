using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonTransitioner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerClickHandler
{
    public Color32 m_NormalColor = Color.white;
    public Color32 m_HoverColor = Color.grey;
    public Color32 m_DownColor = Color.white;

    private Image m_Image = null;

    //OnGUI()
    //{
    //    pressed = GUILayout.Toggle(pressed, "Toggle me !", "Button");
    //}

    private void Awake()
    {
        m_Image = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_Image.color = m_HoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_Image.color = m_NormalColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_Image.color = m_DownColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //m_Image.color = m_DownColor;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        m_Image.color = m_HoverColor;
    }


}
