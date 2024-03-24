using System;
using System.Collections;
using System.Collections.Generic;
using Rolos;
using UnityEngine;

public class RolesManager : MonoBehaviour
{
    [Header("Elements")] 
    [SerializeField] private RoleCategoryContainerUI _roleCategoryContainerUI;
    [SerializeField] private Transform _roleCategoryContainersParent;
    [Header("Data")] [SerializeField] private RoleCategorySO[] _roleCategorySos;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        for (int i = 0; i < _roleCategorySos.Length; i++)
        {
            CreateRoleCategoryContatiner(_roleCategorySos[i]);
        }
    }

    private void CreateRoleCategoryContatiner(RoleCategorySO roleCategorySo)
    {
        RoleCategoryContainerUI roleCategoryContainerUIInstance = Instantiate(_roleCategoryContainerUI, _roleCategoryContainersParent);
        
        roleCategoryContainerUIInstance.Configure(roleCategorySo);
        
    }
}