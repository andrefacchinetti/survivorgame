//    HiddenVars


using System;
using System.Collections.Generic;
using Leguar.HiddenVars.Internal;

namespace Leguar.HiddenVars {
	
	/// <summary>
	/// Main class for HiddenVars. Also only public class.
	/// 
	/// New instance can be create with normal constructor, <code>new HiddenVars()</code> or <code>new HiddenVars(string)</code>. After that, values can be written or read, using Set and Get methods.
	///
	/// 
	/// Note that when reading lists from HiddenVars, returned plain value is copy of hidden value. Meaning that if any changes are made to plain value list,
	/// it does not change value stored in HiddenVars, until Set method is used again to overwrite old value in HiddenVars instance. For example:
	/// 
	/// <code>
	/// HiddenVars hiddenVars = new HiddenVars();
	/// hiddenVars.SetIntList("someKey",new int[]{0,1,2,3,4});
	/// IList&lt;int&gt; intListFromHV = hiddenVars.GetIntList("someKey");
	/// intListFromHV[0] = 5;
	/// Debug.Log(hiddenVars.GetIntList("someKey")[0]); // Prints out "0", previous line didn't change value stored to HiddenVars instance
	/// hiddenVars.SetIntList("someKey",intListFromHV); // Saves the modified list to HiddenVars instance
	/// Debug.Log(hiddenVars.GetIntList("someKey")[0]); // Prints out "5"
	/// </code>
	/// </summary>
	public class HiddenVars : IDisposable {

		private HiddenVarsRep rep;
		private bool disposed;

		#if UNITY_EDITOR
		private int debugId;
		#endif

		/// <summary>
		/// Initializes a new instance of the HiddenVars class.
		/// </summary>
		public HiddenVars() : this(null) {
		}

		/// <summary>
		/// Initializes a new instance of the HiddenVars class with specific debug name.
		/// </summary>
		/// <param name="nameForDebug">
		/// Debug name for this instance, visible in "HiddenVars EditorOnly RunTimeDebug" game object in the Unity Editor when application is running.
		/// There can be multiple instances of HiddenVars with same debug name. If parameter is null, default name is generated.
		/// </param>
		/// <remarks>
		/// Debug name have no effect to actual functionality of this class and have meaning only when running application in Unity Editor.
		/// </remarks>
		public HiddenVars(string nameForDebug) {
			rep=new HiddenVarsRep();
			disposed=false;
			rtdAddInstance(nameForDebug);
		}

		/// <summary>
		/// Returns count of key/value pairs in this instance.
		/// </summary>
		public int Count {
			get {
				return rep.getCount();
			}
		}

		/// <summary>
		/// Gets or sets integer value with the specified key.
		/// </summary>
		/// <remarks>
		/// Functionality is identical compared to using GetInt and SetInt methods.
		/// </remarks>
		/// <param name="key">
		/// Key of the value to set or get
		/// </param>
		/// <value>
		/// Integer value
		/// </value>
		public int this[string key] {
			set {
				SetInt(key,value);
			}
			get {
				return GetInt(key);
			}
		}

		/// <summary>
		/// Set integer value identified by key. If there's any previous value with same key, it will be replaced.
		/// </summary>
		/// <param name="key">
		/// Key of the integer value to set
		/// </param>
		/// <param name="value">
		/// Integer value to be hidden
		/// </param>
		public void SetInt(string key, int value) {
			rep.putByteArray(key,BitConverter.GetBytes(value),false);
			rtdSetValue(key,value);
		}

