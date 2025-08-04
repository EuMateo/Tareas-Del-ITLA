// Configuración de la API
const API_BASE_URL = ' https://localhost:7092/api';

// Variables globales
let allPhrases = [];
let currentCategory = '';
let currentPage = 1;
let editingPhraseId = null;
const itemsPerPage = 6;

// Mapeo de iconos para categorías
const categoryIcons = {
    'Restaurante': 'bi-cup-hot',
    'Hotel': 'bi-building',
    'Direcciones': 'bi-signpost',
    'Emergencias': 'bi-exclamation-triangle',
    'Compras': 'bi-bag',
    'Transporte': 'bi-bus-front',
    'Salud': 'bi-heart-pulse',
    'Entretenimiento': 'bi-camera-reels',
    'default': 'bi-chat-dots'
};

// Funciones de utilidad
function showLoading(show = true) {
    const loading = document.getElementById('phrasesLoading');
    if (loading) {
        loading.style.display = show ? 'block' : 'none';
    }
}

function showError(message) {
    console.error('Error:', message);
    Swal.fire({
        icon: 'error',
        title: 'Error',
        text: message,
        confirmButtonColor: '#667eea'
    });
}

function showSuccess(message) {
    Swal.fire({
        icon: 'success',
        title: 'Éxito',
        text: message,
        timer: 2000,
        showConfirmButton: false,
        toast: true,
        position: 'top-end'
    });
}

function formatDate(dateString) {
    if (!dateString) return '-';
    const date = new Date(dateString);
    return date.toLocaleDateString('es-ES', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit'
    });
}

function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

// Funciones de API
async function fetchPhrases(category = '') {
    try {
        showLoading(true);
        
        let url = `${API_BASE_URL}/frases`;
        if (category && category !== '') {
            url += `/categoria/${encodeURIComponent(category)}`;
        }

        const response = await fetch(url);
        
        if (!response.ok) {
            throw new Error(`Error HTTP: ${response.status}`);
        }

        const result = await response.json();
        
        if (result.data) {
            allPhrases = Array.isArray(result.data) ? result.data : [result.data];
        } else {
            allPhrases = [];
        }
        
        return allPhrases;
        
    } catch (error) {
        console.error('Error loading phrases:', error);
        showError('Error al cargar las frases: ' + error.message);
        allPhrases = [];
        return [];
    } finally {
        showLoading(false);
    }
}

async function createPhrase(data) {
    try {
        const response = await fetch(`${API_BASE_URL}/frases`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });

        const result = await response.json();

        if (!response.ok) {
            if (result.errors && result.errors.length > 0) {
                displayFormErrors(result.errors);
                return false;
            }
            throw new Error(result.message || 'Error al crear la frase');
        }

        showSuccess('Frase creada exitosamente');
        return true;
        
    } catch (error) {
        showError('Error al crear la frase: ' + error.message);
        return false;
    }
}

