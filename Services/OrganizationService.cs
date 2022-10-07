using SalesManagementApp.Data;
using SalesManagementApp.Extensions;
using SalesManagementApp.Models;
using SalesManagementApp.Services.Contracts;

namespace SalesManagementApp.Services;

public class OrganizationService : IOrganizationService
{
  private readonly SalesManagementDbContext salesManagementDbContext;

  public OrganizationService ( SalesManagementDbContext salesManagementDbContext )
  {
    this.salesManagementDbContext = salesManagementDbContext;
  }
  public async Task<List<OrganizationModel>> GetHierarchy ()
  {
    try
    {
      return await salesManagementDbContext.Employees.ConvertToHierarchy ( salesManagementDbContext );
    }
    catch ( Exception )
    {

      throw;
    }
  }
}
