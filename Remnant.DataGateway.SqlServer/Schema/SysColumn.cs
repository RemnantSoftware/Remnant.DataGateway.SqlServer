
using Remnant.Core.Attributes;
using Remnant.DataGateway.Attributes;
using Remnant.DataGateway.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Remnant.DataGateway.SqlServer.Schema
{
	[DbName(Name = "sys.columns")]
  public class SysColumn : DbTableEntity<SysColumn>
  {
    public SysColumn()
    {
      Constraints = new List<SysConstraint>();
    }

    [DbField]
    public string Name { get; set; }

    [DbKey(Name = "object_id")]
    public int ObjectId { get; set; }

    [DbField(Name = "column_id")]
    public int ColumnId { get; set; }

    [DbField(Name = "system_type_id")]
    public byte SystemTypeId { get; set; }

    [DbField(Name = "user_type_id")]
    public int UserTypeId { get; set; }

    [DbField(Name = "max_length")]
    public Int16 MaxLength { get; set; }

    [DbField]
    public byte Precision { get; set; }

    [DbField]
    public byte Scale { get; set; }

    [DbField(Name = "collation_name")]
    public string CollationName { get; set; }

    [DbField(Name = "is_nullable")]
    public bool IsNullable { get; set; }

    [DbField(Name = "is_ansi_padded")]
    public bool IsAnsiPadded { get; set; }

    [DbField(Name = "is_rowguidcol")]
    public bool IsRowGuidCol { get; set; }

    [DbField(Name = "is_identity")]
    public bool IsIdentity { get; set; }

    [DbField(Name = "is_computed")]
    public bool IsComputed { get; set; }

    [DbField(Name = "is_filestream")]
    public bool IsFileStream { get; set; }

    [DbField(Name = "is_replicated")]
    public bool IsReplicated { get; set; }

    [DbField(Name = "is_non_sql_subscribed")]
    public bool IsNonSqlSubscribed { get; set; }

    [DbField(Name = "is_merge_published")]
    public bool IsMergePublished { get; set; }

    [DbField(Name = "is_dts_replicated")]
    public bool IsDtsReplicated { get; set; }

    [DbField(Name = "is_xml_document")]
    public bool IsXmlDocument { get; set; }

    [DbField(Name = "xml_collection_id")]
    public int XmlCollectionId { get; set; }

    [DbField(Name = "default_object_id")]
    public int DefaultObjectId { get; set; }

    [DbField(Name = "rule_object_id")]
    public int RuleObjectId { get; set; }

    [DbField(Name = "is_sparse")]
    public bool IsSparse { get; set; }

    [DbField(Name = "is_column_set")]
    public bool IsColumnSet { get; set; }

    #region Data Gateway specific

    [RuntimeDbField]
    public string DataType { get; set; }

    [RuntimeDbField]
    public string KeyType { get; set; }

    [RuntimeDbField]
    public int Length { get; set; }

    public MetaAttribute Meta { get; set; }

    public List<SysConstraint> Constraints { get; set; }

    public bool IsPrimaryKey
    {
      get
      {
        return Constraints.FirstOrDefault(c => c.EnumType() == ConstraintType.PrimaryKey) != null;
      }
    }

    public bool IsForeignKey
    {
      get
      {
        return Constraints.FirstOrDefault(c => c.EnumType() == ConstraintType.ForeignKey) != null;
      }
    }

		public bool IsUnique
		{
			get
			{
				return Constraints.FirstOrDefault(c => c.EnumType() == ConstraintType.Unique) != null;
			}
		}

		public string DefaultPrimaryKey
    {
      get
      {
        var value = Constraints.FirstOrDefault(c => c.EnumType() == ConstraintType.PrimaryKey);
        return string.IsNullOrEmpty(value.Default) ? string.Empty : value.Default;
      }
    }

    #endregion
  }
}
