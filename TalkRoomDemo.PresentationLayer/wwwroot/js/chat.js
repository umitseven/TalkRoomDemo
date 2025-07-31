"use strict";

const currentUser = window.currentUser || "Bilinmiyor";
const currentUserProfileUrl = "/Login/image/pp.jpg";

const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

document.addEventListener("DOMContentLoaded", () => {
    const messageForm = document.getElementById("messageForm");
    const messageInput = document.getElementById("messageInput");
    const messagesList = document.getElementById("messagesList");
    const sendButton = document.getElementById("sendButton");

    function scrollToBottom() {
        messagesList.scrollTop = messagesList.scrollHeight;
    }

    function appendMessage(user, message) {
        const li = document.createElement("li");
        li.classList.add(user === currentUser ? "mine" : "other");

        const img = document.createElement("img");
        img.classList.add("message-profile-pic");
        img.src = currentUserProfileUrl;

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

    connection.on("ReceiveMessage", (user, message) => {
        appendMessage(user, message);
    });

    connection.start().then(() => {
        sendButton.disabled = false;
    }).catch(err => {
        console.error("SignalR bağlantı hatası:", err.toString());
    });

    function sendMessage() {
        const message = messageInput.value.trim();
        if (message === "") return;

        connection.invoke("SendMessage", currentUser, message).catch(err => {
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
