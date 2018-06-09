using Remnant.Core.Extensions;
using Remnant.DataGateway.Attributes;
using Remnant.DataGateway.Core;

namespace Remnant.DataGateway.SqlServer.Schema
{
  [DbName(Name = "information_schema.KEY_COLUMN_USAGE")]
  public class SysConstraint : DbTableEntity<SysConstraint>
  {
    [DbField(Name = "constraint_name")]
    public string Name { get; set; }

    [DbField(Name = "constraint_type")]
    public string Type { get; set; }

    public string Default { get; set; }

		public string TableNameReference { get; set; }

		public string ColumnNameReference { get; set; }

    public ConstraintType EnumType()
    {
      if (Type == ConstraintType.Check.ToDescription())
          return ConstraintType.Check;

      if (Type == ConstraintType.Unique.ToDescription())
        return ConstraintType.Unique;

      if (Type == ConstraintType.PrimaryKey.ToDescription())
        return ConstraintType.PrimaryKey;

      if (Type == ConstraintType.ForeignKey.ToDescription())
        return ConstraintType.ForeignKey;

      return ConstraintType.Unknown;
    }
  }
}
