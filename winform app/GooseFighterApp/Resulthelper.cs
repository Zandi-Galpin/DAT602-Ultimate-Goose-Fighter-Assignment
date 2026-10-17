using System;
using System.Collections.Generic;
using System.Data;

namespace GooseFighterApp
{
    /// <summary>
    /// Every stored procedure returns a DataTable with a status column the forms use this class
    /// to read that status and to turn the whole row into a line of text so every button reports its result the same way.
    /// </summary>
    public static class ResultHelper
    {
        // Statuses that mean the action worked (shown in green).
        // Every other status is shown in red.
        private static readonly HashSet<string> GoodStatuses = new HashSet<string>
        {
            "SUCCESS", "HIT", "KILLED", "NPC_HIT", "NPC_KILLED", "AMMO_ADDED",
            "STORED_IN_NEST", "DISCARDED", "RETURNED", "SPAWNED"
        };

        /// <summary>
        /// Reads the status from the first row, or NO_RESULT if the table is empty.
        /// </summary>
        public static string GetStatus(DataTable result, string statusColumn)
        {
            if (result == null || result.Rows.Count == 0)
            {
                return "NO_RESULT";
            }
            return Convert.ToString(result.Rows[0][statusColumn]);
        }

        public static bool IsGood(string status)
        {
            return GoodStatuses.Contains(status);
        }

        /// <summary>
        /// Builds a line e.g.  Attack UP: HIT (DamageDealt 60, VictimPlayerID 3, VictimHealth 3940)
        /// Columns that are NULL are left out.
        /// </summary>
        public static string Describe(string action, DataTable result, string statusColumn)
        {
            string status = GetStatus(result, statusColumn);
            if (result == null || result.Rows.Count == 0)
            {
                return action + ": " + status;
            }

            DataRow row = result.Rows[0];
            List<string> details = new List<string>();
            foreach (DataColumn column in result.Columns)
            {
                if (column.ColumnName == statusColumn || row.IsNull(column))
                {
                    continue;
                }
                details.Add(column.ColumnName + " " + row[column]);
            }

            return action + ": " + status + (details.Count > 0 ? " (" + string.Join(", ", details) + ")" : "");
        }
    }
}