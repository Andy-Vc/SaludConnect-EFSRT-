document.addEventListener('DOMContentLoaded', function () {
    const calendarContainer = document.getElementById('myCalendarContainer');
    const appointmentContainer = document.getElementById('appointmentsContainer');
    const selectedDateParagraph = document.querySelector('.bg-light.rounded.m-3.p-3.border-5 p:nth-child(2)');
    const badgeCount = document.querySelector('.badge.bg-light.text-dark');
    const titleSpan = document.querySelector('span.text-black.ms-2');
    const dateLabel = document.querySelector('.bg-light.rounded.m-3.p-3.border-5 p:nth-child(1)');
    const appointmentsCountLabel = document.querySelector('#countLeft');

    // Variables globales para el modal
    let selectedServices = [];
    let currentAppointmentId = null;
    let specialtiesData = [];
    let servicesData = [];

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
            const reason = item.specialty?.nameSpecialty || 'Motivo no especificado';
            const hasMedicalRecordFinal = !!item.medicalRecord?.idRecord;
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
            console.log(`Cita ID: ${item.idAppointment} - ¿Tiene Registro Médico?: ${hasMedicalRecordFinal}`);
            let buttonsHtml = '';

            if (hasMedicalRecordFinal) {
                buttonsHtml = `
                <span class="badge bg-success-subtle text-success py-2 px-3 d-flex align-items-center gap-1">
                    <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" fill="currentColor" class="bi bi-journal-check" viewBox="0 0 16 16">
                     <path fill-rule="evenodd" d="M10.854 6.146a.5.5 0 0 1 0 .708l-3 3a.5.5 0 0 1-.708 0l-1.5-1.5a.5.5 0 1 1 .708-.708L7.5 8.793l2.646-2.647a.5.5 0 0 1 .708 0"/>
                    <path d="M4 1.5a.5.5 0 0 1 .5-.5h7a.5.5 0 0 1 .5.5v13a.5.5 0 0 1-.5.5h-7a.5.5 0 0 1-.5-.5zM4 1h7a1 1 0 0 1 1 1v12a1 1 0 0 1-1 1h-7a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1"/>
                </svg>
                Registro Médico Completo
            </span> `;
            }
            else if (item.state === 'P' && appointmentDate.getTime() === today.getTime()) {
                    buttonsHtml = `
                <div class="d-flex gap-2">
                    <form method="POST" action="/Doctor/ChangeAppointmentState" class="d-inline" onsubmit="return handleAttendanceSubmitGlobal(event, ${item.idAppointment})">
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
                    <button type="button" class="btn btn-outline-info btn-sm d-flex align-items-center gap-1"
            onclick="openAppointmentDetailsModal(${item.idAppointment})">
        <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" fill="currentColor" class="bi bi-info-circle" viewBox="0 0 16 16">
            <path d="M8 15A7 7 0 1 0 8 1a7 7 0 0 0 0 14zm0 1A8 8 0 1 1 8 0a8 8 0 0 1 0 16z"/>
            <path d="m8.93 6.588-2.29.287-.082.38.45.083c.294.07.352.176.288.469l-.738 3.468c-.194.897.105 1.319.808 1.319.545 0 .875-.252 1.05-.598l.088-.416c.076-.36.223-.476.508-.543l.45-.083.082-.38-2.29-.287zm-.93-2.588a1 1 0 1 1 2 0 1 1 0 0 1-2 0z"/>
        </svg>
        Ver Detalles
    </button>
                </div>
            `;
                } else if (item.state === 'A' && appointmentDate.getTime() === today.getTime()) {
                    buttonsHtml = `
                <div class="d-flex gap-2">
                    <button type="button" class="btn btn-primary btn-sm d-flex align-items-center gap-1" 
                            onclick="openMedicalRecordModalGlobal(${item.idAppointment}, '${patientFirstName} ${patientLastName}')">
                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" stroke="currentColor"
                        stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="lucide lucide-file-text w-4 h-4 mr-1">
                        <path d="M14.5 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V7.5L14.5 2z"></path>
                        <polyline points="14 2 14 8 20 8"></polyline>
                        <line x1="16" y1="13" x2="8" y2="13"></line>
                        <line x1="16" y1="17" x2="8" y2="17"></line>
                        <line x1="10" y1="9" x2="8" y2="9"></line>
                        </svg> Registrar Historia
                    </button>
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

    // Manejar el submit del formulario de asistencia
    async function handleAttendanceSubmit(event, appointmentId) {
        event.preventDefault();
        const form = event.target;

        try {
            const response = await fetch(form.action, {
                method: 'POST',
                body: new FormData(form)
            });

            if (response.ok) {
                // Actualizar el estado local
                const appointment = appointmentsData.find(a => a.idAppointment === appointmentId);
                if (appointment) {
                    appointment.state = 'A';
                }
                // Re-renderizar la página
                renderAppointmentsPage();
            }
        } catch (error) {
            console.error('Error al actualizar estado:', error);
        }
        return false; // Prevenir submit del formulario
    }

    // Abrir modal de historia médica
    async function openMedicalRecordModal(appointmentId, patientName) {
        currentAppointmentId = appointmentId;
        selectedServices = [];

        // Cargar especialidades
        await loadSpecialties();

        // Configurar el modal
        document.getElementById('modalPatientName').textContent = patientName;
        document.getElementById('servicesTableBody').innerHTML = '<tr><td colspan="3" class="text-center text-muted">No hay servicios agregados</td></tr>';
        document.getElementById('specialtySelect').value = '';
        document.getElementById('serviceSelect').innerHTML = '<option value="">Seleccione una especialidad primero</option>';
        document.getElementById('observations').value = '';
        document.getElementById('diagnosis').value = '';
        document.getElementById('treatment').value = '';

        // Mostrar el modal
        const modal = new bootstrap.Modal(document.getElementById('medicalRecordModal'));
        modal.show();
    }

    // Cargar especialidades
    async function loadSpecialties() {
        try {
            const response = await fetch('/Doctor/GetSpecialties');
            if (response.ok) {
                specialtiesData = await response.json();
                const select = document.getElementById('specialtySelect');
                select.innerHTML = '<option value="">Seleccione una especialidad</option>';
                specialtiesData.forEach(specialty => {
                    select.innerHTML += `<option value="${specialty.idSpecialty}">${specialty.nameSpecialty}</option>`;
                });
            }
        } catch (error) {
            console.error('Error al cargar especialidades:', error);
        }
    }

    // Cargar servicios por especialidad
    async function loadServicesBySpecialty() {
        const specialtyId = document.getElementById('specialtySelect').value;
        const serviceSelect = document.getElementById('serviceSelect');

        if (!specialtyId) {
            serviceSelect.innerHTML = '<option value="">Seleccione una especialidad primero</option>';
            return;
        }

        try {
            const response = await fetch(`/Doctor/GetServicesBySpecialty?idSpecialty=${specialtyId}`);
            if (response.ok) {
                servicesData = await response.json();
                serviceSelect.innerHTML = '<option value="">Seleccione un servicio</option>';
                servicesData.forEach(service => {
                    serviceSelect.innerHTML += `<option value="${service.idService}">${service.nameService}</option>`;
                });
            }
        } catch (error) {
            console.error('Error al cargar servicios:', error);
            serviceSelect.innerHTML = '<option value="">Error al cargar servicios</option>';
        }
    }
    function buildAppointmentModalHtml(appointment) {
        const patient = appointment.patient;
        const doctor = appointment.doctor;
        const record = appointment.medicalRecord;
        const office = appointment.office;
        const specialty = appointment.specialty;

        let html = `
    <div style="background-color: #f8fafc; padding: 1.5rem; border-radius: 0.75rem;">
        <!-- Información del Paciente -->
        <div class="alert border-0 shadow-sm mb-4" style="background: linear-gradient(135deg, #dbeafe 0%, #e0f2fe 100%); border-left: 4px solid #3b82f6 !important; border-radius: 0.75rem;">
            <div class="d-flex align-items-center mb-3">
                <div class="bg-primary bg-opacity-10 rounded-circle p-2 me-3">
                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" stroke="#3b82f6" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                        <path d="M19 21v-2a4 4 0 0 0-4-4H9a4 4 0 0 0-4 4v2"></path>
                        <circle cx="12" cy="7" r="4"></circle>
                    </svg>
                </div>
                <div>
                    <small class="text-muted fw-semibold d-block mb-1">PACIENTE</small>
                    <strong class="text-dark" style="font-size: 1.1rem;">${patient.firstName} ${patient.lastNamePat} ${patient.lastNameMat}</strong>
                </div>
            </div>
            <div class="row g-3">
                <div class="col-md-6">
                    <small class="text-muted fw-semibold">Documento:</small>
                    <p class="mb-0 fw-semibold text-dark">${patient.document}</p>
                </div>
                <div class="col-md-6">
                    <small class="text-muted fw-semibold">Teléfono:</small>
                    <p class="mb-0 fw-semibold text-dark">${patient.phone}</p>
                </div>
                <div class="col-md-6">
                    <small class="text-muted fw-semibold">Fecha de Nacimiento:</small>
                    <p class="mb-0 fw-semibold text-dark">${formatDate(patient.birthDate)}</p>
                </div>
                <div class="col-md-6">
                    <small class="text-muted fw-semibold">Correo:</small>
                    <p class="mb-0 fw-semibold text-dark">${patient.email}</p>
                </div>
            </div>
        </div>

        <!-- Información de la Cita -->
        <div class="card border-0 shadow-sm mb-4" style="border-radius: 1rem; overflow: hidden;">
            <div class="card-header text-white border-0" style="background: linear-gradient(135deg, #3b82f6 0%, #2563eb 100%); padding: 1rem 1.5rem;">
                <h6 class="mb-0 fw-bold d-flex align-items-center">
                    <div class="bg-white bg-opacity-25 rounded-circle p-1 me-2" style="width: 28px; height: 28px; display: flex; align-items: center; justify-content: center;">
                        <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
                            <path d="M8 2v4" />
                            <path d="M16 2v4" />
                            <rect width="18" height="18" x="3" y="4" rx="2" />
                            <path d="M3 10h18" />
                        </svg>
                    </div>
                    Información de la Cita
                </h6>
            </div>
            <div class="card-body" style="padding: 1.5rem; background-color: white;">
                <div class="row g-3">
                    <div class="col-md-6">
                        <small class="text-muted fw-semibold">Fecha de Cita:</small>
                        <p class="mb-0 fw-semibold text-dark">${formatDateTime(appointment.dateAppointment)}</p>
                    </div>
                    <div class="col-md-6">
                        <small class="text-muted fw-semibold">Especialidad:</small>
                        <p class="mb-0 fw-semibold text-dark">${specialty.nameSpecialty}</p>
                    </div>
                    <div class="col-md-6">
                        <small class="text-muted fw-semibold">Consultorio:</small>
                        <p class="mb-0 fw-semibold text-dark">${office.nroOffice} - Piso ${office.floorNumber}</p>
                    </div>
                    <div class="col-md-6">
                        <small class="text-muted fw-semibold">Médico:</small>
                        <p class="mb-0 fw-semibold text-dark">Dr. ${doctor.firstName} ${doctor.lastNamePat} ${doctor.lastNameMat}</p>
                    </div>
                    <div class="col-md-6">
                        <small class="text-muted fw-semibold">Precio:</small>
                        <p class="mb-0 fw-bold text-success" style="font-size: 1.1rem;">S/. ${appointment.appointmentPrice.toFixed(2)}</p>
                    </div>
                    <div class="col-md-6">
                        <small class="text-muted fw-semibold">Estado:</small>
                        <p class="mb-0"><span class="badge" style="background: linear-gradient(135deg, #3b82f6 0%, #2563eb 100%); padding: 0.4rem 0.8rem; font-size: 0.85rem;">${mapState(appointment.state)}</span></p>
                    </div>
                </div>
            </div>
        </div>
    `;


        return html;
    }

    function formatDate(dateStr) {
        const date = new Date(dateStr);
        return date.toLocaleDateString('es-PE');
    }

    function formatDateTime(dateStr) {
        const date = new Date(dateStr);
        return date.toLocaleString('es-PE', { hour: '2-digit', minute: '2-digit' });
    }

    function mapState(state) {
        switch (state) {
            case 'A': return 'Atendida';
            case 'P': return 'Pendiente';
            case 'N': return 'No Asistió';
            case 'X': return 'Cancelada';
            default: return 'Desconocido';
        }
    }

    window.openAppointmentDetailsModal = function (idAppointment) {
        console.log("Abriendo modal para cita ID:", idAppointment);

        fetch(`/Doctor/GetInformationAppointment?idAppointment=${idAppointment}`)
            .then(response => {
                if (!response.ok) throw new Error("No se encontró la cita.");
                console.log(response)
                return response.json();
            })
            .then(appointment => {
                const modalContent = buildAppointmentModalHtml(appointment);
                document.getElementById("modalContentDetail").innerHTML = modalContent;
                const modal = new bootstrap.Modal(document.getElementById("appointmentDetailsModal"));
                modal.show();
            })
            .catch(error => {
                console.error(error);
                alert("Error al obtener detalles de la cita.");
            });
    }

    function addServiceToTable() {
        const serviceSelect = document.getElementById('serviceSelect');
        const specialtySelect = document.getElementById('specialtySelect');
        const serviceId = parseInt(serviceSelect.value);
        const specialtyId = parseInt(specialtySelect.value);

        if (!serviceId) {
            Swal.fire({
                icon: 'warning',
                title: 'Servicio no seleccionado',
                text: 'Por favor, seleccione un servicio antes de continuar.',
                confirmButtonText: 'Entendido'
            });
            return;
        }

        if (selectedServices.some(s => s.idService === serviceId)) {
            Swal.fire({
                icon: 'info',
                title: 'Servicio duplicado',
                text: 'Este servicio ya ha sido agregado.',
                confirmButtonText: 'OK'
            });
            return;
        }


        const service = servicesData.find(s => s.idService === serviceId);
        const specialty = specialtiesData.find(sp => sp.idSpecialty === specialtyId);

        if (!service || !specialty) return;

        selectedServices.push({
            idService: service.idService,
            nameService: service.nameService,
            price: service.price,
            specialty: {
                idSpecialty: specialty.idSpecialty,
                nameSpecialty: specialty.nameSpecialty
            }
        });

        renderServicesTable();

        serviceSelect.value = '';
    }

    function renderServicesTable() {
        const tbody = document.getElementById('servicesTableBody');

        if (selectedServices.length === 0) {
            tbody.innerHTML = '<tr><td colspan="3" class="text-center text-muted">No hay servicios agregados</td></tr>';
            return;
        }

        tbody.innerHTML = '';

        selectedServices.forEach((service, index) => {
            const row = `
            <tr>
                <td>${index + 1}</td>
                <td>${service.nameService}</td>
                <td>${service.price}</td>
                <td class="text-center">
                    <button type="button" class="btn btn-danger btn-sm" onclick="removeServiceGlobal(${index})">
                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash" viewBox="0 0 16 16">
                            <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z"/>
                            <path fill-rule="evenodd" d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z"/>
                        </svg>
                    </button>
                </td>
            </tr>
        `;
            tbody.innerHTML += row;
        });
    }

    // Remover servicio de la tabla
    function removeService(index) {
        selectedServices.splice(index, 1);
        renderServicesTable();
    }

    // Guardar historia médica
    async function saveMedicalRecord() {
        const observations = document.getElementById('observations').value.trim();
        const diagnosis = document.getElementById('diagnosis').value.trim();
        const treatment = document.getElementById('treatment').value.trim();

        if (!observations) {
            Swal.fire({
                icon: 'warning',
                title: 'Observaciones requeridas',
                text: 'Por favor, ingresa las observaciones antes de continuar.',
                confirmButtonText: 'Entendido'
            });
            return;
        }
        const medicalRecord = {
            idRecord: 0,
            idAppointment: currentAppointmentId,
            dateReport: new Date().toISOString(),
            observations: observations,
            diagnosis: diagnosis || "",
            treatment: treatment || "",
            additionalServices: selectedServices.map(service => ({
                idAddService: 0,
                idRecord: 0,
                service: {
                    idService: service.idService,
                    nameService: service.nameService,
                    description: "",
                    price: service.price,
                    durationMinutes: 0,
                    specialty: {
                        idSpecialty: service.specialty.idSpecialty,
                        nameSpecialty: service.specialty.nameSpecialty,
                        flgDelete: false
                    },
                    flgDelete: false
                },
                priceAtTime: service.price,
                state: "P"
            }))
        };

        console.log('📤 Enviando datos:', medicalRecord);

        try {
            const response = await fetch('/Doctor/RegisterMedicalRecord', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(medicalRecord)
            });


            if (response.ok) {
                const result = await response.json();

                if (result.value) {
                    await Swal.fire({
                        icon: 'success',
                        title: '¡Registro exitoso!',
                        text: 'La historia médica fue registrada correctamente.',
                        confirmButtonText: 'Aceptar'
                    });

                    bootstrap.Modal.getInstance(document.getElementById('medicalRecordModal')).hide();
                    location.reload();
                } else {
                    await Swal.fire({
                        icon: 'error',
                        title: 'Error al registrar',
                        text: result.message || 'No se pudo registrar la historia médica.',
                        confirmButtonText: 'Aceptar'
                    });
                }
            } else {
                const errorText = await response.text();
                await Swal.fire({
                    icon: 'error',
                    title: `Error ${response.status}`,
                    text: errorText || 'Error desconocido del servidor.',
                    confirmButtonText: 'Aceptar'
                });
            }
        } catch (error) {
            await Swal.fire({
                icon: 'error',
                title: 'Error de red',
                text: error.message || 'No se pudo conectar al servidor.',
                confirmButtonText: 'Aceptar'
            });
        }

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

    // EXPONER FUNCIONES AL SCOPE GLOBAL
    window.handleAttendanceSubmitGlobal = handleAttendanceSubmit;
    window.openMedicalRecordModalGlobal = openMedicalRecordModal;
    window.loadServicesBySpecialtyGlobal = loadServicesBySpecialty;
    window.addServiceToTableGlobal = addServiceToTable;
    window.removeServiceGlobal = removeService;
    window.saveMedicalRecordGlobal = saveMedicalRecord;

    // Mantener compatibilidad con código existente
    window.loadAppointmentsGlobal = loadAppointments;
    window.getCalendarInstance = () => calendarInstance;

    // Inicializar calendario
    calendarInstance = flatpickr(calendarContainer, {
        inline: true,
        defaultDate: new Date(),
        locale: 'es',
        onChange: function (selectedDates, dateStr) {
            loadAppointments(dateStr);
        }
    });

    // Cargar citas del día actual
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