"use strict";
var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
Object.defineProperty(exports, "__esModule", { value: true });
var color_palettes_1 = require("onejs/utils/color-palettes");
var color_parser_1 = require("onejs/utils/color-parser");
var preact_1 = require("preact");
var compat_1 = require("preact/compat");
var hooks_1 = require("preact/hooks");
var tweenjs_1 = require("tweenjs");
var UnityEngine_1 = require("UnityEngine");
var plane = UnityEngine_1.GameObject.CreatePrimitive(UnityEngine_1.PrimitiveType.Plane);
plane.GetComp(UnityEngine_1.MeshRenderer).material.color = (0, color_parser_1.namedColor)("maroon");
plane.transform.localScale = new UnityEngine_1.Vector3(10, 1, 10);
var cam = UnityEngine_1.GameObject.Find("Main Camera");
cam.transform.position = new UnityEngine_1.Vector3(0, 30, -60);
cam.transform.LookAt(new UnityEngine_1.Vector3(0, 10, 0));
UnityEngine_1.Physics.gravity = new UnityEngine_1.Vector3(0, -30, 0);
var balls = [];
spawnBalls();
function spawnBalls() {
    for (var i = 0; i < balls.length; i++) {
        UnityEngine_1.Object.Destroy(balls[i]);
    }
    balls = [];
    for (var i = 0; i < 50; i++) {
        createRandomBall();
    }
    setTimeout(spawnBalls, 15000);
}
function createRandomBall() {
    var ball = UnityEngine_1.GameObject.CreatePrimitive(UnityEngine_1.PrimitiveType.Sphere);
    ball.GetComp(UnityEngine_1.MeshRenderer).material.color = (0, color_parser_1.parseColor)(color_palettes_1.palettes[UnityEngine_1.Random.Range(0, 99)][2]);
    ball.transform.position = UnityEngine_1.Random.insideUnitSphere * 10 + new UnityEngine_1.Vector3(0, 30, 0);
    var rb = ball.AddComp(UnityEngine_1.Rigidbody);
    rb.collisionDetectionMode = UnityEngine_1.CollisionDetectionMode.Continuous;
    rb.drag = 0.3;
    var pm = new UnityEngine_1.PhysicMaterial();
    pm.bounciness = 1;
    ball.GetComp(UnityEngine_1.SphereCollider).material = pm;
    balls.push(ball);
}
var Dot = (0, compat_1.forwardRef)(function (props, ref) {
    var _a, _b;
    var color = (_a = props.color) !== null && _a !== void 0 ? _a : (0, color_parser_1.namedColor)("tomato");
    var size = (_b = props.size) !== null && _b !== void 0 ? _b : 80;
    var defaultOuterStyle = {
        width: size, height: size, backgroundColor: "white", borderRadius: size / 2, position: "Absolute", justifyContent: "Center", alignItems: "Center", left: -size / 2, top: -size / 2
    };
    var defaultInnerStyle = {
        width: size - 4, height: size - 4, backgroundColor: color, borderRadius: (size - 4) / 2,
        backgroundImage: props.image, unityBackgroundScaleMode: "ScaleAndCrop",
        justifyContent: "Center", alignItems: "Center", color: "white"
    };
    return ((0, preact_1.h)("div", { ref: ref, onPointerDown: props.onPointerDown, style: __assign(__assign({}, props.style), defaultOuterStyle) },
        (0, preact_1.h)("div", { style: defaultInnerStyle }, props.children)));
});
var App = function () {
    var ref = (0, hooks_1.useRef)();
    var dot1ref = (0, hooks_1.useRef)();
    var dot2ref = (0, hooks_1.useRef)();
    var _a = (0, hooks_1.useState)({ x: 0, y: 0 }), pos1 = _a[0], setPos1 = _a[1];
    var _b = (0, hooks_1.useState)({ x: 0, y: 0 }), pos2 = _b[0], setPos2 = _b[1];
    var _c = (0, hooks_1.useState)(false), inited = _c[0], setInited = _c[1];
    var pointerDowned = false;
    var offsetPosition = { x: 0, y: 0 };
    (0, hooks_1.useEffect)(function () {
        var width = ref.current.ve.resolvedStyle.width;
        var height = ref.current.ve.resolvedStyle.height;
        var p1 = { x: width / 6 * 2, y: height / 6 * 2 };
        var p2 = { x: width / 6 * 4, y: height / 6 * 4 };
        setInited(true);
        setPos1(p1);
        setPos2(p2);
        var tween = new tweenjs_1.Tween(p2).to({ x: p2.x, y: p2.y - height / 6 * 2 }, 5000)
            .easing(tweenjs_1.Easing.Quadratic.InOut).onUpdate(function () {
            dot2ref.current.style.translate = p2;
            ref.current.ve.MarkDirtyRepaint();
        }).repeat(Infinity).yoyo(true).start();
    }, []);
    (0, hooks_1.useEffect)(function () {
        ref.current.ve.generateVisualContent = onGenerateVisualContent;
        ref.current.ve.MarkDirtyRepaint();
    }, [pos2]);
    function onGenerateVisualContent(mgc) {
        var paint2D = mgc.painter2D;
        var x1 = pos1.x, y1 = pos1.y;
        var x2 = pos2.x, y2 = pos2.y;
        paint2D.strokeColor = UnityEngine_1.Color.white;
        paint2D.lineWidth = 10;
        paint2D.BeginPath();
        paint2D.MoveTo(new UnityEngine_1.Vector2(x1, y1));
        paint2D.BezierCurveTo(new UnityEngine_1.Vector2(x1 + 180, y1), new UnityEngine_1.Vector2(x2 - 180, y2), new UnityEngine_1.Vector2(x2, y2));
        paint2D.Stroke();
    }
    function onPointerDown(evt) {
        pointerDowned = true;
        offsetPosition = { x: evt.position.x - pos1.x, y: evt.position.y - pos1.y };
    }
    function onPointerUp(evt) {
        pointerDowned = false;
    }
    function onPointerLeave(evt) {
        pointerDowned = false;
    }
    function onPointerMove(evt) {
        if (!pointerDowned)
            return;
        pos1.x = evt.position.x - offsetPosition.x;
        pos1.y = evt.position.y - offsetPosition.y;
        dot1ref.current.style.translate = pos1;
        ref.current.ve.MarkDirtyRepaint();
    }
    return ((0, preact_1.h)("div", { ref: ref, onPointerUp: onPointerUp, onPointerLeave: onPointerLeave, onPointerMove: onPointerMove, style: { width: "100%", height: "100%" } },
        (0, preact_1.h)(Dot, { ref: dot1ref, onPointerDown: onPointerDown, style: { translate: pos1, display: inited ? "Flex" : "None" } }, "Drag Me"),
        (0, preact_1.h)(Dot, { ref: dot2ref, image: __dirname + "/controller.png", style: { translate: pos2, display: inited ? "Flex" : "None" } })));
};
(0, preact_1.render)((0, preact_1.h)(App, null), document.body);
function animate(time) {
    requestAnimationFrame(animate);
    (0, tweenjs_1.update)(time);
}
requestAnimationFrame(animate);
