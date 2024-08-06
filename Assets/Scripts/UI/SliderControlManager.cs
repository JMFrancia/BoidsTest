using UnityEngine;
using UnityEngine.UI;

/*
 * Controller class for the aggression & spread slider UI
 */
public class SliderControlManager : MonoBehaviour
{
    [SerializeField] private Slider _aggressionSlider;
    [SerializeField] private Slider _spreadSlider;
    
    private void Awake()
    {
        _aggressionSlider.onValueChanged.AddListener(OnAggressionChanged);
        _spreadSlider.onValueChanged.AddListener(OnSpreadChanged);
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        OnAggressionChanged(_aggressionSlider.value);
        OnSpreadChanged(_aggressionSlider.value);
    }

    private void OnAggressionChanged(float value)
    {
       EventManager.TriggerEvent(Constants.Events.SLIDER_AGGRESSION_CHANGED, value);
    }
    
    private void OnSpreadChanged(float value)
    {
        EventManager.TriggerEvent(Constants.Events.SLIDER_SPREAD_CHANGED, value);
    }
}
