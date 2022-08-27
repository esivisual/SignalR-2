var chatBox = $('#ChatBox');

var Connection = new signalR.HubConnectionBuilder()
    .withUrl("/chathub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

Connection.start();

function showChatDialog() {
    chatBox.css("display", "block");
}

$(document).ready(function () {
    Init();
});

function Init() {
    setTimeout(showChatDialog, 1000);

    var NewMessageForm = $("#NewMessageForm");
    NewMessageForm.on("submit", function (e) {
        e.preventDefault();
        var Message = e.target[0].value;
        e.target[0].value = '';
        sendMessage(Message);
    });
}
//ارسال پیام به سرور
function sendMessage(text) {
    Connection.invoke('SendNewMessage', "Visitor", text)
}

//دریافت پیام از سرور
Connection.on('getNewMessage', getMessage);
function getMessage(sender, message, time) {
    $("#Messages").append("<li><div><span class='name'>" + sender + "</span><span class='time'>" + time + "</span></div><div class='message'>" + message + "</div></li>");
}
