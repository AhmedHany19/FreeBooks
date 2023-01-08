using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Helper
    {
        public const string PathImageuser = "/Images/Users/";
        public const string PathSaveImageuser = "Images/Users";


        public const string Success = "success";
        public const string Error = "error";

        public const string MsgType = "msgType";
        public const string Title = "title";
        public const string Msg = "msg";

        public const string Save = "Save";
        public const string Update = "Update";
        public const string Delete = "Delete";
        public const string Admin = "Admin";


        // Data Defaul User "SuperAdmin"
        public const string UserName = "superadmin@test.com";
        public const string Email = "superadmin@test.com";
        public const string Name = "SuperAdmin";
        public const string Password = "Pa$$w0rd";
        public const string ImageUser = "vector.jpg";

        // Data Defaul User "Basic"
        public const string UserNameBasic = "basic@test.com";
        public const string EmailBasic = "basic@test.com";
        public const string NameBasic = "BasicUser";
        public const string PasswordBasic = "Pa$$w0rd";
        public const string ImageUserBasic = "vector.jpg";



        public const string Permission = "Permission";


        public enum eCurrentState
        {
            Active = 1,
            Delete = 0
        }

        public enum Roles
        {
            SuperAdmin,
            Admin,
            Basic
        }

        // List Of Controller And Some Method From AccountsController
        public enum PermissionsModuleName
        {
            Home,
            Accounts,
            Registers,
            Roles,
            Categories
        };

    }
}
