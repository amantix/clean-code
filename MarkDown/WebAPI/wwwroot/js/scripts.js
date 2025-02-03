document.getElementById('markdownForm').addEventListener('submit', async function (event) {
    event.preventDefault();

    const markdownText = document.getElementById('markdownText').value;

    if (!markdownText.trim()) {
        alert("Введите текст для конвертации!");
        return;
    }

    try {
        const response = await fetch('/MarkDown/convert', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ text: markdownText })
        });

        if (!response.ok) throw new Error("Ошибка сервера");

        const data = await response.json();
        document.getElementById('HtmlText').value = data.result;
    } catch (error) {
        alert("Ошибка при отправке: " + error.message);
    }
});

document.getElementById('saveButton').addEventListener('click', async function () {
    const mdText = document.getElementById('markdownText').value;
    const fileName = document.getElementById('inputNameFile').value.trim();

    if (!fileName) {
        alert("Введите название файла!");
        return;
    }

    if (!mdText.trim()) {
        alert("Нет данных для сохранения!");
        return;
    }
    console.log("Отправляем JSON:", { mdText, fileName });
    try {
        const response = await fetch('/Document/save', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ mdText, fileName })
        });

        if (!response.ok) throw new Error("Ошибка сервера");

        const data = await response.json();
        alert("Данные успешно сохранены как " + fileName);
    } catch (error) {
        alert("Ошибка при сохранении: " + error.message);
    }
});

document.getElementById('deleteButton').addEventListener('click', async function () {
    const fileName = document.getElementById('inputNameFile').value.trim();

    if (!fileName) {
        alert("Введите название файла для удаления!");
        return;
    }

    if (!confirm(`Вы уверены, что хотите удалить файл "${fileName}"?`)) {
        return;
    }

    try {
        const response = await fetch(`/Document/delete?fileName=${fileName}`, {
            method: 'DELETE'
        });

        if (!response.ok) throw new Error("Ошибка сервера");
        alert("Файл успешно удален: " + fileName);
    } catch (error) {
        alert("Ошибка при удалении: " + error.message);
    }
});

document.getElementById('showListButton').addEventListener('click', async function () {
    try {
        const response = await fetch('/Document/all', {
            method: 'GET'
        });

        if (!response.ok) throw new Error("Ошибка сервера");

        const data = await response.json();
        const listDocuments = document.getElementById('listDocuments');
        listDocuments.innerHTML = ''; 

        if (data.length === 0) {
            listDocuments.innerHTML = '<li class="item-List">Список пуст</li>';
            return;
        }

        data.forEach(fileName => {
            const listItem = document.createElement('li');
            listItem.textContent = fileName;
            listItem.classList.add('item-List');
            listDocuments.appendChild(listItem);
        });
    } catch (error) {
        alert("Ошибка при получении списка: " + error.message);
    }
});

