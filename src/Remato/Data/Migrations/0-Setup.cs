using System;
using FluentMigrator;

namespace Remato.Data.Migrations
{
    [Migration(0, "Setup")]
    public class Setup : Migration
    {
        public override void Up()
        {
            Create.Table(RematoConstants.DatabaseTableLog)
                .WithColumn("Id").AsInt32().Unique().PrimaryKey()
                .WithColumn("Date").AsDateTime2().NotNullable()
                .WithColumn("ContentId").AsString().NotNullable()
                .WithColumn("UserId").AsString().NotNullable()
                .WithColumn("Message").AsString().NotNullable()
                .WithColumn("Exception").AsString();

            Create.Table(RematoConstants.DatabaseTableUser)
                .WithColumn("Id").AsString().NotNullable().Unique().PrimaryKey()
                .WithColumn("AuthId").AsString().NotNullable()
                .WithColumn("AuthAdapter").AsString().NotNullable()
                .WithColumn("IsEnabled").AsBoolean().WithDefaultValue(true)
                .WithColumn("IsDeleted").AsBoolean().WithDefaultValue(false)
                .WithColumn("IsAdmin").AsBoolean().WithDefaultValue(false)
                .WithColumn("Username").AsString().NotNullable().Unique()
                .WithColumn("Password").AsString().NotNullable()
                .WithColumn("Mail").AsString().Nullable()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("CreatedAt").AsDateTime().WithDefaultValue(DateTime.Now)
                .WithColumn("ChangedAt").AsDateTime().WithDefaultValue(DateTime.Now);

            Create.Table(RematoConstants.DatabaseTableVehicle)
                .WithColumn("Id").AsString().NotNullable().Unique().PrimaryKey()
                .WithColumn("IsEnabled").AsBoolean().WithDefaultValue(true)
                .WithColumn("IsDeleted").AsBoolean().WithDefaultValue(false)
                .WithColumn("CreatedAt").AsDateTime().NotNullable()
                .WithColumn("ChangedAt").AsDateTime().NotNullable()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("State").AsString().NotNullable()
                .WithColumn("VIN").AsString().NotNullable()
                .WithColumn("License").AsString().NotNullable()
                .WithColumn("StartDate").AsDateTime().NotNullable()
                .WithColumn("EndDate").AsDateTime().NotNullable()
                .WithColumn("Note").AsString();

            Create.Table(RematoConstants.DatabaseTableJob)
                .WithColumn("Id").AsString().NotNullable().Unique().PrimaryKey()
                .WithColumn("IsDone").AsBoolean().WithDefaultValue(false)
                .WithColumn("IsDeleted").AsBoolean().WithDefaultValue(false)
                .WithColumn("CreatedAt").AsDateTime().NotNullable()
                .WithColumn("ChangedAt").AsDateTime().NotNullable()
                .WithColumn("Title").AsString().NotNullable()
                .WithColumn("Description").AsString().NotNullable()
                .WithColumn("Type").AsString().NotNullable()
                .WithColumn("Date").AsDateTime().NotNullable();

            Create.ForeignKey($"FK_{RematoConstants.DatabaseTableLog}_{RematoConstants.DatabaseTableUser}")
                .FromTable(RematoConstants.DatabaseTableLog).ForeignColumn("UserId")
                .ToTable(RematoConstants.DatabaseTableUser).PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey($"FK_{RematoConstants.DatabaseTableVehicle}_{RematoConstants.DatabaseTableUser}");

            Delete.Table(RematoConstants.DatabaseTableLog);
            Delete.Table(RematoConstants.DatabaseTableUser);
            Delete.Table(RematoConstants.DatabaseTableVehicle);
            Delete.Table(RematoConstants.DatabaseTableJob);

            // TODO
            // Delete.Table(RematoConstants.DatabaseTableDevice);
            // Delete.Table(RematoConstants.DatabaseTableComment);
            // Delete.Table(RematoConstants.DatabaseTableIssue);
            // Delete.Table(RematoConstants.DatabaseTableTrainee);
        }
    }
}