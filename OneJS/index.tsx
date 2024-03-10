import { emo } from "onejs/styled"
import { h, render } from "preact"
import { useEffect, useState, useRef } from "preact/hooks"
import { Dom } from "OneJS/Dom"
const doorManager = require("doorManager")

const puzzleData = {
    1: {
        rows: 3,
        cols: 3,
        board: [
            [1, 1, 2],
            [2, 3, 1],
            [3, 1, 2]
        ],
        orientation: [
            [2, 2, 2],
            [1, 1, 2],
            [3, 2, 1]
        ],
        solution: [
            [1, 1, 4],
            [0, 0, 2],
            [0, 0, 2]
        ],
    },
}

const Puzzle = ({ roomId, puzzleSolved }: { roomId: number, puzzleSolved: () => void }) => {
    const currentPuzzle = puzzleData[roomId]
    const [board, setBoard] = useState(currentPuzzle.orientation)

    function checkRotation() {
        for (let i = 0; i < currentPuzzle.rows; i++) {
            for (let j = 0; j < currentPuzzle.cols; j++) {
                const solution = currentPuzzle.solution[i][j]
                const current = board[i][j]
                const isNotNeeded = solution === 0
                const isCorrect = solution === current
                const isLine = currentPuzzle.board[i][j] === 1
                const isLineRotatedButCorrect = isLine && (current + solution) % 2 === 0; // 1==3, 2==4
                if (isNotNeeded || isCorrect || isLineRotatedButCorrect) continue; else return
            }
        }
        puzzleSolved()
    }

    useEffect(() => {
        checkRotation()
    }, [board])

    const ref = useRef<Dom>()
    useEffect(() => {
        ref.current.parentNode.ve.generateVisualContent = OnGeometryChanged
        ref.current.parentNode.ve.MarkDirtyRepaint()
    }, [])

    function OnGeometryChanged() {
        const parent = ref.current.parentNode
        const parentWidth = parent.ve.layout.width
        const parentHeight = parent.ve.layout.height
        const parentAspectRatio = parentWidth / parentHeight
        const aspectRatio = currentPuzzle.cols / currentPuzzle.rows

        if (parentAspectRatio > aspectRatio) {
            ref.current.style.width = `${parentHeight * aspectRatio}px`
            ref.current.style.height = `${parentHeight}px`
        } else {
            ref.current.style.width = `${parentWidth}px`
            ref.current.style.height = `${parentWidth / aspectRatio}px`
        }
    }

    return <div class={emo`
        height: 100%;
        margin: 32px;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
    `}>
        <div ref={ref} class={emo`
            background-color: #aaa;
            padding: 32px;
            height: 100%;
        `}>
            {board.map((row, i) => <div class={emo`
                    display: flex;
                    flex-direction: row;
                    justify-content: center;
                    align-items: center;
                `}>
                {row.map((cell, j) => <div class={emo`
                            flex: 1;
                        `} onClick={() => {
                            setBoard((prevBoard) => {
                                const newBoard = prevBoard.map(row => row.slice())
                                newBoard[i][j] = (newBoard[i][j] % 4) + 1
                                return newBoard
                            })
                        }}><div class={emo`
                        position: relative;
                        padding-bottom: 100%;
                    `}>
                        <div class={emo`
                            position: absolute;
                            top: 0;
                            left: 0;
                            right: 0;
                            bottom: 0;
                            display: flex;
                            justify-content: center;
                            align-items: center;
                            margin: 2px;
                            rotate: ${cell === 1 ? "0deg" : cell === 2 ? "90deg" : cell === 3 ? "180deg" : "270deg"};
                        `} style={{backgroundImage: __dirname + `/img/minigame_${currentPuzzle.board[i][j] - 1}.png`}}></div>
                        {(i === 0 && j === 0) && <div class={emo`
                                position: absolute;
                                width: 32px;
                                height: 8px;
                                margin-top: 50%;
                                left: -32px;
                                background-color: #000;
                                translate: 0 -50%;
                            `}></div>}
                        {(i === board.length - 1 && j === board[i].length - 1) && <div class={emo`
                                position: absolute;
                                width: 32px;
                                height: 8px;
                                margin-top: 50%;
                                right: -32px;
                                background-color: #000;
                                translate: 0 -50%;
                            `}></div>}
                    </div>
                </div>)}
            </div>)}
        </div>
    </div>
}

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
        ? <Puzzle roomId={roomId} puzzleSolved={puzzleSolved} />
        : null
}

render(<App />, document.body)