		/// <summary>
		/// Set integer list identified by key. If there's any previous value with same key, it will be replaced.
		/// </summary>
		/// <param name="key">
		/// Key of the integer list to set
		/// </param>
		/// <param name="values">
		/// Integer list to be hidden
		/// </param>
		public void SetIntList(string key, IList<int> values) {
			int count=values.Count;
			int[] rtdIntArray=(rtdIsInUse()?new int[count]:null);
			byte[] byteArray=new byte[count*4];
			byte[] singleIntBytes;
			for (int n=0; n<count; n++) {
				if (rtdIntArray!=null) {
					rtdIntArray[n]=values[n];
				}
				singleIntBytes=BitConverter.GetBytes(values[n]);
				for (int m=0; m<4; m++) {
					byteArray[n*4+m]=singleIntBytes[m];
				}
			}
			rep.putByteArray(key,byteArray,false);
			if (rtdIntArray!=null) {
				rtdSetValue(key,rtdIntArray);
			}
		}

		/// <summary>
		/// Set long value identified by key. If there's any previous value with same key, it will be replaced.
		/// </summary>
		/// <param name="key">
		/// Key of the long value to set
		/// </param>
		/// <param name="value">
		/// Long value to be hidden
		/// </param>
		public void SetLong(string key, long value) {
			rep.putByteArray(key,BitConverter.GetBytes(value),false);
			rtdSetValue(key,value);
		}

		/// <summary>
		/// Set long list identified by key. If there's any previous value with same key, it will be replaced.
		/// </summary>
		/// <param name="key">
		/// Key of the long list to set
		/// </param>
		/// <param name="values">
		/// Long list to be hidden
		/// </param>
		public void SetLongList(string key, IList<long> values) {
			int count=values.Count;
			long[] rtdLongArray=(rtdIsInUse()?new long[count]:null);
			byte[] byteArray=new byte[count*8];
			byte[] singleLongBytes;
			for (int n=0; n<count; n++) {
				if (rtdLongArray!=null) {
					rtdLongArray[n]=values[n];
				}
				singleLongBytes=BitConverter.GetBytes(values[n]);
				for (int m=0; m<8; m++) {
					byteArray[n*8+m]=singleLongBytes[m];
				}
			}
			rep.putByteArray(key,byteArray,false);
			if (rtdLongArray!=null) {
				rtdSetValue(key,rtdLongArray);
			}
		}

		/// <summary>
		/// Set float value identified by key. If there's any previous value with same key, it will be replaced.
		/// </summary>
		/// <param name="key">
		/// Key of the float value to set
		/// </param>
		/// <param name="value">
		/// Float value to be hidden (may be NaN, positive infinity or negative infinity)
		/// </param>
		public void SetFloat(string key, float value) {
			rep.putByteArray(key,BitConverter.GetBytes(value),false);
			rtdSetValue(key,value);
		}

		/// <summary>
		/// Set float list identified by key. If there's any previous value with same key, it will be replaced.
		/// </summary>
		/// <param name="key">
		/// Key of the float list to set
		/// </param>
		/// <param name="values">
		/// Float list to be hidden
		/// </param>
		public void SetFloatList(string key, IList<float> values) {
			int count=values.Count;
			float[] rtdFloatArray=(rtdIsInUse()?new float[count]:null);
			byte[] byteArray=new byte[count*4];
			byte[] singleFloatBytes;
			for (int n=0; n<count; n++) {
				if (rtdFloatArray!=null) {
					rtdFloatArray[n]=values[n];
				}
				singleFloatBytes=BitConverter.GetBytes(values[n]);
				for (int m=0; m<4; m++) {
					byteArray[n*4+m]=singleFloatBytes[m];
				}
			}
			rep.putByteArray(key,byteArray,false);
			if (rtdFloatArray!=null) {
				rtdSetValue(key,rtdFloatArray);
			}
		}

		/// <summary>
		/// Set double value identified by key. If there's any previous value with same key, it will be replaced.
		/// </summary>
		/// <param name="key">
		/// Key of the double value to set
		/// </param>
		/// <param name="value">
		/// Double value to be hidden (may be NaN, positive infinity or negative infinity)
		/// </param>
		public void SetDouble(string key, double value) {
			rep.putByteArray(key,BitConverter.GetBytes(value),false);
			rtdSetValue(key,value);
		}

