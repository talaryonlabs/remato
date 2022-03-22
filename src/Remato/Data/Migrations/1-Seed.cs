using System;
using FluentMigrator;
using Talaryon;

namespace Remato.Data.Migrations
{
    [Migration(1, "Seed")]
    public class Seed : Migration
    {
        public override void Up()
        {
            var uid = TalaryonHelper.UUID();
            
            // Initial Admin User
            Insert.IntoTable(RematoConstants.DatabaseTableUser)
                .Row(new
                {
                    Id = uid,
                    AuthId = uid,
                    AuthAdapter = "default",
                    Username = "admin",
                    Name = "Administrator",
                    Password = "AQAAAAEAACcQAAAAEGTYFmFw+/mzx8Ef4yq2znUwkl5Y6Bs6ZV7NgINEG8GsomDerF2ZV0GfDIbmtBNhDw==", // _storagr
                });
            
        }

        public override void Down()
        {
            Delete.FromTable(RematoConstants.DatabaseTableUser)
                .Row(new
                {
                    Username = "admin"
                });
        }
    }
}