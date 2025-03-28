// ���������� ����������� (�� enlargeImage.js)
function enlargeImage(img) {
    const enlargedDiv = document.getElementById('enlargedImage');
    const largeImg = document.getElementById('largeImg');
    largeImg.src = img.src;
    enlargedDiv.style.display = 'block';
}

// ��������� ����� (������������ � add-topic � ������ ������)
function validateField(field) {
    const errorContainer = document.getElementById(`${field.id}-error`);
    let errors = [];

    switch (field.id) {
        case 'topic-name':
        case 'editTopicName':
            if (field.value.length < 3) errors.push("�������� ���� ������ ��������� �� ����� 3 ��������.");
            break;
        case 'section-name':
        case 'editSectionName':
            if (field.value.length < 3) errors.push("�������� ������� ������ ��������� �� ����� 3 ��������.");
            break;
        case 'description':
        case 'editDescription':
            if (field.value.length < 10) errors.push("�������� ������ ��������� �� ����� 10 ��������.");
            break;
        case 'author-name':
        case 'editAuthorName':
            const nameRegex = /^[A-Za-z�-��-���\s-]+$/;
            if (!nameRegex.test(field.value)) errors.push("��� ������ ������ ��������� ������ ����� � �������.");
            break;
        case 'email':
        case 'editEmail':
            const emailRegex = /^(?!.*\.\.)[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+(?:\.[a-zA-Z]{2,})+$/;
            if (!emailRegex.test(field.value)) errors.push("������� ���������� ����� ����������� �����.");
            break;
        case 'date':
        case 'editDate':
            if (!field.value) errors.push("������� ���� �������� ����.");
            break;
        case 'complexity':
        case 'editDifficulty':
            if (!field.value) errors.push("�������� ���������.");
            break;
        case 'reading-time':
        case 'editTimeLimit':
            if (!field.value) errors.push("�������� ����� ������.");
            break;
    }

    updateErrorMessages(errors, errorContainer);
    return errors.length === 0;
}

function updateErrorMessages(errors, errorContainer) {
    errorContainer.innerHTML = '';
    if (errors.length > 0) {
        errors.forEach(error => {
            const li = document.createElement('li');
            li.textContent = error;
            errorContainer.appendChild(li);
        });
    }
}