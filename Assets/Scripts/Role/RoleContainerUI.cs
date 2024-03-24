using System;
using System.Collections;
using System.Collections.Generic;
using Rolos;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoleContainerUI : MonoBehaviour
{
    [Header("Elements")] 
    [SerializeField] private TextMeshProUGUI roleText;
    [SerializeField] private Button _button;
    
    [Header("Actions")] 
    public static Action<Roles> onButtonClicked;
    
    private Roles roles;
    
    public void Configure(Roles roles)
    {
        _button.onClick.AddListener(OnButtonClicked);
        this.roles = roles;
        roleText.text = roles.title;
    }

    public void OnButtonClicked()
    {
        onButtonClicked?.Invoke(roles);
    }
    
}
