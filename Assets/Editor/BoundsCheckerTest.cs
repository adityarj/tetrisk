using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class BoundsCheckerTest {

	[Test]
	public void checkValidBoundsTotalTest() {
		double[] bounds = new double[2];
		bounds[0] = 0;
		bounds[1] = 10;
		Assert.IsTrue(BoundsChecker.checkValidBoundsTotal(2,bounds));  
	}


}
