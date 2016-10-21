using System.Linq;
using DTO;

namespace Services
{
   public interface IPersonService
    {
       IQueryable<Person> GetPeople();
       Person GetPerson(long id);
       void InsertPerson(Person person);
       void UpdatePerson(Person person);
       void DeletePerson(Person person);
    }
}
