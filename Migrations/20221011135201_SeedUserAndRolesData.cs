using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesManagementApp.Migrations
{
  public partial class SeedUserAndRolesData : Migration
  {
    private const string ADMIN_ROLE_GUID = "9f20bf1f-7e7b-4c67-b79e-3c3757b8aea0";
    private const string SM_ROLE_GUID = "2972a2da-86a8-4b11-bcf1-2ae4eaaad89b";
    private const string TL_ROLE_GUID = "89ce98a4-ffd6-4f03-919a-dd792b8c75da";
    private const string SR_ROLE_GUID = "a3e9be5b-c6e9-459a-b84f-344c1073df84";

    private const string ADMIN_USER_GUID = "4f3c569b-985c-4003-be7b-268a158996c0";
    private const string SM_USER_GUID = "0a5fd9ad-2499-4fe6-abef-347ce1fbe93c";
    private const string TL_USER_GUID = "d79ea687-a263-44cd-9119-840f57701311";
    private const string SR1_USER_GUID = "64c2d461-9bf7-4cca-80da-86b094901c12";
    private const string SR2_USER_GUID = "faddec64-64ca-4b6a-a4d1-0f627f34b598";
    private const string SR3_USER_GUID = "d1df958a-8669-4be0-9432-823f1b56d815";
    private const string SR4_USER_GUID = "20547d44-b0ba-431d-a012-db2b33216eee";

    protected override void Up ( MigrationBuilder migrationBuilder )
    {
      var hasher = new PasswordHasher<IdentityUser> ();

      var passwordHash = hasher.HashPassword ( null, "Password1!" );

      AddUser ( migrationBuilder, "admin@oexl.com", passwordHash, ADMIN_USER_GUID );

      AddUser ( migrationBuilder, "bob.jones@oexl.com", passwordHash, SM_USER_GUID );

      AddUser ( migrationBuilder, "henry.andrews@oexl.com", passwordHash, TL_USER_GUID );

      AddUser ( migrationBuilder, "olivia.mills@oexl.com", passwordHash, SR1_USER_GUID );
      AddUser ( migrationBuilder, "noah.robinson@oexl.com", passwordHash, SR2_USER_GUID );
      AddUser ( migrationBuilder, "benjamin.lucas@oexl.com", passwordHash, SR3_USER_GUID );
      AddUser ( migrationBuilder, "sarah.henderson@oexl.com", passwordHash, SR4_USER_GUID );

      AddRole ( migrationBuilder, "Admin", ADMIN_ROLE_GUID );
      AddRole ( migrationBuilder, "SM", SM_ROLE_GUID );
      AddRole ( migrationBuilder, "TL", TL_ROLE_GUID );
      AddRole ( migrationBuilder, "SR", SR_ROLE_GUID );

      AddUserToRole ( migrationBuilder, ADMIN_USER_GUID, ADMIN_ROLE_GUID );
      AddUserToRole ( migrationBuilder, SM_USER_GUID, SM_ROLE_GUID );

      AddUserToRole ( migrationBuilder, TL_USER_GUID, TL_ROLE_GUID );
      AddUserToRole ( migrationBuilder, SR1_USER_GUID, SR_ROLE_GUID );
      AddUserToRole ( migrationBuilder, SR2_USER_GUID, SR_ROLE_GUID );
      AddUserToRole ( migrationBuilder, SR3_USER_GUID, SR_ROLE_GUID );
      AddUserToRole ( migrationBuilder, SR4_USER_GUID, SR_ROLE_GUID );
    }

    private void AddUser ( MigrationBuilder migrationBuilder, string email, string passwordHash, string userGuid )
    {
      StringBuilder sb = new StringBuilder ();

      string emailCaps = email.ToUpper ();

      sb.AppendLine ( "INSERT INTO AspNetUsers(Id, UserName, NormalizedUserName,Email,EmailConfirmed,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnabled,AccessFailedCount,NormalizedEmail,PasswordHash,SecurityStamp)" );
      sb.AppendLine ( "VALUES(" );
      sb.AppendLine ( $"'{userGuid}'" );
      sb.AppendLine ( $",'{email}'" );
      sb.AppendLine ( $",'{emailCaps}'" );
      sb.AppendLine ( $",'{email}'" );
      sb.AppendLine ( ", 0" );
      sb.AppendLine ( ", 0" );
      sb.AppendLine ( ", 0" );
      sb.AppendLine ( ", 0" );
      sb.AppendLine ( ", 0" );
      sb.AppendLine ( $",'{emailCaps}'" );
      sb.AppendLine ( $", '{passwordHash}'" );
      sb.AppendLine ( ", ''" );
      sb.AppendLine ( ")" );

      migrationBuilder.Sql ( sb.ToString () );
    }
    private void AddRole ( MigrationBuilder migrationBuilder, string roleName, string roleGuid )
    {
      string roleNameCaps = roleName.ToUpper ();

      migrationBuilder.Sql ( $"INSERT INTO AspNetRoles (Id, Name, NormalizedName) VALUES ('{roleGuid}','{roleName}','{roleNameCaps}')" );


    }

    private void AddUserToRole ( MigrationBuilder migrationBuilder, string userGuid, string roleGuid )
    {
      migrationBuilder.Sql ( $"INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES ('{userGuid}','{roleGuid}')" );


    }
    protected override void Down ( MigrationBuilder migrationBuilder )
    {
      RemoveUser ( migrationBuilder, ADMIN_USER_GUID, ADMIN_ROLE_GUID );
      RemoveUser ( migrationBuilder, SM_USER_GUID, SM_ROLE_GUID );
      RemoveUser ( migrationBuilder, TL_USER_GUID, TL_ROLE_GUID );
      RemoveUser ( migrationBuilder, SR1_USER_GUID, SR_ROLE_GUID );
      RemoveUser ( migrationBuilder, SR2_USER_GUID, SR_ROLE_GUID );
      RemoveUser ( migrationBuilder, SR3_USER_GUID, SR_ROLE_GUID );
      RemoveUser ( migrationBuilder, SR4_USER_GUID, SR_ROLE_GUID );

      RemoveRole ( migrationBuilder, ADMIN_ROLE_GUID );
      RemoveRole ( migrationBuilder, SM_ROLE_GUID );
      RemoveRole ( migrationBuilder, TL_ROLE_GUID );
      RemoveRole ( migrationBuilder, SR_ROLE_GUID );
    }

    private void RemoveUser ( MigrationBuilder migrationBuilder, string userGuid, string roleGuid )
    {
      migrationBuilder.Sql ( $"DELETE FROM AspNetUserRoles WHERE UserId = '{userGuid}' AND RoleId = '{roleGuid}'" );

      migrationBuilder.Sql ( $"DELETE FROM AspNetUsers WHERE Id = '{userGuid}'" );
    }
    private void RemoveRole ( MigrationBuilder migrationBuilder, string roleGuid )
    {
      migrationBuilder.Sql ( $"DELETE FROM AspNetRoles WHERE Id = '{roleGuid}'" );
    }
  }
}
