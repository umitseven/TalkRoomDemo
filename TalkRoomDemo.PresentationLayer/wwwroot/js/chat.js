"use strict";

const currentUser = window.currentUser || "Bilinmiyor";
const currentUserProfileUrl = window.currentUserProfileUrl || "/Login/image/pp.jpg";
const roomId = window.roomId;

const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

document.addEventListener("DOMContentLoaded", () => {
    const messageInput = document.getElementById("messageInput");
    const messagesList = document.getElementById("messagesList");
    const sendButton = document.getElementById("sendButton");

    const currentUserId = document.getElementById("currentUserId")?.value;
    const friendUserId = document.getElementById("friendUserId")?.value; // özel mesaj için

    // === UI Fonksiyonları ===
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

        if (user === currentUser) {
            li.appendChild(contentDiv);
            li.appendChild(img);
        } else {
            li.appendChild(img);
            li.appendChild(contentDiv);
        }
        messagesList.appendChild(li);
        scrollToBottom();
    }

    function addMessageToChat(senderId, message) {
        const li = document.createElement("li");
        li.textContent = `${senderId}: ${message}`;
        messagesList.appendChild(li);
        scrollToBottom();
    }

    // === SignalR Eventleri ===
    connection.on("ReceiveMessage", (user, profileUrl, message) => {
        appendMessage(user, profileUrl, message);
    });

    connection.on("UserRecaiverMessage", function (senderId, message) {
        addMessageToChat(senderId, message);
    });

    connection.on("ReceiveFriendListUpdate", function () {
        $("#friendListContainer").load("/Friend/GetFriendPartial");
    });
    connection.on("#", function () {
        $("#RoomFriends").load("/AddRoom/GetServerUsers");
    });

    connection.on("RecaiveFriendRequstUpdate", function () {
        $("notificationBox").load("/FriendRequest/GetFriendList");
    });

    connection.on("RecaiveFriendListUpdate", function () {
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
        } else if (IsAccepted === 2) {
            alert("Arkadaşlık isteğin reddedildi.");
        }
    });

    // === Mesaj Gönderme Fonksiyonları ===
    function sendRoomMessage() {
        const message = messageInput.value.trim();
        if (message === "") return;

        connection.invoke("SendMessage", roomId, currentUser, currentUserProfileUrl, message)
            .catch(err => console.error("Mesaj gönderme hatası:", err.toString()));

        messageInput.value = "";
        messageInput.focus();
    }

    function sendFriendMessage() {
        const text = messageInput.value.trim();
        if (text === "" || !friendUserId) return;

        connection.invoke("SendFriendMessage", friendUserId, text)
            .catch(err => console.error(err.toString()));

        addMessageToChat("Sen", text); // kendi mesajını ekle
        messageInput.value = "";
        messageInput.focus();
    }

    // === Event Binding ===
    sendButton.addEventListener("click", (event) => {
        event.preventDefault();
        if (friendUserId) {
            sendFriendMessage();
        } else {
            sendRoomMessage();
        }
    });

    messageInput.addEventListener("keydown", (e) => {
        if (e.key === "Enter" && !e.shiftKey) {
            e.preventDefault();
            if (friendUserId) {
                sendFriendMessage();
            } else {
                sendRoomMessage();
            }
        }
    });

    // === Bağlantıyı başlat ===
    connection.start()
        .then(() => {
            console.log("SignalR connected.");
            connection.invoke("GetOnlineUsers");
            connection.invoke("JoinRoom", roomId.toString());
        })
        .catch(err => console.error("SignalR connection error:", err.toString()));
});