		/// <summary>
		/// Set double list identified by key. If there's any previous value with same key, it will be replaced.
		/// </summary>
		/// <param name="key">
		/// Key of the double list to set
		/// </param>
		/// <param name="values">
		/// Double list to be hidden
		/// </param>
		public void SetDoubleList(string key, IList<double> values) {
			int count=values.Count;
			double[] rtdDoubleArray=(rtdIsInUse()?new double[count]:null);
			byte[] byteArray=new byte[count*8];
			byte[] singleDoubleBytes;
			for (int n=0; n<count; n++) {
				if (rtdDoubleArray!=null) {
					rtdDoubleArray[n]=values[n];
				}
				singleDoubleBytes=BitConverter.GetBytes(values[n]);
				for (int m=0; m<8; m++) {
					byteArray[n*8+m]=singleDoubleBytes[m];
				}
			}
			rep.putByteArray(key,byteArray,false);
			if (rtdDoubleArray!=null) {
				rtdSetValue(key,rtdDoubleArray);
			}
		}

		/// <summary>
		/// Set boolean value identified by key. If there's any previous value with same key, it will be replaced.
		/// </summary>
		/// <param name="key">
		/// Key of the boolean value to set
		/// </param>
		/// <param name="value">
		/// Boolean value to be hidden
		/// </param>
		public void SetBool(string key, bool value) {
			rep.putByteArray(key,BitConverter.GetBytes(value),false);
			rtdSetValue(key,value);
		}

		/// <summary>
		/// Set boolean list identified by key. If there's any previous value with same key, it will be replaced.
		/// </summary>
		/// <param name="key">
		/// Key of the boolean list to set
		/// </param>
		/// <param name="values">
		/// Boolean list to be hidden
		/// </param>
		public void SetBoolList(string key, IList<bool> values) {
			int count=values.Count;
			bool[] rtdBoolArray=(rtdIsInUse()?new bool[count]:null);
			byte[] byteArray=new byte[count]; // Of course, it would be possible to save 8 bools to single byte, but then need to save array length separately
			for (int n=0; n<count; n++) {
				if (rtdBoolArray!=null) {
					rtdBoolArray[n]=values[n];
				}
				byteArray[n]=(byte)(values[n]?1:0);
			}
			rep.putByteArray(key,byteArray,false);
			if (rtdBoolArray!=null) {
				rtdSetValue(key,rtdBoolArray);
			}
		}

		/// <summary>
		/// Set string value identified by key. If there's any previous value with same key, it will be replaced.
		/// </summary>
		/// <param name="key">
		/// Key of the string value to set
		/// </param>
		/// <param name="value">
		/// String to be hidden (can not be null but length may be zero)
		/// </param>
		public void SetString(string key, string value) {
			int length=value.Length;
			byte[] strBytes=new byte[length*2];
			for (int n=0; n<length; n++) {
				byte[] chrBytes=BitConverter.GetBytes(value[n]);
				strBytes[n*2]=chrBytes[0];
				strBytes[n*2+1]=chrBytes[1];
			}
			rep.putByteArray(key,strBytes,false);
			rtdSetValue(key,value);
		}

		/// <summary>
		/// Set byte array identified by key. If there's any previous value with same key, it will be replaced.
		/// </summary>
		/// <param name="key">
		/// Key of the byte array to set
		/// </param>
		/// <param name="value">
		/// Byte array to be hidden (can not be null but length may be zero)
		/// </param>
		public void SetByteArray(string key, byte[] value) {
			rep.putByteArray(key,value,true);
			if (rtdIsInUse()) {
				int length=value.Length;
				byte[] copy=new byte[length];
				Array.Copy(value,copy,length);
				rtdSetValue(key,copy);
			}
		}

		/// <summary>
		/// Get integer value identified by key.
		/// </summary>
		/// <param name="key">
		/// Key of the integer value to get
		/// </param>
		/// <returns>
		/// Integer value
		/// </returns>
		/// <exception cref="KeyNotFoundException">
		/// Value with specified key doesn't exist
		/// </exception>
		public int GetInt(string key) {
			byte[] valor = rep.getByteArray(key);
			if (valor == null)
            {
				return 0;
            }
            else
            {
				return BitConverter.ToInt32(valor, 0);
			}
		}

