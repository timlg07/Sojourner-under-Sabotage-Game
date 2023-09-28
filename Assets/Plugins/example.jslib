mergeInto(LibraryManager.library, {
    _ShowMessage: function (content) {
        const value = UTF8ToString(content);
        if (window.monacoEditor) {
            window.monacoEditor.setValue(value);
            document.getElementById('monaco-container').style.display = "initial";
        }
    },
});