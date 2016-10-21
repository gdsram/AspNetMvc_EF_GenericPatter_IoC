using System.Linq;
using DAL;
using DTO;

namespace Services
{
   public  class PersonService : IPersonService
    {
       private IRepository<Person> personRepository;
       private IRepository<Job> personJobRepository;

       public PersonService(IRepository<Person> personRepository, IRepository<Job> personJobRepository)
       {
           this.personRepository = personRepository;
           this.personJobRepository = personJobRepository;
       }

       public IQueryable<Person> GetPeople()
       {
           return personRepository.Table;
       }

       public Person GetPerson(long id)
       {
           return personRepository.GetById(id);
       }

       public void InsertPerson(Person person)
       {
           personRepository.Insert(person);
       }

       public void UpdatePerson(Person person)
       {
           personRepository.Update(person);
       }

       public void DeletePerson(Person person)
       {
           personRepository.Delete(person);
       }
    }
}
