// Barra de busqueda con sugerencias
document.addEventListener('DOMContentLoaded', function() {
    const searchInput = document.getElementById('search-input');
    const suggestionsBox = document.getElementById('suggestions');

    // Evento para manejar las sugerencias en la barra de búsqueda
    searchInput.addEventListener('input', function() {
        const query = this.value.trim();
        if (query.length > 1) {
            fetch(`/sugerencias/?term=${query}`)
                .then(response => response.json())
                .then(data => {
                    suggestionsBox.innerHTML = ''; // Limpia las sugerencias previas
                    if (data.length > 0) {
                        suggestionsBox.classList.add('active'); // Muestra el contenedor de sugerencias
                        data.forEach(item => {
                            const suggestionItem = document.createElement('div');
                            suggestionItem.textContent = item.nombre_producto;
                            suggestionItem.classList.add('suggestion-item');
                            suggestionItem.addEventListener('click', () => {
                                searchInput.value = item.nombre_producto;
                                suggestionsBox.innerHTML = '';  // Oculta las sugerencias después de seleccionar
                                suggestionsBox.classList.remove('active'); // Oculta el contenedor
                            });
                            suggestionsBox.appendChild(suggestionItem);
                        });
                    } else {
                        suggestionsBox.classList.remove('active'); // Oculta el contenedor si no hay sugerencias
                    }
                });
        } else {
            suggestionsBox.classList.remove('active'); // Oculta el contenedor si el query es corto
            suggestionsBox.innerHTML = '';
        }
    });

    // Oculta las sugerencias si se hace clic fuera del campo de búsqueda
    document.addEventListener('click', function(event) {
        if (!suggestionsBox.contains(event.target) && event.target !== searchInput) {
            suggestionsBox.classList.remove('active'); // Oculta el contenedor al hacer clic fuera
            suggestionsBox.innerHTML = '';
        }
    });

    // Muestra u oculta la barra de búsqueda al hacer clic en el ícono de la lupa
    document.getElementById('search-icon').addEventListener('click', function() {
        const searchBarContainer = document.querySelector('.search-bar-container');
        searchBarContainer.classList.toggle('show'); // Añadir o quitar la clase 'show'
    });
    
    // Oculta la barra de búsqueda al hacer clic en el ícono de cierre
    document.getElementById('close-search').addEventListener('click', function() {
        const searchBarContainer = document.querySelector('.search-bar-container');
        searchBarContainer.classList.remove('show'); // Quitar la clase 'show' para ocultar
    });
});

window.onload = function() {
    const form = document.querySelector('form');
    form.onsubmit = verificarRut;
};


// Verificador de rut

function verificarRut() {
    const rutClienteInput = document.querySelector("input[name='rut']") || document.getElementById('id_rut');
    let rutCliente = rutClienteInput.value;
    const rutError = document.getElementById('rutError');

    rutCliente = formatearRut(rutCliente);

    if (validarRut(rutCliente)) {
        rutError.style.display = 'none';
        document.getElementById('rutCliente').value = rutCliente;
        return true; // Permitir el envío del formulario
    } else {
        rutError.style.display = 'block';
        return false; // Evitar el envío del formulario
    }
}

function formatearRut(rut) {
    // Eliminar puntos y guiones
    rut = rut.replace(/\./g, '').replace(/-/g, '');

    // Obtener el número y el dígito verificador
    const numero = rut.slice(0, -1);
    const dv = rut.slice(-1);

    // Formatear con puntos
    let formattedRut = '';
    for (let i = numero.length - 1, j = 1; i >= 0; i--, j++) {
        formattedRut = numero.charAt(i) + formattedRut;
        if (j % 3 === 0 && i !== 0) {
            formattedRut = '.' + formattedRut;
        }
    }

    // Agregar el guion y retornar el RUT formateado
    return formattedRut + '-' + dv;
    }

function validarRut(rut) {
    if (!rut || !/^[0-9.]+-[0-9kK]{1}$/.test(rut)) {
        return false;
    }
    const [num, dv] = rut.split('-');
    const cleanNum = num.replace(/\./g, ''); // Remover puntos para la validación
    return dv.toLowerCase() === calcularDV(cleanNum);
}

function calcularDV(rut) {
    let suma = 0;
    let multiplo = 2;

    for (let i = rut.length - 1; i >= 0; i--) {
        suma += multiplo * rut.charAt(i);
        multiplo = (multiplo + 1) % 8 || 2;
    }

    const dv = 11 - (suma % 11);
    if (dv === 11) {
        return '0';
    } else if (dv === 10) {
        return 'k';
    } else {
        return dv.toString();
    }
}