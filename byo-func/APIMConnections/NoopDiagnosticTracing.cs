using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSHA.ApiConnections
{
	public class NoopDiagnosticTracing : IDiagnosticsTracing
	{
		public void Critical(string message)
		{

		}

		public void Error(string message)
		{

		}

		public void Informational(string message)
		{

		}

		public void Verbose(string message)
		{

		}

		public void Warning(string message)
		{

		}
	}
}
