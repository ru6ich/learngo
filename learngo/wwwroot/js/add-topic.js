document.addEventListener('DOMContentLoaded', () => {
    const fields = document.querySelectorAll('#topic-form input, #topic-form textarea, #topic-form select');
    fields.forEach(field => {
        field.addEventListener('input', () => validateField(field));
        field.addEventListener('change', () => validateField(field));
    });

    document.getElementById('topic-form').addEventListener('submit', function (e) {
        e.preventDefault();
        const fields = [
            document.getElementById('topic-name'),
            document.getElementById('section-name'),
            document.getElementById('description'),
            document.getElementById('author-name'),
            document.getElementById('email'),
            document.getElementById('date'),
            document.getElementById('complexity'),
            document.getElementById('reading-time')
        ];

        let hasErrors = false;
        fields.forEach(field => {
            if (!validateField(field)) hasErrors = true;
        });

        if (!hasErrors) {
            this.submit(); // Отправка формы на сервер через MVC
        }
    });
});