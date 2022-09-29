﻿using SalesManagementApp.Entities;
using SalesManagementApp.Models;

namespace SalesManagementApp.Services.Contracts;

public interface IEmployeeManagementService
{
  Task<List<EmployeeModel>> GetEmployees ();
  Task<List<EmployeeJobTitle>> GetJobTitles ();
  Task<List<ReportToModel>> GetReportToEmployees ();
  Task<Employee> AddEmployee ( EmployeeModel employee );
  Task UpdateEmployee ( EmployeeModel employee );
  Task DeleteEmployee ( int id );
}
