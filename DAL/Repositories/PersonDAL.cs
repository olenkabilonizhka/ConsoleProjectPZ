using DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL.Repositories
{
    public class PersonDAL : IPersonDAL
    {
        protected string connStr;

        public PersonDAL(string connStr)
        {
            this.connStr = connStr;
        }

        public PersonDTO CreatePerson(PersonDTO p)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand comm = conn.CreateCommand())
            {
                conn.Open();

                comm.CommandText = "INSERT INTO Person(Firstname,Lastname,Email,Password,RoleId) output INSERTED.PersonId VALUES(@firstname,@lastname,@email,@password,@roleid)";
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@firstname", p.Firstname);
                comm.Parameters.AddWithValue("@lastname", p.Lastname);
                comm.Parameters.AddWithValue("@email", p.Email);
                comm.Parameters.AddWithValue("@password", p.Password);
                comm.Parameters.AddWithValue("@roleid", p.RoleId);
                p.PersonId = (int)comm.ExecuteScalar();

                if ((int)Roles.User == p.RoleId)
                {
                    comm.CommandText = "INSERT INTO Customer(PersonId,Status) VALUES(@personid,@status)";
                    comm.Parameters.Clear();
                    comm.Parameters.AddWithValue("@personid", p.PersonId);
                    comm.Parameters.AddWithValue("@status", 1);
                    comm.ExecuteNonQuery();
                }
            }

            return p;
        }

        public void Delete(PersonDTO p)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand comm = conn.CreateCommand())
            {
                conn.Open();
                if ((int)Roles.User == p.RoleId)
                {
                    comm.CommandText = $"DELETE FROM Customer WHERE PersonId={p.PersonId}";
                    comm.ExecuteNonQuery();
                }
                comm.CommandText = $"DELETE FROM Person WHERE PersonId={p.PersonId}";
                comm.ExecuteNonQuery();
            }
        }

        public void UpdatePersonInfo(PersonDTO p,bool roleChanged = false)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand comm = conn.CreateCommand())
            {
                conn.Open();
                if (roleChanged)
                {
                    if (p.RoleId == (int)Roles.User)
                    {
                        comm.CommandText = "INSERT INTO Customer(PersonId,Status) VALUES(@personid,@status)";
                        comm.Parameters.Clear();
                        comm.Parameters.AddWithValue("@personid", p.PersonId);
                        comm.Parameters.AddWithValue("@status", 1);
                        comm.ExecuteNonQuery();
                    }
                    else if (p.RoleId == (int)Roles.Admin)
                    {
                        comm.CommandText = $"DELETE FROM Customer WHERE PersonId={p.PersonId}";
                        comm.ExecuteNonQuery();
                    }
                    else
                        throw new Exception($"Not exist RoleId={p.RoleId}.");
                }

                comm.CommandText = $"UPDATE Person SET Firstname = @firstname, Lastname = @lastname, Email = @email, Password = @password, RoleId = @roleid, RowUpdateTime = @rowupdatetime WHERE PersonId={p.PersonId}";
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@firstname", p.Firstname);
                comm.Parameters.AddWithValue("@lastname", p.Lastname);
                comm.Parameters.AddWithValue("@email", p.Email);
                comm.Parameters.AddWithValue("@password", p.Password);
                comm.Parameters.AddWithValue("@roleid", p.RoleId);
                comm.Parameters.AddWithValue("@rowupdatetime", DateTime.Now);
                comm.ExecuteNonQuery();
            }
        }

        public List<PersonDTO> GetAll()
        {
            var people = new List<PersonDTO>();
            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand comm = conn.CreateCommand())
            {
                conn.Open();
                comm.CommandText = "SELECT p.PersonId,p.Firstname,p.Lastname,p.Email,p.Password, r.RoleId,r.RoleName," +
                    "c.Status,p.RowInsertTime,p.RowUpdateTime,c.RowInsertTime as RowInsertTimeStatus,c.RowUpdateTime as RowUpdateTimeStatus " +
                    "FROM Person p JOIN [Role] r ON p.RoleId=r.RoleId " +
                    "LEFT OUTER JOIN [Customer] c ON p.PersonId=c.PersonId";
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    var t = (int)Roles.User == (int)reader["RoleId"];
                    people.Add(new PersonDTO
                    {
                        PersonId = (int)reader["PersonId"],
                        Firstname = reader["Firstname"].ToString(),
                        Lastname = reader["Lastname"].ToString(),
                        Email = reader["Email"].ToString(),
                        Password = reader["Password"].ToString(),
                        RoleId = (int)reader["RoleId"],
                        RoleName = reader["RoleName"].ToString(),
                        Status = (int)Roles.User == (int)reader["RoleId"] ? (bool)reader["Status"] : (bool?)null,
                        RowInsertTime = (DateTime)reader["RowInsertTime"],
                        RowUpdateTime = (DateTime)reader["RowUpdateTime"],
                        RowInsertTimeStatus = (int)Roles.User == (int)reader["RoleId"] ? (DateTime)reader["RowInsertTimeStatus"] : (DateTime?)null,
                        RowUpdateTimeStatus = (int)Roles.User == (int)reader["RoleId"] ? (DateTime)reader["RowUpdateTimeStatus"] : (DateTime?)null
                    });
                }
            }

            return people;
        }

        public void UpdateStatus(PersonDTO p)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand comm = conn.CreateCommand())
            {
                conn.Open();

                if (p.RoleId == (int)Roles.User)
                {
                    comm.CommandText = $"UPDATE Customer SET Status = @status, RowUpdateTime = @rowupdatetime WHERE PersonId={p.PersonId}";
                    comm.Parameters.Clear();
                    comm.Parameters.AddWithValue("@status", (bool)p.Status ? 1 : 0);
                    comm.Parameters.AddWithValue("@rowupdatetime", DateTime.Now);
                    comm.ExecuteNonQuery();
                }
                else if (p.RoleId == (int)Roles.Admin)
                    throw new Exception($"Admin does not have a status.");
                else
                    throw new Exception($"Not exist RoleId={p.RoleId}.");
            }
        }

    }
}
