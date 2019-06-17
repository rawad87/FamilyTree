using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FamilyTree
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
            TestQueries().Wait();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        static async Task TestQueries()
        {
            var connStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FamilyTreeDb;Integrated Security=True;";
            var conn = new SqlConnection(connStr);
            var repo = new PersonRepository(conn);
            await repo.DeleteAll();
            await repo.Create(new Person() { FirstName = "Terje", LastName = "Kolderup", PlaceOfBirth = "Larvik", LifeStatus = "Alive", DateOfBirth = new DateTime(1960, 12, 12) });
            await repo.CreateWithFatherId(new Person() { FirstName = "Geir", LastName = "Kolderup", PlaceOfBirth = "Larvik", LifeStatus = "Alive", DateOfBirth = new DateTime(1950, 06, 06), FatherId = 5126 });
            await repo.CreateWithMotherId(new Person() { FirstName = "Eskil", LastName = "Kolderup", PlaceOfBirth = "Larvik", LifeStatus = "Alive", DateOfBirth = new DateTime(1950, 06, 06), MotherId = 5127 });
            await repo.Create(new Person() { FirstName = "Per", LastName = "Olsen", PlaceOfBirth = "Oslo", LifeStatus = "Dead", DateOfBirth = new DateTime(1881, 01, 01) });
            var allPersons = await repo.ReadAll();
            await repo.ReadOneById(5128);
            var terje = allPersons.First();
            await repo.Update(terje);
            //await repo.Delete(null, 512);

            //int rowsDeleted = await conn.ExecuteAsync(deleteAll);

            //int rowsInserted1 = personRepository.Create {  FirstName= "Per", LastName = "Olsen", MiddleName = (string)null, PlaceOfBirth = "Larvik", LifeStatus = "Alive", DateOfBirth = new DateTime(2019, 6, 5) });
            //int rowsInserted2 = await conn.ExecuteAsync(create2, new { FirstName = "Pål", LastName = "Jensen", MiddleName = (string)null, PlaceOfBirth = "Larvik", LifeStatus = "Alive", DateOfBirth = new DateTime(2019, 7, 6) });
            //int rowsInserted3 = await conn.ExecuteAsync(create, new { FirstName = "Hans", LastName = "Olsen", MiddleName = "Kåre", PlaceOfBirth = "Larvik", LifeStatus = "Alive", DateOfBirth = new DateTime(2019, 8, 7), FatherId = 4121 });
            //int rowsInserted4 = await conn.ExecuteAsync(create2, new { FirstName = "Ole", LastName = "Pettersen", MiddleName = (string)null, PlaceOfBirth = "Larvik", LifeStatus = "Alive", DateOfBirth = new DateTime(2000, 2, 7) });

            //IEnumerable<Person> persons = await conn.QueryAsync<Person>(readFamily, new{LastName = "Olsen"});

            //Person per = await conn.QueryFirstOrDefaultAsync<Person>(readOneByName, new { FirstName = "Per" });

            //int rowsAffected2 = await conn.ExecuteAsync(update, new { FirstName = "Petter", LastName = "Pettersen", Id = per.Id });
            //persons = await conn.QueryAsync<Person>(readFamily);

            //int rowsAffected3 = await conn.ExecuteAsync(delete, new { Id = per.Id });
            //persons = await conn.QueryAsync<Person>(readMany);
        }

    }
}
