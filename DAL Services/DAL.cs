using MySql.Data.MySqlClient;
using SendEmailViaSMTP.Models;
using System.Data;
using System.Transactions;

namespace SendEmailViaSMTP.DAL_Services
{
    public class DAL
    { private readonly string _connection;
        
        //public DAL(IConfiguration configuration)
        //{
        //    _connection = configuration.GetConnectionString("Constr");
        //}

        private readonly string str = "server=localhost;port=3306;uid=root;pwd=sobiazafar@2023;database=mvc_crud";
        
        public bool TransactionScope()
        {
            using (var txscope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    using (MySqlConnection objConn = new MySqlConnection(str))
                    {
                        objConn.Open();
                        MySqlCommand objCmd1 = new MySqlCommand("insert into tblProject values(6, 'TestProject')", objConn);
                        MySqlCommand objCmd2 = new MySqlCommand("insert into tblProjectMember(MemberID, ProjectID) values(2, 6)", objConn);

                        objCmd1.ExecuteNonQuery();
                        objCmd2.ExecuteNonQuery(); // Throws exception due to foreign key constraint    

                        //The Transaction will be completed      
                        txscope.Complete();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    // Log error      
                    txscope.Dispose();
                    return false;
                }
            }
        }
        public bool BeginTransaction()
        {
           
            MySqlTransaction objTrans = null;

            using (MySqlConnection objConn = new MySqlConnection(str))
            {
                objConn.Open();
                objTrans = objConn.BeginTransaction();
                MySqlCommand objCmd1 = new MySqlCommand("insert into tblProject values(7, 'beginfrom')", objConn);
                MySqlCommand objCmd2 = new MySqlCommand("insert into tblProjectMember(MemberID, ProjectID) values(9, 7)", objConn);
                try
                {
                    objCmd1.ExecuteNonQuery();
                    objCmd2.ExecuteNonQuery(); // Throws exception due to foreign key constraint   
                    objTrans.Commit();
                    return true;
                }
                catch (Exception)
                {
                    objTrans.Rollback();
                    return false;
                }
                finally
                {
                    objConn.Close();
                }
            }

        }
        
        
        
        public List<UserModel> GetEmployee()
        {
            List<UserModel> lst = new List<UserModel>();
            MySqlConnection con = new MySqlConnection(str);
            MySqlDataReader dr;
            DataTable tbl = new DataTable();
            con.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_display_user";
            dr = cmd.ExecuteReader();
            tbl.Load(dr);
            con.Close();
            foreach (DataRow dar in tbl.Rows)
            {
                lst.Add(new UserModel
                {
                    Id = (int)dar["id"],
                    Name = Convert.ToString(dar["name"]),
                    Email = Convert.ToString(dar["email"]),
                    Age = dar["age"] != DBNull.Value ? Convert.ToInt32(dar["age"]) : int.MinValue,
                    //Email = !Convert.IsDBNull(dar["email"]) ? (int?)dar["email"] :

                    //Join_date = ((DateTime)dar["join_date"]),
                    Join_date = dar["join_date"] != DBNull.Value ? Convert.ToDateTime(dar["join_date"]) : DateTime.MinValue,
                    End_date = dar["end_date"] != DBNull.Value ? Convert.ToDateTime(dar["end_date"]) : DateTime.MinValue

                });

            }

            return lst;
        }
        public bool Add_user(UserModel user)
        {
            MySqlConnection conn = new MySqlConnection(str);
            MySqlCommand cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_insert_user";
            cmd.Parameters.AddWithValue("_name", user.Name);
            cmd.Parameters.AddWithValue("_email", user.Email);
            cmd.Parameters.AddWithValue("_age", user.Age);
            cmd.Parameters.AddWithValue("_join_date", user.Join_date);
            cmd.Parameters.AddWithValue("_end_date", user.End_date);
            try
            {
                int i = cmd.ExecuteNonQuery();
                conn.Close();
                if (i > 0)
                    return true;
                else
                    return false;
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message);
            }

        }
        public bool Update(UserModel user)
        {
            MySqlConnection conn = new MySqlConnection(str);
            MySqlCommand cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_update";
            cmd.Parameters.AddWithValue("_id", user.Id);
            cmd.Parameters.AddWithValue("_name", user.Name);
            cmd.Parameters.AddWithValue("_email", user.Email);
            cmd.Parameters.AddWithValue("_age", user.Age);
            cmd.Parameters.AddWithValue("_join_date", user.Join_date);
            cmd.Parameters.AddWithValue("_end_date", user.End_date);
            try
            {
                int i = cmd.ExecuteNonQuery();
                conn.Close();
                if (i > 0) return true;
                else
                    return false;
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message);
            }

        }
        public bool Delete(int id)
        {
            MySqlConnection conn = new MySqlConnection(str);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_delete_user";
            cmd.Parameters.AddWithValue("_id",id);
            try
            {
                int i = cmd.ExecuteNonQuery();
                conn.Close();
                if (i > 0) return true;
                else
                    return false;
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message);
            }





        }
        public int Sum(int a,int b)
        {
            return a + b;
        }
        public bool CheckDuplication(UserModel user)
        {
            using(MySqlConnection conn=new MySqlConnection(str))
            {
                conn.Open();
                using(MySqlCommand cmd=new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Duplicacy";
                    cmd.Parameters.AddWithValue("_id", user.Id);
                    List<UserModel> list = new List<UserModel>();
                    DataTable tbl = new DataTable();
                    MySqlDataReader dr= cmd.ExecuteReader();
                    tbl.Load(dr);
                    foreach (DataRow rows in tbl.Rows)
                    {
                        UserModel obj = new UserModel() { Id = (int)rows["id"] };
                        list.Add(obj);
                    }

                    if (list.Count > 0)
                    {
                        return true;
                    }
                    else 
                    {
                        return false;
                    }
                }
            }
        }
    }
}
