using UnityEngine;

/*
 * Callbacks for the soldier animator
 */
public class SoldierAnimatorCallbacks : MonoBehaviour
{
    public void OnWeaponFireComplete()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
