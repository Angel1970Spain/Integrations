using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SignatureIntegration
{

	//[Guid("90D36CC5-F813-45F3-98D0-8B04CF5265E3"), ComVisible(true)]
	public enum AuthMethod
	{	
		NONE = 0,
		PASS = 1,
		WIN = 2,
		FEDERATED = 3
	}
}
