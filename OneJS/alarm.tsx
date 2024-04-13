import { emo } from "onejs/styled"
import { h } from "preact"
import { useEffect, useState } from "preact/hooks";
const alarm = require("alarmEvent")

const Alarm = () => {
    const [isAlarmActive, setAlarmActive] = useState(false)

    function toggleAlarm(activate: boolean) {
        log(activate)
        setAlarmActive(activate)
    }

    useEffect(() => {
        alarm.add_ToggleAlarm(toggleAlarm)

        onEngineReload(() => {  // Cleaning up for Live Reload
            alarm.remove_ToggleAlarm(toggleAlarm)
        })

        return () => {  // Cleaning up for side effect
            alarm.remove_ToggleAlarm(toggleAlarm)
        }
    }, [])

    return isAlarmActive ? <div class={emo`
        height: 100%;
        width: 100%;
        position: absolute;
        top: 0; left: 0; right: 0; bottom: 0;
        background-color: #f00;
    `}>
    </div> : null
}

export default Alarm