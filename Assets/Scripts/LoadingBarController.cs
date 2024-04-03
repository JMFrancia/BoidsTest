using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
/*
 * Controller class for the loading bar UI
 */
//TODO: Standardize for Unity toolset
public class LoadingBarController : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _fillBar;
    [SerializeField] private bool _hideUntilFilled = true;
    [SerializeField] private float _inactivityBeforeHide = 3f;
    [SerializeField] private float _hideDuration = 0.5f;
    [SerializeField] private float _loadAnimationTime = .3f;
    [SerializeField] private float _showAlpha = .5f;
    
    private Tween _loadTween;
    private Tween _hideTween;
    private float _timeSinceLastFill;
    private bool _showing;
    
    public void Show()
    {
        if (_showing)
            return;
        
        _hideTween?.Kill();
        _showing = true;
        _canvasGroup.alpha = _showAlpha;
    }

    public void Hide(bool instant = false)
    {
        if(!_showing)
            return;

        _showing = false;

        if (instant)
        {
            _canvasGroup.alpha = 0f;
        }
        else
        {
            _hideTween = DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 0f, _hideDuration).SetEase(Ease.InSine);  
        }
    }

    public void SetFillAmount(float fillAmount)
    {
        Show();
        _fillBar.fillAmount = fillAmount;
        ResetTimeSinceLastFill();
    }
    
    public void AnimateFillAmount(float fillAmount)
    {
        Show();
        if (_loadTween != null && _loadTween.active)
        {
            _loadTween.Kill();
        }
        
        _loadTween = DOTween.To(() => _fillBar.fillAmount, x => _fillBar.fillAmount = x, fillAmount, _loadAnimationTime).SetEase(Ease.OutSine).OnComplete(
            () =>
            {
                ResetTimeSinceLastFill();
            });
    }

    private void Start()
    {
        Hide(true);
    }

    private void Update()
    {
        if (_hideUntilFilled && _showing)
        {
            _timeSinceLastFill += Time.deltaTime;
            if (_timeSinceLastFill >= _inactivityBeforeHide)
            {
                Hide();
            }
        }
    }

    private void ResetTimeSinceLastFill()
    {
        _timeSinceLastFill = 0f;
    }
}
