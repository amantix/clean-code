
document.addEventListener("DOMContentLoaded", async () => {await UpdateUserInfo()} )

// Метод для показа личных документов и скрытия доступных
function showPersonalDocuments() {
    document.getElementById('personal-documents').style.display = 'flex';
    document.getElementById('available-documents').style.display = 'none';
}

// Метод для показа доступных документов и скрытия личных
function showAvailableDocuments() {
    document.getElementById('personal-documents').style.display = 'none';
    document.getElementById('available-documents').style.display = 'flex';
}

// Изначально скрываем доступные документы
document.addEventListener('DOMContentLoaded', function() {
    showPersonalDocuments(); // По умолчанию показываем личные документы
});

// Глобальные переменные для хранения документов
let userDocs = []; // Личные документы пользователя
let sharedDocs = []; // Документы, доступные пользователю

// Функция отображения формы регистрации
function showRegistrationForm() {
    const overlay = createOverlay();
    const popup = createPopup();
    const form = createRegistrationForm(() => removeOverlay(overlay));

    popup.appendChild(form);
    overlay.appendChild(popup);
    document.body.appendChild(overlay);
}

// Функция отображения формы входа
function showLoginForm() {
    const overlay = createOverlay();
    const popup = createPopup();
    const form = createLoginForm(() => removeOverlay(overlay));

    popup.appendChild(form);
    overlay.appendChild(popup);
    document.body.appendChild(overlay);
}

// Функция создания затемнения фона
function createOverlay() {
    const overlay = document.createElement('div');
    overlay.className = "overlay";
    return overlay;
}

// Функция удаления overlay
function removeOverlay(overlay) {
    document.body.removeChild(overlay);
}

// Функция создания всплывающего окна
function createPopup() {
    const popup = document.createElement('div');
    popup.className = "popup";

    const closeButton = document.createElement('button');
    closeButton.textContent = "×";
    closeButton.className = "close-button";
    closeButton.addEventListener('click', () => {
        removeOverlay(document.querySelector('.overlay'));
    });

    popup.appendChild(closeButton);
    return popup;
}

