document.getElementById('convertButton').addEventListener('click', async function (event) {
    event.preventDefault();

    const mdText = document.getElementById('markdownForm').value;

    if (!mdText.trim()) {
        alert("Введите текст для конвертации!");
        return;
    }

    try {
        const response = await fetch('Converter/convert', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ text: mdText })
        });

        if (!response.ok) throw new Error("Ошибка сервера");

        const data = await response.json();
        document.getElementById('htmlForm').value = data.html;
    } catch (error) {
        alert("Ошибка при отправке: " + error.message);
    }
});

document.getElementById('saveButton').addEventListener('click', async function () {
    const mdText = document.getElementById('markdownForm').value;
    const fileName = document.getElementById('inputNameFile').value.trim();

    if (!fileName) {
        alert("Введите название файла!");
        return;
    }

    if (!mdText.trim()) {
        alert("Нет данных для сохранения!");
        return;
    }
    try {
        const response = await fetch('/Converter/save', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ Text: mdText, Title: fileName})
        });

        if (!response.ok) throw new Error("Ошибка сервера");

        const data = await response.json();
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