using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledCS;

namespace TheForgottenKnight.MapComponents
{
	/// <summary>
	/// Tileset used for drawing map components
	/// </summary>
	public class Tileset
	{
		private TiledTileset tiledTileset;
		private Texture2D tex;

		private int firstGid;
		private int lastGid;

		/// <summary>
		/// Default constructor
		/// </summary>
		public Tileset()
		{

		}

		/// <summary>
		/// Create Tileset data needed to map tile gid's to the Tileset
		/// </summary>
		/// <param name="tiledTileset">TiledTilest with data from the .tsx file</param>
		/// <param name="tex">Texture used with the tileset data</param>
		/// <param name="firstGid">First gid assigned to tileset as per the .tmx file</param>
		public Tileset(TiledTileset tiledTileset, Texture2D tex, int firstGid)
		{
			this.TiledTileset = tiledTileset;
			this.Tex = tex;
			this.FirstGid = firstGid;
			LastGid = firstGid + tiledTileset.TileCount - 1;
		}

		public TiledTileset TiledTileset { get => tiledTileset; set => tiledTileset = value; }
		public Texture2D Tex { get => tex; set => tex = value; }
		public int FirstGid { get => firstGid; set => firstGid = value; }
		public int LastGid { get => lastGid; set => lastGid = value; }
	}
}
