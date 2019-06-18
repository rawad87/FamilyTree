using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FamilyTree;
using FamilyTree.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FamilyTree.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly FamilyTreeDbContext _context;
        public string FamilyTreeConStr;

        public PeopleController(IOptions<ConnectionStringList> connectionStrings)
        {
            FamilyTreeConStr = connectionStrings.Value.FamilyTree;
        }

        // GET: api/People
        [HttpGet]
        public async Task<IEnumerable<Person>> GetPersonAsync()
        {
            var connection = new SqlConnection(FamilyTreeConStr);
            var rep = new PersonRepository(connection);
            var test = await rep.ReadAll();
            return test;
        }

        // GET: api/People/5
        [HttpGet("{id}")]
        public async Task<IEnumerable<Person>> GetPerson([FromRoute] int id)
        {
            var connection = new SqlConnection(FamilyTreeConStr);
            var rep = new PersonRepository(connection);
            var test = await rep.ReadOneById(5125);
            return test;
        }

        // PUT: api/People/5
        [HttpPut("{id}")]
        public async Task<int> PutPerson([FromRoute] int id, [FromBody] Person person)
        {
            var connection = new SqlConnection(FamilyTreeConStr);
            var rep = new PersonRepository(connection);
            var test = await rep.Create(new Person {FirstName = "abc", LastName = "d", PlaceOfBirth = "Oslo", LifeStatus = "Alive", DateOfBirth = new DateTime(1789, 3, 22) });
            person.Id = id;
            return test;
        }

        // POST: api/People
        [HttpPost]
        public async Task<int> PostPerson([FromBody] Person person)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //_context.Person.Add(person);
            //await _context.SaveChangesAsync();

            var connection = new SqlConnection(FamilyTreeConStr);
            var rep = new PersonRepository(connection);
            var test = await rep.Update(person);
            return test;
            //return CreatedAtAction("GetPersonAsync", new { id = person.Id }, person);
        }

        // DELETE: api/People/5
        [HttpDelete("{id}")]
        public async Task<int> DeletePerson([FromRoute] int id)
        {

            var connection = new SqlConnection(FamilyTreeConStr);
            var rep = new PersonRepository(connection);
            var test = await rep.Delete(null, id);
            //person.Id = id;
            return test;
        }

        private bool PersonExists(int id)
        {
            return _context.Person.Any(e => e.Id == id);
        }
    }
}