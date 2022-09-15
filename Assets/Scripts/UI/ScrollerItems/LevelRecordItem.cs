using System.Collections;
using UnityEngine;
using EnhancedUI.EnhancedScroller;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelRecordItem : EnhancedScrollerCellView
{
    [SerializeField]
    TMP_Text star;
    [SerializeField]
    Image starImage;
    [SerializeField]
    Transform allyThumbnailContainer;
    [SerializeField]
    Color[] colorPreset; 
    [SerializeField]
    AllyWithNumberCountItem allyCountItemTemplate;
    List<AllyWithNumberCountItem> imageList = new List<AllyWithNumberCountItem>();
    public void Init(LevelRecord levelRecord)
    {
        if (imageList.Count > levelRecord.allies.Count)
        {
            for (int i = imageList.Count - 1; i > levelRecord.allies.Count - 1; i--)
            {
                var image = imageList[i];
                imageList.RemoveAt(i);
                Destroy(image.gameObject);
            }
        }
        else if (levelRecord.allies.Count > imageList.Count)
        {
            for (int i = 0; i < levelRecord.allies.Count - imageList.Count; i++)
            {
                imageList.Add(Instantiate(allyCountItemTemplate, allyThumbnailContainer));
            }
        }

        for (int i = 0; i < imageList.Count; i++)
        {
            var ally = levelRecord.allies[i];
            imageList[i].Init(Main.allyDescriptions[ally.allyName].thumbnail, ally.count);
        }

        star.text = levelRecord.star.ToString();
        //platinum 0 255 104
        //gold 255 255 255
        //bronze 209 33 0
        starImage.color = new Color(starImage.color.r, starImage.color.b, starImage.color.g, 1f);
     
        starImage.color = colorPreset[levelRecord.star - 1];
    }
}
