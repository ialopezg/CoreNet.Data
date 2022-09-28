using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreNet.Data
{
    /// <summary>
    /// Represents a connection to a data source.
    /// </summary>
    public abstract class Database : DataSource
    {
        /// <summary>
        /// Creates a new <see cref="Database"/> instance with default parameters.
        /// </summary>
        public Database()
            :base() { }

        /// <summary>
        /// Creates a new <see cref="Database"/> instance with given parameters.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="connectionString"></param>
        public Database(DbProviderFactory factory, string connectionString)
            : base(factory, connectionString) { }

        /// <summary>
        /// Creates a new <see cref="DataSource"/> instance.
        /// </summary>
        /// <param name="providerName">Provider name.</param>
        /// <param name="connectionString">String used to open the connection</param>
        public Database(string providerName, string connectionString)
            : base(providerName, connectionString) { }

        /// <summary>
        /// Ejecuta una instrucción T-SQL en un objeto de conexión.
        /// </summary>
        /// <returns>Número de filas afectadas.</returns>
        public int ExecuteQuerry()
        {
            int result;
            if (Connection.State.Equals(ConnectionState.Closed))
                Connection.Open();

            if (TransactionIsSet == true)
            {
                if (Transaction == null)
                    Transaction = Connection.BeginTransaction();

                if (Command == null)
                    Command.Transaction = Transaction;
            }

            try
            {
                if (prepared == true)
                    Command.Prepare();

                result = (int)(this.Command.ExecuteNonQuery());
            }
            catch (DbException) { result = -1; }
            finally
            {
                this.Command.Parameters.Clear();
                if (this.TransactionIsSet == false)
                    this.Connection.Close();
            }

            return result;
        }

        /// <summary>
        /// Retuns the data table for current context.
        /// </summary>
        /// <returns>A list of <see cref="DataRow"/>.</returns>
        public DataRow GetDataRow()
        {
            DataTable dataTable = GetDataTable();

            return dataTable.Rows.Count > 0 ? dataTable.Rows[0] : null;
        }

        /// <summary>
        /// Returns a data cache of current context.
        /// </summary>
        /// <returns>A <see cref="DataSet"/> object.</returns>
        public DataSet GetDataSet()
        {
            try
            {
                DbDataAdapter adapter = this.Factory.CreateDataAdapter();
                adapter.SelectCommand = this.Command;
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                return ds;
            }
            catch (Exception) { return null; }
        }

        /// <summary>
        /// Retuns the data table for current context.
        /// </summary>
        /// <returns>A <see cref="DataTable"/> object.</returns>
        public DataTable GetDataTable()
        {
            DataSet dataSet = GetDataSet();

            return dataSet.Tables.Count > 0 ? dataSet.Tables[0] : null;
        }

        /// <summary>
        /// Returns the value of current field.
        /// </summary>
        /// <returns>An <see cref="object"/> value.</returns>
        public object GetFieldValue()
        {
            Connection.Open();
            object result = Command.ExecuteScalar();
            Connection.Close();

            return result ?? string.Empty;
        }

        /// <summary>
        /// Returns next ID for given table name.
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <returns>A <see cref="int"/> value</returns>
        public int GetNextId(string tableName)
        {
            PrepareQuerry("SELECT CurrentValue FROM Sequences WHERE TableName = ?", CommandType.Text);
            AddParameter("TableName", tableName);
            int value = (int)GetFieldValue();
            SetCommand();
            PrepareQuerry("UPDATE Sequences SET CurrentValue = CurrentValue + 1 WHERE TableName = ?", CommandType.Text);
            AddParameter("TableName", tableName);
            ExecuteQuerry();

            return value;
        }

        /// <summary>
        /// Whether if current context has rows.
        /// </summary>
        public bool HasRecords
        { 
            get { return RecordCount() > 0; }
        }

        /// <summary>
        /// Prepare current instance to execute a query.
        /// </summary>
        /// <param name="commandText">SQL Query, Stored Procedure or table name</param>
        /// <param name="commandType">Command type</param>
        public void PrepareQuerry(string commandText, CommandType commandType)
        {
            if (Command.CommandText.Length == 0)
            {
                prepared = true;
                Command.CommandType = commandType;
                Command.CommandText = commandText;
            }
            else
            {
                if (Command.CommandText == commandText)
                {
                    prepared = true;
                    SetCommand();
                    Command.CommandType = commandType;
                    Command.CommandText = commandText;
                }
                else
                    prepared = false;
            }
        }

        /// <summary>
        /// Gets the total rows in current context.
        /// </summary>
        public int RecordCount()
        {
            try
            {
                this.Connection.Open();

                return (int)Command.ExecuteNonQuery();
            }
            catch (Exception) { return 0; }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
       
        }

        /// <summary>
        /// Executes a test connection in current data source.
        /// </summary>
        /// <returns><c>true</c> if server respond, otherwise <c>false</c></returns>
        public bool TestConnection()
        {
            try
            {
                if (Connection.State == ConnectionState.Closed)
                    Connection.Open();

                return Connection.State == ConnectionState.Open;
            }
            catch (Exception) { return false; }
            finally
            {
                if (Connection.State.Equals(ConnectionState.Open))
                    Connection.Close();
            }
        }

        private bool prepared = false;
    }
}
