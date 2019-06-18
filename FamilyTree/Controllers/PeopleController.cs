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
        public async Task<IActionResult> PutPerson([FromRoute] int id, [FromBody] Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != person.Id)
            {
                return BadRequest();
            }

            _context.Entry(person).State = EntityState.Modified;

            var connection = new SqlConnection(FamilyTreeConStr);
            var rep = new PersonRepository(connection);
            var test = await rep.Create(new Person { FirstName = "test", LastName = "testy", PlaceOfBirth = "Africa", LifeStatus = "Dead", DateOfBirth = new DateTime(1789, 3, 22) });
            return test;
        }

        // POST: api/People
        [HttpPost]
        public async Task<IActionResult> PostPerson([FromBody] Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Person.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }

        // DELETE: api/People/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var person = await _context.Person.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.Person.Remove(person);
            await _context.SaveChangesAsync();

            return Ok(person);
        }

        private bool PersonExists(int id)
        {
            return _context.Person.Any(e => e.Id == id);
        }
    }
}