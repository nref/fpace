module Tests

open NUnit.Framework
open RunMath

[<SetUp>]
let Setup () = ()

[<TestCase(3600, 1, 0, 0)>]
[<TestCase(3601, 1, 0, 1)>]
[<TestCase(3660, 1, 1, 0)>]
[<TestCase(3661, 1, 1, 1)>]
let DurationFromSeconds_IsCorrect (totalSeconds, hours, mins, secs) =
    let duration: Duration = RunMath.durationFromSeconds totalSeconds
    Assert.AreEqual(hours, duration.Hours)
    Assert.AreEqual(mins, duration.Minutes)
    Assert.AreEqual(secs, duration.Seconds)

[<TestCase("mi", 1609.344)>]
[<TestCase("km", 1000.00)>]
[<TestCase("yd", 0.9144)>]
[<TestCase("m", 1)>]
let unitStringToMeters_IsCorrect (str, want) =
    let got: float = RunMath.unitStringToMeters (str)
    Assert.AreEqual(want, got)

[<Test>]
let unitStringToMeters_Throws () =
    Assert.Throws(fun () -> RunMath.unitStringToMeters ("booger") |> ignore)
    |> ignore

[<TestCase(0, 0, 1, 1, 1, 0, 0, 1)>] // at a pace of 1 second/meter, . it should take 1s to go 1m
[<TestCase(0, 8, 0, 1609.344, 1609.344, 0, 8, 0)>] // at a pace of 8 min/mi it should take 8min to go 1mi (== 1609.344m), expressed as 1mi
[<TestCase(0, 8, 0, 2.0 * 1609.344, 1609.344, 0, 16, 0)>] // at a pace of 8 min/mi (expressed as 8min/1609.344m) it should take 16min to go 2mi
[<TestCase(0, 5, 0, 1609.344, 1000, 0, 8, 2)>] // at a pace of 5min/km (expressed as 5min/1000m) should be 8:02/mi (== 8:02/1609.344m)
let calcTime_Correct
    (
        paceHours: int,
        paceMinutes: int,
        paceSeconds: int,
        newDistance: float,
        oldDistance: float,
        expectedTimeHours: int,
        expectedTimeMinutes: int,
        expectedTimeSeconds: int
    ) =
    let pace =
        RunMath.Duration(Hours = paceHours, Minutes = paceMinutes, Seconds = paceSeconds)

    let time = RunMath.calcTime pace newDistance oldDistance

    Assert.AreEqual(expectedTimeHours, time.Hours)
    Assert.AreEqual(expectedTimeMinutes, time.Minutes)
    Assert.AreEqual(expectedTimeSeconds, time.Seconds)

[<TestCase(1, 0, 0, 1, 0, 0, 1, 1)>] // if we went 1 meter per hour for 1 hour, we'd expect to have gone 1 meter
[<TestCase(0, 8, 0, 0, 8, 0, 1609.344, 1)>] // if we went 8min/mi for 8 minutes, we'd expect to have gone 1 mile (== 1609.3344m)
[<TestCase(0, 10, 0, 0, 5, 0, 1000, 2)>] // if we went 5min/km for 10 minutes, we'd expect to have gone 2km
let calcDistance_Correct
    (
        timeHours: int,
        timeMinutes: int,
        timeSeconds: int,
        paceHours: int,
        paceMinutes: int,
        paceSeconds: int,
        ratio: float,
        want: float
    ) =

    let time =
        RunMath.Duration(Hours = timeHours, Minutes = timeMinutes, Seconds = timeSeconds)

    let pace =
        RunMath.Duration(Hours = paceHours, Minutes = paceMinutes, Seconds = paceSeconds)

    let got: float = RunMath.calcDistance time pace ratio
    Assert.AreEqual(want, got)

[<TestCase(1, 0, 0, 1, 1, 1, 0, 0)>] // if we went 1 meter in 1 hour, our pace was 1 meter/hour
[<TestCase(0, 8, 0, 2.0 * 1609.344, 1609.344, 0, 4, 0)>] // if we went 2mi (== 2 * 1609.344m) in 8 minutes, our pace was 4min/mi
let calcPace_Correct
    (
        timeHours: int,
        timeMinutes: int,
        timeSeconds: int,
        distance: float,
        ratio: float,
        wantHours: float,
        wantMinutes: float,
        wantSeconds: float
    ) =


    let time =
        RunMath.Duration(Hours = timeHours, Minutes = timeMinutes, Seconds = timeSeconds)

    let pace = RunMath.calcPace time distance ratio

    Assert.AreEqual(wantHours, pace.Hours)
    Assert.AreEqual(wantMinutes, pace.Minutes)
    Assert.AreEqual(wantSeconds, pace.Seconds)
