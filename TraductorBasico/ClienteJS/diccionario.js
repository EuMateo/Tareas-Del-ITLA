// Sistema de Diccionario Visual
class DiccionarioManager {
    constructor() {
        this.imagenes = [];
        this.currentCategory = '';
        this.currentPage = 1;
        this.itemsPerPage = 8;
    }

    // Cargar todas las imágenes del diccionario
    async loadImagenes(categoria = '') {
        try {
            showLoading(true);

            let url = `${API_BASE_URL}/diccionario/imagenes`;
            if (categoria && categoria !== '') {
                url += `/categoria/${encodeURIComponent(categoria)}`;
            }

            const response = await fetch(url);

            if (!response.ok) {
                throw new Error(`Error HTTP: ${response.status}`);
            }

            const result = await response.json();

            if (result.data) {
                this.imagenes = Array.isArray(result.data) ? result.data : [result.data];
            } else {
                this.imagenes = [];
            }

            return this.imagenes;

        } catch (error) {
            console.error('Error loading diccionario images:', error);
            showError('Error al cargar las imágenes del diccionario: ' + error.message);
            this.imagenes = [];
            return [];
        } finally {
            showLoading(false);
        }
    }

    // Cargar imagen específica con sus frases
    async loadImagenWithFrases(imagenId) {
        try {
            const response = await fetch(`${API_BASE_URL}/diccionario/imagenes/${imagenId}/frases`);

            if (!response.ok) {
                throw new Error('Imagen no encontrada');
            }

            const result = await response.json();
            return result.data;

        } catch (error) {
            showError('Error al cargar la imagen: ' + error.message);
            return null;
        }
    }

    // Cargar categorías disponibles
    async loadCategorias() {
        try {
            const response = await fetch(`${API_BASE_URL}/diccionario/categorias`);

            if (!response.ok) {
                throw new Error('Error al cargar categorías');
            }

            const result = await response.json();
            return result.data || [];

        } catch (error) {
            console.error('Error loading categorias:', error);
            return [];
        }
    }

    // Renderizar grid de imágenes
    renderImagenes() {
        const grid = document.getElementById('diccionarioGrid');
        const noResults = document.getElementById('noDiccionarioResults');
        const searchTerm = document.getElementById('diccionarioSearchInput')?.value.toLowerCase().trim() || '';
        const categoryFilter = document.getElementById('diccionarioCategoryFilter')?.value || '';

        // Filtrar imágenes
        let filteredImagenes = this.imagenes.filter(imagen => {
            const matchSearch = !searchTerm ||
                imagen.nombre.toLowerCase().includes(searchTerm) ||
                imagen.descripcion.toLowerCase().includes(searchTerm) ||
                imagen.categoria.toLowerCase().includes(searchTerm);

            const matchCategory = !categoryFilter || imagen.categoria === categoryFilter;

            return matchSearch && matchCategory;
        });

        // Paginación
        const totalPages = Math.ceil(filteredImagenes.length / this.itemsPerPage);
        const startIndex = (this.currentPage - 1) * this.itemsPerPage;
        const endIndex = startIndex + this.itemsPerPage;
        const paginatedImagenes = filteredImagenes.slice(startIndex, endIndex);

        if (paginatedImagenes.length === 0) {
            grid.innerHTML = '';
            if (noResults) noResults.style.display = 'block';
            this.renderDiccionarioPagination(0, 0);
            return;
        }

        if (noResults) noResults.style.display = 'none';

        grid.innerHTML = paginatedImagenes.map(imagen => this.createImagenCard(imagen)).join('');
        this.renderDiccionarioPagination(filteredImagenes.length, totalPages);
    }

    // Crear card de imagen
    createImagenCard(imagen) {
        return `
            <div class="col-lg-3 col-md-4 col-sm-6 mb-4">
                <div class="diccionario-card" onclick="diccionarioManager.showImagenModal(${imagen.id})">
                    <div class="imagen-container">
                        <img src="${escapeHtml(imagen.rutaImagen)}" 
                             alt="${escapeHtml(imagen.nombre)}" 
                             class="diccionario-imagen"
                             onerror="this.src='images/placeholder.png'">
                        <div class="imagen-overlay">
                            <i class="bi bi-eye-fill"></i>
                            <span>Ver frases</span>
                        </div>
                    </div>
                    <div class="diccionario-info">
                        <h6 class="imagen-nombre">${escapeHtml(imagen.nombre)}</h6>
                        <p class="imagen-categoria">
                            <i class="bi bi-tag"></i> ${escapeHtml(imagen.categoria)}
                        </p>
                        <p class="imagen-frases-count">
                            <i class="bi bi-chat-dots"></i> ${imagen.cantidadFrases} frases
                        </p>
                        ${imagen.descripcion ? `<p class="imagen-descripcion">${escapeHtml(imagen.descripcion)}</p>` : ''}
                    </div>
                </div>
            </div>
        `;
    }

