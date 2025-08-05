// Sistema de Favoritos
class FavoritosManager {
    constructor() {
        this.storageKey = 'traductor_favoritos';
        this.favoritos = this.loadFavoritos();
    }

    // Cargar favoritos del localStorage
    loadFavoritos() {
        try {
            const stored = localStorage.getItem(this.storageKey);
            return stored ? JSON.parse(stored) : [];
        } catch (error) {
            console.error('Error loading favoritos:', error);
            return [];
        }
    }

    // Guardar favoritos en localStorage
    saveFavoritos() {
        try {
            localStorage.setItem(this.storageKey, JSON.stringify(this.favoritos));
            this.updateFavoritosCount();
            return true;
        } catch (error) {
            console.error('Error saving favoritos:', error);
            return false;
        }
    }

    // Verificar si una frase es favorita
    isFavorito(fraseId) {
        return this.favoritos.some(fav => fav.id === fraseId);
    }

    // Agregar frase a favoritos
    addFavorito(frase) {
        if (!this.isFavorito(frase.id)) {
            const favorito = {
                id: frase.id,
                español: frase.español,
                ingles: frase.ingles,
                pronunciacion: frase.pronunciacion,
                categoria: frase.categoria,
                fechaAgregado: new Date().toISOString()
            };
            
            this.favoritos.unshift(favorito); // Agregar al inicio
            this.saveFavoritos();
            this.updateFavoritoButton(frase.id, true);
            this.showSuccessToast('Frase agregada a favoritos');
            return true;
        }
        return false;
    }

    // Remover frase de favoritos
    removeFavorito(fraseId) {
        const index = this.favoritos.findIndex(fav => fav.id === fraseId);
        if (index > -1) {
            this.favoritos.splice(index, 1);
            this.saveFavoritos();
            this.updateFavoritoButton(fraseId, false);
            this.showSuccessToast('Frase removida de favoritos');
            
            // Si estamos en la pestaña de favoritos, actualizar la vista
            if (this.isInFavoritosTab()) {
                this.renderFavoritos();
            }
            return true;
        }
        return false;
    }

    // Toggle favorito
    toggleFavorito(frase) {
        if (this.isFavorito(frase.id)) {
            this.removeFavorito(frase.id);
        } else {
            this.addFavorito(frase);
        }
    }

    // Obtener todos los favoritos
    getFavoritos() {
        return [...this.favoritos];
    }

    // Limpiar todos los favoritos
    clearFavoritos() {
        this.favoritos = [];
        this.saveFavoritos();
        this.updateAllFavoritoButtons();
        this.renderFavoritos();
        this.showSuccessToast('Todos los favoritos han sido eliminados');
    }

    // Exportar favoritos como texto
    exportFavoritos() {
        if (this.favoritos.length === 0) {
            this.showErrorToast('No hay favoritos para exportar');
            return;
        }

        const content = this.generateExportContent();
        this.downloadAsFile(content, 'mis-frases-favoritas.txt', 'text/plain');
        this.showSuccessToast('Favoritos exportados exitosamente');
    }

    // Generar contenido para exportar
    generateExportContent() {
        let content = `MIS FRASES FAVORITAS - TRADUCTOR BÁSICO\n`;
        content += `Generado el: ${new Date().toLocaleDateString('es-ES')}\n`;
        content += `Total de frases: ${this.favoritos.length}\n\n`;
        content += '='.repeat(50) + '\n\n';

        const frasesAgrupadas = this.groupByCategory(this.favoritos);
        
        Object.keys(frasesAgrupadas).sort().forEach(categoria => {
            content += `📂 ${categoria.toUpperCase()}\n`;
            content += '-'.repeat(30) + '\n';
            
            frasesAgrupadas[categoria].forEach((frase, index) => {
                content += `${index + 1}. 🇪🇸 ${frase.español}\n`;
                content += `   🇺🇸 ${frase.ingles}\n`;
                content += `   🔊 ${frase.pronunciacion}\n\n`;
            });
            
            content += '\n';
        });

        return content;
    }

