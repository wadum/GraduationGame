public interface ITimeTraveller
{
    TimePeriod GetCurrentTimePeriod();

    TimePeriod GetDefaultTimePeriod();

    void SetTimePeriod(TimePeriod timePeriod);

    void SetPast();

    void SetPresent();

    void SetFuture();

    void ResetToDefaultTimePeriod();
}