		/// <summary>
		/// Get integer value identified by key or default value if key is not found.
		/// </summary>
		/// <param name="key">
		/// Key of the integer value to get
		/// </param>
		/// <param name="defaultValue">
		/// Default integer value to return if key does not exist
		/// </param>
		/// <returns>
		/// Integer value or default integer value
		/// </returns>
		public int GetInt(string key, int defaultValue) {
			try {
				return GetInt(key);
			}
			catch (KeyNotFoundException) {
				return defaultValue;
			}
		}

		/// <summary>
		/// Get integer list identified by key.
		/// </summary>
		/// <param name="key">
		/// Key of the integer list to get
		/// </param>
		/// <returns>
		/// Integer list
		/// </returns>
		/// <exception cref="KeyNotFoundException">
		/// Value with specified key doesn't exist
		/// </exception>
		public List<int> GetIntList(string key) {
			byte[] byteArray=rep.getByteArray(key);
			int count=byteArray.Length/4;
			List<int> values=new List<int>(count);
			for (int n=0; n<count; n++) {
				values.Add(BitConverter.ToInt32(byteArray,n*4));
			}
			return values;
		}

		/// <summary>
		/// Get integer list identified by key or default list if key is not found.
		/// </summary>
		/// <param name="key">
		/// Key of the integer list to get
		/// </param>
		/// <param name="defaultList">
		/// Default integer list to return if key does not exist
		/// </param>
		/// <returns>
		/// Integer list
		/// </returns>
		public List<int> GetIntList(string key, List<int> defaultList) {
			try {
				return GetIntList(key);
			}
			catch (KeyNotFoundException) {
				return defaultList;
			}
		}

		/// <summary>
		/// Get long value identified by key.
		/// </summary>
		/// <param name="key">
		/// Key of the long value to get
		/// </param>
		/// <returns>
		/// Long value
		/// </returns>
		/// <exception cref="KeyNotFoundException">
		/// Value with specified key doesn't exist
		/// </exception>
		public long GetLong(string key) {
			return BitConverter.ToInt64(rep.getByteArray(key),0);
		}

		/// <summary>
		/// Get long value identified by key or default value if key is not found.
		/// </summary>
		/// <param name="key">
		/// Key of the long value to get
		/// </param>
		/// <param name="defaultValue">
		/// Default long value to return if key does not exist
		/// </param>
		/// <returns>
		/// Long value or default value
		/// </returns>
		public long GetLong(string key, long defaultValue) {
			try {
				return GetLong(key);
			}
			catch (KeyNotFoundException) {
				return defaultValue;
			}
		}

		/// <summary>
		/// Get long list identified by key.
		/// </summary>
		/// <param name="key">
		/// Key of the long list to get
		/// </param>
		/// <returns>
		/// Long list
		/// </returns>
		/// <exception cref="KeyNotFoundException">
		/// Value with specified key doesn't exist
		/// </exception>
		public List<long> GetLongList(string key) {
			byte[] byteArray=rep.getByteArray(key);
			int count=byteArray.Length/8;
			List<long> values=new List<long>(count);
			for (int n=0; n<count; n++) {
				values.Add(BitConverter.ToInt64(byteArray,n*8));
			}
			return values;
		}

		/// <summary>
		/// Get long list identified by key or default list if key is not found.
		/// </summary>
		/// <param name="key">
		/// Key of the long list to get
		/// </param>
		/// <param name="defaultList">
		/// Default long list to return if key does not exist
		/// </param>
		/// <returns>
		/// Long list
		/// </returns>
		public List<long> GetLongList(string key, List<long> defaultList) {
			try {
				return GetLongList(key);
			}
			catch (KeyNotFoundException) {
				return defaultList;
			}
		}