    // Agrupar favoritos por categoría
    groupByCategory(favoritos) {
        return favoritos.reduce((grupos, frase) => {
            const categoria = frase.categoria || 'Sin categoría';
            if (!grupos[categoria]) {
                grupos[categoria] = [];
            }
            grupos[categoria].push(frase);
            return grupos;
        }, {});
    }

    // Descargar archivo
    downloadAsFile(content, filename, mimeType) {
        const blob = new Blob([content], { type: mimeType });
        const url = URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = filename;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        URL.revokeObjectURL(url);
    }

    // Actualizar botón de favorito específico
    updateFavoritoButton(fraseId, isFavorito) {
        const buttons = document.querySelectorAll(`[data-frase-id="${fraseId}"] .favorito-btn`);
        buttons.forEach(button => {
            const icon = button.querySelector('i');
            if (isFavorito) {
                icon.className = 'bi bi-heart-fill';
                button.classList.add('favorito-active');
                button.title = 'Remover de favoritos';
            } else {
                icon.className = 'bi bi-heart';
                button.classList.remove('favorito-active');
                button.title = 'Agregar a favoritos';
            }
        });
    }

    // Actualizar todos los botones de favorito
    updateAllFavoritoButtons() {
        document.querySelectorAll('.favorito-btn').forEach(button => {
            const fraseCard = button.closest('[data-frase-id]');
            if (fraseCard) {
                const fraseId = parseInt(fraseCard.getAttribute('data-frase-id'));
                this.updateFavoritoButton(fraseId, this.isFavorito(fraseId));
            }
        });
    }

    // Actualizar contador de favoritos
    updateFavoritosCount() {
        const countElement = document.getElementById('favoritosCount');
        if (countElement) {
            countElement.textContent = this.favoritos.length;
        }

        const badge = document.querySelector('#favorites-tab .badge');
        if (badge) {
            badge.textContent = this.favoritos.length;
            badge.style.display = this.favoritos.length > 0 ? 'inline' : 'none';
        }
    }

    // Verificar si estamos en la pestaña de favoritos
    isInFavoritosTab() {
        const favoritesTab = document.getElementById('favorites-tab');
        return favoritesTab && favoritesTab.classList.contains('active');
    }

    // Renderizar favoritos en la interfaz
    renderFavoritos() {
        const container = document.getElementById('favoritosGrid');
        const noFavoritos = document.getElementById('noFavoritos');
        const favoritosStats = document.getElementById('favoritosStats');

        if (!container) return;

        if (this.favoritos.length === 0) {
            container.innerHTML = '';
            if (noFavoritos) noFavoritos.style.display = 'block';
            if (favoritosStats) favoritosStats.style.display = 'none';
            return;
        }

        if (noFavoritos) noFavoritos.style.display = 'none';
        if (favoritosStats) {
            favoritosStats.style.display = 'block';
            this.updateFavoritosStats();
        }

        container.innerHTML = this.favoritos.map(frase => this.createFavoritoCard(frase)).join('');
        this.updateAllFavoritoButtons();
    }

    // Crear card de favorito
    createFavoritoCard(frase) {
        return `
            <div class="col-lg-6 col-md-12" data-frase-id="${frase.id}">
                <div class="phrase-card favorito-card">
                    <div class="phrase-header">
                        <span class="phrase-category">${escapeHtml(frase.categoria)}</span>
                        <div class="phrase-actions">
                            <button class="btn btn-warning btn-action favorito-btn favorito-active" 
                                    onclick="favoritosManager.removeFavorito(${frase.id})" 
                                    title="Remover de favoritos">
                                <i class="bi bi-heart-fill"></i>
                            </button>
                            <small class="text-muted ms-2">
                                ${this.formatFechaAgregado(frase.fechaAgregado)}
                            </small>
                        </div>
                    </div>
                    <div class="phrase-content">
                        <div class="phrase-row">
                            <div class="phrase-flag flag-es"></div>
                            <span class="phrase-text">${escapeHtml(frase.español)}</span>
                            <button class="play-btn" onclick="speakText('${escapeHtml(frase.español)}', 'es')" title="Escuchar en español">
                                <i class="bi bi-volume-up"></i>
                            </button>
                        </div>
                        <div class="phrase-row">
                            <div class="phrase-flag flag-en"></div>
                            <span class="phrase-text">${escapeHtml(frase.ingles)}</span>
                            <button class="play-btn" onclick="speakText('${escapeHtml(frase.ingles)}', 'en')" title="Escuchar en inglés">
                                <i class="bi bi-volume-up"></i>
                            </button>
                        </div>
                        <div class="pronunciation">
                            <i class="bi bi-info-circle"></i> ${escapeHtml(frase.pronunciacion)}
                        </div>
                    </div>
                </div>
            </div>
        `;
    }

