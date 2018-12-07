using System;									// System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using GXPEngine.Classes;

public class MyGame : Game
{
	public MyGame() : base(800, 600, false,false,pPixelArt:true)		// Create a window that's 800x600 and NOT fullscreen
	{
        Sprite background = new Sprite("Data/map.png");
        AddChild(background);

        Player player = new Player();
        AddChild(player);
        //targetFps = 5;
    }

	void Update()
	{

		// Empty
	}

	static void Main()							// Main() is the first method that's called when the program is run
	{
		new MyGame().Start();					// Create a "MyGame" and start it
	}
}