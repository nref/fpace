module RunMath

type Duration() =
    member val Hours: int = 0 with get, set
    member val Minutes: int = 0 with get, set
    member val Seconds: int = 0 with get, set

let totalSeconds: Duration -> float =
    fun duration ->
        float duration.Hours * 3600.0
        + float duration.Minutes * 60.0
        + float duration.Seconds

// Split seconds into hours, minutes, seconds
let durationFromSeconds: float -> Duration =
    fun newSeconds ->
        let hours = floor (newSeconds / 3600.0)
        let mins = floor ((newSeconds - (3600.0 * hours)) / 60.0)
        let secs = newSeconds - 3600.0 * hours - 60.0 * mins
        Duration(Hours = int hours, Minutes = int mins, Seconds = int secs)

type stringToFloatConverter = string -> float

// Convert 1 of the given unit to meters
let unitStringToMeters: stringToFloatConverter =
    fun unit ->
        match unit with
        | "100mi" -> 160_934.4
        | "100km" -> 100_000
        | "50mi" -> 80_467.2
        | "50km" -> 50_000
        | "marathon" -> 42_195.0
        | "half-marathon" -> 21_097.5
        | "10km" -> 10_000.0
        | "5km" -> 5000.0
        | "mi" -> 1609.344
        | "km" -> 1000.0
        | "800m" -> 800.0
        | "400m" -> 400.0
        | "200m" -> 200.0
        | "100m" -> 100.0
        | "m" -> 1.0
        | "yd" -> 0.9144
        | _ -> failwith "Unknown unit"

// Return the time it took to run the given distance.
// distance given in any length unit (e.g. mi, km)
// with a ratio converting it to the desired output unit, e.g. meters.
// pace given in m/s.
let calcTime (pace: Duration) (distance: float) (ratio: float) : Duration =
    let paceTotalSeconds = totalSeconds pace
    let time = paceTotalSeconds * (distance / ratio)

    durationFromSeconds time

// Return the distance run. pace given in m/s. Use the given a ratio to convert to the desired output distance unit, e.g. mi
let calcDistance (time: Duration) (pace: Duration) (ratio: float) : float =
    let timeTotalSeconds = totalSeconds time
    let paceTotalSeconds = totalSeconds pace

    timeTotalSeconds / paceTotalSeconds

// Return the run pace in m/s.
// distance given in any length unit (e.g. mi, km)
// with a ratio converting it to the desired output unit, e.g. meters
let calcPace (time: Duration) (dist: float) (ratio: float) : Duration =

    // Convert time and distance to pace in the input unit
    let inputSeconds = totalSeconds time
    let paceSeconds = inputSeconds / dist

    // Convert pace to the desired output unit
    let paceConverted = paceSeconds * (dist / ratio)
    let newPace = paceSeconds / paceConverted

    // Get total seconds in the desired unit
    let outputSeconds = inputSeconds * newPace

    durationFromSeconds outputSeconds
