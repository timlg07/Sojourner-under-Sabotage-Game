import { useEventfulState } from "onejs"
import { emo } from "onejs/styled"
import { render, h } from "preact"

let charman = require("charman")

const CharacterPanel = ({ character }) => {
    const [health] = useEventfulState(character, "Health")
    const [maxHealth] = useEventfulState(character, "MaxHealth")

    return <div class={emo`
        height: 80px;
        width: 200px;
        background-color: #333333;
        color: white;
        padding: 20px;
        border-radius: 5px;
        margin: 10px;
        align-items: center;
        justify-content: center;
    `}>
        <div>{character.gameObject.name}</div>
        <div>{`${parseInt(health)} / ${maxHealth}`}</div>
    </div>
}

const App = () => {
    const [characters] = useEventfulState(charman, "CharactersArray")

    return <div class={emo`
        height: 100%;
        width: 100%;
        display: flex;
        flex-wrap: wrap;
        flex-direction: row;
    `}>
        {characters.map((c) => <CharacterPanel character={c} />)}
    </div>
}

render(<App />, document.body)