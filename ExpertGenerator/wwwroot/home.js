const uploadArea = document.getElementById('upload-area');
const fileInput = document.getElementById('file-upload');

uploadArea.addEventListener('dragover', (e) => {
    e.preventDefault();
    uploadArea.classList.add('dragover');
});

uploadArea.addEventListener('dragleave', () => {
    uploadArea.classList.remove('dragover');
});

function uploadFile(file) {
    const formData = new FormData();
    formData.append('file', file);
    console.log("clicked")
    fetch('/upload', {
        method: 'POST',        
        redirect: 'follow',
        body: formData
    }).then(r => {
        window.location.href = r.url
    });    
}

uploadArea.addEventListener('drop', (e) => {
    e.preventDefault();
    uploadArea.classList.remove('dragover');

    const files = e.dataTransfer.files;
    if (files.length) {
        const file = files[0];
        uploadFile(file);
    }
});

uploadArea.addEventListener('click', () => fileInput.click());

fileInput.addEventListener('change', () => {
    const file = fileInput.files[0];
    if (file) {
        uploadFile(file);
    }
});


