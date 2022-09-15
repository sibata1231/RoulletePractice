using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class CSVLoader : MonoBehaviour {
    private TextAsset      m_csvFile;
    private List<string[]> m_nameList;
    public int CurrentMemberCount {
        get => m_nameList.Count;
    }

    public string GetMamber(int index) => m_nameList[index][0];
    public string[] GetMamberList => m_nameList[0];

    public void Initialize(string fileName = "NameList") {
        m_nameList          = new List<string[]>();
        m_csvFile           = Resources.Load(fileName) as TextAsset; // Resources CSV“Ç‚Ýž‚Ý
        StringReader reader = new StringReader(m_csvFile.text);

        int count = 0;
        while (reader.Peek() != -1) {
            string line = reader.ReadLine();
            count++;
            if (count == 1) { // 1s–Ú–³Œø(HEADER)
                continue;
            }

            m_nameList.Add(line.Split(','));
        }
        m_nameList = m_nameList.Where(x => System.Convert.ToBoolean(x[1])).ToList();
        Shuffle<string[]>(m_nameList);
        //foreach (var name in m_nameList) {
        //    Debug.Log(name[0]);
        //}
    }

    private void Shuffle<T>(IList<T> list) {
        for (var i = list.Count - 1; i > 0; --i) {
            var j = UnityEngine.Random.Range(0, i + 1);

            // i”Ô–Ú‚Æj”Ô–Ú‚Ì—v‘f‚ðŒðŠ·‚·‚é
            var tmp  = list[i];
            list[i] = list[j];
            list[j] = tmp;
        }
    }
}