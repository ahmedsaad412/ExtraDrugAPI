using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExtraDrug.Migrations
{
    /// <inheritdoc />
    public partial class Add_ADmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var id = Guid.NewGuid().ToString();
            migrationBuilder.InsertData(
                   table: "AspNetUsers",
                   columns: new[] { "Id", "FirstName", "LastName" , "UserName" , "NormalizedUserName",
                   "Email" , "NormalizedEmail" , "PasswordHash","SecurityStamp" , "ConcurrencyStamp",
                   "AccessFailedCount" ,"EmailConfirmed" , "PhoneNumberConfirmed" ,"TwoFactorEnabled" , "LockoutEnabled"
                   },
                   values: new object[]
                   {
                        id, "Admin" , "Admin" , "Admin" , "Admin".ToUpper(),
                       "admin@extradrug.com" ,"admin@extradrug.com".ToUpper() , "AQAAAAIAAYagAAAAEE7rcEQm6F25+Ujp/W+jX0yM1PHS1qTiaCGhpC9QVliQayygLTrPiiyBB7Sud5Fabg==" , "2VBQZVILEMHPKZ56HTDU36ZUCU57F5BN" , Guid.NewGuid().ToString(),
                      0 , false , false , false , false
                   }
               );
            migrationBuilder.Sql(
                    $"INSERT INTO [AspNetUserRoles] ([UserId],[RoleId]) VALUES ('{id}' , (SELECT [Id] FROM [AspNetRoles]  WHERE [NormalizedName] = 'ADMIN')); "
                );    
                
        }
        // admin@extradrug.com , Admin@123
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                    $"Delete from [AspNetUserRoles] Where [UserId] = (Select [Id] from [AspNetUsers] where [NormalizedUserName] = 'Admin')"
                );

            migrationBuilder.Sql(
                   $"Delete from  AspNetUsers Where [NormalizedUserName] = 'Admin'"
               );
        }
    }
}
