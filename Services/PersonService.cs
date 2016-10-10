using System.Linq;
using DAL;
using DTO;

namespace Services
{
   public  class PersonService : IPersonService
    {
       private IRepository<person> personRepository;
       private IRepository<job> personJobRepository;

       public PersonService(IRepository<person> personRepository, IRepository<job> personJobRepository)
       {
           this.personRepository = personRepository;
           this.personJobRepository = personJobRepository;
       }

       public IQueryable<person> GetPeople()
       {
           return personRepository.Table;
       }

       public person GetPerson(long id)
       {
           return personRepository.GetById(id);
       }

       public void InsertPerson(person person)
       {
           personRepository.Insert(person);
       }

       public void UpdatePerson(person person)
       {
           personRepository.Update(person);
       }

       public void DeletePerson(person person)
       {
           personRepository.Delete(person);
       }
    }
}
