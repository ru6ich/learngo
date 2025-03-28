$(document).ready(function () {
    let loadedSections = 0;
    const sectionsPerLoad = 3;
    let allSections = [];
    let isLoading = false;
    let filtersApplied = false;

    // Загружаем первые разделы при загрузке страницы
    function loadInitialSections() {
        if (isLoading) return;
        isLoading = true;

        $.ajax({
            url: '/api/topics/sections',
            type: 'GET',
            data: { skip: loadedSections, take: sectionsPerLoad },
            success: function (sections) {
                if (sections.length === 0) {
                    isLoading = false;
                    return;
                }
                allSections = allSections.concat(sections);
                renderSections(sections);
                loadedSections += sections.length;
                isLoading = false;
            },
            error: function (xhr) {
                console.error('Ошибка загрузки разделов:', xhr);
                $('.topic-list').html('<p class="error">Ошибка при загрузке разделов</p>');
                isLoading = false;
            }
        });
    }

    // Рендеринг разделов
    function renderSections(sections) {
        const list = $('.topic-list');
        sections.forEach(sec => {
            const li = $('<li></li>');
            const button = $('<button></button>')
                .addClass('topic-button')
                .attr('data-section', sec.sectionName);

            const nameSpan = $('<span></span>')
                .addClass('section-name')
                .text(sec.sectionName);

            const countSpan = $('<span></span>')
                .addClass('topic-count')
                .text(` (${sec.topicCount})`)
                .css('display', 'none');

            button.append(nameSpan).append(countSpan);
            li.append(button);

            const container = $('<div></div>')
                .addClass('cards-container hidden')
                .attr('data-section', sec.sectionName);
            li.append(container);

            button.on('mouseover', () => countSpan.css('display', 'inline'));
            button.on('mouseout', () => countSpan.css('display', 'none'));

            button.on('click', function () {
                const sectionName = $(this).data('section');
                const container = $(this).next();
                if (container.data('loaded')) {
                    container.toggleClass('hidden');
                    return;
                }
                loadTopics(sectionName, container);
            });

            list.append(li);
        });
    }

    // Загрузка тем для раздела
    function loadTopics(sectionName, container) {
        container.html('<p>Загрузка тем...</p>');
        $.ajax({
            url: `/api/topics/section/${encodeURIComponent(sectionName)}`,
            type: 'GET',
            success: function (topics) {
                renderTopics(topics, container);
                container.data('loaded', true);
                container.removeClass('hidden');
            },
            error: function (xhr) {
                console.error('Ошибка загрузки тем:', xhr);
                container.html('<p class="error">Ошибка при загрузке тем</p>');
                container.data('loaded', true);
            }
        });
    }

    // Рендеринг тем
    function renderTopics(topics, container) {
        container.html('');
        if (topics.length === 0) {
            container.html('<p>В этом разделе пока нет тем</p>');
            return;
        }
        const cardsContainer = $('<div></div>');
        topics.forEach(topic => {
            const card = $('<div></div>')
                .addClass('card')
                .attr('data-topic-id', topic.id);
            const stars = '★'.repeat(topic.difficulty) + '☆'.repeat(5 - topic.difficulty);
            card.html(`
                <h3><a href="/Home/TopicDetails?id=${topic.id}">${topic.topicName}</a></h3>
                <p>${topic.description}</p>
                <div class="stars">${stars}</div>
                <div class="time">Время на изучение: ${topic.timeLimit} мин</div>
                <div class="author">Автор: ${topic.authorName}</div>
                <div class="date">Дата создания: ${new Date(topic.createdAt).toLocaleDateString()}</div>
            `);
            cardsContainer.append(card);
        });
        container.append(cardsContainer);
    }

    // Ленивая загрузка при скролле
    $(window).on('scroll', function () {
        if (isLoading || filtersApplied) return;
        if ($(window).scrollTop() + $(window).height() >= $(document).height() - 100) {
            loadInitialSections();
        }
    });

    // Умный поиск
    $('#search').on('input', function () {
        const query = $(this).val();
        const searchResults = $('#search-results');
        if (!query.trim()) {
            searchResults.hide();
            return;
        }

        $.ajax({
            url: `/api/topics/search?query=${encodeURIComponent(query.trim())}`,
            type: 'GET',
            success: function (topics) {
                searchResults.html('');
                if (topics.length === 0) {
                    searchResults.html('<div class="search-item">Ничего не найдено</div>');
                } else {
                    topics.forEach(topic => {
                        const item = $('<div></div>')
                            .addClass('search-item')
                            .html(`
                                <div class="topic-title">${topic.topicName}</div>
                                <div class="topic-section">Раздел: ${topic.section?.sectionName || 'Неизвестно'}</div>
                            `);
                        item.on('click', () => {
                            window.location.href = `/Home/TopicDetails?id=${topic.id}`;
                        });
                        searchResults.append(item);
                    });
                }
                searchResults.show();
            },
            error: function (xhr) {
                console.error('Ошибка поиска:', xhr);
                searchResults.html('<div class="search-item">Ошибка при поиске</div>');
                searchResults.show();
            }
        });
    });

    // Скрытие результатов поиска при клике вне
    $(document).on('click', function (event) {
        const searchResults = $('#search-results');
        const searchInput = $('#search');
        if (!searchResults.is(event.target) && !searchInput.is(event.target) && !searchResults.has(event.target).length) {
            searchResults.hide();
        }
    });

    // Применение фильтров
    function applyFilters() {
        const difficulty = $('#difficulty-filter').val();
        const time = $('#time-filter').val();

        if (!difficulty && !time) {
            // Если фильтры сброшены, возвращаем исходное состояние
            filtersApplied = false;
            $('.topic-list').html('');
            loadedSections = 0;
            allSections = [];
            loadInitialSections();
            return;
        }

        filtersApplied = true;
        const params = new URLSearchParams();
        if (difficulty) params.append('difficulty', difficulty);
        if (time) params.append('time', time);

        $.ajax({
            url: `/api/topics/filter?${params.toString()}`,
            type: 'GET',
            success: function (topics) {
                // Группируем темы по разделам
                const sectionsMap = {};
                topics.forEach(topic => {
                    const sectionName = topic.section?.sectionName || 'Без раздела';
                    if (!sectionsMap[sectionName]) {
                        sectionsMap[sectionName] = [];
                    }
                    sectionsMap[sectionName].push(topic);
                });

                // Рендерим разделы и темы
                const list = $('.topic-list');
                list.html('');
                Object.keys(sectionsMap).forEach(sectionName => {
                    const li = $('<li></li>');
                    const button = $('<button></button>')
                        .addClass('topic-button')
                        .attr('data-section', sectionName);

                    const nameSpan = $('<span></span>')
                        .addClass('section-name')
                        .text(sectionName);

                    const countSpan = $('<span></span>')
                        .addClass('topic-count')
                        .text(` (${sectionsMap[sectionName].length})`)
                        .css('display', 'none');

                    button.append(nameSpan).append(countSpan);
                    li.append(button);

                    const container = $('<div></div>')
                        .addClass('cards-container')
                        .attr('data-section', sectionName);
                    renderTopics(sectionsMap[sectionName], container);
                    li.append(container);

                    button.on('mouseover', () => countSpan.css('display', 'inline'));
                    button.on('mouseout', () => countSpan.css('display', 'none'));

                    button.on('click', function () {
                        $(this).next().toggleClass('hidden');
                    });

                    list.append(li);
                });

                if (Object.keys(sectionsMap).length === 0) {
                    list.html('<p>Темы не найдены</p>');
                }
            },
            error: function (xhr) {
                console.error('Ошибка фильтрации:', xhr);
                $('.topic-list').html('<p class="error">Ошибка при фильтрации тем</p>');
            }
        });
    }

    // Привязка событий для фильтров
    $('#difficulty-filter, #time-filter').on('change', applyFilters);

    // Инициализация
    loadInitialSections();
});