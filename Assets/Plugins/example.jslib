mergeInto(LibraryManager.library, {
    OpenEditors: function (componentName) {
        window.openEditor(UTF8ToString(componentName));
    },
});