using System.Linq;
using DAL;
using DTO;
using System.Collections.Generic;
using Services.Interfaces;

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

       public IEnumerable<Person> GetPeople()
       {
           return personRepository.GetAll;
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
