using System;
using System.Text;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using Remnant.DataGateway.Core;
using Remnant.Core.Extensions;
using Remnant.Core.Services;
using Remnant.Core.Attributes;
using Remnant.DataGateway.Interfaces;

namespace Remnant.DataGateway.SqlServer.Schema
{
	/// <summary>
	/// Sql Server schema
	/// </summary>
	public class SqlSchema : DbSchema<SqlServerDataType>
  {
    #region Fields

    private const string _returnNumberKeySyntax = @"; select scope_identity();";
    private const string _concatSymbol = "+";
    private const string _castToString = @"cast ({0} as nvarchar(max))";
    private const string _topFormat = @"top {0}";
    private const string _withFormat = @"with {0} as";
    private const string _renameTable = "exec sp_rename '{0}', '{1}';";
    private const string _renameColumn = "exec sp_rename '{0}.{1}', '{2}', 'COLUMN';";
    private const string _slectForUpdate = " with (updlock) ";

    #endregion

    #region Constructors and Finalisors

    public SqlSchema(IDbManager dbManager) : base(dbManager)
    {
      DatabaseType = DatabaseType.SqlServer;
      IdentifierPostEscapeChar = ']';
      IdentifierPreEscapeChar = '[';
      BindVariableSymbol = "@";
      AliasFormat = " as {0}";
      AutoSequenceNumberFormat = "identity(1,1)";
      AutoSequenceUniqueIdFormat = "(newsequentialid())";
      MinDateTime = new DateTime(1753, 1, 1, 12, 0, 0);
      MaxDateTime = new DateTime(9999, 12, 31, 23, 59, 59);

      //RegisterParser<CreateTableParser<SqlServerDataType>, CreateTableStatement<SqlServerDataType>>();
      //RegisterParser<AlterTableParser<SqlServerDataType>, AlterTableStatement<SqlServerDataType>>();
    }

    #endregion

    #region Protected Members

    protected override void RegisterDataTypeMappings()
    {
      _mapDataTypes.Add(SqlServerDataType.Nvarchar, typeof(string));
      _mapDataTypes.Add(SqlServerDataType.Ntext, typeof(string));
      _mapDataTypes.Add(SqlServerDataType.Text, typeof(string));
      _mapDataTypes.Add(SqlServerDataType.Varchar, typeof(string));
      _mapDataTypes.Add(SqlServerDataType.Char, typeof(string));
      _mapDataTypes.Add(SqlServerDataType.Nchar, typeof(string));
      _mapDataTypes.Add(SqlServerDataType.Bit, typeof(bool));
      _mapDataTypes.Add(SqlServerDataType.Uniqueindentifier, typeof(Guid));
      _mapDataTypes.Add(SqlServerDataType.Xml, typeof(XDocument));
      _mapDataTypes.Add(SqlServerDataType.Binary, typeof(byte[]));
      _mapDataTypes.Add(SqlServerDataType.Filestream, typeof(byte[]));
      _mapDataTypes.Add(SqlServerDataType.Image, typeof(byte[]));
      _mapDataTypes.Add(SqlServerDataType.Rowversion, typeof(byte[]));
      _mapDataTypes.Add(SqlServerDataType.Timestamp, typeof(byte[]));
      _mapDataTypes.Add(SqlServerDataType.Varbinary, typeof(byte[]));
      _mapDataTypes.Add(SqlServerDataType.Bigint, typeof(Int64));
      _mapDataTypes.Add(SqlServerDataType.Int, typeof(Int32));
      _mapDataTypes.Add(SqlServerDataType.Tinyint, typeof(Byte));
      _mapDataTypes.Add(SqlServerDataType.Smallint, typeof(Int16));
      _mapDataTypes.Add(SqlServerDataType.Date, typeof(DateTime));
      _mapDataTypes.Add(SqlServerDataType.Datetime, typeof(DateTime));
      _mapDataTypes.Add(SqlServerDataType.Datetime2, typeof(DateTime));
      _mapDataTypes.Add(SqlServerDataType.Smalldatetime, typeof(DateTime));
      _mapDataTypes.Add(SqlServerDataType.Time, typeof(TimeSpan));
      _mapDataTypes.Add(SqlServerDataType.Datetimeoffset, typeof(DateTimeOffset));
      _mapDataTypes.Add(SqlServerDataType.Decimal, typeof(decimal));
      _mapDataTypes.Add(SqlServerDataType.Money, typeof(decimal));
      _mapDataTypes.Add(SqlServerDataType.Numeric, typeof(decimal));
      _mapDataTypes.Add(SqlServerDataType.Smallmoney, typeof(decimal));
      _mapDataTypes.Add(SqlServerDataType.Real, typeof(Single));
      _mapDataTypes.Add(SqlServerDataType.Float, typeof(double));
      _mapDataTypes.Add(SqlServerDataType.Sql_variant, typeof(Object));
      _mapDataTypes.Add(SqlServerDataType.Unknown, typeof(Object));
    }

