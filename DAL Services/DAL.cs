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
        
        public bool TransactionScope(TransactionModel mdl)
        {
            using (var txscope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    using (MySqlConnection objConn = new MySqlConnection(str))
                    {
                        objConn.Open();
                       // string query1 = "insert into Applicant(ProjectID,Name) values (LAST_INSERT_ID(),@name)";
                        //string query2 = "insert into tblProjectMember(MemberID,ProjectID) values(@mid,@pid)";
                        string query1 = "insert into Applicant(name,gender,age,qualificaion,total_experience) values (@name,@gender,@age,@qualificaion,@total_experience)";
                        string query2 = "insert into experience(company_name,designation,years_worked,app_id) values(@company_name,@designation,@years_worked,@app_id)";

                        MySqlCommand cmd1 = new MySqlCommand(query1, objConn);
                        // cmd1.Parameters.AddWithValue("@name", mdl.Name);
                        cmd1.Parameters.AddWithValue("@name", mdl.Name);
                        cmd1.Parameters.AddWithValue("@gender", mdl.Gender);
                        cmd1.Parameters.AddWithValue("@age", mdl.Age);
                        cmd1.Parameters.AddWithValue("@qualificaion", mdl.Qualificaion);
                        cmd1.Parameters.AddWithValue("@total_experience", mdl.Total_Experience);
                        cmd1.ExecuteNonQuery();
                        long  appid= cmd1.LastInsertedId;
                       
                        foreach(var experience in mdl.Experiences)
                        {
                            MySqlCommand cmd2 = new MySqlCommand(query2, objConn);
                            cmd2.Parameters.AddWithValue("@company_name", experience.company_name);
                            cmd2.Parameters.AddWithValue("@designation", experience.designation);
                            cmd2.Parameters.AddWithValue("@years_worked", experience.years_worked);
                            cmd2.Parameters.AddWithValue("@app_id", appid);
                            cmd2.ExecuteNonQuery();

                        }
                        // cmd1.Parameters.AddWithValue("_id", mdl.ProjectID);      
                       // cmd2.Parameters.AddWithValue("@mid", mdl.MemberID);
                      //  cmd2.Parameters.AddWithValue("@pid",lastinsertedid);
                       // cmd1.ExecuteNonQuery();
                       // cmd2.ExecuteNonQuery(); // Throws exception due to foreign key constraint    

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
