using System.Collections.Generic;
using Remnant.Core.Attributes;
using Remnant.DataGateway.Attributes;
using FastMember;
using System.Linq;

namespace Remnant.DataGateway.SqlServer.Schema
{
	public class SysProcedure : SysObject
	{
    public List<SysParameter> Parameters { get; set; }

		public MetaAttribute Meta { get; set; }		
	}
}
