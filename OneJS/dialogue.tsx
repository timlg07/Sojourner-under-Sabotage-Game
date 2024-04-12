import { List } from "System/Collections/Generic"
import { Vector2 } from "UnityEngine"
import { ScrollViewMode, ScrollerVisibility } from "UnityEngine/UIElements"
import { clamp } from "math"
import { emo } from "onejs/styled"
import { h } from "preact"
import { useEffect, useRef, useState } from "preact/hooks"
import { Tween, update } from "tweenjs"
const dialogueSystem = require("dialogueSystem")
const stompEventDelegation = require("stompEventDelegation")

const Dialogue = () => {
    const [isDialogueActive, setDialogueActive] = useState(false);
    const [index, setIndex] = useState(0)
    const [currentDialogue, setCurrentDialogue] = useState([])
    const scrollView = useRef()

    function showDialogue(dialogue: List<string> | Array<string>) {
        if (!(dialogue instanceof Array)) dialogue = dialogue.ToArray()
        setCurrentDialogue(dialogue)
        setIndex(0)
        setDialogueActive(true)
    }

    function next() {
        if (index + 1 < currentDialogue.length) {
            setIndex(index + 1)
        } else {
            setDialogueActive(false)
            stompEventDelegation.OnConversationFinished();
        }
    }

    useEffect(() => {
        /*// for testing
        showDialogue([
            "The component was attacked! But luckily your tests detected it soon enough.",
            "Now try to fix the component as soon as possible, so we can turn it back on!",
            "You can do it!",
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
            "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
            "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.",
            "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
        ])*/

        dialogueSystem.add_OnShowDialogue(showDialogue)

        onEngineReload(() => {  // Cleaning up for Live Reload
            dialogueSystem.remove_OnShowDialogue(showDialogue)
        })

        return () => {  // Cleaning up for side effect
            dialogueSystem.remove_OnShowDialogue(showDialogue)
        }
    }, [])

    useEffect(() => {
        if (scrollView.current) {
            const ve = (scrollView.current as any).ve

            const tween = new Tween({ y: ve.scrollOffset.y }).to({ y: ve.contentViewport.layout.size.y + 300 }, 1e3).onUpdate((o) => {
                const ve = (scrollView.current as any).ve
                ve.scrollOffset = new Vector2(0, o.y)
            }).start()

            const animate = time => {
                if (tween.isPlaying()) {
                    update(time);
                    requestAnimationFrame(animate);
                }
            };
            requestAnimationFrame(animate);
        }
    }, [index])

    /*
    useEffect(() => {
        document.addRuntimeUSS(`
        `)
    }, [])
    */

    return isDialogueActive ? (
        <div class={emo`
            height: 100%;
            width: 100%;
            display: flex;
            justify-content: flex-end;
            align-items: flex-end;
        `}
            ref={e => e?.focus()}
            focusable={true}
            onKeyDown={e => {
                if (e.keyCode === 13) next()
            }}>
            <div class={emo`
                    width: 33%;
                    height: 33%;
                    min-height: 300px;
                    max-height: 100%;
                    background-color: #000;
                    display: flex;
                    justify-content: flex-end;
                    align-items: flex-end;
                    margin-right: 50px;
                    border-top-left-radius: 15px;
                    border-top-right-radius: 15px;
                `}>
                <scrollview
                    ref={scrollView}
                    mode={ScrollViewMode.Vertical}
                    vertical-scroller-visibility={ScrollerVisibility.Hidden}
                    horizontal-scroller-visibility={ScrollerVisibility.Hidden}
                    class={emo`
                        padding: 50px;
                        height: auto;
                    `}>
                    {currentDialogue.map((text, i, a) => i <= index ? <div class={emo`
                            color: ${i === index ? "#fff" : "rgba(255, 255, 255, " + clamp(1 - ((index - i) / (index + 1)), .1, .75) + ")"};
                            font-size: 20px;
                            margin-top: ${i == 0 ? "100px" : "0"};
                            padding-top: ${index === i ? "35px" : "10px"};
                            transition: color .4s ease-in-out, scale .4s ease-in-out;
                            scale: ${i === index ? "1" : "0.8"};
                            transform-origin: left;
                        `}>
                        {text}
                    </div> : null)}
                </scrollview>
            </div>
        </div>
    ) : null
}

export default Dialogue