    // Mostrar modal con las frases de una imagen
    async showImagenModal(imagenId) {
        try {
            const imagen = await this.loadImagenWithFrases(imagenId);
            if (!imagen) return;

            const modalTitle = document.getElementById('diccionarioModalTitle');
            const modalBody = document.getElementById('diccionarioModalBody');

            if (modalTitle) {
                modalTitle.innerHTML = `
                    <img src="${escapeHtml(imagen.rutaImagen)}" 
                         alt="${escapeHtml(imagen.nombre)}" 
                         class="modal-imagen"
                         onerror="this.style.display='none'">
                    <div class="modal-title-text">
                        <h5>${escapeHtml(imagen.nombre)}</h5>
                        <span class="badge bg-primary">${escapeHtml(imagen.categoria)}</span>
                    </div>
                `;
            }

            if (modalBody) {
                if (imagen.frasesRelacionadas && imagen.frasesRelacionadas.length > 0) {
                    modalBody.innerHTML = `
                        ${imagen.descripcion ? `<p class="imagen-descripcion-modal">${escapeHtml(imagen.descripcion)}</p>` : ''}
                        <div class="frases-relacionadas">
                            <h6><i class="bi bi-chat-dots"></i> Frases relacionadas (${imagen.frasesRelacionadas.length})</h6>
                            <div class="row">
                                ${imagen.frasesRelacionadas.map(frase => this.createModalFraseCard(frase)).join('')}
                            </div>
                        </div>
                    `;
                } else {
                    modalBody.innerHTML = `
                        ${imagen.descripcion ? `<p class="imagen-descripcion-modal">${escapeHtml(imagen.descripcion)}</p>` : ''}
                        <div class="text-center py-4">
                            <i class="bi bi-chat-x display-4 text-muted"></i>
                            <h6 class="text-muted mt-2">No hay frases relacionadas</h6>
                            <p class="text-muted">Esta imagen aún no tiene frases asociadas.</p>
                        </div>
                    `;
                }
            }

            const modal = new bootstrap.Modal(document.getElementById('diccionarioModal'));
            modal.show();

        } catch (error) {
            showError('Error al cargar la información de la imagen');
        }
    }

    // Crear card de frase en el modal usando data attributes
    createModalFraseCard(frase) {
        const isFavorito = favoritosManager.isFavorito(frase.id);

        return `
            <div class="col-12 mb-3" data-frase-id="${frase.id}">
                <div class="modal-phrase-card">
                    <div class="modal-phrase-header">
                        <button class="btn btn-sm ${isFavorito ? 'btn-warning favorito-active' : 'btn-outline-warning'} favorito-btn" 
                                onclick="favoritosManager.toggleFavorito({
                                    id: ${frase.id},
                                    español: '${escapeHtml(frase.español)}',
                                    ingles: '${escapeHtml(frase.ingles)}',
                                    pronunciacion: '${escapeHtml(frase.pronunciacion)}',
                                    categoria: '${escapeHtml(frase.categoria)}'
                                })" 
                                title="${isFavorito ? 'Remover de favoritos' : 'Agregar a favoritos'}">
                            <i class="bi ${isFavorito ? 'bi-heart-fill' : 'bi-heart'}"></i>
                        </button>
                    </div>
                    <div class="modal-phrase-content">
                        <div class="modal-phrase-row">
                            <div class="phrase-flag flag-es"></div>
                            <span class="phrase-text">${escapeHtml(frase.español)}</span>
                            <button class="play-btn" 
                                    data-text="${escapeHtml(frase.español)}" 
                                    data-lang="es"
                                    onclick="speakFromData(this)" 
                                    title="Escuchar en español">
                                <i class="bi bi-volume-up"></i>
                            </button>
                        </div>
                        <div class="modal-phrase-row">
                            <div class="phrase-flag flag-en"></div>
                            <span class="phrase-text">${escapeHtml(frase.ingles)}</span>
                            <button class="play-btn" 
                                    data-text="${escapeHtml(frase.ingles)}" 
                                    data-lang="en"
                                    onclick="speakFromData(this)" 
                                    title="Escuchar en inglés">
                                <i class="bi bi-volume-up"></i>
                            </button>
                        </div>
                        <div class="modal-pronunciation">
                            <i class="bi bi-info-circle"></i> ${escapeHtml(frase.pronunciacion)}
                        </div>
                    </div>
                </div>
            </div>
        `;
    }

