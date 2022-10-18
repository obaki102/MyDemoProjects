window.ScrollToBottom = (elementName) => {
    element = document.getElementById(elementName);
    if (element !== null) {
        element.scrollTop = element.scrollHeight - element.clientHeight;
    }
}

window.PlayAudioFile = (src) => {
    var audio = document.getElementById('audio-player');
    if (audio != null) {
        var audioSource = document.getElementById('playerSource');
        if (audioSource != null) {
            audioSource.src = src;
            audio.load();
            audio.play();
        }
    }
}