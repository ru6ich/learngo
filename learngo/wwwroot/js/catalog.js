document.addEventListener('DOMContentLoaded', () => {
    let topicsData = [];
    let loadedSections = 0;
    const sectionsPerLoad = 3;

    // Инициализация данных из модели Razor
    topicsData = window.topicsData || [];
    console.log('Topics Data:', topicsData); // Для отладки

    function renderTopics(topics, container) {
        container.innerHTML = '';
        if (topics.length === 0) {
            container.innerHTML = '<p>В этом разделе пока нет тем</p>';
            return;
        }
        const cardsContainer = document.createElement('div');
        topics.forEach(topic => {
            const card = document.createElement('div');
            card.className = 'card';
            card.dataset.topicId = topic.id;
            const stars = '★'.repeat(topic.difficulty) + '☆'.repeat(5 - topic.difficulty);
            card.innerHTML = `
                <h3><a href="/Home/TopicDetails?id=${topic.id}">${topic.topicName}</a></h3>
                <p>${topic.description}</p>
                <div class="stars">${stars}</div>
                <div class="time">Время на изучение: ${topic.timeLimit} мин</div>
                <div class="author">Автор: ${topic.authorName}</div>
                <div class="date">Дата создания: ${new Date(topic.createdAt).toLocaleDateString()}</div>
            `;
            cardsContainer.appendChild(card);
        });
        container.appendChild(cardsContainer);
    }

    function loadSections(start) {
        const list = document.querySelector('.topic-list');
        const end = Math.min(start + sectionsPerLoad, topicsData.length);

        for (let i = start; i < end; i++) {
            const sec = topicsData[i];
            const li = document.createElement('li');
            const button = document.createElement('button');
            button.classList.add('topic-button');
            button.dataset.section = sec.SectionName;

            // Создаём span для имени раздела и количества тем
            const nameSpan = document.createElement('span');
            nameSpan.className = 'section-name';
            nameSpan.textContent = sec.SectionName;

            const countSpan = document.createElement('span');
            countSpan.className = 'topic-count';
            countSpan.textContent = ` (${sec.TopicCount})`;
            countSpan.style.display = 'none'; // Скрываем по умолчанию

            button.appendChild(nameSpan);
            button.appendChild(countSpan);
            li.appendChild(button);

            const container = document.createElement('div');
            container.classList.add('cards-container', 'hidden');
            container.dataset.section = sec.SectionName;
            li.appendChild(container);

            // Показываем количество тем при наведении
            button.addEventListener('mouseover', () => {
                countSpan.style.display = 'inline';
            });
            button.addEventListener('mouseout', () => {
                countSpan.style.display = 'none';
            });

            // Загрузка тем при клике
            button.addEventListener('click', function () {
                const sectionName = this.dataset.section;
                const container = this.nextElementSibling;
                if (container.dataset.loaded) {
                    container.classList.toggle('hidden');
                    return;
                }
                loadTopics(sectionName, container);
            });

            list.appendChild(li);
        }
        loadedSections = end; // Обновляем количество загруженных секций
    }

    function loadTopics(sectionName, container) {
        container.innerHTML = '<p>Загрузка тем...</p>';
        fetch(`/api/topics/section/${encodeURIComponent(sectionName)}`)
            .then(response => {
                if (!response.ok) throw new Error('Network response was not ok');
                return response.json();
            })
            .then(topics => {
                renderTopics(topics, container);
                container.dataset.loaded = 'true';
                container.classList.remove('hidden');
            })
            .catch(error => {
                console.error('Ошибка загрузки тем:', error);
                container.innerHTML = '<p class="error">Ошибка при загрузке тем</p>';
                container.dataset.loaded = 'true';
            });
    }

    function searchTopics(query) {
        const searchResults = document.getElementById('search-results');
        if (!query.trim()) {
            searchResults.style.display = 'none';
            return;
        }
        fetch(`/api/topics/search?query=${encodeURIComponent(query.trim())}`)
            .then(response => response.json())
            .then(topics => {
                searchResults.innerHTML = '';
                if (topics.length === 0) {
                    searchResults.innerHTML = '<div class="search-item">Ничего не найдено</div>';
                } else {
                    topics.forEach(topic => {
                        const item = document.createElement('div');
                        item.className = 'search-item';
                        item.innerHTML = `
                            <div class="topic-title">${topic.topicName}</div>
                            <div class="topic-section">Раздел: ${topic.section?.sectionName || 'Неизвестно'}</div>
                        `;
                        item.addEventListener('click', () => {
                            window.location.href = `/Home/TopicDetails?id=${topic.id}`;
                        });
                        searchResults.appendChild(item);
                    });
                }
                searchResults.style.display = 'block';
            })
            .catch(error => {
                console.error('Ошибка поиска:', error);
                searchResults.innerHTML = '<div class="search-item">Ошибка при поиске</div>';
                searchResults.style.display = 'block';
            });
    }

    function applyFilters() {
        const difficulty = document.getElementById('difficulty-filter').value;
        const time = document.getElementById('time-filter').value;
        let url = '/api/topics/filter?';
        const params = new URLSearchParams();
        if (difficulty) params.append('difficulty', difficulty);
        if (time) params.append('time', time);

        fetch(url + params.toString())
            .then(response => response.json())
            .then(topics => {
                const topicList = document.querySelector('.topic-list');
                topicList.innerHTML = ''; // Очищаем список
                loadSections(0); // Перезагружаем секции (временное решение, можно улучшить)
            })
            .catch(error => {
                console.error('Ошибка фильтрации:', error);
                document.querySelector('.topic-list').innerHTML = '<p class="error">Ошибка при фильтрации тем</p>';
            });
    }

    // Скрытие результатов поиска при клике вне
    document.addEventListener('click', (event) => {
        const searchResults = document.getElementById('search-results');
        const searchInput = document.getElementById('search');
        if (event.target !== searchResults && event.target !== searchInput && !searchResults.contains(event.target)) {
            searchResults.style.display = 'none';
        }
    });

    // Инициализация: загружаем первые 3 раздела
    loadSections(0);

    // Ленивая загрузка при скролле
    window.addEventListener('scroll', () => {
        if (window.innerHeight + window.scrollY >= document.body.offsetHeight - 100 && loadedSections < topicsData.length) {
            loadSections(loadedSections);
        }
    });

    // Привязка событий
    document.getElementById('search').addEventListener('input', (e) => searchTopics(e.target.value));
    document.getElementById('difficulty-filter').addEventListener('change', applyFilters);
    document.getElementById('time-filter').addEventListener('change', applyFilters);
});