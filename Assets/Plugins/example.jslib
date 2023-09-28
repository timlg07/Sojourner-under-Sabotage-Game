mergeInto(LibraryManager.library, {
    _ShowMessage: function (str1, str2) {
        const valueDebug = UTF8ToString(str1);
        const valueTest = UTF8ToString(str2);
        window.monacoEditorDebug.setValue(valueDebug);
        window.monacoEditorTest.setValue(valueTest);
        document.getElementById('ui-overlay').style.display = "initial";
    },
});