using Remnant.DataGateway.Attributes;
using Remnant.DataGateway.Core;
using Remnant.DataGateway.Interfaces;
using FastMember;
using System.Collections.Generic;
using System.Linq;

namespace Remnant.DataGateway.SqlServer.Schema
{
	[DbName(Name = "sys.extended_properties")]
	public class SysExtendedProperty : DbTableEntity<SysExtendedProperty>
  {

    [DbField]
		public byte Class { get; set; }

		[DbField(Name = "class_desc")]
		public string ClassDescription { get; set; }

		[DbField(Name = "major_id")]
		public int MajorId { get; set; }

		[DbField(Name = "minor_id")]
		public int MinorId { get; set; }

		[DbField] 
		public string Name { get; set; }

		[DbField]
		public object Value { get; set; }
	}
}
