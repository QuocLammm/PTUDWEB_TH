using Model.Famework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class AccountModel
    {
        private OnlineShopDbContext context = null;
        public AccountModel()
        {
            context = new OnlineShopDbContext();
        }
        public bool Login(string username, string password)
        {
            object[] sqlParams =
                    {
                new SqlParameter("@Username",username),
                new SqlParameter("@Password",password),
            };
            bool res = context.Database.SqlQuery<bool>("sp_Account_Login @Username, @Password", sqlParams).SingleOrDefault();
            return res;
        }
    }
}
