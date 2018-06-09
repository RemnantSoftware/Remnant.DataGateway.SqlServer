using System;
using Remnant.DataGateway.Attributes;
using Remnant.DataGateway.Interfaces;
using System.Collections.Generic;
using FastMember;
using System.Linq;
using Remnant.DataGateway.Core;

namespace Remnant.DataGateway.SqlServer.Schema
{
	[DbName(Name = "sys.all_objects")]
	public class SysObject : DbTableEntity<SysObject>
  {

    [DbField]
		public string Name { get; set; }

		[DbField(Name = "object_id")]
		public int ObjectId { get; set; }

		[DbField(Name = "principal_id")]
		public int? PrincipalId { get; set; }

		[DbField(Name = "schema_id")]
		public int SchemaId { get; set; }

		[DbField(Name = "parent_object_id")]
		public int ParentObjectId { get; set; }

		[DbField]
		public string Type { get; set; }

		[DbField(Name = "type_desc")]
		public string TypeDescription { get; set; }

		[DbField(Name = "create_date")]
		public DateTime CreateDate { get; set; }

		[DbField(Name = "modify_date")]
		public DateTime ModifiedDate { get; set; }

		[DbField(Name = "is_ms_shipped")]
		public bool IsMsShipped { get; set; }

		[DbField(Name = "is_published")]
		public bool IsPublished { get; set; }

		[DbField(Name = "is_schema_published")]
		public bool IsSchemaPublished { get; set; }

	}
}
