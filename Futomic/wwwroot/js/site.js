//scroll para el navbar 
window.addEventListener('scroll', function () {
    const navbar = document.querySelector('.futomic-navbar');
    if (window.scrollY > 50) {
        navbar.style.backgroundColor = 'rgba(26, 140, 58, 1)'; // opaco al 100%
    } else {
        navbar.style.backgroundColor = 'rgba(26, 140, 58, 0.8)'; // translúcido al top
    }
});


//Precio Reserva
const durationSelect = document.getElementById('durationSelect');
const priceInput = document.getElementById('priceInput');

function updatePrice() {
    const duration = parseInt(durationSelect.value);
    let price = 0;

    switch (duration) {
        case 60: price = 60; break;
        case 120: price = 100; break;
        case 240: price = 200; break;
        default: price = 0;
    }

    priceInput.value = price;
}

durationSelect.addEventListener('change', updatePrice);

// Inicializar precio al cargar
updatePrice();