// Функция создания формы регистрации
function createRegistrationForm(onClose) {
    const form = document.createElement('form');
    form.className = "new-document-form";

    const loginField = createInputField('text', 'login', 'Логин');
    const passwordField = createInputField('password', 'password', 'Пароль');
    const submitButton = createSubmitButton('Зарегистрироваться');

    form.appendChild(loginField);
    form.appendChild(passwordField);
    form.appendChild(submitButton);

    form.addEventListener('submit', async (event) => {
        event.preventDefault();
        const formData = new FormData(form);
        const data = Object.fromEntries(formData.entries());

        try {
            const response = await fetch('http://localhost:5163/User/Register', {
                method: 'POST',
                credentials: 'include',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(data),
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(errorText);
            }

            alert('Регистрация прошла успешно!');
            onClose();
        } catch (error) {
            alert(`Ошибка регистрации: ${error.message}`);
        }
    });

    return form;
}

// Функция создания формы входа
function createLoginForm(onClose) {
    const form = document.createElement('form');
    form.className = "new-document-form";

    const loginField = createInputField('text', 'login', 'Логин');
    const passwordField = createInputField('password', 'password', 'Пароль');
    const submitButton = createSubmitButton('Войти');
    
    form.appendChild(loginField);
    form.appendChild(passwordField);
    form.appendChild(submitButton);

    form.addEventListener('submit', async (event) => {
        event.preventDefault();
        const formData = new FormData(form);
        const data = Object.fromEntries(formData.entries());

        try {
            const response = await fetch('http://localhost:5163/User/Login', {
                method: 'POST',
                credentials: 'include',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(data),
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(errorText);
            }

            alert('Вход выполнен успешно!');
            onClose();
        } catch (error) {
            alert(`Ошибка входа: ${error.message}`);
        } finally {
            await UpdateUserInfo(); // Обновляем информацию о пользователе после входа
        }
    });

    return form;
}

// Функция создания поля ввода
function createInputField(type, name, placeholder) {
    const input = document.createElement('input');
    input.type = type;
    input.name = name;
    input.placeholder = placeholder;
    return input;
}

// Функция создания кнопки отправки
function createSubmitButton(text) {
    const button = document.createElement('button');
    button.type = 'submit';
    button.textContent = text;
    return button;
}

// Метод для обновления информации о пользователе
async function UpdateUserInfo() {
    try {
        // Запрос личных документов
        const myDocsResponse = await fetch('http://localhost:5163/Document/My', {
            credentials: 'include',
        });
        if (!myDocsResponse.ok) {
            throw new Error('Ошибка при получении личных документов');
        }
        userDocs = await myDocsResponse.json();

        // Запрос доступных документов
        const sharedDocsResponse = await fetch('http://localhost:5163/Document/Shared', {
            credentials: 'include',
        });
        if (!sharedDocsResponse.ok) {
            throw new Error('Ошибка при получении доступных документов');
        }
        sharedDocs = await sharedDocsResponse.json();

        // Обновляем интерфейс
        updateDocumentLists();
    } catch (error) {
        console.error('Ошибка при обновлении информации о пользователе:', error);
    }
}

// Функция для обновления списков документов в интерфейсе
function updateDocumentLists() {
    const personalDocumentsContainer = document.getElementById('personal-documents');
    const availableDocumentsContainer = document.getElementById('available-documents');

    // Очищаем контейнеры
    personalDocumentsContainer.innerHTML = '';
    availableDocumentsContainer.innerHTML = '';

    // Добавляем личные документы
    userDocs.forEach((doc, index) => {
        const card = createDocumentCard(doc, `pd_${doc.id}`);
        personalDocumentsContainer.appendChild(card);
    });

    // Добавляем доступные документы
    sharedDocs.forEach((doc, index) => {
        const card = createDocumentCard(doc, `sd_${doc.id}`);
        availableDocumentsContainer.appendChild(card);
    });
}

// Функция для создания карточки документа
function createDocumentCard(doc, cardId) {
    const card = document.createElement('div');
    card.className = 'document-card';
    card.id = cardId;

    const documentInfo = document.createElement('div');
    documentInfo.className = 'document-info';

    const documentName = document.createElement('span');
    documentName.className = 'document-name';
    documentName.textContent = doc.name;

    const documentAuthor = document.createElement('span');
    documentAuthor.className = 'document-author';
    documentAuthor.textContent = `Автор: ${doc.authorName}`;

    documentInfo.appendChild(documentName);
    documentInfo.appendChild(documentAuthor);

    const documentActions = document.createElement('div');
    documentActions.className = 'document-actions';

    const accessButton = document.createElement('button');
    accessButton.className = 'access-button';
    accessButton.textContent = 'Доступ';
    
    accessButton.onclick = () => showAccessManagementForm(doc.id)

    const openButton = document.createElement('button');
    openButton.className = 'open-button';
    openButton.textContent = 'Открыть';

    // Привязываем метод переадресации
    openButton.onclick = () => {
        window.location.href = `/Page/Convertor?documentId=${doc.id}`;
    };

    const downloadButton = document.createElement('button');
    downloadButton.className = 'download-button';
    downloadButton.textContent = 'Скачать';
    downloadButton.style.backgroundColor = "blue";
    downloadButton.style.color = "white";
    downloadButton.onclick = () => createDownloadPopup(doc.id);

    const deleteButton = document.createElement('button');
    deleteButton.className = 'delete-button';
    deleteButton.textContent = 'Удалить';
    deleteButton.style.backgroundColor = 'red';
    deleteButton.style.color = 'white';
    deleteButton.onclick = () => removeCard(doc.id);

    const renameButton = createRenameButton(doc.id);

    documentActions.appendChild(accessButton);
    documentActions.appendChild(openButton);
    documentActions.appendChild(deleteButton);
    documentActions.appendChild(renameButton);
    documentActions.appendChild(downloadButton);

    card.appendChild(documentInfo);
    card.appendChild(documentActions);

    return card;
}

// Функция удаления карточки документа
async function removeCard(documentId) {
    try {
        const response = await fetch('http://localhost:5163/Document/Delete', {
            method: 'DELETE',
            credentials: 'include',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ documentId: documentId }),
        });

        if (!response.ok) {
            throw new Error(await response.text());
        }

        // Удаляем карточки с указанным documentId
        const personalCard = document.getElementById(`pd_${documentId}`);
        const sharedCard = document.getElementById(`sd_${documentId}`);

        if (personalCard) personalCard.remove();
        if (sharedCard) sharedCard.remove();

        alert('Документ успешно удалён!');
    } catch (error) {
        console.error('Ошибка при удалении документа:', error);
        alert(`Не удалось удалить документ: ${error.message}`);
    }
}