		/// <summary>
		/// Get float value identified by key.
		/// </summary>
		/// <param name="key">
		/// Key of the float value to get
		/// </param>
		/// <returns>
		/// Float value
		/// </returns>
		/// <exception cref="KeyNotFoundException">
		/// Value with specified key doesn't exist
		/// </exception>
		public float GetFloat(string key) {
			byte[] valor = rep.getByteArray(key);
			if(valor == null)
            {
				return 0;
            }
            else
            {
				return BitConverter.ToSingle(rep.getByteArray(key), 0);
			}
		}
		
		/// <summary>
		/// Get float value identified by key or default value if key is not found.
		/// </summary>
		/// <param name="key">
		/// Key of the float value to get
		/// </param>
		/// <param name="defaultValue">
		/// Default float value to return if key does not exist
		/// </param>
		/// <returns>
		/// Float value or default float value
		/// </returns>
		public float GetFloat(string key, float defaultValue) {
			try {
				return GetFloat(key);
			}
			catch (KeyNotFoundException) {
				return defaultValue;
			}
		}

		/// <summary>
		/// Get float list identified by key.
		/// </summary>
		/// <param name="key">
		/// Key of the float list to get
		/// </param>
		/// <returns>
		/// Float list
		/// </returns>
		/// <exception cref="KeyNotFoundException">
		/// Value with specified key doesn't exist
		/// </exception>
		public List<float> GetFloatList(string key) {
			byte[] byteArray=rep.getByteArray(key);
			int count=byteArray.Length/4;
			List<float> values=new List<float>(count);
			for (int n=0; n<count; n++) {
				values.Add(BitConverter.ToSingle(byteArray,n*4));
			}
			return values;
		}

		/// <summary>
		/// Get float list identified by key or default list if key is not found.
		/// </summary>
		/// <param name="key">
		/// Key of the float list to get
		/// </param>
		/// <param name="defaultList">
		/// Default float list to return if key does not exist
		/// </param>
		/// <returns>
		/// Float list
		/// </returns>
		public List<float> GetFloatList(string key, List<float> defaultList) {
			try {
				return GetFloatList(key);
			}
			catch (KeyNotFoundException) {
				return defaultList;
			}
		}

		/// <summary>
		/// Get double value identified by key.
		/// </summary>
		/// <param name="key">
		/// Key of the double value to get
		/// </param>
		/// <returns>
		/// Double value
		/// </returns>
		/// <exception cref="KeyNotFoundException">
		/// Value with specified key doesn't exist
		/// </exception>
		public double GetDouble(string key) {
			return BitConverter.ToDouble(rep.getByteArray(key),0);
		}

		/// <summary>
		/// Get double value identified by key or default value if key is not found.
		/// </summary>
		/// <param name="key">
		/// Key of the double value to get
		/// </param>
		/// <param name="defaultValue">
		/// Default double value to return if key does not exist
		/// </param>
		/// <returns>
		/// Double value or default value
		/// </returns>
		public double GetDouble(string key, double defaultValue) {
			try {
				return GetDouble(key);
			}
			catch (KeyNotFoundException) {
				return defaultValue;
			}
		}

		/// <summary>
		/// Get double list identified by key.
		/// </summary>
		/// <param name="key">
		/// Key of the double list to get
		/// </param>
		/// <returns>
		/// Double list
		/// </returns>
		/// <exception cref="KeyNotFoundException">
		/// Value with specified key doesn't exist
		/// </exception>
		public List<double> GetDoubleList(string key) {
			byte[] byteArray=rep.getByteArray(key);
			int count=byteArray.Length/8;
			List<double> values=new List<double>(count);
			for (int n=0; n<count; n++) {
				values.Add(BitConverter.ToDouble(byteArray,n*8));
			}
			return values;
		}

		/// <summary>
		/// Get double list identified by key or default list if key is not found.
		/// </summary>
		/// <param name="key">
		/// Key of the double list to get
		/// </param>
		/// <param name="defaultList">
		/// Default double list to return if key does not exist
		/// </param>
		/// <returns>
		/// Double list
		/// </returns>
		public List<double> GetDoubleList(string key, List<double> defaultList) {
			try {
				return GetDoubleList(key);
			}
			catch (KeyNotFoundException) {
				return defaultList;
			}
		}

