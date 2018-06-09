using System;
using System.ComponentModel;

namespace Remnant.DataGateway.SqlServer
{
	[Flags]
	public enum SqlServerObjectType
	{
		[Description("P")]
		StoredProcedure = 1,
		[Description("X")]
		ExtendedStoredProcedure = 2,
		[Description("V")]
		View = 4,
		[Description("TF")]
		TableValuedFunction = 8,
		[Description("IF")]
		InlineTableValuedFunction = 16,
		[Description("FN")]
		Function = 32,
		[Description("AF")]
		AggregatedFunction = 64,
		[Description("U")]
		UserTable = 128,
		[Description("PC")]
		ClrStoredProcedure = 256,
		[Description("S")]
		SystemTable = 512
	}

	public enum SqlServerDataType
	{
		[Description("Unknown")]
		Unknown = 0,
		[Description("nvarchar")]
		Nvarchar,
		[Description("ntext")]
		Ntext,
		[Description("text")]
		Text,
		[Description("varchar")]
		Varchar,
		[Description("char")]
		Char,
		[Description("nchar")]
		Nchar,
		[Description("bit")]
		Bit,
		[Description("uniqueidentifier")]
		Uniqueindentifier,
		[Description("xml")]
		Xml,
		[Description("binary")]
		Binary,
		[Description("filestream")]
		Filestream,
		[Description("image")]
		Image,
		[Description("rowversion")]
		Rowversion,
		[Description("timestamp")]
		Timestamp,
		[Description("varbinary")]
		Varbinary,
		[Description("bigint")]
		Bigint,
		[Description("int")]
		Int,
		[Description("tinyint")]
		Tinyint,
		[Description("smallint")]
		Smallint,
		[Description("date")]
		Date,
		[Description("datetime")]
		Datetime,
		[Description("datetime2")]
		Datetime2,
		[Description("smalldatetime")]
		Smalldatetime,
		[Description("time")]
		Time,
		[Description("datetimeoffset")]
		Datetimeoffset,
		[Description("decimal")]
		Decimal,
		[Description("money")]
		Money,
		[Description("numeric")]
		Numeric,
		[Description("smallmoney")]
		Smallmoney,
		[Description("real")]
		Real,
		[Description("float")]
		Float,
		[Description("sql_variant")]
		Sql_variant
	}

}
