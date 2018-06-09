
using Remnant.DataGateway.Interfaces;
using System;
using System.Data;
using Remnant.DataGateway.SqlServer.Schema;
using System.Data.SqlClient;

namespace Remnant.DataGateway.SqlServer
{
	/// <summary>
	/// SQl Server database registration
	/// </summary>
	public class SqlServerRegistration : IDbRegistration
  {
    private readonly string _name;
    private readonly string _connectionString;
    private Type _connectionType;
    private readonly string _softConnectionType;
    private readonly IsolationLevel _isolationLevel;
    private SqlSchema _schema;
    private IDbManager _dbManager;

    public SqlServerRegistration(string name, string connectionString, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
      Shield.AgainstNullOrEmpty(name, "name").Raise();
      Shield.AgainstNullOrEmpty(connectionString, "ConnectionString").Raise();

      _name = name;
      _connectionString = connectionString;
      _isolationLevel = isolationLevel;
      _connectionType = typeof(SqlConnection);
      _softConnectionType = typeof(SqlConnection).FullName;
    }

    public string ConnectionString => _connectionString;

    public Type ConnectionType
    {
      get { return _connectionType; }
      set { _connectionType = value; }
    }

    public IsolationLevel IsolationLevel => _isolationLevel;

    public string Name => _name;

    public IDbSchema Schema => _schema;

    public string SoftConnectionType => _softConnectionType;

    public IDbManager DbManager
    {
      get { return _dbManager; }
      set
      {
        _dbManager = value;
        _schema = new SqlSchema(_dbManager);
      }
    }
      
    
  }
}
