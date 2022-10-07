using SalesManagementApp.Models;

namespace SalesManagementApp.Services.Contracts;

public interface IOrganizationService
{
  Task<List<OrganizationModel>> GetHierarchy ();
}
