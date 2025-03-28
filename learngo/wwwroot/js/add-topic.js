$(document).ready(function () {
    $("#topic-form").validate({
        rules: {
            "TopicName": {
                required: true,
                minlength: 3,
                pattern: /^[A-Za-zА-Яа-яЁё\s-]+$/
            },
            "SectionName": {
                required: true,
                minlength: 3,
                pattern: /^[A-Za-zА-Яа-яЁё\s-]+$/
            },
            "Description": {
                required: true,
                minlength: 10
            },
            "AuthorName": {
                required: true,
                minlength: 3,
                pattern: /^[A-Za-zА-Яа-яЁё\s-]+$/
            },
            "Email": {
                required: true,
                email: true
            },
            "CreationDate": {
                required: true
            },
            "Complexity": {
                required: true,
                range: [1, 5]
            },
            "ReadingTime": {
                required: true,
                min: 1
            }
        },
        messages: {
            "TopicName": {
                required: "Название темы обязательно",
                minlength: "Введите не менее 3 символов",
                pattern: "Используйте только буквы, пробелы или дефис"
            },
            "SectionName": {
                required: "Название раздела обязательно",
                minlength: "Введите не менее 3 символов",
                pattern: "Используйте только буквы, пробелы или дефис"
            },
            "Description": {
                required: "Описание обязательно",
                minlength: "Введите не менее 10 символов"
            },
            "AuthorName": {
                required: "Имя автора обязательно",
                minlength: "Введите не менее 3 символов",
                pattern: "Используйте только буквы, пробелы или дефис"
            },
            "Email": {
                required: "Email обязателен",
                email: "Введите корректный email"
            },
            "CreationDate": {
                required: "Укажите дату создания"
            },
            "Complexity": {
                required: "Выберите сложность",
                range: "Выберите значение от 1 до 5"
            },
            "ReadingTime": {
                required: "Выберите время чтения",
                min: "Выберите корректное время"
            }
        },
        errorElement: "span",
        errorClass: "error-message",
        errorPlacement: function (error, element) {
            error.appendTo("#" + element.attr("id") + "-error");
        },
        highlight: function (element) {
            $(element).addClass("error");
        },
        unhighlight: function (element) {
            $(element).removeClass("error");
            $("#" + element.id + "-error").text(""); // Очищаем сообщение об ошибке
        },
        onfocusout: function (element) {
            this.element(element); // Проверяем всегда
        },
        onkeyup: function (element) {
            this.element(element); // Проверяем всегда
        },
        onchange: function (element) {
            this.element(element); // Проверяем всегда
        },
        // Добавляем обработку ошибок для полей с pattern
        invalidHandler: function (form, validator) {
            // Ничего не делаем, просто для отладки
            console.log("Форма содержит ошибки:", validator.errorList);
        },
        submitHandler: function (form) {
            const formData = {
                TopicName: $("#topic-name").val(),
                SectionName: $("#section-name").val(),
                Description: $("#description").val(),
                AuthorName: $("#author-name").val(),
                Email: $("#email").val(),
                CreationDate: $("#creation-date").val(),
                Complexity: $("#complexity").val(),
                ReadingTime: parseInt($("#reading-time").val())
            };

            console.log("Отправляемые данные:", formData);

            $.ajax({
                url: '/api/topics/add-topic',
                type: 'POST',
                data: JSON.stringify(formData),
                contentType: 'application/json',
                success: function (response) {
                    alert(response.message);
                    form.reset();
                },
                error: function (xhr) {
                    let errorMessage = 'Ошибка при добавлении темы';
                    if (xhr.responseJSON) {
                        if (xhr.responseJSON.message) {
                            errorMessage = xhr.responseJSON.message;
                        }
                        if (xhr.responseJSON.errors) {
                            if (Array.isArray(xhr.responseJSON.errors)) {
                                errorMessage += ': ' + xhr.responseJSON.errors.join(', ');
                            } else {
                                errorMessage += ': ' + JSON.stringify(xhr.responseJSON.errors);
                            }
                        }
                    }
                    alert(errorMessage);
                }
            });
        }
    });

    // Добавляем кастомную валидацию для полей с pattern, чтобы избежать ошибок
    $.validator.addMethod("customPattern", function (value, element, param) {
        if (!value || value.trim().length === 0) {
            return true; // Пропускаем проверку, если поле пустое (обработает required)
        }
        return param.test(value);
    }, "Используйте только буквы, пробелы или дефис");

    // Переопределяем правила для полей с pattern
    $("#topic-name").rules("remove", "pattern");
    $("#topic-name").rules("add", {
        customPattern: /^[A-Za-zА-Яа-яЁё\s-]+$/
    });

    $("#section-name").rules("remove", "pattern");
    $("#section-name").rules("add", {
        customPattern: /^[A-Za-zА-Яа-яЁё\s-]+$/
    });

    $("#author-name").rules("remove", "pattern");
    $("#author-name").rules("add", {
        customPattern: /^[A-Za-zА-Яа-яЁё\s-]+$/
    });
});