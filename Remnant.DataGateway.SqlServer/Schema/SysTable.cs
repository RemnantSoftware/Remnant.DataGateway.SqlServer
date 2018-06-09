using System.Collections.Generic;
using Remnant.Core.Attributes;
using FastMember;
using System.Linq;

namespace Remnant.DataGateway.SqlServer.Schema
{
	public class SysTable : SysObject
	{
    public List<SysColumn> Columns { get; set; }

		public MetaAttribute Meta { get; set; }		
	}
}
