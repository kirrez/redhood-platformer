
public interface ITimePlayedCounter
{
    void StartCount();
    void StopCount();
    int GetTimeAndReset();
}
