using Foundation.Iot.Time;

namespace Foundation.Iot.Test;

[TestClass]
public class FreeRunningTimerTests
{
    [TestMethod]
    [Timeout(5000)]
    public async Task ChangeTypeUInt16TestAsync()
    {
        var timeStamp = FreeRunningTimer.ElapsedTime;
        await Task.Delay(10);
        var timeStamp2 = FreeRunningTimer.ElapsedTime;

        FreeRunningTimer.IsHighResolution.ShouldBe(true);
        timeStamp2.ShouldBeGreaterThan(timeStamp);

        // This has intermittent issues on the CI build server AppVeyor.
        //(timeStamp2 - timeStamp).TotalMilliseconds.ShouldBeGreaterThanOrEqualTo(10);
    }
}
