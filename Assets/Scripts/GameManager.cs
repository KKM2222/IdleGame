using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int jelatin;
    public int gold;
    public List<Jelly> jelly_list = new List<Jelly>();
    public List<Data> jelly_data_list = new List<Data>();
    public bool[] jelly_unlock_list;
    public bool[] Upgrade_list;
    public int num_level;
    public int click_level;

    public int max_jelatin;
    public int max_gold;

    public bool isSell;
    public bool isLive;

    public Sprite[] jelly_spritelist;
    public string[] jelly_namelist;
    public int[] jelly_jelatinlist;
    public int[] jelly_goldlist;

    public int[] num_gold_list;
    public int[] click_gold_list;

    public Text page_text;
    public Image unlock_group_jelly_img;
    public Text unlock_group_gold_text;
    public Text unlock_group_name_text;

    public Sprite[] Upgrade_spritelist;
    public Image Upgrade_Group_img;
    public GameObject Upgrade_Group;
    public GameObject lock_group;
    public Image lock_group_jelly_img;
    public Text lock_group_jelatin_text;

    public RuntimeAnimatorController[] level_ac;

    public Text jelatin_text;
    public Text gold_text;

    public Image jelly_panel;
    public Image plant_panel;
    public Image option_panel;
    public Image relic_panel;

    public GameObject prefab;

    public GameObject data_manager_obj;

    public Text num_sub_text;
    public Text num_btn_text;
    public Button num_btn;

    public Text click_sub_text;
    public Text click_btn_text;
    public Button click_btn;

    DataManager data_manager;

    Animator jelly_anim;
    Animator plant_anim;
    Animator relic_anim;

    bool isJellyClick;
    bool isPlantClick;
    bool isRelicClick;
    bool isOption;

    int page;

    void Awake()
    {
        instance = this;

        jelly_anim = jelly_panel.GetComponent<Animator>();
        plant_anim = plant_panel.GetComponent<Animator>();
        relic_anim = relic_panel.GetComponent<Animator>();

        isLive = true;

        data_manager = data_manager_obj.GetComponent<DataManager>();

        page = 0;
        jelly_unlock_list = new bool[15];
    }

    void Start()
    {
        Invoke("LoadData", 0.1f);
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (isJellyClick) ClickJellyBtn();
            else if (isPlantClick) ClickPlantBtn();
            else if (isRelicClick) ClickRelicBtn();
            else Option();
        }
    }

    void LateUpdate()
    {
        jelatin_text.text = string.Format("{0:n0}", (int)Mathf.SmoothStep(float.Parse(jelatin_text.text), jelatin, 0.5f));
        gold_text.text = string.Format("{0:n0}", (int)Mathf.SmoothStep(float.Parse(gold_text.text), gold, 0.5f));
    }

    public void ChangeAc(Animator anim, int level)
    {
        anim.runtimeAnimatorController = level_ac[level - 1];

        SoundManager.instance.PlaySound("Grow");
    }

    public void GetJelatin(int id, int level)
    {
        jelatin += (id + 1) * level * click_level;

        if (jelatin > max_jelatin)
            jelatin = max_jelatin;
    }

    public void GetGold(int id, int level, Jelly jelly)
    {
        gold += jelly_goldlist[id] * level;

        if (gold > max_gold)
            gold = max_gold;

        jelly_list.Remove(jelly);

        SoundManager.instance.PlaySound("Sell");
    }

    public void CheckSell()
    {
        isSell = isSell == false;
    }

    public void ClickJellyBtn()
    {
        if (isPlantClick)
        {
            plant_anim.SetTrigger("doHide");
            isPlantClick = false;
            isLive = true;
        }

        if (isJellyClick)
            jelly_anim.SetTrigger("doHide");
        else
            jelly_anim.SetTrigger("doShow");

        isJellyClick = !isJellyClick;
        isLive = !isLive;

        SoundManager.instance.PlaySound("Button");
    }

    public void ClickPlantBtn()
    {
        if (isJellyClick)
        {
            jelly_anim.SetTrigger("doHide");
            isJellyClick = false;
            isLive = true;
        }

        if (isPlantClick)
            plant_anim.SetTrigger("doHide");
        else
            plant_anim.SetTrigger("doShow");

        isPlantClick = !isPlantClick;
        isLive = !isLive;

        SoundManager.instance.PlaySound("Button");
    }
    public void ClickRelicBtn()
    {
        if (isJellyClick)
        {
            jelly_anim.SetTrigger("doHide");
            isJellyClick = false;
            isLive = true;
        }

        if (isRelicClick)
            relic_anim.SetTrigger("doHide");
        else
            relic_anim.SetTrigger("doShow");

        isRelicClick = !isRelicClick;
        isLive = !isLive;

        SoundManager.instance.PlaySound("Button");
    }

    public void Option()
    {
        isOption = !isOption;
        isLive = !isLive;

        option_panel.gameObject.SetActive(isOption);
        Time.timeScale = isOption == true ? 0 : 1;

        if (isOption) SoundManager.instance.PlaySound("Pause In");
        else SoundManager.instance.PlaySound("Pause Out");
    }

    public void PageUp()
    {
        if (page >= 14)
        {
            SoundManager.instance.PlaySound("Fail");
            return;
        }

        ++page;
        ChangePage();

        SoundManager.instance.PlaySound("Button");
    }

    public void PageDown()
    {
        if (page <= 0)
        {
            SoundManager.instance.PlaySound("Fail");
            return;
        }

        --page;
        ChangePage();

        SoundManager.instance.PlaySound("Button");
    }

    void ChangePage()
    {
        lock_group.gameObject.SetActive(!jelly_unlock_list[page]);

        page_text.text = string.Format("#{0:00}", (page + 1));

        if (lock_group.activeSelf)
        {
            lock_group_jelly_img.sprite = jelly_spritelist[page];
            lock_group_jelatin_text.text = string.Format("{0:n0}", jelly_jelatinlist[page]);

            lock_group_jelly_img.SetNativeSize();
        }
        else
        {
            unlock_group_jelly_img.sprite = jelly_spritelist[page];
            unlock_group_name_text.text = jelly_namelist[page];
            unlock_group_gold_text.text = string.Format("{0:n0}", jelly_goldlist[page]);

            unlock_group_jelly_img.SetNativeSize();
        }
    }
    public void UpgradePageUp()
    {
        if (page >= 11)
        {
            SoundManager.instance.PlaySound("Fail");
            return;
        }

        ++page;
        UpgradeChangePage();

        SoundManager.instance.PlaySound("Button");
    }

    public void UpgradePageDown()
    {
        if (page <= 0)
        {
            SoundManager.instance.PlaySound("Fail");
            return;
        }

        --page;
        UpgradeChangePage();

        SoundManager.instance.PlaySound("Button");
    }

    void UpgradeChangePage()//강화 페이지
    {
        Upgrade_Group.gameObject.SetActive(!Upgrade_list[page]);

        if(Upgrade_Group.activeSelf)
        {
            Upgrade_Group_img.sprite = Upgrade_spritelist[page];
            Upgrade_Group_img.SetNativeSize();
        }
        else
        {

        }
    }

    public void Unlock()
    {
        if (jelatin < jelly_jelatinlist[page])
        {
            SoundManager.instance.PlaySound("Fail");
            return;
        }

        jelly_unlock_list[page] = true;
        ChangePage();

        jelatin -= jelly_jelatinlist[page];

        SoundManager.instance.PlaySound("Unlock");
    }

    public void BuyJelly()
    {
        if (gold < jelly_goldlist[page] || jelly_list.Count >= num_level * 4)
        {
            SoundManager.instance.PlaySound("Fail");
            return;
        }

        gold -= jelly_goldlist[page];

        GameObject obj = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        Jelly jelly = obj.GetComponent<Jelly>();
        obj.name = "Jelly " + page;
        jelly.id = page;
        jelly.sprite_renderer.sprite = jelly_spritelist[page];

        jelly_list.Add(jelly);

        SoundManager.instance.PlaySound("Buy");
    }

    public void NumUpgrade()
    {
        if (gold < num_gold_list[num_level])
        {
            SoundManager.instance.PlaySound("Fail");
            return;
        }

        gold -= num_gold_list[num_level++];

        num_sub_text.text = "현재 몬스터 수용량 : " + num_level * 4;

        if (num_level >= 5) num_btn.gameObject.SetActive(false);
        else num_btn_text.text = string.Format("{0:n0}", num_gold_list[num_level]);

        SoundManager.instance.PlaySound("Unlock");
    }

    public void ClickUpgrade()
    {
        if (gold < click_gold_list[click_level])
        {
            SoundManager.instance.PlaySound("Fail");
            return;
        }

        gold -= click_gold_list[click_level++];

        click_sub_text.text = "클릭 생산량 X " + click_level;

        if (click_level >= 5) click_btn.gameObject.SetActive(false);
        else click_btn_text.text = string.Format("{0:n0}", click_gold_list[click_level]);

        SoundManager.instance.PlaySound("Unlock");
    }

    void LoadData()
    {
        jelatin_text.text = jelatin.ToString();
        gold_text.text = gold.ToString();
        unlock_group_gold_text.text = jelly_goldlist[0].ToString();
        lock_group_jelatin_text.text = jelly_jelatinlist[0].ToString();

        lock_group.gameObject.SetActive(!jelly_unlock_list[page]);

        num_sub_text.text = "현재 몬스터 수용량 : " + num_level * 4;
        if (num_level >= 5) num_btn.gameObject.SetActive(false);
        else num_btn_text.text = string.Format("{0:n0}", num_gold_list[num_level]);

        click_sub_text.text = "클릭 생산량 X " + click_level;
        if (click_level >= 5) click_btn.gameObject.SetActive(false);
        else click_btn_text.text = string.Format("{0:n0}", click_gold_list[click_level]);

        for (int i = 0; i < jelly_data_list.Count; ++i)
        {
            GameObject obj = Instantiate(prefab, jelly_data_list[i].pos, Quaternion.identity);
            Jelly jelly = obj.GetComponent<Jelly>();
            jelly.id = jelly_data_list[i].id;
            jelly.level = jelly_data_list[i].level;
            jelly.exp = jelly_data_list[i].exp;
            jelly.sprite_renderer.sprite = jelly_spritelist[jelly.id];
            jelly.anim.runtimeAnimatorController = level_ac[jelly.level - 1];
            obj.name = "Jelly " + jelly.id;

            jelly_list.Add(jelly);
        }
    }

    public void Exit()
    {
        data_manager.JsonSave();

        SoundManager.instance.PlaySound("Pause Out");

        Application.Quit();
    }
}