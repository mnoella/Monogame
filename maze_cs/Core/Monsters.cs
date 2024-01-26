using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;



namespace maze_cs.Core;

public class Monsters 
{
   
    public Texture2D Texture { get; set; }
	public Vector2 Position { get; set; }
	public Vector2  Size { get; private set; }
	

	public Monsters(Texture2D texture, Vector2 position, Vector2 size) {

		Texture = texture;
		Position = position;
		Size = size;
	}
}