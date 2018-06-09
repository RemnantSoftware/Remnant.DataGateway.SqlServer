using Remnant.Core;
using Remnant.Core.Attributes;
using Remnant.Core.Extensions;
using Remnant.DataGateway.Attributes;
using Remnant.DataGateway.Core;
using Remnant.DataGateway.Interfaces;
using FastMember;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Remnant.DataGateway.SqlServer.Schema
{
	[DbName(Name = "sys.parameters")]
	public class SysParameter : DbTableEntity<SysParameter>
  {
    [DbField]
		public string Name { get; set; }

		[DbField(Name = "object_id")]
		public int ObjectId { get; set; }

		[DbField(Name = "parameter_id")]
		public int ParameterId { get; set; }

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

		[DbField(Name = "is_output")]
		public bool IsOutput { get; set; }

		[DbField(Name = "is_cursor_ref")]
		public bool IsCursorRef { get; set; }

		[DbField(Name = "has_default_value")]
		public bool HasDefaultValue { get; set; }

		[DbField(Name = "default_value")]
		public object DefaultValue { get; set; }

		[DbField(Name = "is_xml_document")]
		public bool IsXmlDocument { get; set; }

		[DbField(Name = "xml_collection_id")]
		public int XmlCollectionId { get; set; }

		[DbField(Name = "is_readonly")]
		public bool IsReadOnly { get; set; }

		#region Gateway specific

		[RuntimeDbField]
		public string DataType { get; set; }		

		public string NetName
		{
			get { return Name.Replace("@", string.Empty).ToCase(Case.Pascal); }
		}

		public MetaAttribute Meta { get; set; }

		#endregion
	}
}
