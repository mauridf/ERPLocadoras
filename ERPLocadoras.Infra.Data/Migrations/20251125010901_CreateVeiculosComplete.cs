using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPLocadoras.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateVeiculosComplete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locacoes_Veiculos_VeiculoId",
                table: "Locacoes");

            migrationBuilder.AddColumn<int>(
                name: "AnoFabricacao",
                table: "Veiculos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AnoModelo",
                table: "Veiculos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ApoliceSeguro",
                table: "Veiculos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Capacidade",
                table: "Veiculos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Categoria",
                table: "Veiculos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Chassi",
                table: "Veiculos",
                type: "nvarchar(17)",
                maxLength: 17,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Combustivel",
                table: "Veiculos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Cor",
                table: "Veiculos",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataAquisicao",
                table: "Veiculos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DataProximaRevisao",
                table: "Veiculos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataUltimaRevisao",
                table: "Veiculos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Documentacao",
                table: "Veiculos",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FotosAnexos",
                table: "Veiculos",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Marca",
                table: "Veiculos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Modelo",
                table: "Veiculos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Observacoes",
                table: "Veiculos",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Placa",
                table: "Veiculos",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "QuilometragemAtual",
                table: "Veiculos",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Renavam",
                table: "Veiculos",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Seguradora",
                table: "Veiculos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Veiculos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Tipo",
                table: "Veiculos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorCompra",
                table: "Veiculos",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorMercadoAtual",
                table: "Veiculos",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VencimentoSeguro",
                table: "Veiculos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VersaoMotorizacao",
                table: "Veiculos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_Chassi",
                table: "Veiculos",
                column: "Chassi",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_Marca_Modelo",
                table: "Veiculos",
                columns: new[] { "Marca", "Modelo" });

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_Placa",
                table: "Veiculos",
                column: "Placa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_Renavam",
                table: "Veiculos",
                column: "Renavam",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_Status",
                table: "Veiculos",
                column: "Status");

            migrationBuilder.AddForeignKey(
                name: "FK_Locacoes_Veiculos_VeiculoId",
                table: "Locacoes",
                column: "VeiculoId",
                principalTable: "Veiculos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locacoes_Veiculos_VeiculoId",
                table: "Locacoes");

            migrationBuilder.DropIndex(
                name: "IX_Veiculos_Chassi",
                table: "Veiculos");

            migrationBuilder.DropIndex(
                name: "IX_Veiculos_Marca_Modelo",
                table: "Veiculos");

            migrationBuilder.DropIndex(
                name: "IX_Veiculos_Placa",
                table: "Veiculos");

            migrationBuilder.DropIndex(
                name: "IX_Veiculos_Renavam",
                table: "Veiculos");

            migrationBuilder.DropIndex(
                name: "IX_Veiculos_Status",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "AnoFabricacao",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "AnoModelo",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "ApoliceSeguro",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "Capacidade",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "Categoria",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "Chassi",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "Combustivel",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "Cor",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "DataAquisicao",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "DataProximaRevisao",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "DataUltimaRevisao",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "Documentacao",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "FotosAnexos",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "Marca",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "Modelo",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "Observacoes",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "Placa",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "QuilometragemAtual",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "Renavam",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "Seguradora",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "ValorCompra",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "ValorMercadoAtual",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "VencimentoSeguro",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "VersaoMotorizacao",
                table: "Veiculos");

            migrationBuilder.AddForeignKey(
                name: "FK_Locacoes_Veiculos_VeiculoId",
                table: "Locacoes",
                column: "VeiculoId",
                principalTable: "Veiculos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
