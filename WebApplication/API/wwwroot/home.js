let documents = [];
let currentDocumentId = null;
let permissions = [];

const documentList = document.getElementById('documentList');
const createDocumentBtn = document.getElementById('createDocumentBtn');
const documentModal = document.getElementById('documentModal');
const userModal = document.getElementById('userModal');
const closeModal = document.querySelector('.close');
const documentNameInput = document.getElementById('documentName');
const documentAccessInput = document.getElementById('documentAccess');
const modalTitle = document.getElementById('modalTitle');
const submitDocumentBtn = document.getElementById('submitDocumentBtn');
const userEmailInput = document.getElementById('userEmail');
const userAccessInput = document.getElementById('userAccess');
const submitUserBtn = document.getElementById('submitUserBtn');
document.addEventListener('DOMContentLoaded', loadDocuments);
submitUserBtn.addEventListener('click',e => provideAccess(e));
// Открытие модального окна для создания документа
createDocumentBtn.addEventListener('click', () => {
    currentDocumentId = null;
    documentNameInput.value = '';
    documentAccessInput.value = '2';
    modalTitle.textContent = 'Create Document';
    submitDocumentBtn.textContent = 'Create';
    documentModal.style.display = 'flex';
});

// Закрытие модального окна
closeModal.addEventListener('click', () => {
    documentModal.style.display = 'none';
    userModal.style.display = 'none';
});

// Закрытие модального окна при клике вне его
window.addEventListener('click', (event) => {
    if (event.target === documentModal || event.target === userModal) {
        documentModal.style.display = 'none';
        userModal.style.display = 'none';
    }
});

// Обработка отправки формы
submitDocumentBtn.addEventListener('click', async (event) => {
    event.preventDefault();
    const name = documentNameInput.value;
    const access = parseInt(documentAccessInput.value);

    if (currentDocumentId === null) {
        // Создание нового документа
        const newDocument = { title:name, type:access };
        await saveDocument(newDocument);
    } else {
        // Обновление существующего документа
        const document = documents.find(doc => doc.id === currentDocumentId);
        document.title = name;
        document.type = access;
        await updateDocument(document);
    }

    await renderDocuments();
    documentModal.style.display = 'none';
});

async function provideAccess(event) {
    const email = userEmailInput.value;
    const access = parseInt(userAccessInput.value);
    const response = await fetch(`api/document/provide/${currentDocumentId}`, {
        method: 'POST',
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify({email: email, accessLevel: access}),
    })
    if (response.ok) {
        console.log("success provide");
    }
    userModal.style.display = 'none';
    currentDocumentId = null;
}
// Рендеринг списка документов
async function renderDocuments() {
    documentList.innerHTML = '';
    documents.forEach(doc => {
        const li = document.createElement('li');
        const a = document.createElement('a');
        a.textContent = doc.title;
        a.href = `/${doc.id}`;
        const documentEditBtn = document.createElement('button');
        documentEditBtn.textContent = 'Edit';
        const documentDeleteBtn = document.createElement('button');
        documentDeleteBtn.textContent = 'Delete';
        const permissionBtn = document.createElement('button');
        permissionBtn.textContent = 'Access';
        permissionBtn.addEventListener('click', (e) => {
            currentDocumentId = e.currentTarget.parentElement.id;
            userModal.style.display = 'flex';
        });
        const viewBtn = document.createElement('button');
        viewBtn.textContent = 'View';
        viewBtn.addEventListener('click', async (e) => {
            const id = e.currentTarget.parentElement.id;
            await loadPermissions(id);
        });
        li.id = doc.id;
        li.appendChild(a);
        li.appendChild(documentEditBtn);
        li.appendChild(documentDeleteBtn);
        li.appendChild(permissionBtn);
        li.appendChild(viewBtn);
        documentList.appendChild(li);
        documentEditBtn.addEventListener('click', (event) => {
            const id = event.currentTarget.parentElement.id;
            editDocument(id);
        })

        documentDeleteBtn.addEventListener('click', event => deleteDocument(event));
    });
}

// Редактирование документа
function editDocument(id) {
    const document = documents.find(doc => doc.id === id);
    currentDocumentId = document.id;
    documentNameInput.value = document.title;
    documentAccessInput.value = document.type;
    modalTitle.textContent = 'Edit Document';
    submitDocumentBtn.textContent = 'Update';
    documentModal.style.display = 'flex';
}

// Удаление документа
async function deleteDocument(event) {
    const id = event.currentTarget.parentElement.id;
    const response = await fetch(`api/document/delete/${id}`, {
        method: 'DELETE'
    });
    console.log('Document deleted');
    documents = documents.filter(doc => doc.id !== id);
    await renderDocuments();
}

// Сохранение документа (AJAX)
async function saveDocument(document) {
    const response = await fetch('api/document/create', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(document),
    })
    const data = await response.json();
    console.log('Document saved:', data);
    const newDocument = {id: data.id, title:data.title,type:data.accessType};
    documents.push(newDocument);
}

// Обновление документа (AJAX)
async function updateDocument(document) {
    const response = await fetch(`api/document/update/${document.id}`, {
        method: 'PATCH',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(document),
    });
}

async function loadDocuments() {
    const response = await fetch('api/document/all', {
        method: 'GET',
    })
    const data = await response.json();
    console.log(data);
    data.forEach(document => {
        const doc = {id: document.id, title: document.title, type: document.accessType};
        documents.push(doc);
    });
    await renderDocuments();
}

async function loadPermissions(pId){
    const response = await fetch(`api/document/provide/get/${pId}`)
    const data = await response.json();
    permissions.push(data);
    const modalListId = document.getElementById('modal-list-id');
    data.provides.forEach(permission => {
        const li = document.createElement('li');
        li.classList.add('modal-item');
        li.id = data.documentId;
        const delBtn = document.createElement('button');
        delBtn.textContent = 'Delete';
        delBtn.classList.add('delete-btn');
        delBtn.addEventListener('click', async (e) => {
            const id = e.currentTarget.parentElement.id;
            const email = permission.email;
            const level = permission.level;
            await deletePermission(e, id, email, level);
        })
        const emailSpan = document.createElement('span');
        emailSpan.classList.add('email');
        emailSpan.innerText = permission.email;
        const levelSpan = document.createElement('span');
        levelSpan.classList.add('access-level');
        levelSpan.innerText = permission.level === 1 ? 'Read' : 'Edit';
        li.appendChild(delBtn);
        li.appendChild(emailSpan);
        li.appendChild(levelSpan);
        modalListId.appendChild(li);
    })
    document.getElementById('permissions').style.display = 'flex';
}

async function deletePermission(e, pId, email, level){
    const response = await fetch(`api/document/provide/delete/${pId}`,{
        method: 'DELETE',
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify({email: email, accessLevel: level}),
    });
    if (response.ok) {
        e.currentTarget.parentElement.remove();
    }
}