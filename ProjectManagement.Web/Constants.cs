using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagement.Web
{
	public class Constants
	{
		// Should match status database table
		public enum Status
		{
			Live = 1,
			Spec = 2,
			Dead = 3,
			ConstructedFC = 4,
			ConstructedPC = 5
		};
	}
}