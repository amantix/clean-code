const inputText = document.getElementById('inputText');
const outputText = document.getElementById('outputText');
const convertBtn = document.getElementById('convertBtn');
const saveBtn = document.getElementById('saveBtn');
const downloadBtn = document.getElementById('downloadBtn');
const url = window.location.href.split('/');
const id = url[url.length - 1];
loadContent().then()
document.getElementById('downloadHtmlBtn').addEventListener('click', () => {
    // Получаем содержимое элемента
    const content = outputText.innerHTML;

    // Создаем Blob из содержимого
    const blob = new Blob([content], { type: 'text/html' });

    // Создаем ссылку для скачивания
    const url = URL.createObjectURL(blob);

    // Создаем временный элемент <a> для скачивания
    const a = document.createElement('a');
    a.href = url;
    a.download = `${id}.html`;  // Имя файла
    document.body.appendChild(a);

    // Программно нажимаем на ссылку
    a.click();

    // Удаляем ссылку из DOM
    document.body.removeChild(a);

    // Освобождаем объект URL
    URL.revokeObjectURL(url);
});
downloadBtn.addEventListener('click', async () => downloadDocument(id));
// Обработка нажатия на кнопку "Конвертировать"
convertBtn.addEventListener('click', async () => {
    const text = inputText.value;
    await convert(text);
});

// Обработка нажатия на кнопку "Сохранить"
saveBtn.addEventListener('click', async () => {
    const text = {text:inputText.value};

    const response2 = await fetch(`document/access/update/${id}`,{
        method: 'PATCH',
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify(text),
    });
    if (!response2.ok) {
        saveBtn.disabled = true;
    }
});

async function loadContent() {
    const response = await fetch(`document/access/${id}`);
    let data;
    if (response.ok) {
        data = await response.text();
        inputText.innerText = data;
        await convert(data);
    }
    else{
        document.body.textContent = '403';
    }

    const md = {text:data};
    const response2 = await fetch(`document/access/update/${id}`,{
        method: 'PATCH',
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify(md),
    });
    if (!response2.ok) {
        saveBtn.disabled = true;
    }
}
async function convert(text) {
    const md = {text: text};
    const response = await fetch('md/convert', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(md),
    });

    const html = await response.text();
    outputText.innerHTML = html;
}

async function downloadDocument(documentId) {
    try {
        const response = await fetch(`api/document/url/${documentId}`);
        if (!response.ok) throw new Error(await response.text());

        const blob = await response.blob();
        const contentDisposition = response.headers.get('Content-Disposition');
        let filename = `document_${documentId}.md`;

        if (contentDisposition) {
            const match = contentDisposition.match(/filename="(.+)"/);
            if (match && match[1]) {
                filename = match[1];
            }
        }

        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = filename;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
    } catch (error) {
        console.error('Ошибка при скачивании документа:', error);
        alert('Не удалось скачать документ.');
    }
}

async function downloadDocument(documentId) {
    try {
        const response = await fetch(`api/document/url/${documentId}`);
        if (!response.ok) throw new Error(await response.text());

        const blob = await response.blob();
        const contentDisposition = response.headers.get('Content-Disposition');
        let filename = `document_${documentId}.md`;

        if (contentDisposition) {
            const match = contentDisposition.match(/filename="(.+)"/);
            if (match && match[1]) {
                filename = match[1];
            }
        }

        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = filename;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
    } catch (error) {
        console.error('Ошибка при скачивании документа:', error);
        alert('Не удалось скачать документ.');
    }
}