    protected static MetaAttribute CreateMetaFromExtProps(List<SysExtendedProperty> extendedProperties)
    {
      var meta = new MetaAttribute();
      foreach (var extendedProperty in extendedProperties)
      {
        var propInfo = ReflectionService.GetProperty(meta, extendedProperty.Name, true);
        if (propInfo != null)
        {
          var value = Convert.ChangeType(extendedProperty.Value, propInfo.PropertyType.UnderlyingSystemType);
          propInfo.SetValue(meta, value, null);
        }
      }
      return meta;
    }

    #endregion

    #region Public Members

    public override string SelectForUpdateFormat { get { return _slectForUpdate; } }

    public override string CteWithFormat(ISelectWithStatement statement)
    {
      return string.Format(_withFormat, statement.ExpressionName);
    }

    public override void ExpressionFormat(ISelectStatement statement, ISqlExpression expression)
    {
      if (expression.Type == SqlSelectExpression.Top)
      {
        expression.Value = string.Format(_topFormat, expression.Value);
        expression.ParseWhere = SqlSelectExpressionParse.BeforeColumns;
      }
    }

    public override string ConcatFormat(List<string> columns, string delimiter = null)
    {
      delimiter = delimiter == null
              ? "'\'\'"
              : delimiter.ToSingleQuoted();

      var sb = new StringBuilder();
      for (int i = 0; i < columns.Count; i++)
      {
        var column = columns[i];
        sb.Append(string.Format(_castToString, column));

        if (i < columns.Count - 1)
        {
          if (!string.IsNullOrEmpty(delimiter))
            sb.Append(_concatSymbol + delimiter);
          sb.Append(_concatSymbol);
        }
      }

      return sb.ToString();
    }

    public override string InsertReturnKeySyntax(string autoSequence, string statement, string primaryKeyColumnName = null)
    { 
      if (autoSequence == AutoSequenceUniqueIdFormat)
      {
        statement = "declare @pkTable Table (pk uniqueIdentifier) " + statement;

        var valuesPos = statement.IndexOf(" Values ");

        statement = statement.Insert(valuesPos, $" Output Inserted.{primaryKeyColumnName} INTO @pkTable ");
        statement = statement + Environment.NewLine + "select * from @pkTable";
      }
      else
      {
        statement = statement + Environment.NewLine + _returnNumberKeySyntax;
      }
       
      return statement;
    }

    public virtual IDbAdmin<SqlServerDataType> Admin(IDbContext context = null)
    {
      //return new DbAdmin<SqlServerDataType>(_dbManager, context);
      throw new NotImplementedException();
    }

    public List<SysConstraint> FetchColumnConstraints(string tableName, string columnName)
    {
      return _dbManager.Sql()
        .DefineBindParameter("pTableName", tableName)
        .DefineBindParameter("pColumnName", columnName)
        .Select()
				.Column("tc.constraint_name", "Name")
        .Column("tc.constraint_type", "Type")
        .Column("coldef.column_default", "Default")
				.Column("sc2.table_name", "TableNameReference")
				.Column("sc2.column_name", "ColumnNameReference")
				.From
        .Table("information_schema.KEY_COLUMN_USAGE", "sc")
        .InnerJoin("information_schema.TABLE_CONSTRAINTS", "tc")
        .On("tc.constraint_name", SqlOperand.Equal, "sc.constraint_name")
        .LeftJoin("information_schema.columns", "coldef")
        .On("coldef.table_name", SqlOperand.Equal, "pTableName")
        .And("coldef.column_name", SqlOperand.Equal, "pColumnName")
				.LeftJoin("information_schema.REFERENTIAL_CONSTRAINTS", "rc")
				.On("rc.constraint_name", SqlOperand.Equal, "sc.constraint_name")
				.LeftJoin("information_schema.KEY_COLUMN_USAGE", "sc2")
				.On("sc2.constraint_name", SqlOperand.Equal, "rc.unique_constraint_name")
				.Where
        .Criteria("sc.table_name", SqlOperand.Equal, "pTableName")
        .Criteria("sc.column_name", SqlOperand.Equal, "pColumnName")
        .Execute<SysConstraint>();
    }

    public List<SysColumn> FetchTableColumns(SysTable table)
    {
      return _dbManager.Sql()
        .DefineBindParameter("pTableName", table.Name)
        .DefineBindParameter("pSysname", "sysname")
        .Select()
        .AllColumns("sc")
        .Column("t.name", "DataType")
        .From
        .Table<SysColumn>("sc")
        .InnerJoin("sys.types", "t")
        .On("t.system_type_id", SqlOperand.Equal, "sc.systemTypeId")
        .And("t.Name", SqlOperand.NotEqual, "pSysname")
        .Where
        .Criteria("ObjectId", SqlOperand.Equal, table.ObjectId)
        .OrderBy
        .Column("ColumnId")
        .Execute<SysColumn>();
    }

