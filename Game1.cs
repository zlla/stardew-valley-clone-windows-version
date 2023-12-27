using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TiledCS;
using MonoGame.Extended.Tiled;
using TiledMap = TiledCS.TiledMap;
using Microsoft.Xna.Framework.Audio;

namespace StardewValleyClone;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private FrameCounter _frameCounter = new();
    private Settings _settings = new();
    private Level _level;

    static public TiledMap _map;
    static public int TileWidth;
    static public int TileHeight;
    static public Texture2D _tilesetTexture;

    static public TiledTileset HouseFurnitureBottomTileSet;
    static public Texture2D HouseFurnitureBottomTexture;
    static public int HouseFurnitureBottomTileTilesWide;
    static public int HouseFurnitureBottomTileTilesHeight;

    static public TiledTileset HouseWallsTileSet;
    static public Texture2D HouseWallsTexture;
    static public int HouseWallsTileTilesWide;
    static public int HouseWallsTileTilesHeight;

    static public TiledTileset HouseFloorTileSet;
    static public Texture2D HouseFloorTexture;
    static public int HouseFloorTileTilesWide;
    static public int HouseFloorTileTilesHeight;

    static public TiledTileset HouseFurnitureTopTileSet;
    static public Texture2D HouseFurnitureTopTexture;
    static public int HouseFurnitureTopTileTilesWide;
    static public int HouseFurnitureTopTileTilesHeight;

    static public TiledTileset FencesTileSet;
    static public Texture2D FencesTexture;
    static public int FencesTileTilesWide;
    static public int FencesTileTilesHeight;

    static public TiledTileset WaterTileSet;
    static public int WaterTileTilesWide;
    static public int WaterTileTilesHeight;

    static public TiledTileset InteractionTileSet;
    static public int InteractionTileTilesWide;
    static public int InteractionTileTilesHeight;

    static public SpriteFont spriteFont;

    static public SoundEffect axeSound;
    static public SoundEffect successSound;
    static public SoundEffect hoeSound;
    static public SoundEffect watering;
    static public SoundEffect plantSound;
    static public SoundEffect music;
    static public SoundEffect buySound;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = _settings.SCREEN_WIDTH;
        _graphics.PreferredBackBufferHeight = _settings.SCREEN_HEIGHT;
        _graphics.ApplyChanges();

        // Set the "Copy to Output Directory" property of these two files to `Copy if newer`
        // by clicking them in the solution explorer.
        _map = new TiledMap(Content.RootDirectory + "/map.tmx");
        TileWidth = 64;
        TileHeight = 64;

        HouseFurnitureBottomTileSet = new TiledTileset(Content.RootDirectory + "/House.tsx");
        HouseFloorTileSet = new TiledTileset(Content.RootDirectory + "/House Decoration.tsx");
        HouseFurnitureTopTileSet = new TiledTileset(Content.RootDirectory + "/House Decoration.tsx");
        HouseWallsTileSet = new TiledTileset(Content.RootDirectory + "/House.tsx");
        FencesTileSet = new TiledTileset(Content.RootDirectory + "/Fences.tsx");
        WaterTileSet = new TiledTileset(Content.RootDirectory + "/Water.tsx");
        InteractionTileSet = new TiledTileset(Content.RootDirectory + "/interaction.tsx");


        // Not the best way to do this but it works. It looks for "exampleTileset.xnb" file
        // which is the result of building the image file with "Content.mgcb".
        HouseFloorTexture = Content.Load<Texture2D>("House");
        HouseFurnitureBottomTexture = Content.Load<Texture2D>("House Decoration");
        HouseWallsTexture = Content.Load<Texture2D>("House");
        HouseFurnitureTopTexture = Content.Load<Texture2D>("House Decoration");
        FencesTexture = Content.Load<Texture2D>("Fences");

        // Amount of tiles on each row (left right)
        HouseFurnitureBottomTileTilesWide = HouseFurnitureBottomTileSet.Columns;
        // Amount of tiels on each column (up down)
        HouseFurnitureBottomTileTilesHeight = HouseFurnitureBottomTileSet.TileCount / HouseFurnitureBottomTileSet.Columns;

        HouseFloorTileTilesWide = HouseFloorTileSet.Columns;
        HouseFloorTileTilesHeight = HouseFloorTileSet.TileCount / HouseFloorTileSet.Columns;

        HouseWallsTileTilesWide = HouseWallsTileSet.Columns;
        HouseWallsTileTilesHeight = HouseWallsTileSet.TileCount / HouseWallsTileSet.Columns;

        HouseFurnitureTopTileTilesWide = HouseFurnitureTopTileSet.Columns;
        HouseFurnitureTopTileTilesHeight = HouseFurnitureTopTileSet.TileCount / HouseFurnitureTopTileSet.Columns;

        FencesTileTilesWide = FencesTileSet.Columns;
        FencesTileTilesHeight = FencesTileSet.TileCount / FencesTileSet.Columns;

        WaterTileTilesWide = WaterTileSet.Columns;
        WaterTileTilesHeight = WaterTileSet.TileCount / WaterTileSet.Columns;

        InteractionTileTilesWide = InteractionTileSet.Columns;
        InteractionTileTilesHeight = InteractionTileSet.TileCount / InteractionTileSet.Columns;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        spriteFont = Content.Load<SpriteFont>("LycheeSoda");
        axeSound = Content.Load<SoundEffect>("axe");
        successSound = Content.Load<SoundEffect>("success");
        hoeSound = Content.Load<SoundEffect>("hoe");
        watering = Content.Load<SoundEffect>("water");
        plantSound = Content.Load<SoundEffect>("plant");
        music = Content.Load<SoundEffect>("music");
        buySound = Content.Load<SoundEffect>("buy");

        _level = new(GraphicsDevice, _settings);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            Exit();

        // TODO: Add your update logic here
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _frameCounter.Update(deltaTime);

        _level.Run(_frameCounter.AverageFramesPerSecond / 1000);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _level.Draw(_frameCounter.AverageFramesPerSecond / 1000, _spriteBatch);

        base.Draw(gameTime);
    }
}

