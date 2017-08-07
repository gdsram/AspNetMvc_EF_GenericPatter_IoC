using System.Linq;
using DTO;
using System.Collections.Generic;

namespace Services.Interfaces
{
   public interface IPersonService
    {
       IEnumerable<Person> GetPeople();
       Person GetPerson(long id);
       void InsertPerson(Person person);
       void UpdatePerson(Person person);
       void DeletePerson(Person person);
    }
}
