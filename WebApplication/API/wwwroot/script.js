// Открытие модальных окон
document.getElementById('signupBtn').addEventListener('click', () => {
    document.getElementById('signupModal').style.display = 'flex';
});

document.getElementById('loginBtn').addEventListener('click', () => {
    document.getElementById('loginModal').style.display = 'flex';
});

// Закрытие модальных окон
document.querySelectorAll('.close').forEach(button => {
    button.addEventListener('click', () => {
        button.closest('.modal').style.display = 'none';
    });
});

// Закрытие модальных окон при клике вне их
window.addEventListener('click', (event) => {
    if (event.target.classList.contains('modal')) {
        event.target.style.display = 'none';
    }
});

// AJAX для регистрации
document.getElementById('signupForm').addEventListener('click', (event) => {
    event.preventDefault();
    const name = document.getElementById('name').value;
    const email = document.getElementById('emailSignup').value;
    const password = document.getElementById('passwordSignup').value;

    const data = {
        userName: name,
        userEmail: email,
        password: password
    };

    fetch('api/user/signup', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
        .then(() => {
            document.getElementById('signupModal').style.display = 'none';
        })
        .catch(error => console.error('Error:', error));
});

// AJAX для входа
document.getElementById('loginForm').addEventListener('click', (event) => {
    event.preventDefault();
    const email = document.getElementById('emailLogin').value;
    const password = document.getElementById('passwordLogin').value;

    const data = {
        userEmail: email,
        password: password
    };

    fetch('api/user/login/email', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
        .then(() => {
            document.getElementById('loginModal').style.display = 'none';
        })
        .then(() => window.location.href='home')
        .catch(error => console.error('Error:', error));
});