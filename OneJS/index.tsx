import { h, render } from "preact"
import Minigame from "minigame"
import Dialogue from "dialogue"
import { emo } from "onejs/styled"

const App = () => {
    return <div class={emo`
        height: 100%;
        width: 100%;
    `}>
        <Minigame />
        <Dialogue />
    </div>
}

render(<App />, document.body)