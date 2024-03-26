using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using Michsky.MUIP;
using Rolos;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoleCategoryContainerUI : MonoBehaviour
{
    [Header("Elements")] 
    [SerializeField] private TextMeshProUGUI titletext;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private ButtonManager _button;

    private Roles roles;
    
    public void Configure(RoleCategorySO roleCategoryData)
    {
        roles = new Roles(roleCategoryData);
        titletext.text = roleCategoryData.title;
        descriptionText.text = roleCategoryData.des;
        _button.onClick.AddListener(OnButtonClicked);
    }
    public void OnButtonClicked()
    {
        OnRequestRole.Trigger(roles);
    }
}