// Функция отображения формы создания нового документа
function showCreateDocumentForm() {
    const overlay = createOverlay();
    const popup = createPopup();
    const form = createNewDocumentForm(() => removeOverlay(overlay));

    popup.appendChild(form);
    overlay.appendChild(popup);
    document.body.appendChild(overlay);
}

// Функция создания формы нового документа
function createNewDocumentForm(onClose) {
    const form = document.createElement('form');
    form.className = 'new-document-form';

    const nameField = createInputField('text', 'name', 'Имя документа');
    const submitButton = createSubmitButton('Создать');

    form.appendChild(nameField);
    form.appendChild(submitButton);

    form.addEventListener('submit', async (event) => {
        event.preventDefault();
        const formData = new FormData(form);
        const data = Object.fromEntries(formData.entries());

        try {
            const response = await fetch('http://localhost:5163/Document/New', {
                method: 'POST',
                credentials: 'include',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(data),
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(errorText);
            }

            const newDocumentId = await response.text();
            
            // Редирект на Page/Convertor с передачей documentId
            window.location.href = `/Page/Convertor?documentId=${newDocumentId.replaceAll('\"', '')}`;
        } catch (error) {
            alert(`Ошибка создания документа: ${error.message}`);
        }
    });

    return form;
}

// Функция для создания кнопки переименования
function createRenameButton(docId) {
    const renameButton = document.createElement('button');
    renameButton.className = 'rename-button';
    renameButton.textContent = 'Переименовать';
    renameButton.style.backgroundColor = 'purple';
    renameButton.style.color = 'white';
    renameButton.onclick = () => showRenameForm(docId);
    return renameButton;
}

// Функция отображения формы переименования
function showRenameForm(documentId) {
    const overlay = createOverlay();
    const popup = createPopup();
    const form = createRenameForm(documentId, () => removeOverlay(overlay));

    popup.appendChild(form);
    overlay.appendChild(popup);
    document.body.appendChild(overlay);
}

// Функция создания формы переименования документа
function createRenameForm(documentId, onClose) {
    const form = document.createElement('form');
    form.className = 'rename-document-form';

    const nameField = createInputField('text', 'newName', 'Новое имя документа');
    const submitButton = createSubmitButton('Подтвердить');

    form.appendChild(nameField);
    form.appendChild(submitButton);

    form.addEventListener('submit', async (event) => {
        event.preventDefault();
        const formData = new FormData(form);
        const { newName } = Object.fromEntries(formData.entries());

        if (!newName) {
            alert('Введите новое имя документа!');
            return;
        }

        try {
            const response = await fetch('/Document/Rename', {
                method: 'PATCH',
                credentials: 'include',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ documentId, text: newName }),
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(errorText);
            }

            alert('Документ успешно переименован!');
            updateDocumentName(documentId, newName);
            onClose();
        } catch (error) {
            alert(`Ошибка: ${error.message}`);
        }
    });

    return form;
}

