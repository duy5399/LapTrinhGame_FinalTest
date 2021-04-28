using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public Image img;
    public Sprite[] characterSprite;
    public int selectedCharacter = 0;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        //class PlayerPrefs trong Unity để lưu thông tin và trạng thái trong game
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);     //tham số đầu tiên là tên đại điện cho giá trị được lưu trữ, tham số còn lại là nội dung cần lưu trữ.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void NextCharacter()
    {
        if (selectedCharacter == 3)
            selectedCharacter = 0;
        else
            selectedCharacter++;
        img.sprite = characterSprite[selectedCharacter];
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
        Debug.Log(selectedCharacter);
        Debug.Log(PlayerPrefs.GetInt("selectedCharacter"));
    }
    public void PreviousCharacter()
    {
        if (selectedCharacter == 0)
            selectedCharacter = 3;
        else
            selectedCharacter--;
        img.sprite = characterSprite[selectedCharacter];
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
        Debug.Log(PlayerPrefs.GetInt("selectedCharacter"));
        Debug.Log(selectedCharacter);
    }
}
