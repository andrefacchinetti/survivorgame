//  HiddenVars - RunTimeDebugContent

//  This contains plain versions of values stored to HiddenVars, for debug purposes.
//  This class only exists when running application in Unity Editor and values are not in plain format when application is built for target platform.
//  Even if for some reason it would happen, that this and RunTimeDebugObject classes gets included, variables from this class are used only
//  to be printed out for debug. So changing any variables here do not change the actual hidden variables stored in HiddenVars/HiddenValue instances.


#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Leguar.HiddenVars.Internal {

	public class RunTimeDebugContent {

		private string name;
		private Dictionary<string,object> content;

		internal RunTimeDebugContent(string name) {
			this.name=name;
			content=new Dictionary<string,object>();
		}

		public string getName() {
			return name;
		}

		public Dictionary<string,object> getContent() {
			return content;
		}

	}

}

#endif
