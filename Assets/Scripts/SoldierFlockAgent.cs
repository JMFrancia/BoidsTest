using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanFireWeapon : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _muzzleFlash;
    
    public void FireWeapon(FlockAgent target)
    {
        Debug.Log("Bang!");
        _muzzleFlash.SetActive(true);
        _muzzleFlash.transform.LookAt(target.transform, Vector3.back);
        _animator.SetTrigger(Constants.AnimationTriggers.FIRE_WEAPON);
        target.Die();
    }
    
    private void Awake()
    {
        _muzzleFlash.SetActive(false);
    }

}