    public MetaAttribute FetchObjectExtProperties(int objectId)
    {
      var extProps = _dbManager.Sql()
        .Select()
        .AllColumns()
        .From
        .Table<SysExtendedProperty>()
        .Where
        .Criteria("MajorId", SqlOperand.Equal, objectId)
        .Criteria("MinorId", SqlOperand.Equal, 0)
        .Execute<SysExtendedProperty>();

      return CreateMetaFromExtProps(extProps);
    }

    public MetaAttribute FetchColumnExtProperties(int tableId, SysColumn sysColumn)
    {
      var extProps = _dbManager.Sql()
        .Select()
        .AllColumns()
        .From
        .Table<SysExtendedProperty>()
        .Where
        .Criteria("MajorId", SqlOperand.Equal, tableId)
        .Criteria("MinorId", SqlOperand.Equal, sysColumn.ColumnId)
        .Execute<SysExtendedProperty>();

      // add default info
      //if (GetNetDataType(sysColumn.DataType, false) == typeof(string))
      if (sysColumn.MaxLength != 0)
        extProps.Add(new SysExtendedProperty { Name = "Length", Value = sysColumn.MaxLength });
      if (sysColumn.IsNullable)
        extProps.Add(new SysExtendedProperty { Name = "IsNullable", Value = true });
      if (sysColumn.IsPrimaryKey || sysColumn.IsForeignKey)
      {
        extProps.Add(new SysExtendedProperty { Name = "IsVisible", Value = false });
        extProps.Add(new SysExtendedProperty { Name = "IsRequired", Value = true });
      }

      return CreateMetaFromExtProps(extProps);
    }

    public List<SysObject> FetchAllObjects()
    {
      return _dbManager.Sql()
        .Select()
        .AllColumns<SysObject>("SysObjects")
        .From
        .Table<SysObject>("SysObjects")
        .Execute<SysObject>();
    }

    public List<SysTable> FetchUserTables(string[] includeTables, params string[] ignoreTables)
    {
      var sql = _dbManager.Sql()
        .Select()
        .AllColumns<SysTable>("SysObjects")
        .From
        .Table<SysTable>("SysObjects")
        .Where
        .Criteria("Type", SqlOperand.In, new[] { SqlServerObjectType.UserTable.ToDescription(), SqlServerObjectType.View.ToDescription() })
        .And
        .Criteria("IsMsShipped", SqlOperand.Equal, false);

      foreach (var includeTable in includeTables)
        sql.Criteria("Name", SqlOperand.Like, includeTable);

      foreach (var ignoreTable in ignoreTables)
        sql.Criteria("Name", SqlOperand.NotLike, ignoreTable);

      var tables = sql.Execute<SysTable>();

      tables.ForEach(table =>
        {
          table.Meta = FetchObjectExtProperties(table.ObjectId);
          table.Columns = FetchTableColumns(table);

          table.Columns.ForEach(column =>
          {
            column.Meta = FetchColumnExtProperties(table.ObjectId, column);
            column.Constraints = FetchColumnConstraints(table.Name, column.Name);
          });
        });
      return tables;
    }

    public List<SysParameter> FetchStoredProcParameters(int sprocId)
    {
      return _dbManager.Sql()
        .Select()
        .AllColumns("SysParameters")
        .Column("Types.name", "DataType")
        .From
        .Table<SysParameter>("SysParameters")
        .LeftJoin("sys.types", "Types")
        .On("Types.system_type_id", SqlOperand.Equal, "SysParameters.SystemTypeId")
        .Where
        .Criteria("ObjectId", SqlOperand.Equal, sprocId)
        .Criteria("Types.Name", SqlOperand.NotEqual, "sysname")
        .OrderBy
        .Column("ParameterId")
        .Execute<SysParameter>();
    }

    public List<SysProcedure> FetchUserStoredProcs(params string[] ignoreStoredProcs)
    {
      var sql = _dbManager.Sql()
        .Select()
        .AllColumns<SysProcedure>("SysObjects")
        .From
        .Table<SysProcedure>("SysObjects")
        .Where
        .Criteria("Type", SqlOperand.Equal, SqlServerObjectType.StoredProcedure.ToDescription())
        .And
        .Criteria("IsMsShipped", SqlOperand.Equal, false);

      foreach (var ignoreStoredProc in ignoreStoredProcs)
        sql.Criteria("Name", SqlOperand.NotLike, ignoreStoredProc);

      var sprocs = sql.Execute<SysProcedure>();
      sprocs.ForEach(sproc => sproc.Parameters = FetchStoredProcParameters(sproc.ObjectId));
      sprocs.ForEach(sproc => sproc.Meta = FetchObjectExtProperties(sproc.ObjectId));
      return sprocs;
    }

    #endregion

    #region Admin

    public override string AdminRenameTable(string oldName, string newName)
    {
      return string.Format(_renameTable, oldName, newName);
    }

    public override string AdminRenameColumn(string tableName, string oldName, string newName)
    {
      return string.Format(_renameColumn, tableName, oldName, newName);
    }

    #endregion

  }
}
