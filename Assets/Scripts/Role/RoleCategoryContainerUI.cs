using System.Collections;
using System.Collections.Generic;
using Rolos;
using TMPro;
using UnityEngine;

public class RoleCategoryContainerUI : MonoBehaviour
{
    [Header("Elements")] 
    [SerializeField] private TextMeshProUGUI titletext;

    [SerializeField] private RoleContainerUI _roleContainerUI;
    [SerializeField] private Transform roleContainersParent;
    public void Configure(RoleCategorySO roleCategoryData)
    {
        titletext.text = roleCategoryData.title;
        CreateRoleContainers(roleCategoryData);
    }

    private void CreateRoleContainers(RoleCategorySO roleCategoryData)
    {
        for (int i = 0; i < roleCategoryData.roles.Length; i++)
        {
            Roles role = roleCategoryData.roles[i];
            RoleContainerUI roleContainerInstance = Instantiate(_roleContainerUI, roleContainersParent);
            roleContainerInstance.Configure(role);
        }

    }
}
