import { useEventfulState } from "onejs"
import { emo } from "onejs/styled"
import { h, render } from "preact"
import { useEffect, useState } from "preact/hooks"
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
            [1, 0, 0],
            [2, 1, 0],
            [0, 1, 2]
        ]
    },
}

const Puzzle = ({ roomId, puzzleSolved }: { roomId: number, puzzleSolved: () => void }) => {
    const currentPuzzle = puzzleData[roomId]
    const [board, setBoard] = useState(currentPuzzle.orientation)

    function checkRotation() {
        for (let i = 0; i < currentPuzzle.rows; i++) {
            for (let j = 0; j < currentPuzzle.cols; j++) {
                const solution = currentPuzzle.solution[i][j]
                if (solution !== 0 && solution !== board[i][j]) {
                    return
                }
            }
        }
        puzzleSolved()
    }

    useEffect(() => {
        checkRotation()
    }, [board])

    return <div class="w-full h-full flex justify-center items-center">
        <div class={emo`
            margin: 16px 32px;
            background-color: #aaa;
            padding: 32px;
            width: 100%;
            height: 100%;
            max-width: 600px;

            display: flex;
            flex-direction: column;
            justify-content: stretch;
        `}>
                {board.map((row, i) => <div class={emo`
                    flex: 1;
                    display: flex;
                    flex-direction: row;
                    justify-content: stretch;
                `}>
                    {row.map((cell, j) => <div class={emo`
                        flex: 1;
                        border: 1px solid #000;
                        background-color: #fff;
                        margin: 4px;
                    `} onClick={() => {
                        setBoard((prevBoard) => {
                            const newBoard = prevBoard.map(row => row.slice())
                            newBoard[i][j] = (newBoard[i][j] % 4) + 1
                            return newBoard
                        })
                    }}>{cell}</div>)}
                </div>)}
        </div>
        
        Room: {roomId}</div>
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