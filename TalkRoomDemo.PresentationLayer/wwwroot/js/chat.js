"use strict";

const currentUser = window.currentUser || "Bilinmiyor";
const currentUserProfileUrl = window.currentUserProfileUrl || "/Login/image/pp.jpg";

const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

document.addEventListener("DOMContentLoaded", () => {
    const messageForm = document.getElementById("messageForm");
    const messageInput = document.getElementById("messageInput");
    const messagesList = document.getElementById("messagesList");
    const sendButton = document.getElementById("sendButton");

    function scrollToBottom() {
        messagesList.scrollTop = messagesList.scrollHeight;
    }

    function appendMessage(user, profileUrl, message) {
        const li = document.createElement("li");
        li.classList.add(user === currentUser ? "mine" : "other");

        const img = document.createElement("img");
        img.classList.add("message-profile-pic");
        img.src = profileUrl;

        const contentDiv = document.createElement("div");
        contentDiv.classList.add("message-content");

        const usernameDiv = document.createElement("div");
        usernameDiv.classList.add("message-username");
        usernameDiv.textContent = user;

        const messageDiv = document.createElement("div");
        messageDiv.classList.add("message-text");
        messageDiv.textContent = message;

        contentDiv.appendChild(usernameDiv);
        contentDiv.appendChild(messageDiv);
        li.appendChild(img);
        li.appendChild(contentDiv);
        messagesList.appendChild(li);

        scrollToBottom();
    }

    connection.on("ReceiveMessage", (user, profileUrl, message) => {
        appendMessage(user, profileUrl, message);
    });

    connection.on("ReceiveFriendListUpdate", function () {
        console.log("arkadaş listesi güncellendi")
        $("#friendListContainer").load("/Friend/GetFriendPartial");
    });
    connection.on("RecaiveFriendRequstUpdate", function () {
        console.log("arkadaş davetleri güncellendi")
        $("notificationBox").load("/FriendRequest/GetFriendList");
    });
    connection.on("RecaiveFriendListUpdate", function () {
        console.log("Bildirim kutusu güncellendi")
        fetch("/FriendRequest/GetFriendList")
            .then(response => response.text())
            .then(html => {
                document.getElementById("notificationPanel").innerHTML = html;
            });
    });
   

    connection.on("UserOnline", function (user) {
        const element= document.querySelector(`[data-userid='${user}'] .status`);
        if (element) {
            element.classList.remove("offline");
            element.classList.add("online");
        }
    });
    connection.on("UserOffline", function (user) {
        const element = document.querySelector(`[data-userid='${user}'] .status`);
        if (element) {
            element.classList.remove("online");
            element.classList.add("offline");
        }
    });

    connection.on("ReceiveOnlineUsers", function (userIds) {
        userIds.forEach(user => {
            const element = document.querySelector(`[data-userid='${user}'] .status`);
            if (element) {
                element.classList.remove("offline");
                element.classList.add("online");
            }
        });
    });
    
    connection.on("FriendRequestResponse", function (userId, IsAccepted) {
        if (IsAccepted === 1) {
            alert("Arkadaşlık kabul edildi!");
            // burada arkadaş listeni AJAX ile yeniden çekebilirsin
        } else if (IsAccepted === 2) {
            alert("Arkadaşlık isteğin reddedildi.");
        }
    });

 
    connection.start().then(() => {
        console.log("SignalR connected.");

        connection.invoke("GetOnlineUsers");
    });

    function acceptRequest(requestId) {
        $.post("/FriendRequest/approvedInvite", { requestId: requestId })
            .done(function (data) {
                $(`form[data-request-id='${requestId}']`).remove();
                $("#friendListContainer").load("/Friend/GetFriendPartial");
                // Karşı tarafa kabul bilgisini gönder
                connection.invoke("RespondFriendRequest", requestId, 1);
            })
            .fail(function (err) {
                alert("Hata: " + err.responseText);
            });
    }

    function rejectRequest(requestId) {
        $.post("/FriendRequest/RejectInvite", { requestId: requestId })
            .done(function (data) {
                $(`form[data-request-id='${requestId}']`).remove();
                connection.invoke("RespondFriendRequest", requestId, 2);
            })
            .fail(function (err) {
                alert("Hata: " + err.responseText);
            });
    }



    function sendMessage() {
        const message = messageInput.value.trim();
        if (message === "") return;

        connection.invoke("SendMessage", currentUser, currentUserProfileUrl, message).catch(err => {
            console.error("Mesaj gönderme hatası:", err.toString());
        });

        messageInput.value = "";
        messageInput.focus();
    }

   
    sendButton.addEventListener("mousedown", (e) => {
        e.preventDefault(); // input'tan odak çıkmasını engeller
    });
    sendButton.addEventListener("click", (event) => {
        event.preventDefault();
        sendMessage();
        messageInput.focus();
    });
    messageInput.addEventListener("keydown", (e) => {
        if (e.key === "Enter" && !e.shiftKey) {
            e.preventDefault();
            sendMessage();
        }
    });
});
