//    HiddenVars - Example


using UnityEngine;
using System.Collections.Generic;
using Leguar.HiddenVars;

namespace Leguar.HiddenVars.Example {

	public class Example : MonoBehaviour {

		private HiddenVars userVars;
		private HiddenVars gameVars;

		private bool startNewGame;
		private bool exampleFinished;

		void Start() {

			Debug.Log("-- Start --");

			Debug.Log("Note: When application is playing, you can see contents of HiddenVars instances by opening debug window from Unity Editor menu: Window -> HiddenVars Runtime");

			userVars=new HiddenVars("User"); // New instance with specific debug name, visible in "HiddenVars EditorOnly RunTimeDebug" object during runtime
			userVars.SetString("name","Example Player");
			userVars.SetInt("gamesPlayed",0); // Could also be userVars["gamesPlayed"]=0;

			startNewGame=true;
			exampleFinished=false;

		}

		void Update() {

			if (exampleFinished) {
				return;
			}

			if (startNewGame) {

				if (!userVars.GetBool("anyGamePlayed",false)) { // Variable "anyGamePlayed" may not be defined so provide default return value
					Debug.Log("Game start: First game start");
				} else {
					Debug.Log("Game start: New game start");
				}

				gameVars=new HiddenVars();
				gameVars["lives"]=3;
				gameVars["score"]=0;

				startNewGame=false;

			}

			gameVars["score"]+=Random.Range(1,100);

			if (Random.value<0.01f) {

				gameVars["lives"]--;

				if (gameVars["lives"]==0) {

					userVars["gamesPlayed"]=userVars["gamesPlayed"]+1; // Just another way to write userVars["gamesPlayed"]++;
					userVars.SetBool("anyGamePlayed",true);

					Debug.Log("Game over: "+userVars.GetString("name")+"'s game ended with score "+gameVars["score"]+", now "+userVars["gamesPlayed"]+" game(s) played");

					List<int> scoreHistory=userVars.GetIntList("scoreHistory",new List<int>());
					scoreHistory.Add(gameVars["score"]);
					userVars.SetIntList("scoreHistory",scoreHistory);

					if (gameVars["score"]>userVars.GetInt("bestScore",0)) {
						Debug.Log("That was new record!");
						userVars["bestScore"]=gameVars["score"];
					}
					gameVars.Dispose(); // Not necessary but in case you want instantly destroy any variables in this HiddenVars

					if (userVars["gamesPlayed"]<5) {
						startNewGame=true;
					} else {
						exampleFinished=true;
						Debug.Log("-- End --");
					}

				}

			}

		}

	}

}
