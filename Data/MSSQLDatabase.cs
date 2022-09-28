using System.Data.Common;

namespace CoreNet.Data
{
    /// <summary>
    /// Microsoft SQL Server Database
    /// </summary>
    public class MSSQLDatabase : Database
    {
        const string CONNECTIONSTRING_STANDAR = "Data Source={0};Initial Catalog={1};User Id={2};Password={3};";
        const string CONNECTIONSTRING_TRUSTED = "Data Source={0};Initial Catalog={1};Integrated Security=SSPI;";

        /// <summary>
        /// Creates a new <see cref="MSSQLDatabase"/> instance.
        /// </summary>
        /// <param name="serverName">Server name</param>
        /// <param name="catalogName">Database name</param>
        public MSSQLDatabase(string serverName, string catalogName)
            : base("System.Data.SqlClient", string.Format(CONNECTIONSTRING_TRUSTED, serverName, catalogName)) { }

        /// <summary>
        /// Creates a new <see cref="MSSQLDatabase"/> instance.
        /// </summary>
        /// <param name="ip">Server IP Address</param>
        /// <param name="port">Server port</param>
        /// <param name="catalogName">Database name</param>
        /// <param name="userName">Username</param>
        /// <param name="password">Password</param>
        public MSSQLDatabase(string ip, string port, string catalogName, string userName, string password)
            : this(ip + "," + port, catalogName, userName, password) { }

        /// <summary>
        /// Creates a new <see cref="MSSQLDatabase"/> instance.
        /// </summary>
        /// <param name="serverName">Server name</param>
        /// <param name="catalogName">Database name</param>
        /// <param name="userName">Username</param>
        /// <param name="password">Password</param>
        public MSSQLDatabase(string serverName, string catalogName, string userName, string password)
            : base("System.Data.SqlClient", string.Format(CONNECTIONSTRING_STANDAR, serverName, catalogName, userName, password)) { }

        /// <summary>
        /// Creates a new <see cref="MSSQLDatabase"/> instance with given parameters.
        /// </summary>
        /// <param name="connectionString">String used to open the connection</param>
        public MSSQLDatabase(string connectionString)
            : base(DbProviderFactories.GetFactory("System.Data.SqlClient"), connectionString) { }

        public override DataSourceType DataSourceType { get; } = DataSourceType.MSSQL;
    }
}
