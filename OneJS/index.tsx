import { useEventfulState } from "onejs"
import { h, render } from "preact"
const iot = require("iot")

const Progress = ({ progress }: { progress: number }) => {
    return <div class="w-full h-full flex justify-center items-center text-[#305fbc]" onClick={() => iot.OpenWebView()}>{progress}</div>
}

const App = () => {
    const [progress, _] = useEventfulState(iot, "Progress")
    return <Progress progress={progress} />
}

render(<App />, document.body)