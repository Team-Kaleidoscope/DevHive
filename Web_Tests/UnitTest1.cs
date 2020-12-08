using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Classes;

namespace Web_Tests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
			Test test = new Test();
			test.TestMe();
		}
	}
}
