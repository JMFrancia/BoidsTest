using UnityEngine;

public class CanFireWeapon : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private float _reloadTime = 3f;
    
    private bool _reloading;
    private float _timeSinceLastShot;
    
    public void FireWeapon(FlockAgent target)
    {
        if (_reloading)
            return;
        
        _muzzleFlash.SetActive(true);
        _muzzleFlash.transform.right = -1 * (target.transform.position - _muzzleFlash.transform.position).normalized;
        _animator.SetTrigger(Constants.AnimationTriggers.FIRE_WEAPON);
        target.Die();
        _reloading = true;
        _timeSinceLastShot = 0f;
    }

    private void Update()
    {
        UpdateReload();
    }

    private void UpdateReload()
    {
        if (_reloading)
        {
            _timeSinceLastShot += Time.deltaTime;
            if(_timeSinceLastShot >= _reloadTime)
                _reloading = false;
        }
    }
    
    private void Awake()
    {
        _muzzleFlash.SetActive(false);
    }

}
