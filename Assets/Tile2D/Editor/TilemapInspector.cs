using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.IO;

[CustomEditor(typeof(TilemapBehaviour))]
[CanEditMultipleObjects]
public class TilemapInspector : Editor
{
    private TilemapBehaviour tilemapBehaviour;

    public override void OnInspectorGUI()
    {
        tilemapBehaviour = (TilemapBehaviour)target;
        base.OnInspectorGUI();
        if (GUILayout.Button("导出地图"))
        {
            ExportMap();
        }

        if (GUILayout.Button("清除地图"))
        {
            if (EditorUtility.DisplayDialog("提示", "确定要清除地图吗？", "确定", "取消"))
                ClearMap();
        }
    }


    public void ExportMap()
    {
        TileBase[] tileArray = tilemapBehaviour.Tilemap.GetTilesBlock(tilemapBehaviour.area);

        Debug.Log(string.Format("Tilemap：{0} 准备导出地图数据", tilemapBehaviour.Tilemap.name));

        int tilecount = 0;

        Dictionary<string, string> data = new Dictionary<string, string>();

        for (int i = tilemapBehaviour.area.xMin; i < tilemapBehaviour.area.xMax; i++)
        {
            for (int j = tilemapBehaviour.area.yMin; j < tilemapBehaviour.area.yMax; j++)
            {
                
                Vector3Int tempVec = new Vector3Int(i, j, 0);
                if (tilemapBehaviour.Tilemap.GetTile(tempVec) == null)
                    continue;

                Debug.Log(string.Format("位置：{0} Tile：{1}", tempVec.ToString(), tilemapBehaviour.Tilemap.GetTile(tempVec).ToString()));

                data.Add(i + "_" + j, tilemapBehaviour.Tilemap.GetTile(tempVec).name);

                tilecount++;
            }
        }

        string jsonData = LitJson.JsonMapper.ToJson(data);

        ExportMap(jsonData, tilemapBehaviour.Tilemap.name);

        Debug.Log(string.Format("Tilemap：{0} 总共Tile数量：{1}", tilemapBehaviour.Tilemap.name, tilecount.ToString()));

    }


    public void ExportMap(string data, string mapName)
    {
        string fullPath = EditorUtility.SaveFilePanel("保存地图文件", Application.dataPath, mapName, "json");
        if (string.IsNullOrEmpty(fullPath))
            return;

        TextWriter tw = new StreamWriter(fullPath, false);
        tw.Write(data);
        tw.Close();
        Debug.Log("导出地图完成 path:" + fullPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public void ClearMap()
    {
        tilemapBehaviour.Tilemap.ClearAllTiles();
    }
}
