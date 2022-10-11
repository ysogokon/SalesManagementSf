using Microsoft.AspNetCore.Components.Authorization;
using SalesManagementApp.Data;
using SalesManagementApp.Entities;
using SalesManagementApp.Extensions;
using SalesManagementApp.Models;
using SalesManagementApp.Services.Contracts;

namespace SalesManagementApp.Services;

public class AppointmentService : IAppointmentService
{
  private readonly SalesManagementDbContext salesManagementDbContext;
  private readonly AuthenticationStateProvider authenticationStateProvider;

  public AppointmentService ( SalesManagementDbContext salesManagementDbContext, AuthenticationStateProvider authenticationStateProvider )
  {
    this.salesManagementDbContext = salesManagementDbContext;
    this.authenticationStateProvider = authenticationStateProvider;
  }

  public async Task AddAppointment ( AppointmentModel appointmentModel )
  {
    try
    {
      var employee = await GetLoggedOnEmployee ();

      appointmentModel.EmployeeId = employee.Id;

      Appointment appointment = appointmentModel.Convert ();
      await salesManagementDbContext.AddAsync ( appointment );
      await salesManagementDbContext.SaveChangesAsync ();
    }
    catch ( Exception )
    {
      throw;
    }
  }

  public async Task DeleteAppointment ( int id )
  {
    try
    {
      Appointment? appointment = await salesManagementDbContext.Appointments.FindAsync ( id );

      if ( appointment != null )
      {
        salesManagementDbContext.Remove ( appointment );
        await salesManagementDbContext.SaveChangesAsync ();
      }
    }
    catch ( Exception )
    {

      throw;
    }
  }

  public async Task<List<AppointmentModel>> GetAppointments ()
  {
    try
    {
      var employee = await GetLoggedOnEmployee ();

      return await salesManagementDbContext.Appointments.Where ( e => e.EmployeeId == employee.Id ).Convert ();
    }
    catch ( Exception )
    {

      throw;
    }
  }

  public async Task UpdateAppointment ( AppointmentModel appointmentModel )
  {
    try
    {
      Appointment? appointment = await salesManagementDbContext.Appointments.FindAsync ( appointmentModel.Id );

      if ( appointment != null )
      {
        appointment.Description = appointmentModel.Description;
        appointment.IsAllDay = appointmentModel.IsAllDay;
        appointment.RecurrenceId = appointmentModel.RecurrenceId;
        appointment.RecurrenceRule = appointmentModel.RecurrenceRule;
        appointment.RecurrenceException = appointmentModel.RecurrenceException;
        appointment.StartTime = appointmentModel.StartTime;
        appointment.EndTime = appointmentModel.EndTime;
        appointment.Location = appointmentModel.Location;
        appointment.Subject = appointmentModel.Subject;

        await salesManagementDbContext.SaveChangesAsync ();
      }
    }
    catch ( Exception )
    {

      throw;
    }
  }

  private async Task<Employee> GetLoggedOnEmployee ()
  {
    var authState = await this.authenticationStateProvider.GetAuthenticationStateAsync ();
    var user = authState.User;

    return await user.GetEmployeeObject ( this.salesManagementDbContext );
  }
}