async function updatePhrase(id, data) {
    try {
        const response = await fetch(`${API_BASE_URL}/frases/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });

        const result = await response.json();

        if (!response.ok) {
            if (result.errors && result.errors.length > 0) {
                displayFormErrors(result.errors);
                return false;
            }
            throw new Error(result.message || 'Error al actualizar la frase');
        }

        showSuccess('Frase actualizada exitosamente');
        return true;
        
    } catch (error) {
        showError('Error al actualizar la frase: ' + error.message);
        return false;
    }
}

async function deletePhrase(id) {
    try {
        const response = await fetch(`${API_BASE_URL}/frases/${id}`, {
            method: 'DELETE'
        });

        if (!response.ok) {
            const result = await response.json();
            throw new Error(result.message || 'Error al eliminar la frase');
        }

        showSuccess('Frase eliminada exitosamente');
        return true;
        
    } catch (error) {
        showError('Error al eliminar la frase: ' + error.message);
        return false;
    }
}

async function fetchPhraseById(id) {
    try {
        const response = await fetch(`${API_BASE_URL}/frases/${id}`);
        
        if (!response.ok) {
            throw new Error('Frase no encontrada');
        }

        const result = await response.json();
        return result.data;
        
    } catch (error) {
        showError('Error al cargar la frase: ' + error.message);
        return null;
    }
}

// Funciones de renderizado
function renderPhrases() {
    const grid = document.getElementById('phrasesGrid');
    const noResults = document.getElementById('noResults');
    const searchTerm = document.getElementById('phraseSearchInput').value.toLowerCase().trim();
    const categoryFilter = document.getElementById('categoryFilter').value;
    
    // Filtrar frases
    let filteredPhrases = allPhrases.filter(phrase => {
        const matchSearch = !searchTerm || 
            phrase.español.toLowerCase().includes(searchTerm) ||
            phrase.ingles.toLowerCase().includes(searchTerm) ||
            phrase.pronunciacion.toLowerCase().includes(searchTerm);
        
        const matchCategory = !categoryFilter || phrase.categoria === categoryFilter;
        
        return matchSearch && matchCategory;
    });

    // Paginación
    const totalPages = Math.ceil(filteredPhrases.length / itemsPerPage);
    const startIndex = (currentPage - 1) * itemsPerPage;
    const endIndex = startIndex + itemsPerPage;
    const paginatedPhrases = filteredPhrases.slice(startIndex, endIndex);

    if (paginatedPhrases.length === 0) {
        grid.innerHTML = '';
        noResults.style.display = 'block';
        renderPagination(0, 0);
        return;
    }

    noResults.style.display = 'none';
    
    grid.innerHTML = paginatedPhrases.map(phrase => `
        <div class="col-lg-6 col-md-12">
            <div class="phrase-card">
                <div class="phrase-header">
                    <span class="phrase-category">${phrase.categoria}</span>
                    <div class="phrase-actions">
                        <button class="btn btn-primary btn-action" onclick="editPhrase(${phrase.id})" title="Editar">
                            <i class="bi bi-pencil"></i>
                        </button>
                        <button class="btn btn-danger btn-action" onclick="confirmDeletePhrase(${phrase.id})" title="Eliminar">
                            <i class="bi bi-trash"></i>
                        </button>
                    </div>
                </div>
                <div class="phrase-content">
                    <div class="phrase-row">
                        <div class="phrase-flag flag-es"></div>
                        <span class="phrase-text">${escapeHtml(phrase.español)}</span>
                        <button class="play-btn" onclick="speakText('${escapeHtml(phrase.español)}', 'es')" title="Escuchar en español">
                            <i class="bi bi-volume-up"></i>
                        </button>
                    </div>
                    <div class="phrase-row">
                        <div class="phrase-flag flag-en"></div>
                        <span class="phrase-text">${escapeHtml(phrase.ingles)}</span>
                        <button class="play-btn" onclick="speakText('${escapeHtml(phrase.ingles)}', 'en')" title="Escuchar en inglés">
                            <i class="bi bi-volume-up"></i>
                        </button>
                    </div>
                    <div class="pronunciation">
                        <i class="bi bi-info-circle"></i> ${escapeHtml(phrase.pronunciacion)}
                    </div>
                </div>
            </div>
        </div>
    `).join('');

    renderPagination(filteredPhrases.length, totalPages);
}

function renderCategories() {
    const grid = document.getElementById('categoriesGrid');
    
    // Obtener categorías únicas con conteo
    const categories = {};
    allPhrases.forEach(phrase => {
        if (categories[phrase.categoria]) {
            categories[phrase.categoria]++;
        } else {
            categories[phrase.categoria] = 1;
        }
    });

    const categoryArray = Object.entries(categories).map(([name, count]) => ({
        name,
        count,
        icon: categoryIcons[name] || categoryIcons.default
    }));

    if (categoryArray.length === 0) {
        grid.innerHTML = '<div class="col-12 text-center py-5"><h5 class="text-muted">No hay categorías disponibles</h5></div>';
        return;
    }

    grid.innerHTML = categoryArray.map(category => `
        <div class="col-lg-3 col-md-4 col-sm-6 mb-4">
            <div class="category-card" onclick="selectCategoryFromGrid('${category.name}')">
                <div class="category-icon">
                    <i class="bi ${category.icon}"></i>
                </div>
                <div class="category-name">${category.name}</div>
                <div class="category-count">${category.count} ${category.count === 1 ? 'frase' : 'frases'}</div>
            </div>
        </div>
    `).join('');
}

function renderPagination(totalItems, totalPages) {
    const pagination = document.getElementById('phrasesPagination');
    
    if (totalPages <= 1) {
        pagination.innerHTML = '';
        return;
    }

    let html = '';
    
    // Botón anterior
    if (currentPage > 1) {
        html += `
            <li class="page-item">
                <a class="page-link" href="#" onclick="changePage(${currentPage - 1})">
                    <i class="bi bi-chevron-left"></i>
                </a>
            </li>
        `;
    }
    
    // Páginas
    const startPage = Math.max(1, currentPage - 2);
    const endPage = Math.min(totalPages, currentPage + 2);
    
    if (startPage > 1) {
        html += `<li class="page-item"><a class="page-link" href="#" onclick="changePage(1)">1</a></li>`;
        if (startPage > 2) {
            html += `<li class="page-item disabled"><span class="page-link">...</span></li>`;
        }
    }
    
    for (let i = startPage; i <= endPage; i++) {
        html += `
            <li class="page-item ${i === currentPage ? 'active' : ''}">
                <a class="page-link" href="#" onclick="changePage(${i})">${i}</a>
            </li>
        `;
    }
    
    if (endPage < totalPages) {
        if (endPage < totalPages - 1) {
            html += `<li class="page-item disabled"><span class="page-link">...</span></li>`;
        }
        html += `<li class="page-item"><a class="page-link" href="#" onclick="changePage(${totalPages})">${totalPages}</a></li>`;
    }
    
    // Botón siguiente
    if (currentPage < totalPages) {
        html += `
            <li class="page-item">
                <a class="page-link" href="#" onclick="changePage(${currentPage + 1})">
                    <i class="bi bi-chevron-right"></i>
                </a>
            </li>
        `;
    }
    
    pagination.innerHTML = html;
}

function updateStats() {
    const totalPhrases = allPhrases.length;
    const categories = [...new Set(allPhrases.map(p => p.categoria))];
    
    document.getElementById('totalPhrasesCount').textContent = totalPhrases;
    document.getElementById('totalCategoriesCount').textContent = categories.length;
    
    // Categoría con más frases
    if (categories.length > 0) {
        const categoryCounts = {};
        allPhrases.forEach(phrase => {
            categoryCounts[phrase.categoria] = (categoryCounts[phrase.categoria] || 0) + 1;
        });
        
        const favoriteCategory = Object.entries(categoryCounts)
            .sort(([,a], [,b]) => b - a)[0];
        
        document.getElementById('favoriteCategory').textContent = favoriteCategory[0];
    }
    
    // Última actualización (simulada)
    document.getElementById('lastUpdate').textContent = new Date().toLocaleDateString('es-ES');
}

function updateCategoryFilter() {
    const select = document.getElementById('categoryFilter');
    const categories = [...new Set(allPhrases.map(p => p.categoria))].sort();
    
    const currentValue = select.value;
    select.innerHTML = '<option value="">Todas las categorías</option>';
    
    categories.forEach(category => {
        const option = document.createElement('option');
        option.value = category;
        option.textContent = category;
        select.appendChild(option);
    });
    
    select.value = currentValue;
}

// Funciones de eventos
function changePage(page) {
    currentPage = page;
    renderPhrases();
}

function selectCategoryFromGrid(category) {
    // Cambiar a la pestaña de frases
    const phrasesTab = new bootstrap.Tab(document.getElementById('phrases-tab'));
    phrasesTab.show();
    
    // Filtrar por categoría
    document.getElementById('categoryFilter').value = category;
    currentPage = 1;
    renderPhrases();
}

async function editPhrase(id) {
    try {
        const phrase = await fetchPhraseById(id);
        if (!phrase) return;

        editingPhraseId = id;
        document.getElementById('phraseModalTitle').innerHTML = '<i class="bi bi-pencil"></i> Editar Frase';
        
        // Llenar formulario
        document.getElementById('phraseSpanish').value = phrase.español;
        document.getElementById('phraseEnglish').value = phrase.ingles;
        document.getElementById('phrasePronunciation').value = phrase.pronunciacion;
        document.getElementById('phraseCategory').value = phrase.categoria;
        
        clearFormErrors();
        
        const modal = new bootstrap.Modal(document.getElementById('phraseModal'));
        modal.show();
        
    } catch (error) {
        showError('Error al cargar la frase para editar');
    }
}

function confirmDeletePhrase(id) {
    Swal.fire({
        title: '¿Estás seguro?',
        text: "Esta acción no se puede deshacer",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#dc3545',
        cancelButtonColor: '#6c757d',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then(async (result) => {
        if (result.isConfirmed) {
            const success = await deletePhrase(id);
            if (success) {
                await loadPhrases();
            }
        }
    });
}

function speakText(text, lang) {
    if ('speechSynthesis' in window) {
        const utterance = new SpeechSynthesisUtterance(text);
        utterance.lang = lang === 'es' ? 'es-ES' : 'en-US';
        utterance.rate = 0.8;
        utterance.pitch = 1;
        speechSynthesis.speak(utterance);
    } else {
        showError('Tu navegador no soporta síntesis de voz');
    }
}

// Funciones de formulario
function clearFormErrors() {
    const errorElements = document.querySelectorAll('.invalid-feedback');
    errorElements.forEach(element => {
        element.textContent = '';
    });
    
    const inputs = document.querySelectorAll('.form-control');
    inputs.forEach(input => {
        input.classList.remove('is-invalid');
    });
}

function showFieldError(fieldId, message) {
    const field = document.getElementById(fieldId.replace('Error', ''));
    const errorElement = document.getElementById(fieldId);
    
    if (field) field.classList.add('is-invalid');
    if (errorElement) errorElement.textContent = message;
}

function displayFormErrors(errors) {
    clearFormErrors();
    
    errors.forEach(error => {
        if (error.includes('Español')) {
            showFieldError('spanishError', error);
        } else if (error.includes('Inglés')) {
            showFieldError('englishError', error);
        } else if (error.includes('pronunciación')) {
            showFieldError('pronunciationError', error);
        } else if (error.includes('categoría')) {
            showFieldError('categoryError', error);
        }
    });
}

function validateForm(data) {
    let isValid = true;
    clearFormErrors();

    if (!data.español || data.español.trim() === '') {
        showFieldError('spanishError', 'El campo en español es requerido');
        isValid = false;
    }

    if (!data.ingles || data.ingles.trim() === '') {
        showFieldError('englishError', 'El campo en inglés es requerido');
        isValid = false;
    }

    if (!data.pronunciacion || data.pronunciacion.trim() === '') {
        showFieldError('pronunciationError', 'La pronunciación es requerida');
        isValid = false;
    }

    if (!data.categoria || data.categoria.trim() === '') {
        showFieldError('categoryError', 'La categoría es requerida');
        isValid = false;
    }

    // Validar caracteres especiales
    const specialChars = /[<>"'&]/;
    if (data.español && specialChars.test(data.español)) {
        showFieldError('spanishError', 'No se permiten caracteres especiales como <, >, ", \', &');
        isValid = false;
    }

    if (data.ingles && specialChars.test(data.ingles)) {
        showFieldError('englishError', 'No se permiten caracteres especiales como <, >, ", \', &');
        isValid = false;
    }

    // Validar categoría (solo letras y espacios)
    const categoryRegex = /^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$/;
    if (data.categoria && !categoryRegex.test(data.categoria)) {
        showFieldError('categoryError', 'La categoría solo puede contener letras y espacios');
        isValid = false;
    }

    return isValid;
}

// Funciones de carga
async function loadPhrases(category = '') {
    await fetchPhrases(category);
    renderPhrases();
    updateCategoryFilter();
    updateStats();
}

function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

// Event Listeners
document.addEventListener('DOMContentLoaded', async function() {
    // Cargar fecha actual
    document.getElementById('currentDate').textContent = new Date().toLocaleDateString('es-ES', {
        weekday: 'long',
        year: 'numeric',
        month: 'long',
        day: 'numeric'
    });
    
    // Cargar datos iniciales
    await loadPhrases();
    
    // Event listeners para filtros
    document.getElementById('phraseSearchInput').addEventListener('input', debounce(() => {
        currentPage = 1;
        renderPhrases();
    }, 300));
    
    document.getElementById('categoryFilter').addEventListener('change', () => {
        currentPage = 1;
        renderPhrases();
    });
    
    document.getElementById('clearFilters').addEventListener('click', () => {
        document.getElementById('phraseSearchInput').value = '';
        document.getElementById('categoryFilter').value = '';
        currentPage = 1;
        renderPhrases();
    });
    
    // Event listener para agregar frase
    document.getElementById('addPhraseBtn').addEventListener('click', () => {
        editingPhraseId = null;
        document.getElementById('phraseModalTitle').innerHTML = '<i class="bi bi-plus-lg"></i> Agregar Nueva Frase';
        document.getElementById('phraseForm').reset();
        clearFormErrors();
        
        const modal = new bootstrap.Modal(document.getElementById('phraseModal'));
        modal.show();
    });
    
    // Event listener para guardar frase
    document.getElementById('savePhraseBtn').addEventListener('click', async () => {
        const formData = {
            español: document.getElementById('phraseSpanish').value.trim(),
            ingles: document.getElementById('phraseEnglish').value.trim(),
            pronunciacion: document.getElementById('phrasePronunciation').value.trim(),
            categoria: document.getElementById('phraseCategory').value.trim()
        };
        
        if (!validateForm(formData)) {
            return;
        }
        
        let success = false;
        
        if (editingPhraseId) {
            success = await updatePhrase(editingPhraseId, formData);
        } else {
            success = await createPhrase(formData);
        }
        
        if (success) {
            const modal = bootstrap.Modal.getInstance(document.getElementById('phraseModal'));
            modal.hide();
            await loadPhrases();
        }
    });
    
    // Event listener para tabs
    document.querySelectorAll('[data-bs-toggle="tab"]').forEach(tab => {
        tab.addEventListener('shown.bs.tab', (e) => {
            const targetId = e.target.getAttribute('data-bs-target');
            
            if (targetId === '#categories') {
                renderCategories();
            } else if (targetId === '#stats') {
                updateStats();
            }
        });
    });
    
    // Cerrar modal con Escape
    document.addEventListener('keydown', (e) => {
        if (e.key === 'Escape') {
            const modal = bootstrap.Modal.getInstance(document.getElementById('phraseModal'));
            if (modal) {
                modal.hide();
            }
        }
    });
    
    console.log('Traductor Básico inicializado correctamente');
});