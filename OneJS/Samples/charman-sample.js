"use strict";
var __makeTemplateObject = (this && this.__makeTemplateObject) || function (cooked, raw) {
    if (Object.defineProperty) { Object.defineProperty(cooked, "raw", { value: raw }); } else { cooked.raw = raw; }
    return cooked;
};
Object.defineProperty(exports, "__esModule", { value: true });
var onejs_1 = require("onejs");
var styled_1 = require("onejs/styled");
var preact_1 = require("preact");
var charman = require("charman");
var CharacterPanel = function (_a) {
    var character = _a.character;
    var health = (0, onejs_1.useEventfulState)(character, "Health")[0];
    var maxHealth = (0, onejs_1.useEventfulState)(character, "MaxHealth")[0];
    return (0, preact_1.h)("div", { class: (0, styled_1.emo)(templateObject_1 || (templateObject_1 = __makeTemplateObject(["\n        height: 80px;\n        width: 200px;\n        background-color: #333333;\n        color: white;\n        padding: 20px;\n        border-radius: 5px;\n        margin: 10px;\n        align-items: center;\n        justify-content: center;\n    "], ["\n        height: 80px;\n        width: 200px;\n        background-color: #333333;\n        color: white;\n        padding: 20px;\n        border-radius: 5px;\n        margin: 10px;\n        align-items: center;\n        justify-content: center;\n    "]))) },
        (0, preact_1.h)("div", null, character.gameObject.name),
        (0, preact_1.h)("div", null, "".concat(parseInt(health), " / ").concat(maxHealth)));
};
var App = function () {
    var characters = (0, onejs_1.useEventfulState)(charman, "CharactersArray")[0];
    return (0, preact_1.h)("div", { class: (0, styled_1.emo)(templateObject_2 || (templateObject_2 = __makeTemplateObject(["\n        height: 100%;\n        width: 100%;\n        display: flex;\n        flex-wrap: wrap;\n        flex-direction: row;\n    "], ["\n        height: 100%;\n        width: 100%;\n        display: flex;\n        flex-wrap: wrap;\n        flex-direction: row;\n    "]))) }, characters.map(function (c) { return (0, preact_1.h)(CharacterPanel, { character: c }); }));
};
(0, preact_1.render)((0, preact_1.h)(App, null), document.body);
var templateObject_1, templateObject_2;
