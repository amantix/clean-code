﻿document.getElementById('markdownForm').addEventListener('submit', async function (event) {
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
    const htmlText = document.getElementById('HtmlText').value;
    const fileName = document.getElementById('inputNameFile').value.trim();

    if (!fileName) {
        alert("Введите название файла!");
        return;
    }

    if (!htmlText.trim()) {
        alert("Нет данных для сохранения!");
        return;
    }
    console.log("Отправляем JSON:", { htmlText, fileName });
    try {
        const response = await fetch('/Document/save', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ htmlText, fileName })
        });

        if (!response.ok) throw new Error("Ошибка сервера");

        const data = await response.json();
        alert("Данные успешно сохранены как " + fileName);
    } catch (error) {
        alert("Ошибка при сохранении: " + error.message);
    }
});