// Функция обновления имени документа в карточке
function updateDocumentName(documentId, newName) {
    const card = document.getElementById(`pd_${documentId}`) || document.getElementById(`sd_${documentId}`);
    if (card) {
        const nameElement = card.querySelector('.document-name');
        if (nameElement) nameElement.textContent = newName;
    }
}

function showAccessManagementForm(documentId) {
    const overlay = createOverlay();
    const popup = createPopup();
    const form = createAccessManagementForm(documentId, () => removeOverlay(overlay));

    popup.appendChild(form);
    overlay.appendChild(popup);
    document.body.appendChild(overlay);
}

// Функция создания формы управления доступом
function createAccessManagementForm(documentId, onClose) {
    const form = document.createElement('form');
    form.className = 'access-management-form';

    // Поле для ввода логина пользователя
    const userLoginField = createInputField('text', 'userLogin', 'Логин пользователя');
    form.appendChild(userLoginField);

    // Селект с вариантами разрешить/запретить доступ
    const accessOptionsField = document.createElement('select');
    accessOptionsField.name = 'accessOptions';
    accessOptionsField.className = 'access-options-field';

    const viewerOption = document.createElement('option');
    viewerOption.value = '1';
    viewerOption.textContent = 'Читатель';
    accessOptionsField.appendChild(viewerOption);

    const editorOption = document.createElement('option');
    editorOption.value = '2';
    editorOption.textContent = 'Редактор';
    accessOptionsField.appendChild(editorOption);

    const noneOption = document.createElement('option');
    noneOption.value = '0';
    noneOption.textContent = 'Запретить доступ';
    accessOptionsField.appendChild(noneOption);

    form.appendChild(accessOptionsField);

    // Кнопка подтверждения
    const submitButton = createSubmitButton('Подтвердить');
    form.appendChild(submitButton);

    form.addEventListener('submit', async (event) => {
        event.preventDefault();
        const formData = new FormData(form);
        const { userLogin, accessOptions } = Object.fromEntries(formData.entries());
        
        if (!userLogin) {
            alert('Введите логин пользователя!');
            return;
        }
        
        var accessLevel = Number(accessOptions)

        const url = '/Access/Change';
        const payload = {
            userLogin,
            documentId,
            accessLevel
        };

        try {
            const response = await fetch(url, {
                method: 'POST',
                credentials: 'include',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload),
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(errorText);
            }

            alert('Действие выполнено успешно!');
            onClose(); // Закрываем форму при успешной операции
        } catch (error) {
            alert(`Ошибка: ${error.message}`);
        }
    });

    return form;
}

async function downloadDocument(documentId, format) {
    try {
        const endpoint = `/Document/Get/${format}/${documentId}`;
        const response = await fetch(endpoint, { credentials: 'include' });
        if (!response.ok) throw new Error(await response.text());

        const blob = await response.blob();
        const contentDisposition = response.headers.get('Content-Disposition');
        let filename = `document_${documentId}.${format}`; // Значение по умолчанию

        if (contentDisposition && contentDisposition.includes('filename=')) {
            // Извлекаем имя файла из заголовка
            const filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
            const matches = filenameRegex.exec(contentDisposition);
            if (matches != null && matches[1]) {
                filename = matches[1].replace(/['"]/g, ''); // Убираем кавычки, если они есть
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
        alert(`Не удалось скачать документ: ${error.message}`);
    }
}

function createDownloadPopup(documentId) {
    const overlay = createOverlay();
    const popup = createPopup();

    const htmlButton = document.createElement('button');
    htmlButton.textContent = 'HTML';
    htmlButton.onclick = () => downloadDocument(documentId, 'html');

    const mdButton = document.createElement('button');
    mdButton.textContent = 'MD';
    mdButton.onclick = () => downloadDocument(documentId, 'md');

    popup.appendChild(htmlButton);
    popup.appendChild(mdButton);
    overlay.appendChild(popup);
    document.body.appendChild(overlay);
}