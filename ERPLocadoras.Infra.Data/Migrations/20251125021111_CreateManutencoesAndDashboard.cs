using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPLocadoras.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateManutencoesAndDashboard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Manutencoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DataEntrada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataSaidaPrevista = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataSaidaReal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KmEntrada = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    KmSaida = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    OficinaPrestador = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CnpjContatoOficina = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ResponsavelManutencaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustoPecas = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    CustoMaoDeObra = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    CustoTotal = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    GarantiaServico = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DataProximaRevisaoSugerida = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Observacoes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Anexos = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    VeiculoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocadoraId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manutencoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Manutencoes_Locadoras_LocadoraId",
                        column: x => x.LocadoraId,
                        principalTable: "Locadoras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Manutencoes_Usuarios_ResponsavelManutencaoId",
                        column: x => x.ResponsavelManutencaoId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Manutencoes_Veiculos_VeiculoId",
                        column: x => x.VeiculoId,
                        principalTable: "Veiculos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Manutencoes_DataEntrada",
                table: "Manutencoes",
                column: "DataEntrada");

            migrationBuilder.CreateIndex(
                name: "IX_Manutencoes_DataSaidaPrevista",
                table: "Manutencoes",
                column: "DataSaidaPrevista");

            migrationBuilder.CreateIndex(
                name: "IX_Manutencoes_LocadoraId",
                table: "Manutencoes",
                column: "LocadoraId");

            migrationBuilder.CreateIndex(
                name: "IX_Manutencoes_ResponsavelManutencaoId",
                table: "Manutencoes",
                column: "ResponsavelManutencaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Manutencoes_Status",
                table: "Manutencoes",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Manutencoes_VeiculoId",
                table: "Manutencoes",
                column: "VeiculoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Manutencoes");
        }
    }
}
