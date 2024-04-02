mergeInto(LibraryManager.library, {
    OpenEditors: function (componentName) {
        window.openEditor(UTF8ToString(componentName));
    },
    SendRoomUnlockedEvent: function (roomId) {
        window.es.sendEvent(new RoomUnlockedEvent(roomId));
    },
    SendConversationFinishedEvent: function () {
        window.es.sendEvent(new ConversationFinishedEvent());
    }
});