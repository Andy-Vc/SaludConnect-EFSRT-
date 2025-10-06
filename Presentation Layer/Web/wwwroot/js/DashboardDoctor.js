document.addEventListener('DOMContentLoaded', function () {
    const calendarContainer = document.getElementById('myCalendarContainer');
    const appointmentContainer = document.getElementById('appointmentsContainer');
    const selectedDateParagraph = document.querySelector('.bg-light.rounded.m-3.p-3.border-5 p:nth-child(2)');
    const badgeCount = document.querySelector('.badge.bg-light.text-dark');
    const titleSpan = document.querySelector('span.text-black.ms-2');
    const dateLabel = document.querySelector('.bg-light.rounded.m-3.p-3.border-5 p:nth-child(1)');
    const appointmentsCountLabel = document.querySelector('#countLeft');

    if (!calendarContainer || !appointmentContainer || !selectedDateParagraph || !badgeCount || !titleSpan) {
        console.error("No se encontró algún elemento del DOM necesario para el calendario.");
        return;
    }

    // Variables globales para paginación
    let appointmentsData = [];
    let currentPage = 1;
    const itemsPerPage = 3;
    let calendarInstance = null;

    function loadAppointments(dateStr) {
        console.log("Cargando citas para fecha:", dateStr);
        if (!dateStr) return;

        fetch(`/Doctor/GetAppointmentsJson?date=${dateStr}`)
            .then(response => response.json())
            .then(data => {
                appointmentContainer.innerHTML = '';

                const [year, month, day] = dateStr.split('-').map(Number);
                const selectedDate = new Date(year, month - 1, day);
                const formattedDate = selectedDate.toLocaleDateString('es-ES', {
                    weekday: 'short',
                    year: 'numeric',
                    month: 'short',
                    day: 'numeric'
                });

                titleSpan.textContent = `Citas para el ${formattedDate}`;
                selectedDateParagraph.textContent = `${data.length} citas`;
                dateLabel.textContent = formattedDate;
                appointmentsCountLabel.textContent = `${data.length} citas`;

                if (!data || data.length === 0) {
                    appointmentContainer.innerHTML = `
						<div class="text-center py-5">
							<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24"
								viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"
								stroke-linecap="round" stroke-linejoin="round"
								class="lucide lucide-calendar w-12 h-12 text-gray-300 scale-3 mx-auto mb-4"
								aria-hidden="true">
								<path d="M8 2v4"></path>
								<path d="M16 2v4"></path>
								<rect width="18" height="18" x="3" y="4" rx="2"></rect>
								<path d="M3 10h18"></path>
							</svg>
							<p class="text-muted">No hay citas para esta fecha</p>
						</div>
					`;
                    return;
                }

                appointmentsData = data;
                currentPage = 1;
                renderAppointmentsPage();
            })
            .catch(err => {
                console.error('Error al obtener citas:', err);
                appointmentContainer.innerHTML = `
					<div class="text-danger text-center mt-3">❌ Error al cargar citas</div>
				`;
            });
    }

    function renderAppointmentsPage() {
        const startIndex = (currentPage - 1) * itemsPerPage;
        const endIndex = startIndex + itemsPerPage;
        const pageItems = appointmentsData.slice(startIndex, endIndex);

        appointmentContainer.innerHTML = '';

        // Obtener el token anti-falsificación una sola vez
        const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value || '';

        pageItems.forEach(item => {
            const patientFirstName = item.patient?.firstName || 'Paciente';
            const patientLastNamePat = item.patient?.lastNamePat || '';
            const patientLastNameMat = item.patient?.lastNameMat || '';
            const patientLastName = [patientLastNamePat, patientLastNameMat].filter(Boolean).join(' ');

            const initials = (patientFirstName[0] || '') + (patientLastNamePat[0] || '');
            const reason = item.service?.nameService || 'Motivo no especificado';
            const date = new Date(item.dateAppointment);
            const time = date.toLocaleTimeString('es-ES', { hour: '2-digit', minute: '2-digit' });

            let statusText = 'Desconocido';
            switch (item.state) {
                case 'A': statusText = 'Atendida'; break;
                case 'P': statusText = 'Pendiente'; break;
                case 'N': statusText = 'No asistió'; break;
                case 'X': statusText = 'Cancelada'; break;
            }
            const today = new Date();
            today.setHours(0, 0, 0, 0);

            const appointmentDate = new Date(item.dateAppointment);
            appointmentDate.setHours(0, 0, 0, 0);

            let buttonsHtml = '';
            if (item.state === 'P' && appointmentDate.getTime() === today.getTime()) {
                buttonsHtml = `
    <div class="d-flex gap-2">
        <form method="POST" action="/Doctor/ChangeAppointmentState" class="d-inline">
            <input type="hidden" name="__RequestVerificationToken" value="${token}" />
            <input type="hidden" name="IdAppointment" value="${item.idAppointment}" />
            <input type="hidden" name="State" value="A" />
            <button type="submit" class="btn btn-outline-success btn-sm d-flex align-items-center gap-1">
                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" stroke="currentColor"
                stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="lucide lucide-circle-check-big w-4 h-4 mr-1">
                <path d="M21.801 10A10 10 0 1 1 17 3.335"></path> <path d="m9 11 3 3L22 4"></path>
                </svg> Asistió 
            </button>
        </form>
        
        <form method="POST" action="/Doctor/ChangeAppointmentState" class="d-inline">
            <input type="hidden" name="__RequestVerificationToken" value="${token}" />
            <input type="hidden" name="IdAppointment" value="${item.idAppointment}" />
            <input type="hidden" name="State" value="N" />
            <button type="submit" class="btn btn-outline-danger btn-sm d-flex align-items-center gap-1">
                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" stroke="currentColor"
                stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="lucide lucide-circle-x w-4 h-4 mr-1">
                <circle cx="12" cy="12" r="10"></circle>
                <path d="m15 9-6 6"></path> <path d="m9 9 6 6"></path>
                </svg> No Asistió
            </button>
        </form>
    </div>
    `;
            }


            const html = `
        <div class="p-4 border rounded-3 shadow-sm hover:shadow transition mb-3">
            <div class="d-flex justify-content-between align-items-start mb-3">
                <div class="d-flex align-items-center gap-3">
                    <div class="bg-primary bg-opacity-10 text-primary rounded-circle d-flex align-items-center justify-content-center"
                        style="width: 40px; height: 40px;">
                        <span class="fw-semibold small">${initials}</span>
                    </div>
                    <div>
                        <p class="mb-0 fw-semibold text-dark">${patientFirstName} ${patientLastName}</p>
                        <small class="text-muted">${reason}</small>
                    </div>
                </div>
                <div class="text-end">
                    <p class="mb-1 fw-semibold text-dark">${time}</p>
                    <span class="badge bg-light text-black fw-medium small">${statusText}</span>
                </div>
            </div>
            ${buttonsHtml}
        </div>
    `;

            appointmentContainer.innerHTML += html;
        });

        renderPaginationControls();
    }
    function renderPaginationControls() {
        const totalPages = Math.ceil(appointmentsData.length / itemsPerPage);

        if (totalPages <= 1) return;

        const paginationHtml = `
			<div class="d-flex justify-content-center mt-3 gap-2">
				<button class="btn btn-outline-primary btn-sm" ${currentPage === 1 ? 'disabled' : ''} id="prevPageBtn">
                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <path d="M19 12H5"/>
                <path d="m12 19-7-7 7-7"/>
                </svg></button>
				<span class="align-self-center fw-semibold">Página ${currentPage} de ${totalPages}</span>
				<button class="btn btn-outline-primary btn-sm" ${currentPage === totalPages ? 'disabled' : ''} id="nextPageBtn">
                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <path d="M5 12h14"/>
                <path d="m12 5 7 7-7 7"/>
                </svg></button>
			</div>
		`;

        appointmentContainer.innerHTML += paginationHtml;

        document.getElementById("prevPageBtn")?.addEventListener("click", () => {
            if (currentPage > 1) {
                currentPage--;
                renderAppointmentsPage();
            }
        });

        document.getElementById("nextPageBtn")?.addEventListener("click", () => {
            if (currentPage < totalPages) {
                currentPage++;
                renderAppointmentsPage();
            }
        });
    }

    calendarInstance = flatpickr(calendarContainer, {
        inline: true,
        defaultDate: new Date(),
        locale: 'es',
        onChange: function (selectedDates, dateStr) {
            loadAppointments(dateStr);
        }
    });

    window.loadAppointmentsGlobal = loadAppointments;
    window.getCalendarInstance = () => calendarInstance;

    const now = new Date();
    const todayStr = now.getFullYear() + '-' + String(now.getMonth() + 1).padStart(2, '0') + '-' + String(now.getDate()).padStart(2, '0');
    loadAppointments(todayStr);

});


// Gráfico de citas
document.addEventListener("DOMContentLoaded", function () {
    fetch("/Doctor/GetAppointmentsChartData")
        .then(response => {
            if (!response.ok) throw new Error("Error al obtener datos del gráfico");
            return response.json();
        })
        .then(data => {
            const ctx = document.getElementById("appointmentsChart").getContext("2d");

            new Chart(ctx, {
                type: 'line',
                data: {
                    labels: data.labels,
                    datasets: [{
                        label: "Citas por día",
                        data: data.values,
                        fill: false,
                        borderColor: 'rgba(59, 130, 246, 1)',
                        backgroundColor: 'rgba(59, 130, 246, 0.2)',
                        tension: 0.4
                    }]
                },
                options: {
                    responsive: true,
                    plugins: {
                        legend: {
                            display: false
                        }
                    },
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });
        })
        .catch(error => {
            console.error("Error al cargar gráfico:", error);
        });
});