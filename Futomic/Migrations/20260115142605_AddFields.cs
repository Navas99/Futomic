using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Futomic.Migrations
{
    /// <inheritdoc />
    public partial class AddFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Fields",
                columns: new[] { "FieldId", "EmailContact", "Latitude", "Location", "Longitude", "Name", "PlusCode" },
                values: new object[,]
                {
                    { 4, "contacto@laelipa.com", 0.0, "C. Sierra de Alcaraz, 23, Madrid", 0.0, "Campo Deportivo La Elipa", "G8V7+R4 Madrid" },
                    { 5, "contacto@puertadehierro.com", 0.0, "Av. de la Moncloa, 28040 Madrid", 0.0, "Campo Municipal Puerta de Hierro", "G9F7+2F Madrid" },
                    { 6, "contacto@orcasitas.com", 0.0, "C. Albufera, 35, Madrid", 0.0, "Complejo Deportivo Orcasitas", "G8V8+H9 Madrid" },
                    { 7, "contacto@vallehermoso.com", 0.0, "C. Fernández de los Ríos, 42, Madrid", 0.0, "Campo Municipal Vallehermoso", "G9F9+JG Madrid" },
                    { 8, "contacto@villaverde.com", 0.0, "Av. de los Poblados, 15, Madrid", 0.0, "Polideportivo Villaverde", "G8V6+XH Madrid" },
                    { 9, "contacto@sanblas.com", 0.0, "C. Alcalá, 523, Madrid", 0.0, "Campo Municipal San Blas", "G9F8+4J Madrid" },
                    { 10, "contacto@lavaguada.com", 0.0, "Av. Monforte de Lemos, 31, Madrid", 0.0, "Complejo Deportivo La Vaguada", "G9F7+9V Madrid" },
                    { 11, "contacto@carabanchel.com", 0.0, "C. Eugenia de Montijo, 53, Madrid", 0.0, "Campo Municipal Carabanchel", "G8V7+QX Madrid" },
                    { 12, "contacto@ciudadlineal.com", 0.0, "C. Arturo Soria, 244, Madrid", 0.0, "Campo Deportivo Ciudad Lineal", "G9F8+PM Madrid" },
                    { 13, "contacto@lastablas.com", 0.0, "C. Palas del Rey, 11, Madrid", 0.0, "Campo Municipal Las Tablas", "G9F9+W2 Madrid" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Fields",
                keyColumn: "FieldId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Fields",
                keyColumn: "FieldId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Fields",
                keyColumn: "FieldId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Fields",
                keyColumn: "FieldId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Fields",
                keyColumn: "FieldId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Fields",
                keyColumn: "FieldId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Fields",
                keyColumn: "FieldId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Fields",
                keyColumn: "FieldId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Fields",
                keyColumn: "FieldId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Fields",
                keyColumn: "FieldId",
                keyValue: 13);
        }
    }
}
