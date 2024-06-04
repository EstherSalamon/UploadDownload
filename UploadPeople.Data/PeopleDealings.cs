using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Faker;
using Microsoft.EntityFrameworkCore;

namespace UploadPeople.Data
{
    public class PeopleDealings
    {
        private readonly string _connection;

        public PeopleDealings(string connection)
        {
            _connection = connection;
        }

        public List<Person> GetPeople()
        {
            PeopleDataContext context = new PeopleDataContext(_connection);
            return context.People.ToList();
        }

        public List<Person> GeneratePeople(int amount)
        {
            List<Person> people = new List<Person>();

            for (int i = 0; i < amount; i++)
            {
                Random rnd = new Random();
                Person p = new Person
                {
                    FirstName = Name.First(),
                    LastName = Name.Last(),
                    Age = rnd.Next(20, 60)
                };
                p.Email = $"{p.LastName}.{p.FirstName}@gmail.com";
                people.Add(p);
            }

            return people;
        }

        public void InsertIntoDatabase(List<Person> people)
        {
            PeopleDataContext context = new PeopleDataContext(_connection);
            context.People.AddRange(people);
            context.SaveChanges();
        }

        public void DeleteDatabase()
        {
            PeopleDataContext context = new PeopleDataContext(_connection);
            context.Database.ExecuteSqlInterpolated($"DELETE FROM People");
            context.SaveChanges();
        }

  
    }
}
