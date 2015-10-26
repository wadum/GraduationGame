using UnityEngine;
using System.Collections;

public abstract class TimeTraveller : MonoBehaviour, ITimeTraveller {

    public abstract TimePeriod GetCurrentTimePeriod();
    
    public abstract TimePeriod GetDefaultTimePeriod();

    public void SetTimePeriod(TimePeriod timePeriod)
    {
        switch (timePeriod)
        {
            case TimePeriod.Future:
                SetFuture();
                break;
            case TimePeriod.Present:
                SetPresent();
                break;
            case TimePeriod.Past:
                SetPast();
                break;
        }
    }
    
    public abstract void SetPast();
    
    public abstract void SetPresent();
    
    public abstract void SetFuture();

    public void ResetToDefaultTimePeriod()
    {
        SetTimePeriod(GetDefaultTimePeriod());
    }
}
