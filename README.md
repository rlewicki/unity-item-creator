# Item Creator
### Extension is under development and is not ready to use yet!

### Model code
![Model Code](https://github.com/rlewicki/unity-item-creator/Screenshots/code.png)

### Inspector look
![Inspector](https://github.com/rlewicki/unity-item-creator/Screenshots/inspector.png)

### Description
Item Creator is a Unity extension that enables user to easly create his item data and save it to `json` file (I have plans for creating reader that will read all items into actual game in the future).

### Development
I am using Unity 2017.1 along with Visual Studio 2017 Community Edition with experimanetal C# 6 support (I am using string interpolation inside).

### Usage
First you need to create a script that will include public class which will be `[System.Serializable]` (this is necessary in order to be able to read class properties). Then you just need to drag a script into the specified field and *vuala*. In order to create item and append it to your destination file click `Create item` button.