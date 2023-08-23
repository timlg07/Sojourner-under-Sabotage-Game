import { Dom } from "OneJS/Dom"
import { palettes } from "onejs/utils/color-palettes"
import { namedColor, parseColor } from "onejs/utils/color-parser"
import { h, render } from "preact"
import { forwardRef } from "preact/compat"
import { useEffect, useRef, useState } from "preact/hooks"
import { ColorInfo, Style } from "preact/jsx"
import { Easing, Tween, update } from "tweenjs"
import { CollisionDetectionMode, Color, GameObject, MeshRenderer, PhysicMaterial, PrimitiveType, Random, Rigidbody, SphereCollider, Vector2, Vector3, Object, Physics, Time } from "UnityEngine"
import { MeshGenerationContext, PointerDownEvent, PointerLeaveEvent, PointerMoveEvent, PointerUpEvent } from "UnityEngine/UIElements"

let plane = GameObject.CreatePrimitive(PrimitiveType.Plane)
plane.GetComp(MeshRenderer).material.color = namedColor("maroon")
plane.transform.localScale = new Vector3(10, 1, 10)

var cam = GameObject.Find("Main Camera")
cam.transform.position = new Vector3(0, 30, -60)
cam.transform.LookAt(new Vector3(0, 10, 0))

Physics.gravity = new Vector3(0, -30, 0)

let balls: GameObject[] = []

spawnBalls()

function spawnBalls() {
    for (let i = 0; i < balls.length; i++) {
        Object.Destroy(balls[i])
    }
    balls = []
    for (let i = 0; i < 50; i++) {
        createRandomBall()
    }
    setTimeout(spawnBalls, 15000)
}

function createRandomBall() {
    let ball = GameObject.CreatePrimitive(PrimitiveType.Sphere)
    ball.GetComp(MeshRenderer).material.color = parseColor(palettes[Random.Range(0, 99)][2])
    ball.transform.position = (Random.insideUnitSphere as any) * 10 + (new Vector3(0, 30, 0) as any)
    let rb = ball.AddComp(Rigidbody)
    rb.collisionDetectionMode = CollisionDetectionMode.Continuous
    rb.drag = 0.3
    let pm = new PhysicMaterial()
    pm.bounciness = 1
    ball.GetComp(SphereCollider).material = pm
    balls.push(ball)
}

interface DotProps {
    children?: any
    color?: ColorInfo,
    image?: string
    size?: number
    style?: Style
    onPointerDown?: (evt: PointerDownEvent) => void
}

const Dot = forwardRef((props: DotProps, ref) => {
    const color = props.color ?? namedColor("tomato")
    const size = props.size ?? 80

    const defaultOuterStyle: Style = {
        width: size, height: size, backgroundColor: "white", borderRadius: size / 2, position: "Absolute", justifyContent: "Center", alignItems: "Center", left: -size / 2, top: -size / 2
    }

    const defaultInnerStyle: Style = {
        width: size - 4, height: size - 4, backgroundColor: color, borderRadius: (size - 4) / 2,
        backgroundImage: props.image, unityBackgroundScaleMode: "ScaleAndCrop",
        justifyContent: "Center", alignItems: "Center", color: "white"
    }

    return (
        <div ref={ref} onPointerDown={props.onPointerDown} style={{ ...props.style, ...defaultOuterStyle }}>
            <div style={defaultInnerStyle}>{props.children}</div>
        </div>
    )
})

const App = () => {
    const ref = useRef<Dom>();
    const dot1ref = useRef<Dom>();
    const dot2ref = useRef<Dom>();
    const [pos1, setPos1] = useState({ x: 0, y: 0 })
    const [pos2, setPos2] = useState({ x: 0, y: 0 })
    const [inited, setInited] = useState(false)

    let pointerDowned = false
    let offsetPosition = { x: 0, y: 0 }

    useEffect(() => {
        let width = ref.current.ve.resolvedStyle.width;
        let height = ref.current.ve.resolvedStyle.height;
        let p1 = { x: width / 6 * 2, y: height / 6 * 2 }
        let p2 = { x: width / 6 * 4, y: height / 6 * 4 }
        setInited(true)
        setPos1(p1)
        setPos2(p2)

        const tween = new Tween(p2).to({ x: p2.x, y: p2.y - height / 6 * 2 }, 5000)
            .easing(Easing.Quadratic.InOut).onUpdate(() => {
                dot2ref.current.style.translate = p2
                ref.current.ve.MarkDirtyRepaint();
            }).repeat(Infinity).yoyo(true).start()
    }, [])

    useEffect(() => {
        ref.current.ve.generateVisualContent = onGenerateVisualContent
        ref.current.ve.MarkDirtyRepaint()
    }, [pos2])

    function onGenerateVisualContent(mgc: MeshGenerationContext) {
        var paint2D = mgc.painter2D

        let { x: x1, y: y1 } = pos1
        let { x: x2, y: y2 } = pos2

        paint2D.strokeColor = Color.white
        paint2D.lineWidth = 10;
        paint2D.BeginPath()
        paint2D.MoveTo(new Vector2(x1, y1))
        paint2D.BezierCurveTo(new Vector2(x1 + 180, y1), new Vector2(x2 - 180, y2), new Vector2(x2, y2))
        paint2D.Stroke()
    }

    function onPointerDown(evt: PointerDownEvent) {
        pointerDowned = true
        offsetPosition = { x: evt.position.x - pos1.x, y: evt.position.y - pos1.y }
    }

    function onPointerUp(evt: PointerUpEvent) {
        pointerDowned = false
    }

    function onPointerLeave(evt: PointerLeaveEvent) {
        pointerDowned = false
    }

    function onPointerMove(evt: PointerMoveEvent) {
        if (!pointerDowned)
            return
        pos1.x = evt.position.x - offsetPosition.x
        pos1.y = evt.position.y - offsetPosition.y
        dot1ref.current.style.translate = pos1
        ref.current.ve.MarkDirtyRepaint();
    }

    return (
        <div ref={ref} onPointerUp={onPointerUp} onPointerLeave={onPointerLeave} onPointerMove={onPointerMove} style={{ width: "100%", height: "100%" }}>
            <Dot ref={dot1ref} onPointerDown={onPointerDown} style={{ translate: pos1, display: inited ? "Flex" : "None" }}>Drag Me</Dot>
            <Dot ref={dot2ref} image={__dirname + "/controller.png"} style={{ translate: pos2, display: inited ? "Flex" : "None" }} />
        </div>
    )
}

render(<App />, document.body)

function animate(time) {
    requestAnimationFrame(animate)
    update(time)
}
requestAnimationFrame(animate)