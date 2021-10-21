using DTO;
using System.Collections.Generic;

namespace DAL
{
    public interface IPersonDAL
    {
        PersonDTO CreatePerson(PersonDTO p);
        List<PersonDTO> GetAll();
        void UpdatePersonInfo(PersonDTO p, bool roleChanged);
        void UpdateStatus(PersonDTO p);
        void Delete(PersonDTO p);
    }
}
