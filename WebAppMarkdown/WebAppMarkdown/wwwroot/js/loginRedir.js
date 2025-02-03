document.getElementById('loginForm').addEventListener('submit', async function(event) {
    event.preventDefault();

    const formData = new FormData(this);
    const data = {
        email: formData.get('email'),
        password: formData.get('password')
    };

    const response = await fetch('/User/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    });


    if (response.ok) {
        location.href = '/dashboard';
    } else {
        console.error('Login failed:', response.statusText); // Логирование ошибки
    }
});