    // Renderizar paginación del diccionario
    renderDiccionarioPagination(totalItems, totalPages) {
        const pagination = document.getElementById('diccionarioPagination');

        if (!pagination || totalPages <= 1) {
            if (pagination) pagination.innerHTML = '';
            return;
        }

        let html = '';

        // Botón anterior
        if (this.currentPage > 1) {
            html += `
                <li class="page-item">
                    <a class="page-link" href="#" onclick="diccionarioManager.changePage(${this.currentPage - 1})">
                        <i class="bi bi-chevron-left"></i>
                    </a>
                </li>
            `;
        }

        // Páginas
        const startPage = Math.max(1, this.currentPage - 2);
        const endPage = Math.min(totalPages, this.currentPage + 2);

        if (startPage > 1) {
            html += `<li class="page-item"><a class="page-link" href="#" onclick="diccionarioManager.changePage(1)">1</a></li>`;
            if (startPage > 2) {
                html += `<li class="page-item disabled"><span class="page-link">...</span></li>`;
            }
        }

        for (let i = startPage; i <= endPage; i++) {
            html += `
                <li class="page-item ${i === this.currentPage ? 'active' : ''}">
                    <a class="page-link" href="#" onclick="diccionarioManager.changePage(${i})">${i}</a>
                </li>
            `;
        }

        if (endPage < totalPages) {
            if (endPage < totalPages - 1) {
                html += `<li class="page-item disabled"><span class="page-link">...</span></li>`;
            }
            html += `<li class="page-item"><a class="page-link" href="#" onclick="diccionarioManager.changePage(${totalPages})">${totalPages}</a></li>`;
        }

        // Botón siguiente
        if (this.currentPage < totalPages) {
            html += `
                <li class="page-item">
                    <a class="page-link" href="#" onclick="diccionarioManager.changePage(${this.currentPage + 1})">
                        <i class="bi bi-chevron-right"></i>
                    </a>
                </li>
            `;
        }

        pagination.innerHTML = html;
    }

    // Cambiar página
    changePage(page) {
        this.currentPage = page;
        this.renderImagenes();
    }

    // Actualizar filtro de categorías
    async updateDiccionarioCategoryFilter() {
        const select = document.getElementById('diccionarioCategoryFilter');
        if (!select) return;

        const categorias = await this.loadCategorias();
        const currentValue = select.value;

        select.innerHTML = '<option value="">Todas las categorías</option>';

        categorias.forEach(categoria => {
            const option = document.createElement('option');
            option.value = categoria.categoria;
            option.textContent = `${categoria.categoria} (${categoria.cantidadFrases})`;
            select.appendChild(option);
        });

        select.value = currentValue;
    }

    // Buscar imágenes
    searchImagenes(searchTerm) {
        this.currentPage = 1;
        this.renderImagenes();
    }

    // Filtrar por categoría
    filterByCategory(categoria) {
        this.currentCategory = categoria;
        this.currentPage = 1;
        this.renderImagenes();
    }

    // Limpiar filtros
    clearFilters() {
        const searchInput = document.getElementById('diccionarioSearchInput');
        const categoryFilter = document.getElementById('diccionarioCategoryFilter');

        if (searchInput) searchInput.value = '';
        if (categoryFilter) categoryFilter.value = '';

        this.currentCategory = '';
        this.currentPage = 1;
        this.renderImagenes();
    }
}

// Funciones externas a la clase
function speakFromData(button) {
    const text = button.getAttribute('data-text');
    const lang = button.getAttribute('data-lang');

    if (!text) return;

    speakTextSafe(text, lang);
}

function speakTextSafe(text, language = 'en') {
    if (!('speechSynthesis' in window)) {
        console.warn('Tu navegador no soporta síntesis de voz');
        return;
    }

    try {
        const cleanText = text.trim();
        window.speechSynthesis.cancel();

        const utterance = new SpeechSynthesisUtterance(cleanText);
        utterance.lang = language === 'es' ? 'es-ES' : 'en-US';
        utterance.rate = 0.8;
        utterance.pitch = 1;
        utterance.volume = 1;

        utterance.onerror = function (event) {
            console.error('Error en síntesis de voz:', event.error);
        };

        window.speechSynthesis.speak(utterance);

    } catch (error) {
        console.error('Error al reproducir texto:', error);
    }
}

// Instancia global del manager del diccionario
const diccionarioManager = new DiccionarioManager();
