    let stream;
    let isMicOn = true;
    let audioTrack;
    let audioContext;
    let source;
    let gainNode;

    const volumePopup = document.getElementById("volume-popup");
    const volumeSlider = document.getElementById("volumeSlider");
    const muteCheckbox = document.getElementById("muteCheckbox");
    const chatBox = document.querySelector('.chat-box');

    chatBox.scrollTop = chatBox.scrollHeight;
    async function initMic() {
        if (!stream) {
        stream = await navigator.mediaDevices.getUserMedia({ audio: true });
    audioTrack = stream.getAudioTracks()[0];

    audioContext = new AudioContext();
    source = audioContext.createMediaStreamSource(stream);
    gainNode = audioContext.createGain();

    source.connect(gainNode).connect(audioContext.destination);
        }
    }

    function toggleVolumePopup() {
        volumePopup.style.display = volumePopup.style.display === 'flex' ? 'none' : 'flex';
    initMic();
    }

    volumeSlider.addEventListener("input", () => {
        if (gainNode) {
        gainNode.gain.value = volumeSlider.value;
        }
    });

    muteCheckbox.addEventListener("change", () => {
        if (audioTrack) {
        audioTrack.enabled = !muteCheckbox.checked;
        }
    });

    // Dışarı tıklayınca pencereyi kapat
    document.addEventListener("click", function (event) {
        const isClickInside = volumePopup.contains(event.target) || event.target.classList.contains("mic-btn");
    if (!isClickInside) {
        volumePopup.style.display = "none";
        }
    });

const openModalBtn = document.getElementById('openModalBtn');
const roomModal = document.getElementById('roomModal');
const closeModalBtn = document.getElementById('closeModalBtn');
const createRoomForm = document.getElementById('createRoomForm');


document.addEventListener("DOMContentLoaded", function () {
    const openModalBtn2 = document.getElementById('openModalBtn2');
    const roomModal2 = document.getElementById('roomModal2');
    const closeModalBtn2 = document.getElementById('closeModalBtn2');
    const createRoomForm2 = document.getElementById('createRoomForm2');
    const roomNameInput = document.getElementById('roomName2');

    openModalBtn2.addEventListener('click', () => {
        // Backend'den gelen kodu inputa ata
        roomNameInput.value = window.serverInviteCode || "#11111"; // fallback kod
        roomModal2.style.display = 'flex';
        roomNameInput.focus(); // input'a odaklan
    });

    closeModalBtn2.addEventListener('click', () => {
        roomModal2.style.display = 'none';
    });

    window.addEventListener('click', (e) => {
        if (e.target === roomModal2) {
            roomModal2.style.display = 'none';
        }
    });

    createRoomForm2.addEventListener('submit', async (e) => {
        e.preventDefault();
        try {
            await navigator.clipboard.writeText(roomNameInput.value);
            alert("Kopyalandı: " + roomNameInput.value);
        } catch (err) {
            roomNameInput.select();
            roomNameInput.setSelectionRange(0, 99999);
            document.execCommand("copy");
            ashowCopyNotification();
        }
        roomModal2.style.display = 'none';
    });
});
function showCopyNotification() {
    const notification = document.getElementById('copyNotification');
    notification.style.opacity = '1';
    notification.style.bottom = '40px'; // yukarı doğru hareket
    notification.style.pointerEvents = 'auto';

    setTimeout(() => {
        notification.style.opacity = '0';
        notification.style.bottom = '20px';
        notification.style.pointerEvents = 'none';
    }, 2000); // 5 saniye sonra gizle
}

openModalBtn.addEventListener('click', () => {
    roomModal.style.display = 'flex';
});

closeModalBtn.addEventListener('click', () => {
    roomModal.style.display = 'none';
});

window.addEventListener('click', (e) => {
    if (e.target === roomModal) {
        roomModal.style.display = 'none';
    }
   
});


createRoomForm.addEventListener('submit', (e) => {
    e.preventDefault();
    const roomName = document.getElementById('roomName').value.trim();
    if (roomName) {
        // Buraya odanın oluşturulma kodunu ekleyebilirsin.
        roomModal.style.display = 'none';
        createRoomForm.reset();
    }
});