		/// <summary>
		/// Get boolean value identified by key.
		/// </summary>
		/// <param name="key">
		/// Key of the boolean value to get
		/// </param>
		/// <returns>
		/// Boolean value
		/// </returns>
		/// <exception cref="KeyNotFoundException">
		/// Value with specified key doesn't exist
		/// </exception>
		public bool GetBool(string key) {
			return BitConverter.ToBoolean(rep.getByteArray(key),0);
		}
		
		/// <summary>
		/// Get boolean value identified by key or default value if key is not found.
		/// </summary>
		/// <param name="key">
		/// Key of the boolean value to get
		/// </param>
		/// <param name="defaultValue">
		/// Default boolean value to return if key does not exist
		/// </param>
		/// <returns>
		/// Boolean value or default boolean value
		/// </returns>
		public bool GetBool(string key, bool defaultValue) {
			try {
				return GetBool(key);
			}
			catch (KeyNotFoundException) {
				return defaultValue;
			}
		}

		/// <summary>
		/// Get boolean list identified by key.
		/// </summary>
		/// <param name="key">
		/// Key of the boolean list to get
		/// </param>
		/// <returns>
		/// Boolean list
		/// </returns>
		/// <exception cref="KeyNotFoundException">
		/// Value with specified key doesn't exist
		/// </exception>
		public List<bool> GetBoolList(string key) {
			byte[] byteArray=rep.getByteArray(key);
			int count=byteArray.Length;
			List<bool> values=new List<bool>(count);
			for (int n=0; n<count; n++) {
				values.Add(byteArray[n]==1);
			}
			return values;
		}

		/// <summary>
		/// Get bool list identified by key or default list if key is not found.
		/// </summary>
		/// <param name="key">
		/// Key of the bool list to get
		/// </param>
		/// <param name="defaultList">
		/// Default bool list to return if key does not exist
		/// </param>
		/// <returns>
		/// Bool list
		/// </returns>
		public List<bool> GetBoolList(string key, List<bool> defaultList) {
			try {
				return GetBoolList(key);
			}
			catch (KeyNotFoundException) {
				return defaultList;
			}
		}

		/// <summary>
		/// Get string identified by key.
		/// </summary>
		/// <param name="key">
		/// Key of the string to get
		/// </param>
		/// <returns>
		/// String
		/// </returns>
		/// <exception cref="KeyNotFoundException">
		/// Value with specified key doesn't exist
		/// </exception>
		public string GetString(string key) {
			byte[] bytes=rep.getByteArray(key);
			int strLength=bytes.Length/2;
			char[] strChars=new char[strLength];
			for (int n=0; n<strLength; n++) {
				strChars[n]=BitConverter.ToChar(bytes,n*2);
			}
			return (new string(strChars));
		}
		
		/// <summary>
		/// Get string identified by key or default value if key is not found.
		/// </summary>
		/// <param name="key">
		/// Key of the string to get
		/// </param>
		/// <param name="defaultValue">
		/// Default string value to return if key does not exist (may be null)
		/// </param>
		/// <returns>
		/// String or default string
		/// </returns>
		public string GetString(string key, string defaultValue) {
			try {
				return GetString(key);
			}
			catch (KeyNotFoundException) {
				return defaultValue;
			}
		}

		/// <summary>
		/// Get byte array identified by key.
		/// </summary>
		/// <param name="key">
		/// Key of the byte array to get
		/// </param>
		/// <returns>
		/// Byte array
		/// </returns>
		/// <exception cref="KeyNotFoundException">
		/// Value with specified key doesn't exist
		/// </exception>
		public byte[] GetByteArray(string key) {
			return rep.getByteArray(key);
		}

