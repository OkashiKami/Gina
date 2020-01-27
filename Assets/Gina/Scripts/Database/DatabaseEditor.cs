using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Object = UnityEngine.Object;
using System.IO;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
public class DatabaseEditor : EditorWindow
{
    private static DatabaseEditor win;

    [MenuItem("Window/Database")]
   public static void Open()
    {
        win = EditorWindow.GetWindow<DatabaseEditor>();
        win.titleContent = new GUIContent("Database");
        win.menu_item = 0;
        Database.Refresh();
        win.Show();
    }


    public enum MenuItems { Items, LootTables, Quests }
    public bool _edit = false;

    public MenuItems menu_item;
    private Item _item = null;
    private LootTable _loot = null;
    private Quest _quest = null;
    private bool _itemErrors;
    private GUIContent _itemContent = new GUIContent();
    private Vector2 sv;
    private Vector2 esv;

    private void OnGUI()
    {
        if (!win) Open();
        Repaint();
        OnGUIHeader();
        switch (menu_item)
        {
            case MenuItems.Items: OnGUIItems(_edit); break;
            case MenuItems.LootTables: OnGUILoot(_edit); break;
            case MenuItems.Quests: OnGUIQuest(_edit); break;
        }
        OnGUIFooter(_edit);
    }

    private void OnGUIHeader()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginDisabledGroup(_edit);
        menu_item = (MenuItems)GUILayout.Toolbar((int)menu_item, Enum.GetNames(typeof(MenuItems)));
        EditorGUI.EndDisabledGroup();
        EditorGUI.BeginDisabledGroup(Database.refreshing);
        if (GUILayout.Button("R", GUILayout.ExpandWidth(false)))
        {
            Database.Refresh();
        }
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Box(menu_item.ToString(), GUILayout.ExpandWidth(true));
        EditorGUI.BeginDisabledGroup(_edit != false);
        if (GUILayout.Button("+", GUILayout.ExpandWidth(false)))
        {
            switch(menu_item)
            {
                case MenuItems.Items:
                    _item = new Item();
                    _itemContent = new GUIContent();
                    _edit = true;
                    break;
                case MenuItems.LootTables:
                    _loot = new LootTable();
                    _itemContent = new GUIContent();
                    _edit = true;
                    break;
                case MenuItems.Quests:
                    _quest = new Quest();
                    _itemContent = new GUIContent();
                    _edit = true;
                    break;
            }
        }
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndHorizontal();
        
    }

    private void OnGUIItems(bool edit = false)
    {
        if (!edit)
        {
            sv = EditorGUILayout.BeginScrollView(sv, GUILayout.Height(position.height - 60));
            foreach (var item in Database.GetAll<Item>(auto:false))
            {
                EditorGUILayout.BeginHorizontal();
                var context = new GUIContent();
                context.image = item.Texture;
                context.text = item.name + "\n" + item.GetID;
                GUI.skin.box.alignment = TextAnchor.MiddleLeft;
                GUILayout.Box(context, GUILayout.Height(40), GUILayout.Width(position.width - 190));
                GUI.skin.box.alignment = TextAnchor.MiddleCenter;
                if (GUILayout.Button("Edit", GUILayout.Height(40), GUILayout.Width(40)))
                {
                    _item = item;
                    _itemContent = new GUIContent();
                    _edit = true;
                }
                GUI.color = Color.yellow;
                if (GUILayout.Button("Duplicate", GUILayout.Height(40), GUILayout.Width(70)))
                {
                    _item = Database.Duplicate(item);
                    _itemContent = new GUIContent();
                    _edit = true;
                }
                GUI.color = Color.red;
                if (GUILayout.Button("Delete", GUILayout.Height(40), GUILayout.Width(50)))
                    Database.Delete(item);
                GUI.color = Color.white;
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }
        else
        {
            esv = EditorGUILayout.BeginScrollView(esv, GUILayout.Height(position.height - 80));
            EditorGUILayout.BeginVertical(GUILayout.Width(position.width - 20));
            try
            {
                _itemContent.image = _item.Texture;
                if (GUILayout.Button(_itemContent, GUI.skin.box, GUILayout.Width(64), GUILayout.Height(64)))
                {
                    var startpath = string.IsNullOrEmpty(_item.icon) ? Application.dataPath : _item.icon;
                    var image_url = EditorUtility.OpenFilePanel("Select Image", startpath, "png").Replace("\\", "/");
                    image_url = image_url.Replace(Application.dataPath + "/", string.Empty);

                    if (!string.IsNullOrEmpty(image_url))
                        _item.icon = image_url;
                    _item._texture = null;
                    _item._sprite = null;
                }

                _item.name = EditorGUILayout.TextField("Name", _item.name);
                EditorGUILayout.PrefixLabel("Desc");
                _item.desc = EditorGUILayout.TextArea(_item.desc);

                _item.isEquipment = EditorGUILayout.Toggle("IS Equipment", _item.isEquipment);
                if (_item.isEquipment)
                    _item.equipmentType = (EquiptmentType)EditorGUILayout.EnumPopup("Equipment Type", _item.equipmentType);
                _item.isStackable = EditorGUILayout.Toggle("Is Stackable", _item.isStackable);
                if(_item.isStackable)
                    _item.maxStack = EditorGUILayout.IntSlider("Max Stack", _item.maxStack, _item.curStack, 64);

                GUILayout.Box("Stats", GUILayout.ExpandWidth(true));
                _item.healthBonus = EditorGUILayout.Slider("Health+", _item.healthBonus, 0, 10);
                _item.staminaBonus = EditorGUILayout.Slider("Stamina+", _item.staminaBonus, 0, 10);
                _item.manaBonus = EditorGUILayout.Slider("Mana+", _item.manaBonus, 0, 10);
                _item.expBonus = EditorGUILayout.Slider("Exp+", _item.expBonus, 0, 10);
                GUILayout.Space(10);
                _item.strengthBonus = EditorGUILayout.Slider("Strength+", _item.strengthBonus, 0, 10);
                _item.agilityBonus = EditorGUILayout.Slider("Agility+", _item.agilityBonus, 0, 10);
                _item.dexterityBonus = EditorGUILayout.Slider("Dexterity+", _item.dexterityBonus, 0, 10);
                _item.worth = EditorGUILayout.FloatField("Worth", _item.worth);

                _item._object = AssetDatabase.LoadAssetAtPath(_item.prefab, typeof(GameObject));
                _item.prefab = AssetDatabase.GetAssetPath(EditorGUILayout.ObjectField("Prefab", _item._object, typeof(GameObject), true));

                _itemErrors = false;
            }
            catch { _itemErrors = true; }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
    }
    private void OnGUILoot(bool edit = false)
    {
        if(!edit)
        {
            sv = EditorGUILayout.BeginScrollView(sv, GUILayout.Height(position.height - 60));
            foreach (var loot in Database.GetAll<LootTable>(auto: false))
            {
                EditorGUILayout.BeginHorizontal();
                var context = new GUIContent();
                context.image = Resources.Load<Texture2D>("Textures/gina");
                context.text = loot.name + "\n" + loot.GetID;
                GUI.skin.box.alignment = TextAnchor.MiddleLeft;
                GUILayout.Box(context, GUILayout.Height(40), GUILayout.Width(position.width - 190));
                GUI.skin.box.alignment = TextAnchor.MiddleCenter;
                if (GUILayout.Button("Edit", GUILayout.Height(40), GUILayout.Width(40)))
                {
                    _loot = loot;
                    _itemContent = new GUIContent();
                    _edit = true;
                }
                GUI.color = Color.yellow;
                if (GUILayout.Button("Duplicate", GUILayout.Height(40), GUILayout.Width(70)))
                {
                    _loot = Database.Duplicate(loot);
                    _itemContent = new GUIContent();
                    _edit = true;
                }
                GUI.color = Color.red;
                if (GUILayout.Button("Delete", GUILayout.Height(40), GUILayout.Width(50)))
                    Database.Delete(loot);
                GUI.color = Color.white;
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }
        else
        {
            esv = EditorGUILayout.BeginScrollView(esv, GUILayout.Height(position.height - 80));
            EditorGUILayout.BeginVertical(GUILayout.Width(position.width - 20));
            try
            {
                EditorGUILayout.LabelField(_loot.file);
                _loot.name = EditorGUILayout.TextField("Name", _loot.name);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Box("Loot Items", GUILayout.ExpandWidth(true));
                if(GUILayout.Button("+", GUILayout.ExpandWidth(false)))
                    _loot.items.Add(new LootEntry());
                EditorGUILayout.EndHorizontal();
                for (int i = 0; i < _loot.items.Count; i++)
                {
                    var entry = _loot.items[i];
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("Item / Drop %");
                    GUI.color = Database.Get<Item>(entry.item) != null ? Color.green : Color.white;
                    entry.item = EditorGUILayout.TextField(entry.item);
                    GUI.color = Color.white;
                    entry.dropPercentage = EditorGUILayout.IntField(entry.dropPercentage, GUILayout.Width(50));
                    GUI.color = Color.red;
                    if(GUILayout.Button("X", GUILayout.ExpandWidth(false)))
                        _loot.items.RemoveAt(i);
                    GUI.color = Color.white;

                    EditorGUILayout.EndHorizontal();
                    if (i < _loot.items.Count)
                        GUILayout.Space(5);
                }

                _itemErrors = false;
            }
            catch (Exception ex) { _itemErrors = true; Debug.LogError(ex.StackTrace); }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
    }
    private void OnGUIQuest(bool edit = false)
    {
        if (!edit)
        {
            sv = EditorGUILayout.BeginScrollView(sv, GUILayout.Height(position.height - 60));
            foreach (var quest in Database.GetAll<Quest>(auto: false))
            {
                EditorGUILayout.BeginHorizontal();
                var context = new GUIContent();
                context.image = Resources.Load<Texture2D>("Textures/gina");
                context.text = quest.title + "\n" + quest.GetID;
                GUI.skin.box.alignment = TextAnchor.MiddleLeft;
                GUILayout.Box(context, GUILayout.Height(40), GUILayout.Width(position.width - 190));
                GUI.skin.box.alignment = TextAnchor.MiddleCenter;
                if (GUILayout.Button("Edit", GUILayout.Height(40), GUILayout.Width(40)))
                {
                    _quest = quest;
                    _itemContent = new GUIContent();
                    _edit = true;
                }
                GUI.color = Color.yellow;
                if (GUILayout.Button("Duplicate", GUILayout.Height(40), GUILayout.Width(70)))
                {
                    _quest = Database.Duplicate(quest);
                    _itemContent = new GUIContent();
                    _edit = true;
                }
                GUI.color = Color.red;
                if (GUILayout.Button("Delete", GUILayout.Height(40), GUILayout.Width(50)))
                    Database.Delete(quest);
                GUI.color = Color.white;
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }
        else
        {
            esv = EditorGUILayout.BeginScrollView(esv, GUILayout.Height(position.height - 80));
            EditorGUILayout.BeginVertical(GUILayout.Width(position.width - 20));
            try
            {
                EditorGUILayout.LabelField(_quest.file);
                _quest.title = EditorGUILayout.TextField("Title", _quest.title);
                EditorGUILayout.PrefixLabel("Objective");
                _quest.background = EditorGUILayout.TextArea(_quest.background);

                EditorGUILayout.BeginHorizontal();
                GUILayout.Box("Objective", GUILayout.ExpandWidth(true));
                if (GUILayout.Button("+", GUILayout.ExpandWidth(false)))
                    _quest.objectives.Add(new QuestObjective());
                EditorGUILayout.EndHorizontal();
                for (int i = 0; i < _quest.objectives.Count; i++)
                {
                    var entry = _quest.objectives[i];
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.BeginVertical();
                    entry.verb = EditorGUILayout.TextField("Verb", entry.verb);
                    entry.amount = EditorGUILayout.IntField("Amount", entry.amount);
                    entry.what = EditorGUILayout.TextField("What", entry.what);
                    EditorGUILayout.EndVertical();

                    GUI.color = Color.red;
                    if (GUILayout.Button("X", GUILayout.Height(60), GUILayout.ExpandWidth(false)))
                        _quest.objectives.RemoveAt(i);
                    GUI.color = Color.white;
                    EditorGUILayout.EndHorizontal();
                    if (i < _quest.objectives.Count)
                        GUILayout.Space(5);
                }



                EditorGUILayout.BeginHorizontal();
                GUILayout.Box("Rewards", GUILayout.ExpandWidth(true));
                if (GUILayout.Button("+", GUILayout.ExpandWidth(false)))
                    _quest.reward.Add(new QuestReward());
                EditorGUILayout.EndHorizontal();
                for (int i = 0; i < _quest.reward.Count; i++)
                {
                    var entry = _quest.reward[i];
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.BeginVertical();
                    GUI.color = Database.Get<Item>(entry.item) != null ? Color.green : Color.white;
                    entry.item = EditorGUILayout.TextField("Item", entry.item);
                    GUI.color = Color.white;
                    entry.amount = EditorGUILayout.IntSlider("Amount", entry.amount, 1, 64);
                    EditorGUILayout.EndVertical();
                    
                    GUI.color = Color.red;
                    if (GUILayout.Button("X", GUILayout.Height(40), GUILayout.ExpandWidth(false)))
                        _quest.reward.RemoveAt(i);
                    GUI.color = Color.white;
                    EditorGUILayout.EndHorizontal();
                    if (i < _quest.reward.Count)
                        GUILayout.Space(5);
                }

                _itemErrors = false;
            }
            catch (Exception ex) { _itemErrors = true; Debug.LogError(ex.StackTrace); }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
    }

    private void OnGUIFooter(bool edit = false)
    {
        if (!edit) return;
        switch(menu_item)
        {
            case MenuItems.Items:
                GUILayout.BeginArea(new Rect(new Rect(5, position.height - 25, position.width - 10, 20)));
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(!_item.IsValid || _itemErrors);
                if (GUILayout.Button("Save"))
                {
                    _edit = false;
                    Database.Save(_item);
                    _item = null;
                }
                EditorGUI.EndDisabledGroup();
                GUI.color = Color.red;
                if (GUILayout.Button("Cancel"))
                {
                    _edit = false;
                    _item = null;
                }
                GUI.color = Color.white;
                EditorGUILayout.EndHorizontal();
                GUILayout.EndArea();
                break;
            case MenuItems.LootTables:
                GUILayout.BeginArea(new Rect(new Rect(5, position.height - 25, position.width - 10, 20)));
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(_loot == null || _itemErrors);
                if (GUILayout.Button("Save"))
                {
                    _edit = false;
                    Database.Save(_loot);
                    _loot = null;
                }
                EditorGUI.EndDisabledGroup();
                GUI.color = Color.red;
                if (GUILayout.Button("Cancel"))
                {
                    _edit = false;
                    _loot = null;
                }
                GUI.color = Color.white;
                EditorGUILayout.EndHorizontal();
                GUILayout.EndArea();
                break;
            case MenuItems.Quests:
                GUILayout.BeginArea(new Rect(new Rect(5, position.height - 25, position.width - 10, 20)));
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(_loot == null || _itemErrors);
                if (GUILayout.Button("Save"))
                {
                    _edit = false;
                    Database.Save(_quest);
                    _quest = null;
                }
                EditorGUI.EndDisabledGroup();
                GUI.color = Color.red;
                if (GUILayout.Button("Cancel"))
                {
                    _edit = false;
                    _quest = null;
                }
                GUI.color = Color.white;
                EditorGUILayout.EndHorizontal();
                GUILayout.EndArea();
                break;
        }
    }
    
}
#endif