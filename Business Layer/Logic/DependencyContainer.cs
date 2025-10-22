using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interface;
using Data.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Logic
{
    public static class DependencyContainer
    {
        public static void AddDependencies(this IServiceCollection services)
        {
            services.AddScoped<IService, ServiceRepository>();
            services.AddScoped<IAuthorization, AuthorizationRepository>();
            services.AddScoped<IAppointment, AppointmentRepository>();
            services.AddScoped<IUser, UserRepository>();
            services.AddScoped<ISpecialty, SpecialtyRepository>();
            services.AddScoped<IPatient, PatientRepository>();
            services.AddScoped<IMedicalRecord, MedicalReportRepository>();
            services.AddScoped<IDoctor, DoctorRepository>();


            services.AddScoped<PatientBL>();
            services.AddScoped<ServiceBL>();
            services.AddScoped<AppointmentBL>();
            services.AddScoped<AuthorizationBL>();
            services.AddScoped<UserBL>();
            services.AddScoped<SpecialtyBL>();
            services.AddScoped<MedicalRecordBL>();
            services.AddScoped<DoctorBL>();
        }
    }
}
