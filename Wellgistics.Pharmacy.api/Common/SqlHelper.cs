using Microsoft.Data.SqlClient;
using System.Data;

namespace Wellgistics.Pharmacy.api.Common
{
    public static class SqlHelper
    {
        public static SqlParameter CheckNotNullString(string sqlParamName, string? sqlParamValue)
        {
            if (string.IsNullOrEmpty(sqlParamValue))
            {
                return new SqlParameter(sqlParamName, SqlDbType.VarChar) { Value = DBNull.Value };
            }
            return new SqlParameter(sqlParamName, sqlParamValue.Replace("'", "''"));
        }
        public static SqlParameter CheckNotNullDateTime(string sqlParamName, DateTime? sqlParamValue)
        {
            if (!sqlParamValue.HasValue)
            {
                return new SqlParameter(sqlParamName, SqlDbType.DateTime) { Value = DBNull.Value };
            }
            return new SqlParameter(sqlParamName, sqlParamValue.Value);
        }
        public static SqlParameter CheckNotNullTimeSpan(string sqlParamName, DateTime? sqlParamValue)
        {
            if (!sqlParamValue.HasValue)
            {
                return new SqlParameter(sqlParamName, SqlDbType.Time) { Value = DBNull.Value };
            }
            return new SqlParameter(sqlParamName, sqlParamValue.Value.TimeOfDay);
        }
        public static SqlParameter CheckNotNullDecimal(string sqlParamName, decimal? sqlParamValue)
        {
            if (!sqlParamValue.HasValue || sqlParamValue == 0m)
            {
                return new SqlParameter(sqlParamName, SqlDbType.Decimal) { Value = DBNull.Value };
            }
            return new SqlParameter(sqlParamName, sqlParamValue.Value);
        }

        public static SqlParameter CheckNotNullInt(string sqlParamName, int? sqlParamValue)
        {
            if (!sqlParamValue.HasValue || sqlParamValue == 0)
            {
                return new SqlParameter(sqlParamName, SqlDbType.Int) { Value = DBNull.Value };
            }
            return new SqlParameter(sqlParamName, sqlParamValue.Value);
        }
        public static SqlParameter CheckNotNullLong(string sqlParamName, long? sqlParamValue)
        {
            if (!sqlParamValue.HasValue || sqlParamValue == 0)
            {
                return new SqlParameter(sqlParamName, SqlDbType.BigInt) { Value = DBNull.Value };
            }
            return new SqlParameter(sqlParamName, sqlParamValue.Value);
        }

        public static SqlParameter CheckNotNullBool(string sqlParamName, bool? sqlParamValue)
        {
            if (!sqlParamValue.HasValue)
            {
                return new SqlParameter(sqlParamName, SqlDbType.Bit) { Value = DBNull.Value };
            }
            return new SqlParameter(sqlParamName, sqlParamValue.Value);
        }
        /// ‹summary>
        /// Checks the null and not zero.
        /// </summary>
        /// ‹param name="sqlParamValue">The value of sql parameter.‹/param>
        /// ‹param name="sqlParamName">Name of the sql parameter‹/param>
        /// ‹returns>A bool.‹/returns>
        public static SqlParameter CheckNotNull(string sqlParamName, string? sqlParamValue)
        {
            SqlParameter? parameter = null;
            if (sqlParamValue != null)
                parameter = new SqlParameter(sqlParamName, sqlParamValue.Replace("'", "''"));
            else
            {
                parameter = new SqlParameter(sqlParamName, SqlDbType.VarChar);
                parameter.Value = DBNull.Value;
            }

            return parameter;
        }
    }
}
