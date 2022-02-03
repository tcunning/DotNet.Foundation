using Foundation.Iot.Collection;

namespace Foundation.Iot.Test.Collection;

[TestClass]
[ExcludeFromCodeCoverage]
public class EnumeratorTrackerTest
{
    [DataTestMethod]
    [DataRow(1)]
    [DataRow(5)]
    [DataRow(0)]
    public void TrackerTest(int maxCount)
    {
        var tracker = new EnumeratorTracker(maxCount);
        tracker.Count.ShouldBe(maxCount);
        tracker.CurrentIndex.ShouldBe(EnumeratorTracker.EnumerationNotStarted);
        for( int index = 0; index < maxCount; index++ )
        {
            tracker.MoveNext().ShouldBe(true);
            tracker.CurrentIndex.ShouldBe(index);
        }
        tracker.MoveNext().ShouldBe(false);
      
    }

    [TestMethod]
    public void TrackerErrorTest()
    {
        Should.Throw<ArgumentOutOfRangeException>(() =>
        {
            _ = new EnumeratorTracker(EnumeratorTracker.EnumerationNotStarted);
        });
    }
}
