import { List } from "System/Collections/Generic"
import { emo } from "onejs/styled"
import { h } from "preact"
import { useEffect, useState } from "preact/hooks"
const dialogueSystem = require("dialogueSystem")

const Dialogue = () => {
    const [isDialogueActive, setDialogueActive] = useState(false);
    const [index, setIndex] = useState(0)
    const [currentDialogue, setCurrentDialogue] = useState([])

    function showDialogue(dialogue: List<string>) {
        setCurrentDialogue(dialogue.ToArray())
        setIndex(0)
        setDialogueActive(true)
    }

    function next() {
        if (index + 1 < currentDialogue.length) {
            setIndex(index + 1)
        } else {
            setDialogueActive(false)
        }
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
        <div class={emo`
            height: 100%;
            width: 100%;
            display: flex;
            justify-content: flex-end;
            align-items: center;
        `}
            ref={e => e?.focus()}
            focusable={true}
            onKeyDown={e => {
                if (e.keyCode === 13) next()
            }}>
            <p class={emo`
                background-color: #000;
                color: #fff;
            `}>
                {currentDialogue[index]}
            </p>
        </div>
    ) : null
}

export default Dialogue