    // Formatear fecha de agregado
    formatFechaAgregado(fechaISO) {
        const fecha = new Date(fechaISO);
        const ahora = new Date();
        const diffMs = ahora - fecha;
        const diffDias = Math.floor(diffMs / (1000 * 60 * 60 * 24));

        if (diffDias === 0) return 'Hoy';
        if (diffDias === 1) return 'Ayer';
        if (diffDias < 7) return `Hace ${diffDias} días`;
        return fecha.toLocaleDateString('es-ES');
    }

    // Actualizar estadísticas de favoritos
    updateFavoritosStats() {
        const totalElement = document.getElementById('totalFavoritosCount');
        const categoriasElement = document.getElementById('categoriasFavoritasCount');
        const recenteElement = document.getElementById('favoritoReciente');

        if (totalElement) {
            totalElement.textContent = this.favoritos.length;
        }

        if (categoriasElement) {
            const categorias = [...new Set(this.favoritos.map(f => f.categoria))];
            categoriasElement.textContent = categorias.length;
        }

        if (recenteElement && this.favoritos.length > 0) {
            const reciente = this.favoritos[0]; // El más reciente (está al inicio)
            recenteElement.textContent = `${reciente.español} (${reciente.categoria})`;
        }
    }

    // Mostrar toast de éxito
    showSuccessToast(message) {
        if (typeof Swal !== 'undefined') {
            Swal.fire({
                icon: 'success',
                title: message,
                timer: 2000,
                showConfirmButton: false,
                toast: true,
                position: 'top-end'
            });
        }
    }

    // Mostrar toast de error
    showErrorToast(message) {
        if (typeof Swal !== 'undefined') {
            Swal.fire({
                icon: 'error',
                title: message,
                timer: 3000,
                showConfirmButton: false,
                toast: true,
                position: 'top-end'
            });
        }
    }

    // Buscar en favoritos
    searchFavoritos(searchTerm) {
        if (!searchTerm.trim()) {
            this.renderFavoritos();
            return;
        }

        const term = searchTerm.toLowerCase();
        const filteredFavoritos = this.favoritos.filter(frase =>
            frase.español.toLowerCase().includes(term) ||
            frase.ingles.toLowerCase().includes(term) ||
            frase.pronunciacion.toLowerCase().includes(term) ||
            frase.categoria.toLowerCase().includes(term)
        );

        this.renderFilteredFavoritos(filteredFavoritos);
    }

    // Renderizar favoritos filtrados
    renderFilteredFavoritos(favoritos) {
        const container = document.getElementById('favoritosGrid');
        const noFavoritos = document.getElementById('noFavoritos');

        if (!container) return;

        if (favoritos.length === 0) {
            container.innerHTML = '';
            if (noFavoritos) {
                noFavoritos.innerHTML = `
                    <i class="bi bi-search display-1 text-muted"></i>
                    <h4 class="text-muted">No se encontraron favoritos</h4>
                    <p class="text-muted">Intenta con otros términos de búsqueda</p>
                `;
                noFavoritos.style.display = 'block';
            }
            return;
        }

        if (noFavoritos) noFavoritos.style.display = 'none';
        container.innerHTML = favoritos.map(frase => this.createFavoritoCard(frase)).join('');
        this.updateAllFavoritoButtons();
    }
}

// Instancia global del manager de favoritos
const favoritosManager = new FavoritosManager();