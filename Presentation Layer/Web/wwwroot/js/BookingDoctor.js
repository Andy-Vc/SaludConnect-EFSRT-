// BookingDoctor.js - Script optimizado para Rocket
(function () {
    'use strict';

    // Obtener configuración desde la vista
    const config = window.bookingDoctorConfig || {};
    let isNavigating = false;

    // Elementos del DOM
    const searchInput = document.getElementById('doctor-search');
    const availabilityFilter = document.getElementById('availability-filter');
    const experienceFilter = document.getElementById('experience-filter');
    const languageFilter = document.getElementById('language-filter');
    const resetFiltersBtn = document.getElementById('reset-filters');
    const doctorCards = document.querySelectorAll('.doctor-card');
    const noResults = document.getElementById('no-results');

    // Obtener specialtyId
    const specialtyIdElement = document.getElementById('selected-specialty-id');
    const currentSpecialtyId = specialtyIdElement ?
        specialtyIdElement.value :
        (config.specialtyId || null);

    console.log('=== 🚀 BOOKINGDOCTOR INICIADO ===');

    function filterDoctors() {
        const searchTerm = searchInput.value.toLowerCase();
        const availability = availabilityFilter.value;
        const experience = experienceFilter.value;
        const language = languageFilter.value.toLowerCase();

        let visibleCount = 0;

        doctorCards.forEach(card => {
            const doctorName = card.dataset.doctor;
            const doctorExperience = parseInt(card.dataset.experience);
            const doctorLanguages = card.dataset.languages;
            const doctorAvailability = card.dataset.availability;

            const matchesSearch = doctorName.includes(searchTerm);
            const matchesAvailability = availability === 'all' || doctorAvailability.includes(availability);

            let matchesExperience = true;
            if (experience !== 'all') {
                const minExperience = parseInt(experience.replace('+', ''));
                matchesExperience = doctorExperience >= minExperience;
            }

            const matchesLanguage = language === 'all' || doctorLanguages.includes(language);

            if (matchesSearch && matchesAvailability && matchesExperience && matchesLanguage) {
                card.style.display = 'block';
                visibleCount++;
            } else {
                card.style.display = 'none';
            }
        });

        noResults.classList.toggle('hidden', visibleCount > 0);
    }

    // FUNCIÓN PRINCIPAL MEJORADA PARA ROCKET
    window.selectDoctor = function (idDoctor, doctorName, event) {
        console.log('🎯 selectDoctor ejecutada - Iniciando navegación...');

        // PREVENCIÓN AGGRESIVA para Rocket
        if (event) {
            if (typeof event.preventDefault === 'function') event.preventDefault();
            if (typeof event.stopPropagation === 'function') event.stopPropagation();
            if (typeof event.stopImmediatePropagation === 'function') event.stopImmediatePropagation();
        }

        // Cancelar cualquier navegación existente
        if (isNavigating) {
            console.log('⏳ Navegación ya en curso');
            return false;
        }

        // Validaciones rápidas
        if (!idDoctor || idDoctor === '0' || !currentSpecialtyId || currentSpecialtyId === '0') {
            console.error('❌ IDs inválidos:', { idDoctor, currentSpecialtyId });
            return false;
        }

        isNavigating = true;

        // Construir URL ABSOLUTA
        const baseUrl = window.location.origin;
        const url = `${baseUrl}/Patient/BookingDate?idDoctor=${idDoctor}&idSpecialty=${currentSpecialtyId}`;

        console.log('🔗 Navegando a:', url);

        // Feedback visual INMEDIATO
        if (event && event.target) {
            const button = event.target;
            const originalText = button.textContent;
            button.textContent = '🔄 Redirigiendo...';
            button.disabled = true;
        }

        // NAVEGACIÓN INMEDIATA - Sin delay para evitar interferencias de Rocket
        console.log('🎯 EJECUTANDO REDIRECCIÓN INMEDIATA');
        window.location.href = url;

        return false;
    };

    // REMOVER el event listener de respaldo que causa conflicto con Rocket
    // (Elimina toda la sección del DOMContentLoaded con event listeners)

    // SOLO mantener los event listeners para filtros
    searchInput.addEventListener('input', filterDoctors);
    availabilityFilter.addEventListener('change', filterDoctors);
    experienceFilter.addEventListener('change', filterDoctors);
    languageFilter.addEventListener('change', filterDoctors);

    resetFiltersBtn.addEventListener('click', function (event) {
        event.preventDefault();
        searchInput.value = '';
        availabilityFilter.value = 'all';
        experienceFilter.value = 'all';
        languageFilter.value = 'all';
        filterDoctors();
    });

    searchInput.addEventListener('keydown', function (e) {
        if (e.key === 'Escape') {
            this.value = '';
            filterDoctors();
            this.blur();
        }
    });

    // Inicializar
    filterDoctors();
    console.log('✅ BookingDoctor.js listo');

})();