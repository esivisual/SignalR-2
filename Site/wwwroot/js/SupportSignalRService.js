var activeRoomId = '';

var supportConnection = new signalR.HubConnectionBuilder()
    .withUrl("/supporthub")
    .build();


var chatRoomConnection = new signalR.HubConnectionBuilder()
    .withUrl("/chathub")
    .build();


function Init() {
    supportConnection.start();
    chatRoomConnection.start();

    var answerForm = $('#answerForm');

    answerForm.on('submit', function (e) {
        e.preventDefault();

        var text = e.target[0].value;
        e.target[0].value = '';

        sendMessage(text);

    });
}

function sendMessage(text) {
    if (text && text.length) {
        supportConnection.invoke('SendMessage', activeRoomId, text);
    }
}

chatRoomConnection.on('getNewMessage', showMessage);

$(document).ready(function () {
    Init();
})

supportConnection.on("getNewMessage", addMessage);

function addMessage(messages) {
    if (!messages) return;

    messages.forEach(function (m) {
        showMessage(m.sender, m.message, m.time);
    });
}

function showMessage(sender, message, time) {
    $('#chatMessage').append('<li><div><span class="name"> ' + sender + ' </span><span class="time">' + time + '</span></div><div class="message"> ' + message + ' </div></li>');
}
supportConnection.on('GetRooms', loadRooms);

function loadRooms(rooms) {
    if (!rooms) return;
    var roomId = Object.keys(rooms);
    if (!roomId.length) return;
    removeAllchildern(roomListEl);
    roomId.forEach(function (id) {
        var roomInfo = rooms[id];
        if (!roomInfo) return;
        //ایجاد دکمه برای لیست
        return $("#roomList").append("<a class='list-group-item list-group-item-action d-flex justify-content-between align-items-center' data-id='" + roomInfo + "' href='#'>" + roomInfo + "</a>");
    });
}

var roomListEl = document.getElementById('roomList');
var roomMessageEl = document.getElementById('chatMessage');

function removeAllchildern(node) {
    if (!node) return;
    while (node.lastChild) {
        node.removeChild(node.lastChild);
    }
}

function setActiveRoomButton(el) {
    var allButtons = roomListEl.querySelectorAll('a.list-group-item');

    allButtons.forEach(function (btn) {
        btn.classList.remove('active');
    });
    el.classList.add('active');
}

function switchActiveRoomTo(id) {
    if (id === activeRoomId) return;
    removeAllchildern(roomMessageEl);

    activeRoomId = id;
    chatRoomConnection.invoke('LeaveRoom', activeRoomId);

    chatRoomConnection.invoke('JoinRoom', activeRoomId);
    supportConnection.invoke('LoadMessage', activeRoomId);
}

roomListEl.addEventListener('click', function (e) {
    roomMessageEl.style.display = 'block';
    setActiveRoomButton(e.target);
    var roomId = e.target.getAttribute('data-id');
    switchActiveRoomTo(roomId);
});