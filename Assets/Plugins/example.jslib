mergeInto(LibraryManager.library, {
    _ShowMessage: function (content) {
        const id = 'ui-overlay';
        let element;
        if (!(element = document.getElementById(id))) {
            element = document.createElement('div');
            element.setAttribute('id', id);
            element.setAttribute('style', `
                position: fixed;
                inset: 0;
                max-width: 30rem;
                max-height: 30rem;
                margin: auto;
                background-color: white;
                display: grid;
                place-items: center;
            `)
            document.body.appendChild(element);
        }
        element.innerHTML = UTF8ToString(content);
    },
});