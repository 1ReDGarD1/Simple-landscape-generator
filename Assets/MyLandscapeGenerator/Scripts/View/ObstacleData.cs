using MyLandscapeGenerator.Scripts.Pool;

namespace MyLandscapeGenerator.Scripts.View
{
    public struct ObstacleData
    {
        public int MapX { get; }
        public int MapY { get; }

        public PrefabPoolSetting PrefabPoolSetting { get; }

        public ObstacleData(int mapX, int mapY, PrefabPoolSetting prefabPoolSetting)
        {
            MapX = mapX;
            MapY = mapY;
            PrefabPoolSetting = prefabPoolSetting;
        }
    }
}