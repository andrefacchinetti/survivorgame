//    HiddenVars - HiddenVarsRep - HiddenValue


using System;

namespace Leguar.HiddenVars.Internal {

	internal class HiddenValue {

//		private static int[] test_byteCountsCheck=new int[256];

		private byte[] hiddenValue;
		private int hideSeed;

		internal HiddenValue(byte[] hiddenValue, int hideSeed) {
			this.hiddenValue=hiddenValue;
			this.hideSeed=hideSeed;
		}

		internal byte[] getHiddenValue() {
			return hiddenValue;
		}

		internal int getHideSeed() {
			return hideSeed;
		}

		internal void destroy(Random rnd) {
			if (hiddenValue!=null) {
				rnd.NextBytes(hiddenValue);
				hiddenValue=null;
			}
			hideSeed=0;
		}

/*
		internal static void test_printDistribution() {
			System.Text.StringBuilder sb=new System.Text.StringBuilder();
			for (int n=0; n<256; n++) {
				sb.Append(test_byteCountsCheck[n]);
				if (n<test_byteCountsCheck.Length-1) {
					sb.Append(',');
				}
			}
			UnityEngine.Debug.Log("D: "+sb.ToString());
		}
//*/

	}

}
