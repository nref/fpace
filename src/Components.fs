namespace rec App

open Feliz
open Feliz.Router
open RunMath

type Components =

    [<ReactComponent>]
    static member Calculator() =
        let (tHours, setThours) = React.useState ("0")
        let (tMins, setTmins) = React.useState ("0")
        let (tSecs, setTsecs) = React.useState ("0")

        let (dist, setDist) = React.useState ("1.0")

        let (pHours, setPhours) = React.useState ("0")
        let (pMins, setPmins) = React.useState ("0")
        let (pSecs, setPsecs) = React.useState ("0")

        let (unitString, setUnitString) = React.useState ("mi")

        let (perDistMeters, setPerDistMeters) =
            React.useState (RunMath.unitStringToMeters "mi")

        let (distMeters, setDistMeters) = React.useState (perDistMeters)

        Html.table
            [ Html.tbody
                  [ Html.tr
                        [ prop.className [ "time-row" ]
                          //prop.style [ style.backgroundColor "#7981fe" ] // Moved to CSS, left here as an example
                          prop.children
                              [ Html.td [ Html.text "Time" ]

                                Html.td
                                    [ Html.table
                                          [ Html.tbody
                                                [ Html.tr
                                                      [ Html.th [ Html.text "h" ]
                                                        Html.th [ Html.text "m" ]
                                                        Html.th [ Html.text "s" ] ]
                                                  Html.tr
                                                      [ Html.td
                                                            [ Html.input
                                                                  [ prop.value tHours
                                                                    prop.onChange (fun h -> setThours h) ] ]
                                                        Html.td
                                                            [ Html.input
                                                                  [ prop.value tMins
                                                                    prop.onChange (fun m -> setTmins m) ] ]
                                                        Html.td
                                                            [ Html.input
                                                                  [ prop.value tSecs
                                                                    prop.onChange (fun s -> setTsecs s) ] ] ]

                                                  ] ] ]

                                Html.td
                                    [ Html.button
                                          [

                                            prop.text "Get Time"
                                            prop.onClick (fun _ ->

                                                let pace =
                                                    RunMath.Duration(
                                                        Hours = int pHours,
                                                        Minutes = int pMins,
                                                        Seconds = int pSecs
                                                    )

                                                let duration: RunMath.Duration =
                                                    RunMath.calcTime pace distMeters perDistMeters

                                                let h = string duration.Hours
                                                let m = string duration.Minutes
                                                let s = string duration.Seconds

                                                setThours h
                                                setTmins m
                                                setTsecs s

                                                printfn "Time: %s %s %s" h m s
                                                ())

                                            ] ] ] ]


                    Html.tr
                        [ prop.className [ "pace-row" ]
                          prop.children
                              [

                                Html.td [ Html.text "Pace" ]
                                Html.td
                                    [ Html.table
                                          [ Html.tbody

                                                [ Html.tr
                                                      [ Html.th [ Html.text "h" ]
                                                        Html.th [ Html.text "m" ]
                                                        Html.th [ Html.text "s" ] ]
                                                  Html.tr
                                                      [ Html.td
                                                            [ Html.input
                                                                  [ prop.value pHours
                                                                    prop.onChange (fun h -> setPhours h) ] ]
                                                        Html.td
                                                            [ Html.input
                                                                  [ prop.value pMins
                                                                    prop.onChange (fun m -> setPmins m) ] ]
                                                        Html.td
                                                            [ Html.input
                                                                  [ prop.value pSecs
                                                                    prop.onChange (fun s -> setPsecs s) ] ] ]
                                                  Html.tr
                                                      [ Html.td
                                                            [ Html.text "per"
                                                              Html.select
                                                                  [ prop.onChange (fun e ->
                                                                        setPerDistMeters (RunMath.unitStringToMeters e)

                                                                        printfn
                                                                            "Selected pace per %s (%fm)"
                                                                            e
                                                                            perDistMeters

                                                                        ())
                                                                    prop.children
                                                                        [ Html.option [ Html.text "mi" ]
                                                                          Html.option [ Html.text "km" ]
                                                                          Html.option [ Html.text "800m" ]
                                                                          Html.option [ Html.text "400m" ]
                                                                          Html.option [ Html.text "200m" ]
                                                                          Html.option [ Html.text "100m" ]
                                                                          Html.option [ Html.text "m" ]
                                                                          Html.option [ Html.text "yd" ] ] ] ] ] ] ] ]
                                Html.td
                                    [ Html.button
                                          [ prop.text "Get Pace"
                                            prop.onClick (fun _ ->
                                                let time =
                                                    RunMath.Duration(
                                                        Hours = int tHours,
                                                        Minutes = int tMins,
                                                        Seconds = int tSecs
                                                    )

                                                let newTime: Duration = RunMath.calcPace time distMeters perDistMeters

                                                let h = string newTime.Hours
                                                let m = string newTime.Minutes
                                                let s = string newTime.Seconds

                                                setPhours h
                                                setPmins m
                                                setPsecs s

                                                printfn "Pace: %s %s %s" h m s) ] ] ] ]
                    Html.tr
                        [ prop.className [ "distance-row" ]
                          prop.children
                              [ Html.td [ Html.text "Distance" ]
                                Html.td
                                    [ Html.input
                                          [ prop.value dist
                                            prop.onChange (fun newDist ->
                                                setDist newDist

                                                let newDistMeters: float =
                                                    RunMath.unitStringToMeters unitString * float newDist

                                                setDistMeters newDistMeters

                                                printfn
                                                    "Distance changed to %s %s (%fm)"
                                                    newDist
                                                    unitString
                                                    newDistMeters) ]
                                      Html.select
                                          [ prop.id "distance-select"
                                            prop.onChange (fun newUnitString ->
                                                let oldUnitString = unitString

                                                let oldDistMeters =
                                                    RunMath.unitStringToMeters oldUnitString * float dist

                                                let newDistMeters =
                                                    RunMath.unitStringToMeters newUnitString * float dist

                                                setDistMeters newDistMeters
                                                setUnitString newUnitString

                                                printfn
                                                    "Distance changed to %s%s (%fm)"
                                                    dist
                                                    newUnitString
                                                    newDistMeters)
                                            prop.children
                                                [ Html.option [ prop.value "mi"; prop.text "Miles" ]
                                                  Html.option [ prop.value "km"; prop.text "Kilometers" ]
                                                  Html.option [ prop.value "m"; prop.text "Meters" ]
                                                  Html.option [ prop.value "yd"; prop.text "Yards" ] ] ]

                                      Html.select
                                          [ prop.onChange (fun newUnitString ->
                                                let oldUnitString = unitString
                                                let oldDist = RunMath.unitStringToMeters oldUnitString
                                                let newDist = RunMath.unitStringToMeters newUnitString

                                                // preserve selected unit after drop-down distance is selected
                                                // e.g. A user having previously selected "mi" still wants to see marathon as 26.2mi, not 42195m
                                                setDist (sprintf "%.2f" (newDist / oldDist))
                                                setDistMeters newDist)

                                            prop.children
                                                [ Html.option [ prop.value "5km"; prop.text "or pick event" ]
                                                  Html.option [ prop.value "5km"; prop.text "5km" ]
                                                  Html.option [ prop.value "10km"; prop.text "10km" ]
                                                  Html.option
                                                      [ prop.value "half-marathon"; prop.text "Half-marathon (13.1)" ]
                                                  Html.option [ prop.value "marathon"; prop.text "Marathon (26.2)" ]
                                                  Html.option [ prop.value "50km"; prop.text "Ultramarathon (50km)" ]
                                                  Html.option [ prop.value "50mi"; prop.text "Ultramarathon (50mi)" ]
                                                  Html.option [ prop.value "100km"; prop.text "Ultramarathon (100km)" ]
                                                  Html.option [ prop.value "100mi"; prop.text "Ultramarathon (100mi)" ] ] ] ]

                                Html.td
                                    [ Html.button
                                          [ prop.text "Get Distance"
                                            prop.onClick (fun _ ->

                                                let time =
                                                    RunMath.Duration(
                                                        Hours = int tHours,
                                                        Minutes = int tMins,
                                                        Seconds = int tSecs
                                                    )

                                                let pace =
                                                    RunMath.Duration(
                                                        Hours = int pHours,
                                                        Minutes = int pMins,
                                                        Seconds = int pSecs
                                                    )

                                                let totalSeconds = RunMath.totalSeconds pace
                                                let unitToMeters = RunMath.unitStringToMeters unitString

                                                let newDist: float = RunMath.calcDistance time pace unitToMeters

                                                let newDistMeters: float =
                                                    RunMath.unitStringToMeters unitString * float newDist

                                                setDist (string newDist)
                                                setDistMeters newDistMeters

                                                printfn
                                                    "Distance changed to %f%s (%fm)"
                                                    newDist
                                                    unitString
                                                    newDistMeters

                                                ()) ] ] ] ]

                    ] ]
