using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPLocadoras.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateLocacoesComplete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locacoes_Clientes_ClienteId",
                table: "Locacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Locacoes_Locadoras_LocadoraId",
                table: "Locacoes");

            migrationBuilder.AddColumn<string>(
                name: "Anexos",
                table: "Locacoes",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChecklistDevolucao",
                table: "Locacoes",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChecklistEntrega",
                table: "Locacoes",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataInicio",
                table: "Locacoes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DataPrevistaDevolucao",
                table: "Locacoes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DataRealDevolucao",
                table: "Locacoes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DescontosAcrescimos",
                table: "Locacoes",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FormaCaucao",
                table: "Locacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FormaCobranca",
                table: "Locacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "FranquiaKmInclusa",
                table: "Locacoes",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "KmDevolucao",
                table: "Locacoes",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "KmEntrega",
                table: "Locacoes",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "NivelCombustivelDevolucao",
                table: "Locacoes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NivelCombustivelEntrega",
                table: "Locacoes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObservacoesInternas",
                table: "Locacoes",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlanoLocacao",
                table: "Locacoes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponsavelDevolucao",
                table: "Locacoes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponsavelEntrega",
                table: "Locacoes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Situacao",
                table: "Locacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TipoLocacao",
                table: "Locacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorCaucao",
                table: "Locacoes",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorDiaria",
                table: "Locacoes",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorKmAdicional",
                table: "Locacoes",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorTotalFinal",
                table: "Locacoes",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorTotalPrevisto",
                table: "Locacoes",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Locacoes_DataInicio",
                table: "Locacoes",
                column: "DataInicio");

            migrationBuilder.CreateIndex(
                name: "IX_Locacoes_DataPrevistaDevolucao",
                table: "Locacoes",
                column: "DataPrevistaDevolucao");

            migrationBuilder.CreateIndex(
                name: "IX_Locacoes_Situacao",
                table: "Locacoes",
                column: "Situacao");

            migrationBuilder.AddForeignKey(
                name: "FK_Locacoes_Clientes_ClienteId",
                table: "Locacoes",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Locacoes_Locadoras_LocadoraId",
                table: "Locacoes",
                column: "LocadoraId",
                principalTable: "Locadoras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locacoes_Clientes_ClienteId",
                table: "Locacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Locacoes_Locadoras_LocadoraId",
                table: "Locacoes");

            migrationBuilder.DropIndex(
                name: "IX_Locacoes_DataInicio",
                table: "Locacoes");

            migrationBuilder.DropIndex(
                name: "IX_Locacoes_DataPrevistaDevolucao",
                table: "Locacoes");

            migrationBuilder.DropIndex(
                name: "IX_Locacoes_Situacao",
                table: "Locacoes");

            migrationBuilder.DropColumn(
                name: "Anexos",
                table: "Locacoes");

            migrationBuilder.DropColumn(
                name: "ChecklistDevolucao",
                table: "Locacoes");

            migrationBuilder.DropColumn(
                name: "ChecklistEntrega",
                table: "Locacoes");

            migrationBuilder.DropColumn(
                name: "DataInicio",
                table: "Locacoes");

            migrationBuilder.DropColumn(
                name: "DataPrevistaDevolucao",
                table: "Locacoes");

            migrationBuilder.DropColumn(
                name: "DataRealDevolucao",
                table: "Locacoes");

            migrationBuilder.DropColumn(
                name: "DescontosAcrescimos",
                table: "Locacoes");

            migrationBuilder.DropColumn(
                name: "FormaCaucao",
                table: "Locacoes");

            migrationBuilder.DropColumn(
                name: "FormaCobranca",
                table: "Locacoes");

            migrationBuilder.DropColumn(
                name: "FranquiaKmInclusa",
                table: "Locacoes");

            migrationBuilder.DropColumn(
                name: "KmDevolucao",
                table: "Locacoes");

            migrationBuilder.DropColumn(
                name: "KmEntrega",
                table: "Locacoes");

            migrationBuilder.DropColumn(
                name: "NivelCombustivelDevolucao",
                table: "Locacoes");

            migrationBuilder.DropColumn(
                name: "NivelCombustivelEntrega",
                table: "Locacoes");

            migrationBuilder.DropColumn(
                name: "ObservacoesInternas",
                table: "Locacoes");

            migrationBuilder.DropColumn(
                name: "PlanoLocacao",
                table: "Locacoes");

            migrationBuilder.DropColumn(
                name: "ResponsavelDevolucao",
                table: "Locacoes");

            migrationBuilder.DropColumn(
                name: "ResponsavelEntrega",
                table: "Locacoes");

            migrationBuilder.DropColumn(
                name: "Situacao",
                table: "Locacoes");

            migrationBuilder.DropColumn(
                name: "TipoLocacao",
                table: "Locacoes");

            migrationBuilder.DropColumn(
                name: "ValorCaucao",
                table: "Locacoes");

            migrationBuilder.DropColumn(
                name: "ValorDiaria",
                table: "Locacoes");

            migrationBuilder.DropColumn(
                name: "ValorKmAdicional",
                table: "Locacoes");

            migrationBuilder.DropColumn(
                name: "ValorTotalFinal",
                table: "Locacoes");

            migrationBuilder.DropColumn(
                name: "ValorTotalPrevisto",
                table: "Locacoes");

            migrationBuilder.AddForeignKey(
                name: "FK_Locacoes_Clientes_ClienteId",
                table: "Locacoes",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Locacoes_Locadoras_LocadoraId",
                table: "Locacoes",
                column: "LocadoraId",
                principalTable: "Locadoras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
