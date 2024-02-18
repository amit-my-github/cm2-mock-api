using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;

namespace CM.WebAPI
{
    /// <summary>
    /// Database helper class
    /// </summary>
    /// <seealso cref="IDisposable" />
    public class MsSqlClient : IDisposable
    {
        /// <summary>
        /// The factory
        /// </summary>
        private readonly DbProviderFactory? _factory;

        /// <summary>
        /// The connection
        /// </summary>
        private readonly DbConnection? _connection;

        /// <summary>
        /// The command
        /// </summary>
        private readonly DbCommand? _command;

        /// <summary>
        /// The command timeout
        /// </summary>
        private const int CommandTimeout = 600;

        /// <summary>
        /// The disposed
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="MsSqlClient"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public MsSqlClient(string connectionString)
            : this(connectionString, SqlClientFactory.Instance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MsSqlClient"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="providerFactory">The provider factory.</param>
        public MsSqlClient(string connectionString, DbProviderFactory providerFactory)
        {
            if (providerFactory != null)
            {
                _factory = providerFactory;
                _connection = _factory.CreateConnection();
                _command = _factory.CreateCommand();
                if (_connection != null)
                {
                    _connection.ConnectionString = connectionString;
                    if (_command != null)
                    {
                        _command.Connection = _connection;
                    }
                }
            }
        }

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns>Returns index of the DbParameter object in the collection</returns>
        public int AddParameter(string name, object? value)
        {
            var parameter = _factory?.CreateParameter();
            if (parameter == null || name == null) return -1;
            parameter.ParameterName = name;
            parameter.Value = value;
            if (_command != null)
            { return _command.Parameters.Add(parameter); }
            return -1;
        }

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="dataType">Type of the data.</param>
        /// <returns>Returns index of the DbParameter object in the collection</returns>
        public int AddParameter(string name, object? value, DbType dataType)
        {
            var parameter = _factory?.CreateParameter();
            if (parameter == null || name == null) return -1;
            parameter.ParameterName = name;
            parameter.Value = value;
            parameter.DbType = dataType;
            if (_command != null)
            { return _command.Parameters.Add(parameter); }
            return -1;

        }

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Returns index of the DbParameter object in the collection</returns>
        public int AddParameter(DbParameter parameter)
        {
            if (_command != null && parameter != null)
            {
                return _command.Parameters.Add(parameter);
            }
            return -1;

        }

        /// <summary>
        /// Adds the parameters.
        /// </summary>
        /// <typeparam name="T">Type of value</typeparam>
        /// <param name="prefix">The prefix.</param>
        /// <param name="values">The values.</param>
        /// <returns>Returns comma separated parameter</returns>
        public string AddParameters<T>(string prefix, ICollection<T> values)
        {
            var parameterNames = new StringBuilder();
            if (values != null)
            {
                for (var i = 0; i <= values.Count - 1; i++)
                {
                    var parameter = "@" + prefix + i.ToString(CultureInfo.InvariantCulture);
                    AddParameter(parameter, values.ElementAt(i));
                    parameterNames.Append(parameter + ",");
                }
            }
            return parameterNames.ToString().TrimEnd(",".ToCharArray());
        }

        /// <summary>
        /// Adds the parameters.
        /// </summary>
        /// <typeparam name="T">Type of value</typeparam>
        /// <param name="prefix">The prefix.</param>
        /// <param name="values">The values.</param>
        /// <param name="dataType">Type of the data.</param>
        /// <returns>Returns comma separated parameter</returns>
        public string AddParameters<T>(string prefix, ICollection<T> values, DbType dataType)
        {
            var parameterNames = new StringBuilder();
            if (values != null)
            {
                for (var i = 0; i <= values.Count - 1; i++)
                {
                    var parameter = "@" + prefix + i.ToString(CultureInfo.InvariantCulture);
                    AddParameter(parameter, values.ElementAt(i), dataType);
                    parameterNames.Append(parameter + ",");
                }
            }
            return parameterNames.ToString().TrimEnd(",".ToCharArray());
        }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        public void BeginTransaction()
        {
            if (_connection != null && _command != null)
            {
                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                }
                _command.Transaction = _connection.BeginTransaction();
            }

        }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public void CommitTransaction()
        {
            if (_connection != null && _command != null && _command.Transaction != null)
            {
                _command.Transaction.Commit();
                _connection.Close();
            }

        }

