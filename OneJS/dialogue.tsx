import { List } from "System/Collections/Generic"
import { emo } from "onejs/styled"
import { h } from "preact"
import { useEffect, useState } from "preact/hooks"
const dialogueSystem = require("dialogueSystem")

const Dialogue = () => {
    const [isDialogueActive, setDialogueActive] = useState(false);
    function showDialogue(dialogue: List<string>) {
        log(dialogue)
        setDialogueActive(true)
    }
    useEffect(() => {
        dialogueSystem.add_OnShowDialogue(showDialogue)

        onEngineReload(() => {  // Cleaning up for Live Reload
            dialogueSystem.remove_OnShowDialogue(showDialogue)
        })

        return () => {  // Cleaning up for side effect
            dialogueSystem.remove_OnShowDialogue(showDialogue)
        }
    }, [])
    return isDialogueActive ? (
        <div class={emo``}>
            <p>Hi! I'm a dialogue box!</p>
        </div>
    ) : null
}

export default Dialogue