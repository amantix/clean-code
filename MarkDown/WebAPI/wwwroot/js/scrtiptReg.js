document.getElementById('regForm').addEventListener('submit', async function (event) {
    event.preventDefault();

    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;

    const response = await fetch("/User/register", {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password })
    });
    if (response.ok) {
        alert("Регистрация прошла успешна. Теперь необходимо авторизоваться");
        location.href = "/login";
    } else {
        alert("Ошибка регистрации");
    }
});