using Futomic.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Futomic.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // TABLAS
        public DbSet<Team> Teams { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Ranking> Rankings { get; set; }
        public DbSet<Match> Matches { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            
            // RELACIÓN USER → TEAM (1:N)
            builder.Entity<User>()
                .HasOne(u => u.Team)
                .WithMany(t => t.Users)
                .HasForeignKey(u => u.TeamId)
                .OnDelete(DeleteBehavior.SetNull);

           
            // RELACIÓN TEAM ↔ FIELD (N:M)
            builder.Entity<Team>()
                .HasMany(t => t.Fields)
                .WithMany(f => f.Teams)
                .UsingEntity(j => j.ToTable("TeamFields"));

            
            // RELACIÓN TEAM -> RESERVATION (1:N)
            builder.Entity<Reservation>()
                .HasOne(r => r.Team)
                .WithMany(t => t.Reservations)
                .HasForeignKey(r => r.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            
            // RELACIÓN FIELD -> RESERVATION (1:N)
            builder.Entity<Reservation>()
                .HasOne(r => r.Field)
                .WithMany(f => f.Reservations)
                .HasForeignKey(r => r.FieldId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Team -> Ranking (1:1)
            builder.Entity<Ranking>()
                   .HasOne(r => r.Team)
                   .WithOne(t => t.Ranking)
                   .HasForeignKey<Ranking>(r => r.TeamId)
                   .OnDelete(DeleteBehavior.Cascade);

            //Relacion Match
            builder.Entity<Match>()
                .HasOne(m => m.TeamA)
                .WithMany(t => t.MatchesAsTeamA)
                .HasForeignKey(m => m.TeamAId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Match>()
                .HasOne(m => m.TeamB)
                .WithMany(t => t.MatchesAsTeamB)
                .HasForeignKey(m => m.TeamBId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Field>().HasData(
               new Field
               {
                   FieldId = 1,
                   Name = "Campo Fútbol CD Tajamar",
                   Location = "Vallecas, Madrid",
                   EmailContact = "contacto@tajamar.com",
                   PlusCode = "98WW+VV Madrid"
               },
               new Field
               {
                   FieldId = 2,
                   Name = "Campos de fútbol Iker Casillas",
                   Location = "Av. de Iker Casillas, Móstoles, Madrid",
                   EmailContact = "contacto@iker.com",
                   PlusCode = "84G4+JG Móstoles"
               },
               new Field
               {
                   FieldId = 3,
                   Name = "Campo Municipal Federico Zarza Núñez",
                   Location = "C. San Isidro, 3, Cubas de la Sagra, Madrid",
                   EmailContact = "contacto@federicozarza.com",
                   PlusCode = "84G4+JG Cubas"
               },
               new Field
               {
                   FieldId = 4,
                   Name = "Campo Deportivo La Elipa",
                   Location = "C. Sierra de Alcaraz, 23, Madrid",
                   EmailContact = "contacto@laelipa.com",
                   PlusCode = "G8V7+R4 Madrid"
               },
               new Field
               {
                   FieldId = 5,
                   Name = "Campo Municipal Puerta de Hierro",
                   Location = "Av. de la Moncloa, 28040 Madrid",
                   EmailContact = "contacto@puertadehierro.com",
                   PlusCode = "G9F7+2F Madrid"
               },
                new Field
                {
                    FieldId = 6,
                    Name = "Complejo Deportivo Orcasitas",
                    Location = "C. Albufera, 35, Madrid",
                    EmailContact = "contacto@orcasitas.com",
                    PlusCode = "G8V8+H9 Madrid"
                },
                new Field
                {
                    FieldId = 7,
                    Name = "Campo Municipal Vallehermoso",
                    Location = "C. Fernández de los Ríos, 42, Madrid",
                    EmailContact = "contacto@vallehermoso.com",
                    PlusCode = "G9F9+JG Madrid"
                },
                new Field
                {
                    FieldId = 8,
                    Name = "Polideportivo Villaverde",
                    Location = "Av. de los Poblados, 15, Madrid",
                    EmailContact = "contacto@villaverde.com",
                    PlusCode = "G8V6+XH Madrid"
                },
                new Field
                {
                    FieldId = 9,
                    Name = "Campo Municipal San Blas",
                    Location = "C. Alcalá, 523, Madrid",
                    EmailContact = "contacto@sanblas.com",
                    PlusCode = "G9F8+4J Madrid"
                },
                new Field
                {
                    FieldId = 10,
                    Name = "Complejo Deportivo La Vaguada",
                    Location = "Av. Monforte de Lemos, 31, Madrid",
                    EmailContact = "contacto@lavaguada.com",
                    PlusCode = "G9F7+9V Madrid"
                },
                new Field
                {
                    FieldId = 11,
                    Name = "Campo Municipal Carabanchel",
                    Location = "C. Eugenia de Montijo, 53, Madrid",
                    EmailContact = "contacto@carabanchel.com",
                    PlusCode = "G8V7+QX Madrid"
                },
                new Field
                {
                    FieldId = 12,
                    Name = "Campo Deportivo Ciudad Lineal",
                    Location = "C. Arturo Soria, 244, Madrid",
                    EmailContact = "contacto@ciudadlineal.com",
                    PlusCode = "G9F8+PM Madrid"
                },
                new Field
                {
                    FieldId = 13,
                    Name = "Campo Municipal Las Tablas",
                    Location = "C. Palas del Rey, 11, Madrid",
                    EmailContact = "contacto@lastablas.com",
                    PlusCode = "G9F9+W2 Madrid"
                }
           );

        }
    }
}
