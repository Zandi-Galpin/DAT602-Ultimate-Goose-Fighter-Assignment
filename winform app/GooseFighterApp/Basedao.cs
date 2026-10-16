using System;
using System.Data;
using System.Data.SqlClient;

namespace GooseFighterApp
{
    /// <summary>
    /// Every DAO method opens its own
    /// connection, calls one stored procedure and returns the result set
    /// as a DataTable. No SQL in C#, procedures instead
    /// </summary>
    public abstract class BaseDao
    {
        protected readonly DatabaseConnection _dbConnection = new DatabaseConnection();

        /// <summary>
        /// Runs stored procedure and returns whatever results from the select
        /// SqlDataAdapter.Fill opens and closes the connection.
        /// </summary>
        protected DataTable ExecuteProcedure(string procedureName, params SqlParameter[] parameters)
        {
            DataTable result = new DataTable();

            using (SqlConnection connection = _dbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(procedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddRange(parameters);

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(result);
                }
            }

            return result;
        }

        /// <summary>
        /// Builds a parameter with a sql type and turns C# null into SQL NULL.
        /// </summary>
        protected static SqlParameter Param(string name, SqlDbType type, object value)
        {
            return new SqlParameter(name, type) { Value = value ?? DBNull.Value };
        }
    }
}