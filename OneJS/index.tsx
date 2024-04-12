import { h, render } from "preact"
import Minigame from "minigame"
import Dialogue from "dialogue"

const App = () => {
    return <div>
        <Minigame />
        <Dialogue />
    </div>
}

render(<App />, document.body)