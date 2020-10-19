using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class dialogMgr : MonoBehaviour
{
    Animator animator;
    bool isReading = false;
    bool isCoroutine = false;

    Text name, descript;
    Image sprite;

    List<List<string>> serifDatas = new List<List<string>>();
    int serifIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        name = gameObject.transform.Find("textField/name").GetComponent<Text>();
        descript = gameObject.transform.Find("textField/discript").GetComponent<Text>();
        sprite = gameObject.transform.Find("standImg").GetComponent<Image>();

        //CreateXml();
    }

    void CreateXml()
    {
        //init xml
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.AppendChild(xmldoc.CreateXmlDeclaration("1.0", "utf-8", "yes"));

        //create root
        XmlNode root = xmldoc.CreateNode(XmlNodeType.Element, "root", string.Empty);
        xmldoc.AppendChild(root);

        XmlNode name = xmldoc.CreateNode(XmlNodeType.Element, "name", string.Empty);
        root.AppendChild(name);

        XmlNode text = xmldoc.CreateNode(XmlNodeType.Element, "text", string.Empty);
        root.AppendChild(text);

        XmlNode img = xmldoc.CreateNode(XmlNodeType.Element, "img", string.Empty);
        root.AppendChild(img);

        XmlNode delay = xmldoc.CreateNode(XmlNodeType.Element, "delay", string.Empty);
        root.AppendChild(delay);

        xmldoc.Save("./Assets/Resources/textXml/Test.xml");
    }

    void LoadXml(string fileName)
    {
        TextAsset textAsset = (TextAsset)Resources.Load("textXml/" + fileName);
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(textAsset.text);

        XmlNodeList serifs = xmlDoc.SelectNodes("root/serif");
        int index = 0;
        foreach (XmlNode serif in serifs)
        {
            serifDatas.Add(new List<string>());
            serifDatas[index].Add(serif.SelectSingleNode("name").InnerText);
            serifDatas[index].Add(serif.SelectSingleNode("text").InnerText);
            serifDatas[index].Add(serif.SelectSingleNode("img").InnerText);
            serifDatas[index].Add(serif.SelectSingleNode("delay").InnerText);
            index++;
        }
    }
    public void runDialog(string path)
    {
        animator.SetBool("open", true);
        //Todo textData transform array
        LoadXml(path);
        isReading = true;
        serifIndex = 0;
    }

    public void onClickDialog()
    {
        if (!isReading) return;
        Debug.Log("read");
        if(!isCoroutine) StartCoroutine("ReadPerClick");
        Debug.Log(serifIndex);
    }

    IEnumerator ReadPerClick()
    {
        isCoroutine = true;

        Debug.Log(serifDatas.Count);
        name.text = serifDatas[serifIndex][0];
        descript.text = serifDatas[serifIndex][1];
        sprite.sprite = Resources.Load<Sprite>(serifDatas[serifIndex][2]);
        serifIndex++;
        //텍스트 재생
        yield return new WaitForSeconds(1f);
        isCoroutine = false;
        StopCoroutine("ReadPerClick");
    }
}
