console.log("UserId: " + userId, "RecvId: " + recvId);
// globalne promenljive
//var myEmail;

class MessageObjectJS {
    constructor(Id_send_user, Id_recv_user, message, date_time) {
        this.Id_send_user = Id_send_user;
        this.Id_recv_user = Id_recv_user;
        this.date_time = date_time;
        this.message = message;
    }
}

function PlaySound() {
    var snd = new Audio("../Content/ak47-1.wav"); // buffers automatically when created
    snd.play();
}

$(function () {
    $.ajax({
        type: "POST",
        dataType: "json",
        data: { "userId": userId, "recvId": recvId },
        url: "/Friends/AllMessages",
        chache: false,
        success: function (data) {
            console.log(data);
            var msgsDiv = $("#messagesDiv");
            for (var i = 0; i < data.length; i++) {
                if (data[i].Id_send_user == userId) {
                    msgsDiv.append("<div class='col-xs-10' style='float:right; background-color:forestgreen; border-radius: 2% / 10%'>" +
                        "<div class='col-xs-8' style='color:white'><label>" + data[i].message + "</label></div>" +
                        "<div class='col-xs-4'><img src='../Files/" + userImage + "' style='height:50px; width:50px; border-radius: 50% / 50%''/> </div>" + "</div> <br /><br />");
                }
                else {
                    msgsDiv.append("<div class='col-xs-10' style='float:left; background-color:#B8F9B8; border-radius: 2% / 10%'>" +
                        "<div class='col-xs-4'><img src='" + recvImage + "' style='height:50px; width:50px; border-radius: 50% / 50%' /> </div>" +
                        "<div class='col-xs-8' style='color:forestgreen'><label>" + data[i].message + "</label> </div>" + "</div> <br /><br />");
                }

            }
        }
    });

    var hub = $.connection.hubSignalR;

    //slanje poruke serveru i ostalim klijentima
    $.connection.hub.qs = { "userId": userId };                     //ovo sam dodala
    $.connection.hub.start().done(function () {
        console.log("Hub signalR started");
        $('#btnSend').click(function () {
            var message = $("#txtMessageInput").val();
            if (message == "")
                return;
            var msg = new MessageObjectJS(userId, recvId, message, "");
            hub.server.sendMessage(JSON.stringify(msg));
            $("#txtMessageInput").val("");
        });

    });


    // klijentske funkcije

    hub.client.newMessage = function (msgObjJSON) {
        var msgObj = jQuery.parseJSON(msgObjJSON);

        var msgsDiv = $("#messagesDiv");

        if (msgObj.Id_send_user == userId) {
            msgsDiv.append("<div class='col-xs-10' style='float:right; background-color:forestgreen; border-radius: 2% / 10%'>" +
                "<div class='col-xs-8' style='color:white'><label>" + msgObj.message + "</label></div>" +
                "<div class='col-xs-4'><img src='../Files/" + userImage + "' style='height:50px; width:50px; border-radius: 50% / 50%''/> </div>" + "</div> <br /><br />");
        }
        else if (chat == "Chat") {
            msgsDiv.append("<div class='col-xs-10' style='float:left; background-color:#B8F9B8; border-radius: 2% / 10%'>" +
                "<div class='col-xs-4'><img src='" + recvImage + "' style='height:50px; width:50px; border-radius: 50% / 50%' /> </div>" +
                "<div class='col-xs-8' style='color:forestgreen'><label>" + msgObj.message + "</label> </div>" + "</div> <br /><br />");
        } else {
            notifyMe(msgObj.message);
        }

        // if ($("input[name='cbxSound']").is(':checked'))
        PlaySound();

        msgsDiv.scrollTop(msgsDiv[0].scrollHeight);
    }
});

function searchkey(ele) {
    if (event.key === 'Enter') {
        $("#btnSend").click();
    }
}

function notifyMe(message) {
    if (Notification.permission !== "granted")
        Notification.requestPermission();
    else {
        var notification = new Notification("New message", {
            icon: 'http://cdn.sstatic.net/stackexchange/img/logos/so/so-icon.png',
            body: message,
        });

        notification.onclick = function () {
            window.open("http://stackoverflow.com/a/13328397/1269037");
        };

    }

}