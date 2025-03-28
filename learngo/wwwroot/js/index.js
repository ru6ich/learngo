document.addEventListener('DOMContentLoaded', () => {
    // Кнопка прокрутки вверх
    function scrollToTop() {
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    }

    window.onscroll = function () {
        const button = document.getElementById('scrollToTop');
        if (document.body.scrollTop > 100 || document.documentElement.scrollTop > 100) {
            button.style.display = "block";
        } else {
            button.style.display = "none";
        }
    };

    // Переключение списка особенностей
    document.getElementById('toggle-list').addEventListener('click', function () {
        const list = document.getElementById('features-list');
        const icon = document.getElementById('icon');
        if (list.style.display === 'none') {
            list.style.display = 'block';
            icon.textContent = '▲';
        } else {
            list.style.display = 'none';
            icon.textContent = '▼';
        }
    });

    // Маркированный текст в футере
    const marquee = document.getElementById('marquee');
    const footer = document.getElementById('footer');

    function stopMarquee() {
        marquee.style.animationPlayState = 'paused';
    }

    function startMarquee() {
        marquee.style.animationPlayState = 'running';
    }

    footer.addEventListener('mouseover', stopMarquee);
    footer.addEventListener('mouseout', startMarquee);

    // Подключение кнопки прокрутки
    document.getElementById('scrollToTop').addEventListener('click', scrollToTop);
});