		/// <summary>
		/// Get byte array identified by key or default value if key is not found.
		/// </summary>
		/// <param name="key">
		/// Key of the byte array to get
		/// </param>
		/// <param name="defaultArray">
		/// Default byte array to return if key does not exist (may be null)
		/// </param>
		/// <returns>
		/// Byte array or default byte array
		/// </returns>
		public byte[] GetByteArray(string key, byte[] defaultArray) {
			try {
				return GetByteArray(key);
			}
			catch (KeyNotFoundException) {
				return defaultArray;
			}
		}
		
		/// <summary>
		/// Checks whether any type of value identified by key exists.
		/// </summary>
		/// <param name="key">
		/// Key to locate
		/// </param>
		/// <returns>
		/// True if key and value exists
		/// </returns>
		public bool ContainsKey(string key) {
			return rep.containsKey(key);
		}

		/// <summary>
		/// Get list of all keys in this instance. Returned array is copy of the keys at the moment when this method was called.
		/// Changing values or setting them null in returned array doesn't affect this HiddenVars instance in any way.
		/// </summary>
		/// <returns>
		/// Array of string keys.
		/// </returns>
		public string[] GetKeys() {
			return rep.getKeys();
		}

		/// <summary>
		/// Remove any type of value identified by key.
		/// </summary>
		/// <param name="key">
		/// Key of the value to be removed
		/// </param>
		/// <returns>
		/// True if key was found and value removed, otherwise false
		/// </returns>
		public bool Remove(string key) {
			bool ret=rep.remove(key);
			rtdRemoveValue(key);
			return ret;
		}

		/// <summary>
		/// Remove all the keys and values.
		/// </summary>
		public void Clear() {
			rep.clear();
			rtdClear();
	    }

		/// <summary>
		/// Clear and destroy this instance. This method will also remove instance from RunTimeDebug object if running application in Unity Editor.
		/// After calling this method, instance is not usable any more and trying to add or read any values will cause exception.
		/// </summary>
		public void Dispose() {
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing) {
			if (!disposed) {
				if (disposing) {
					rep.destroy();
					rep=null;
				}
				rtdRemoveInstance();
				disposed=true;
			}
		}

		~HiddenVars() {
			Dispose(false); // This will remove instance from debug object if running in Unity Editor
		}

		private bool rtdIsInUse() {
			#if UNITY_EDITOR
			return true;
			#else
			return false;
			#endif
		}

		private void rtdAddInstance(string nameForDebug) {
			#if UNITY_EDITOR
			RunTimeDebugObject rtdObject=RunTimeDebugObject.getOrCreateInstance();
			if (rtdObject!=null) {
				debugId=rtdObject.addDebugDictionary(nameForDebug);
			}
			#endif
		}

		private void rtdSetValue(string key, object value) {
			#if UNITY_EDITOR
			RunTimeDebugObject rtdObject=RunTimeDebugObject.getInstance();
			if (rtdObject!=null) {
				rtdObject.setDebugDictionaryValue(debugId,key,value);
			}
			#endif
		}

		private void rtdRemoveValue(string key) {
			#if UNITY_EDITOR
			RunTimeDebugObject rtdObject=RunTimeDebugObject.getInstance();
			if (rtdObject!=null) {
				rtdObject.removeDebugDictionaryValue(debugId,key);
			}
			#endif
		}

		private void rtdClear() {
			#if UNITY_EDITOR
			RunTimeDebugObject rtdObject=RunTimeDebugObject.getInstance();
			if (rtdObject!=null) {
				rtdObject.clearDebugDictionary(debugId);
			}
			#endif
		}

		private void rtdRemoveInstance() {
			#if UNITY_EDITOR
			if (!RunTimeDebugObject.isDestroyedByUser()) {
				RunTimeDebugObject.getInstance().removeDebugDictionary(debugId);
			}
// This will cause error to log at least in Unity 4, if this method was called by GC thread
//			RunTimeDebugObject rtdObject=RunTimeDebugObject.getInstance();
//			if (rtdObject!=null) {
//				rtdObject.removeDebugDictionary(debugId);
//			}
			#endif
		}

	}

}
