// Функция для получения параметра из URL
function getQueryParam(param) {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get(param);
}

// При загрузке страницы
window.addEventListener('DOMContentLoaded', async () => {
    const inputField = document.getElementById('input_text_field');
    const outputField = document.getElementById('output_text_field');
    const documentId = getQueryParam('documentId');

    if (documentId) {
        try {
            const response = await fetch(`http://localhost:5163/Document/Get/md/${documentId}`);

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(`Error ${response.status}: ${errorText}`);
            }

            const text = await response.text();
            inputField.value = text;
        } catch (error) {
            alert(`Произошла ошибка при загрузке документа: ${error.message}`);
        }

        try {
            const response = await fetch(`http://localhost:5163/Document/Get/html/${documentId}`);

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(`Error ${response.status}: ${errorText}`);
            }

            const text = await response.text();
            outputField.innerHTML = text;
        } catch (error) {
            alert(`Произошла ошибка при загрузке документа: ${error.message}`);
        }
    }
});

// Логика конвертации
document.getElementById('convert_button').addEventListener('click', async () => {
    const inputField = document.getElementById('input_text_field');
    const outputField = document.getElementById('output_text_field');

    const mdText = inputField.value;

    try {
        const response = await fetch('http://localhost:5163/Processor/Convert', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({mdText}),
        });

        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(`Error ${response.status}: ${errorText}`);
        }

        const responseJson = await response.json();
        const result = responseJson.html;
        outputField.innerHTML = result;
    } catch (error) {
        alert(`Произошла ошибка: ${error.message}`);
    }
});

document.getElementById('save_button').addEventListener('click', async () => {
    const inputField = document.getElementById('input_text_field');
    const documentId = new URLSearchParams(window.location.search).get('documentId');

    if (documentId) {
        const text = inputField.value;

        try {
            const response = await fetch(`http://localhost:5163/Document/Update`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ documentId, text }),
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(`Error ${response.status}: ${errorText}`);
            }

            alert('Документ успешно сохранён!');
        } catch (error) {
            alert(`Произошла ошибка при сохранении: ${error.message}`);
        }
    } else {
        alert('Не найден documentId в параметрах URL.');
    }
});
