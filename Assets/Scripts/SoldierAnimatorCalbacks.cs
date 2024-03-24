using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAnimatorCalbacks : MonoBehaviour
{
    public void OnWeaponFireComplete()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
