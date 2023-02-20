=========================
 HiddenVars - ReadMe.txt
=========================



Basic usege
-----------

Simply create new HiddenVars instance in your code using:

HiddenVars hiddenVars = new HiddenVars();

You can then save and read different type of data using Set and Get methods.

Class HiddenVars contains full, inline c# documentation for constructors and methods.



Using indexer
-------------

Since integers are most common data to be saved securely, you can also use indexer to save and read integers:

HiddenVars hiddenVars = new HiddenVars();
hiddenVars["score"] = 0;
hiddenVars["score"] += 100;
Debug.Log("Current score: "+hiddenVars["score"]); // Prints out "Current score: 100"



Default values
--------------

All the Get methods have also second version where you can define default value in case value is not available.
This is handy when it is completely okay that some value doesn't yet exist. For example:

int lastGameScore = myHiddenVars.GetInt("LastGameScore",0);
string previousMessage = myOtherHiddenVars.GetString("previous_message",null);



Note: No type check
-------------------

There is no type check when reading values. If you save some value as int and try to read it out as long,
you end up getting error message. Or if you save something as string and then try to read it as int,
results may vary. You may get error message or you may get back some meaningless integer value.

So make sure to read values using same type that is used to save them!



Debugging in Unity Editor
-------------------------

When running your application in Unity Editor, first call to HiddenVars will create gameobject
"HiddenVars EditorOnly RunTimeDebug" to hierarchy.

When choosing this object, you can see current content of all HiddenVars instances in the inspector window.

This gameobject does not appear in builds outside Unity Editor and it can be also deleted during runtime
without any effect to actual functionality of HiddenVars.



Additional documents
--------------------

Inline c# documents included to HiddenVars class are also available online in html format:

http://www.leguar.com/unity/hiddenvars/apidoc/1.4/

See also the example scene and code Example.cs included in this package.



Feedback
--------


If you are happy with this asset, please rate us or leave feedback in Unity Asset Store:

https://assetstore.unity.com/packages/slug/64436


If you have any problems, or maybe suggestions for future versions, feel free to contact:

http://www.leguar.com/contact/?about=hiddenvars
