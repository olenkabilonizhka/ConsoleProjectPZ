using DAL.Repositories;
using DTO;
using NUnit.Framework;
using System.Configuration;
//using System.EnterpriseServices;
using System.Linq;
using System.Transactions;

namespace DALTests
{
    [TestFixture]
    public class PersonDALTests
    {
        protected TransactionScope transactionScope;
        PersonDAL personDal;

        [SetUp]
        public void SetUp()
        {
            transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew);

            personDal = new PersonDAL(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString);
        }

        [Test]
        public void CreatePersonTest()
        {
            var result = personDal.CreatePerson(new PersonDTO
            {
                Firstname = "Liam",
                Lastname = "Kaur",
                Email = "liam.kaur@gmail.com",
                Password = "user",
                RoleId = (int)Roles.User,
                Status = true
            });

            Assert.IsTrue(result.PersonId != 0);
        }
        
        [Test]
        public void GetAllTest()
        {
            var result = personDal.CreatePerson(new PersonDTO
            {
                Firstname = "Liam",
                Lastname = "Kaur",
                Email = "liam.kaur@gmail.com",
                Password = "user",
                RoleId = (int)Roles.User,
                Status = true
            });

            var dal = personDal.GetAll();

            Assert.IsTrue(dal.Any(m => m.PersonId == result.PersonId));
        }

        [Test]
        public void DeleteTest()
        {
            var result = personDal.GetAll().Last();
            personDal.Delete(result);

            var dal = personDal.GetAll();

            Assert.IsFalse(dal.Contains(result));
        }

        [Test]
        public void UpdatePersonInfoTest()
        {
            var temp = personDal.GetAll().First();
            temp.Firstname = "Oksi";
            temp.Email = "falseEmail";
            personDal.UpdatePersonInfo(temp);

            var dal = personDal.GetAll();

            Assert.AreEqual(1,dal.Count(x => x.Firstname == temp.Firstname && x.Email == temp.Email));
        }

        [Test]
        public void UpdatePersonInfoWithRoleTest()
        {
            var temp = personDal.GetAll().FindAll(x=>x.RoleId == (int)Roles.Admin).First();
            temp.RoleId = (int)Roles.User;
            personDal.UpdatePersonInfo(temp,true);

            var dal = personDal.GetAll();

            Assert.IsTrue(dal.Any(x => x.PersonId == temp.PersonId && x.Status != null));
        }

        [Test]
        public void UpdateStatusTest()
        {
            var temp = personDal.GetAll().FindAll(x => x.RoleId == (int)Roles.User).Last();
            temp.Status = (bool)temp.Status ? false : true;
            personDal.UpdateStatus(temp);

            var dal = personDal.GetAll();

            Assert.IsFalse(dal.Exists(x=>x.PersonId == temp.PersonId && x.Status != temp.Status));
        }

        [TearDown]
        public void Teardown()
        {
            transactionScope.Dispose();
        }
    }
}