        /// <summary>
        /// Rollbacks the transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            if (_connection != null && _command != null && _command.Transaction != null)
            {
                _command.Transaction.Rollback();
                _connection.Close();
            }

        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>Returns count of affected records.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "Support of parameterize query is exist.")]
        public int ExecuteNonQuery(string query, CommandType commandType)
        {
            if (_command != null && _connection != null)
            {
                _command.CommandText = query;
                _command.CommandType = commandType;
                _command.CommandTimeout = CommandTimeout;
                int count;
                try
                {
                    if (_connection.State == ConnectionState.Closed)
                    {
                        _connection.Open();
                    }
                    count = _command.ExecuteNonQuery();
                }
                finally
                {
                    _command.Parameters.Clear();
                    if (_connection.State == ConnectionState.Open)
                    {
                        _connection.Close();
                    }
                }
                return count;
            }
            return 0;

        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>Returns count of affected records.</returns>
        public int ExecuteNonQuery(string query)
        {
            return ExecuteNonQuery(query, CommandType.Text);
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>Returns result of query</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "Support of parameterize query is exist.")]
        public object ExecuteScalar(string query, CommandType commandType)
        {

            _command.CommandText = query;
            _command.CommandType = commandType;
            _command.CommandTimeout = CommandTimeout;
            object output;
            try
            {
                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                }
                output = _command.ExecuteScalar();
            }
            finally
            {
                _command.Parameters.Clear();
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
            return output;

        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>Returns result of query</returns>
        public object ExecuteScalar(string query)
        {
            return ExecuteScalar(query, CommandType.Text);
        }

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="closeConnectionOnExit">Close connection on exit.</param>
        /// <returns>Returns reader</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "Support of parameterize query is exist.")]
        public DbDataReader ExecuteReader(string query, CommandType commandType, bool closeConnectionOnExit)
        {
            _command.CommandText = query;
            _command.CommandType = commandType;
            _command.CommandTimeout = CommandTimeout;
            DbDataReader reader;
            try
            {
                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                }
                reader = closeConnectionOnExit ? _command.ExecuteReader(CommandBehavior.CloseConnection) : _command.ExecuteReader();
            }
            finally
            {
                _command.Parameters.Clear();
            }
            return reader;
        }

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>Returns reader</returns>
        public DbDataReader ExecuteReader(string query)
        {
            return ExecuteReader(query, CommandType.Text, true);
        }

        /// <summary>
        /// Executes the data set.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>Returns data set</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "Support of parameterize query is exist.")]
        public DataSet ExecuteDataSet(string query, CommandType commandType)
        {
            _command.CommandText = query;
            _command.CommandType = commandType;
            _command.CommandTimeout = CommandTimeout;
            var dataSet = new DataSet
            {
                Locale = CultureInfo.InvariantCulture
            };
            var adapter = _factory.CreateDataAdapter();
            if (adapter != null)
            {
                adapter.SelectCommand = _command;
                try
                {
                    adapter.Fill(dataSet);
                }
                finally
                {
                    _command.Parameters.Clear();
                    if (_connection.State == ConnectionState.Open)
                    {
                        _connection.Close();
                    }
                }
            }
            return dataSet;
        }

        /// <summary>
        /// Executes the data set.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>Returns data set</returns>
        public DataSet ExecuteDataSet(string query)
        {
            return ExecuteDataSet(query, CommandType.Text);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_connection.State == ConnectionState.Open)
                    {
                        _connection.Close();
                    }
                    _connection.Dispose();
                    _command.Dispose();
                }
                _disposed = true;
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="MsSqlClient"/> class.
        /// </summary>
        ~MsSqlClient()
        {
            Dispose(false);
        }

    }
}
