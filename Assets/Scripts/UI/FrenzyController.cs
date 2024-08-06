using UnityEngine;

public class FrenzyController : MonoBehaviour
{
    public void ActivateFrenzy()
    {
        EventManager.TriggerEvent(Constants.Events.FRENZY_ACTIVATED);
    }
}
