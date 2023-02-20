//    HiddenVars - HiddenVarsRep


using System;
using System.Collections.Generic;

namespace Leguar.HiddenVars.Internal {
	
	internal class HiddenVarsRep {

		private Dictionary<string,HiddenValue> hiddenVars;
		private Random pseudoRandomKeyGen;

		internal HiddenVarsRep() {
			hiddenVars=new Dictionary<string,HiddenValue>();
			pseudoRandomKeyGen=new Random();
		}

		internal int getCount() {
			return hiddenVars.Count;
		}

		internal void putByteArray(string key, byte[] value, bool copy) {
			clearHiddenValue(key); // Clear possible previous value with same key from memory before losing reference
			int hideSeed=pseudoRandomKeyGen.Next();
			hiddenVars[key]=new HiddenValue((copy?xorCopy(value,hideSeed):xor(value,hideSeed)),hideSeed);
		}

		internal byte[] getByteArray(string key) {
			HiddenValue hiddenValue;
			if (!hiddenVars.TryGetValue(key,out hiddenValue)) {
				return null;
			}
			return xorCopy(hiddenValue.getHiddenValue(),hiddenValue.getHideSeed());
		}

		internal bool containsKey(string key) {
			return hiddenVars.ContainsKey(key);
		}

		internal string[] getKeys() {
			string[] keys=new string[hiddenVars.Count];
			hiddenVars.Keys.CopyTo(keys,0);
			return keys;
		}

		internal bool remove(string key) {
			clearHiddenValue(key); // Clear value from memory before removing reference
			return hiddenVars.Remove(key);
		}

		internal void clear() {
			Random trashRnd=new Random();
			foreach (HiddenValue hiddenValue in hiddenVars.Values) {
				hiddenValue.destroy(trashRnd);
			}
			hiddenVars.Clear();
		}

		internal void destroy() {
			if (hiddenVars!=null) {
				clear();
				hiddenVars=null;
			}
			pseudoRandomKeyGen=null;
		}

		private void clearHiddenValue(string key) {
			HiddenValue hiddenValue;
			if (hiddenVars.TryGetValue(key,out hiddenValue)) {
				hiddenValue.destroy(new Random());
			}
		}

		private byte[] xor(byte[] bytes, int hideSeed) {
			Random tmpRnd=new Random(hideSeed);
			for (int n=0; n<bytes.Length; n++) {
				bytes[n]=(byte)(bytes[n]^tmpRnd.Next(256));
			}
			return bytes;
		}
		
		private byte[] xorCopy(byte[] bytes, int hideSeed) {
			byte[] copy=new byte[bytes.Length];
			Random tmpRnd=new Random(hideSeed);
			for (int n=0; n<bytes.Length; n++) {
				copy[n]=(byte)(bytes[n]^tmpRnd.Next(256));
			}
			return copy;
		}

/*
		public static string test_byteArrayAsStringForDebug(byte[] bytes) {
			System.Text.StringBuilder sb=new System.Text.StringBuilder();
			for (int n=0; n<bytes.Length; n++) {
				sb.Append(bytes[n]);
				if (n<bytes.Length-1) {
					sb.Append(',');
				}
			}
			return sb.ToString();
		}
//*/

	}

}
