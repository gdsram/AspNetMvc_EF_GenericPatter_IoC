using System.Linq;
using DTO;

namespace Services
{
   public interface IPersonService
    {
       IQueryable<person> GetPeople();
       person GetPerson(long id);
       void InsertPerson(person person);
       void UpdatePerson(person person);
       void DeletePerson(person person);
    }
}
