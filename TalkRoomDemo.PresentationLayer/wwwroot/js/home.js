    let stream;
    let isMicOn = true;
    let audioTrack;
    let audioContext;
    let source;
    let gainNode;

    const volumePopup = document.getElementById("volume-popup");
    const volumeSlider = document.getElementById("volumeSlider");
    const muteCheckbox = document.getElementById("muteCheckbox");

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
        alert(`Yeni oda oluşturuldu: ${roomName}`);
        // Buraya odanın oluşturulma kodunu ekleyebilirsin.
        roomModal.style.display = 'none';
        createRoomForm.reset();
    }
});

