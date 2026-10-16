using System;
using System.Data;

namespace GooseFighterApp
{
    public class LoginDao : BaseDao
    {
        /// <summary>
        /// CRUD 1. Calls usp_LoginPlayer.
        /// LoginStatus is either SUCCESS, INCORRECT_PASSWORD, LOCKED_OUT or NOT_FOUND.
        /// The lockout counting happens in the procedure, not here
        /// </summary>
        public DataTable Login(string username, string password)
        {
            return ExecuteProcedure("dbo.usp_LoginPlayer",
                Param("@Username", SqlDbType.VarChar, username),
                Param("@Password", SqlDbType.VarChar, password));
        }

        /// <summary>
        /// CRUD 2. Calls usp_RegisterPlayer. 
        /// RegisterStatus is either SUCCESS, MISSING_FIELDS, USERNAME_TAKEN or EMAIL_TAKEN.
        /// </summary>
        public DataTable Register(string email, string username, string password)
        {
            return ExecuteProcedure("dbo.usp_RegisterPlayer",
                Param("@Email", SqlDbType.VarChar, email),
                Param("@Username", SqlDbType.VarChar, username),
                Param("@Password", SqlDbType.VarChar, password));
        }

        /// <summary>
        /// CRUD 9 (single player). Calls usp_LogoutPlayer
        /// Used by "Return to title screen".
        /// </summary>
        public DataTable Logout(int playerId)
        {
            return ExecuteProcedure("dbo.usp_LogoutPlayer",
                Param("@PlayerID", SqlDbType.Int, playerId));
        }

        /// <summary>
        /// OLD prototype method (usp_TestLogin). LoginForm still calls this
        /// delete it later
        /// </summary>
        [Obsolete("Replaced by Login(). Delete once LoginForm is updated.")]
        public DataTable AttemptLogin(string username, string password)
        {
            return ExecuteProcedure("dbo.usp_TestLogin",
                Param("@Username", SqlDbType.VarChar, username),
                Param("@Password", SqlDbType.VarChar, password));
        }
    }
}