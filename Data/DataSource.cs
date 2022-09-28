using System;
using System.Data;
using System.Data.Common;

namespace CoreNet.Data
{
    /// <summary>
    /// Represents a base class for database-specific classes that represent data sources.
    /// </summary>
    public abstract class DataSource : IDataSource
    {
        bool disposed = false;

        /// <summary>
        /// Dispose current instance.
        /// </summary>
        ~DataSource()
        {
            Dispose(false);
        }

        /// <summary>
        /// Creates a new <see cref="DataSource"/> instance.
        /// </summary>
        public DataSource() { }

        /// <summary>
        /// Creates a new <see cref="DataSource"/> instance.
        /// </summary>
        /// <param name="provider">Data source provider implementation</param>
        /// <param name="connectionString">String used to open the connection</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> is null</exception>
        public DataSource(DbProviderFactory provider, string connectionString)
        {
            this.Factory = provider ?? throw new ArgumentNullException("provider");
            this.Connection = provider.CreateConnection();
            this.Connection.ConnectionString = connectionString;
            this.SetCommand();
        }

        /// <summary>
        /// Creates a new <see cref="DataSource"/> instance.
        /// </summary>
        /// <param name="providerName">Provider name.</param>
        /// <param name="connectionString">String used to open the connection</param>
        public DataSource(string providerName, string connectionString)
            : this(DbProviderFactories.GetFactory(providerName), connectionString) { }

        /// <summary>
        /// Dispose current instance and realease current memory used.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose current data source object.
        /// </summary>
        /// <param name="disposing">Whether current instance is disposing.</param>
        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (Transaction != null)
                        Transaction.Dispose();
                    if (Connection.State != ConnectionState.Open)
                        Connection.Close();
                    Connection.Dispose();
                    if (Command != null)
                        Command.Dispose();
                    if (Factory != null)
                        Factory = null;
                }
                disposed = true;
            }
        }

        /// <summary>
        /// Creates the command <see cref="DbCommand"/> object associated with the current connection.
        /// </summary>
        protected void SetCommand()
        {
            Command = Connection.CreateCommand();
            Command.Connection = Connection;
            if (DataSourceType == DataSourceType.MSSQL)
                Command.CommandType = CommandType.StoredProcedure;
            else
                Command.CommandType = CommandType.Text;
        }

        /// <summary>
        /// Adds a <see cref="bool"/> parameter type.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        public void AddParameter(string name, bool value)
        {
            AddParameter(name, value, DbType.Boolean);
        }

        /// <summary>
        /// Adds a <see cref="byte"/> parameter type.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        public void AddParameter(string name, byte value)
        {
            AddParameter(name, value, DbType.Byte);
        }

        /// <summary>
        /// Adds a <see cref="byte[]"/> parameter type.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        public void AddParameter(string name, byte[] value)
        {
            AddParameter(name, value, DbType.Binary);
        }

        /// <summary>
        /// Adds a <see cref="DateTime"/> parameter type.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        public void AddParameter(string name, DateTime value)
        {
            AddParameter(name, value, DbType.DateTime);
        }

        /// <summary>
        /// Adds a <see cref="decimal"/> parameter type.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        public void AddParameter(string name, decimal value)
        {
            AddParameter(name, value, DbType.Currency);
        }

        /// <summary>
        /// Adds a <see cref="double"/> parameter type.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        public void AddParameter(string name, double value)
        {
            AddParameter(name, value, DbType.Double);
        }

        /// <summary>
        /// Adds a <see cref="float"/> parameter type.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        public void AddParameter(string name, float value)
        {
            AddParameter(name, value, DbType.Single);
        }

        /// <summary>
        /// Adds a <see cref="int"/> parameter type.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        public void AddParameter(string name, int value)
        {
            AddParameter(name, value, DbType.Int32);
        }

        /// <summary>
        /// Adds a <see cref="long"/> parameter type.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        public void AddParameter(string name, long value)
        {
            AddParameter(name, value, DbType.Int64);
        }

        /// <summary>
        /// Adds a <see cref="object"/> parameter type.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        public void AddParameter(string name, object value)
        {
            AddParameter(name, value ?? DBNull.Value, DbType.Object);
        }

        /// <summary>
        /// Adds a <see cref="short"/> parameter type.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        public void AddParameter(string name, short value)
        {
            AddParameter(name, value, DbType.Int16);
        }

        /// <summary>
        /// Adds a <see cref="string"/> parameter type.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        public void AddParameter(string name, string value)
        {
            AddParameter(name, value, DbType.String);
        }

        private void AddParameter<T>(string name, T value, DbType type)
        {
            if (Command.Parameters.IndexOf(name) == -1)
            {
                DbParameter param = Command.CreateParameter();
                param.ParameterName = name;
                param.DbType = type;
                param.Direction = ParameterDirection.Input;
                if (value != null)
                {
                    if (value is string)
                        param.Size = value.ToString().Length;

                    param.Value = value;
                }
                else
                {
                    param.Size = 1;
                    param.Value = DBNull.Value;
                }
                Command.Parameters.Add(param);
            }
            else
            {
                if (value != null)
                    Command.Parameters[name].Value = value;
                else
                    Command.Parameters[name].Value = DBNull.Value;
            }
        }

        public void ConfirmTransaction()
        {
            if (!TransactionIsSet)
                throw new InvalidOperationException("TransactionIsSet no se ha establecido a true.");

            Transaction.Commit();
        }

        public void UndoTransaction()
        {
            if (!TransactionIsSet)
                throw new InvalidOperationException("TransactionIsSet no se ha establecido a true.");

            Transaction.Rollback();
        }


        /// <summary>
        /// Represents an SQL statement or stored procedure to execute against a data source.
        /// Provides a base class for database-specific classes that represent commands.
        /// Overload:System.Data.Common.DbCommand.ExecuteNonQueryAsync
        /// </summary>
        protected DbCommand Command { get; set; } = null;

        /// <summary>
        /// Gets or sets the connection to a database.
        /// </summary>
        protected DbConnection Connection { get; set; }

        /// <summary>
        /// Gets the string used to open the connection.
        /// </summary>
        protected string ConnectionString { get { return Connection.ConnectionString; } }

        /// <summary>
        /// Gets current data source type.
        /// </summary>
        public abstract DataSourceType DataSourceType { get; }

        /// <summary>
        /// Gets or sets the object for creating instances of a provider's implementation of the data source classes.
        /// </summary>
        protected DbProviderFactory Factory { get; set; }

        /// <summary>
        /// Gets or sets a transaction.
        /// </summary>
        protected DbTransaction Transaction { get; set; } = null;

        /// <summary>
        /// Whether is a transaction is set.
        /// </summary>
        public bool TransactionIsSet { get; set; } = false;
    }
}
