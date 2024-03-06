import { useEventfulState } from "onejs"
import { h, render } from "preact"
import { useEffect, useState } from "preact/hooks"
const doorManager = require("doorManager")

const App = () => {
    const [roomId, setRoomId] = useState(0)
    const [isMinigameActive, setMinigameActive] = useState(false);

    function tryUnlockDoor(pRoomId: number) {
        setMinigameActive(true)
        setRoomId(pRoomId)
    }

    function puzzleSolved() {
        setMinigameActive(false)
        doorManager.UnlockRoom(roomId)
    }
    
    useEffect(() => {
        doorManager.add_OnTryUnlockRoom(tryUnlockDoor)

        onEngineReload(() => {  // Cleaning up for Live Reload
            doorManager.remove_OnTryUnlockRoom(tryUnlockDoor)
        })

        return () => {  // Cleaning up for side effect
            doorManager.remove_OnTryUnlockRoom(tryUnlockDoor)
        }
    }, [])

    return isMinigameActive 
        ? <div class="w-full h-full flex justify-center items-center text-[#305fbc] bg-stone-600" onClick={puzzleSolved}>Room: {roomId}</div> 
        : null
}

render(<App />, document.body)