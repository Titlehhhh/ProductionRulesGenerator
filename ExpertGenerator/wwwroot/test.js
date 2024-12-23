function downloadFile(fileId) {
    const a = document.createElement('a');
    a.href = `/download/${fileId}`;
    a.target = '_blank';
    a.click();
}

