using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace FamilyTree
{
    public class PersonRepository
    {
        private SqlConnection _connection;

        public PersonRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> Create(Person person)
        {
            var sql = @"INSERT INTO PersonInfo (FirstName, LastName, MiddleName, PlaceOfBirth, DateOfBirth, LifeStatus)                         
                            VALUES (@FirstName, @LastName, @MiddleName, @PlaceOfBirth, @DateOfBirth, @LifeStatus)";
            return await _connection.ExecuteAsync(sql, person);

        }
        public async Task<int> CreateWithFatherId(Person person)
        {
            var sql = @"INSERT INTO PersonInfo (FirstName, LastName, MiddleName, PlaceOfBirth, DateOfBirth, LifeStatus, FatherId)                         
                            VALUES (@FirstName, @LastName, @MiddleName, @PlaceOfBirth, @DateOfBirth, @LifeStatus, @FatherId)";
            return await _connection.ExecuteAsync(sql, person);

        }
        public async Task<int> CreateWithMotherId(Person person)
        {
            var sql = @"INSERT INTO PersonInfo (FirstName, LastName, MiddleName, PlaceOfBirth, DateOfBirth, LifeStatus, MotherId)                         
                            VALUES (@FirstName, @LastName, @MiddleName, @PlaceOfBirth, @DateOfBirth, @LifeStatus, @MotherId)";
            return await _connection.ExecuteAsync(sql, person);

        }

        public async Task<IEnumerable<Person>> ReadAll()
        {
            var sql = @"SELECT Id, FirstName, LastName, DateOfBirth
                            FROM PersonInfo";
            return await _connection.QueryAsync<Person>(sql);
        }

        //public async Task<IEnumerable<Person>> ReadYongerThan(int DateOfBirthMin)
        //{
        //    var sql = @"SELECT Id, FirstName, LastName, DateOfBirth
        //                    FROM PersonInfo WHERE DateOfBirth > @DateOfBirthMin";
        //    return await _connection.QueryAsync<Person>(sql, new { DateOfBirthMin = DateOfBirthMin });
        //}
        public async Task<IEnumerable<Person>> ReadOneById(int id)
        {
            var sql = @"SELECT Id, FirstName, LastName, DateOfBirth
                            FROM PersonInfo WHERE Id = @Id";
            return await _connection.QueryAsync<Person>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Person>> ReadFamily()
        {
            var sql = @"
            SELECT p.*, f.FirstName FatherFirstName
            FROM PersonInfo p
                LEFT JOIN PersonInfo f ON p.FatherId = f.Id
            WHERE p.Id = @ID
            UNION
            SELECT * , '' FatherFirstName
                FROM PersonInfo
            WHERE FatherId = @ID";

            return await _connection.QueryAsync<Person>(sql);
        }

        public async Task<int> Update(Person person)
        {
            var sql = @"UPDATE PersonInfo
                           SET LastName = @LastName, FirstName = @FirstName, DateOfBirth = @DateOfBirth
                           WHERE Id = @Id";
            return await _connection.ExecuteAsync(sql, person);

        }
        public async Task<int> Delete(Person person = null, int? id = null)
        {
            var sql = @"Delete FROM PersonInfo
                           WHERE Id = @Id";
            return await _connection.ExecuteAsync(sql, person ?? (object)new { Id = id.Value });

        }
        public async Task<int> DeleteAll()
        {
            var sql = @"Delete FROM PersonInfo";
            return await _connection.ExecuteAsync(sql);

        }
    }
}
