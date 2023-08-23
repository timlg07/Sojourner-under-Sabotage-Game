"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var preact_1 = require("preact");
var App = function () {
    return ((0, preact_1.h)("div", null,
        (0, preact_1.h)("label", { text: "Select something to remove from your suitcase:" }),
        (0, preact_1.h)("box", null,
            (0, preact_1.h)("toggle", { name: "boots", label: "Boots", value: true }),
            (0, preact_1.h)("toggle", { name: "helmet", label: "Helmet", value: false }),
            (0, preact_1.h)("toggle", { name: "cloak", label: "Cloak of invisibility" })),
        (0, preact_1.h)("radiobuttongroup", null,
            (0, preact_1.h)("radiobutton", { name: "sword", label: "Sword", value: true }),
            (0, preact_1.h)("radiobutton", { name: "bow", label: "Bow", value: false }),
            (0, preact_1.h)("radiobutton", { name: "axe", label: "Axe", value: false })),
        (0, preact_1.h)("box", null,
            (0, preact_1.h)("button", { name: "cancel", text: "Cancel", onClick: function (e) {
                    log("Foo");
                    e.currentTarget.Blur();
                } }),
            (0, preact_1.h)("button", { name: "ok", text: "OK" }),
            (0, preact_1.h)("textfield", { onInput: function (e) { return log(e.newData); }, onKeyDown: function (e) { return log(e.keyCode); } }))));
};
(0, preact_1.render)((0, preact_1.h)(App, null), document.body);
