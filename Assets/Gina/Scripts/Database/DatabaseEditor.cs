using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
public class DatabaseEditor : EditorWindow
{
    private static DatabaseEditor win;

    [MenuItem("Window/ItemDatabase")]
   public static void Open()
    {
        win = EditorWindow.GetWindow<DatabaseEditor>();
        win.titleContent = new GUIContent("Database");
        win.menu_item = 0;
        Database.Refresh();
        win.Show();
    }


    enum MenuItems { Items, Editor }
    public int menu_item = (int)MenuItems.Items;
    private Item _item = null;
    private GUIContent _itemContent = new GUIContent();
    private Vector2 sv;
    private Vector2 esv;

    private void OnGUI()
    {
        if (!win) Open();
        Repaint();

        //menu_item = GUILayout.Toolbar(menu_item, Enum.GetNames(typeof(MenuItems)));
        EditorGUILayout.BeginHorizontal();
        GUILayout.Box(((MenuItems)menu_item).ToString(), GUILayout.ExpandWidth(true));
        EditorGUI.BeginDisabledGroup(((MenuItems)menu_item) != MenuItems.Items);
        if (GUILayout.Button("+", GUILayout.ExpandWidth(false)))
        {
            _item = new Item();
            _itemContent = new GUIContent();
            menu_item = (int)MenuItems.Editor;
        }
        EditorGUI.EndDisabledGroup();
        if (GUILayout.Button("R", GUILayout.ExpandWidth(false)))
        {
            Database.Refresh();
        }
        EditorGUILayout.EndHorizontal();
        if (MenuItems.Items == (MenuItems)menu_item)
        {
            sv = EditorGUILayout.BeginScrollView(sv);
            foreach (var item in Database.GetItems)
            {
                EditorGUILayout.BeginHorizontal();
                var context = new GUIContent();
                context.image = item.Get<Texture2D>(Options.icon);
                context.text = item.Get<string>(Options.name);
                GUI.skin.box.alignment = TextAnchor.MiddleLeft;
                GUILayout.Box(context, GUILayout.Height(40), GUILayout.Width(position.width - 120));
                GUI.skin.box.alignment = TextAnchor.MiddleCenter;
                if(GUILayout.Button("Edit", GUILayout.Height(40), GUILayout.Width(40))) 
                {
                    _item = item;
                    _itemContent = new GUIContent();
                    menu_item = (int)MenuItems.Editor;
                }
                GUI.color = Color.red;
                if (GUILayout.Button("Delete", GUILayout.Height(40), GUILayout.Width(50))) { }
                GUI.color = Color.white;
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }
        else
        {
            if(_item != null)
            {
                GUILayout.Space(5);
                esv = EditorGUILayout.BeginScrollView(esv, GUILayout.Height(position.height - 60));
                CreateIconField(Options.icon, _item);
                CreateTextField(Options.name, _item);
                CreateTextField(Options.desc, _item);
                CreateEquiptmentField(_item);

                GUILayout.Box("Stats", GUILayout.ExpandWidth(true));
                CreateSlider(Options.health, _item, 0, 10);
                CreateSlider(Options.stamina, _item, 0, 10);
                CreateSlider(Options.mana, _item, 0, 10);
                CreateSlider(Options.strength, _item, 0, 10);
                CreateSlider(Options.agility, _item, 0, 10);
                CreateSlider(Options.dexterity, _item, 0, 10);

                GUILayout.Box("Equipt Item", GUILayout.ExpandWidth(true));
                CreateObjectField(Options.prefab, _item);
                EditorGUILayout.EndScrollView();
                GUILayout.BeginArea(new Rect(new Rect(5, position.height - 25, position.width - 10, 20)));
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(!_item.IsValid);
                if (GUILayout.Button("Save"))
                {
                    Database.Save(_item);
                    _item = null;
                    menu_item = 0;
                }
                EditorGUI.EndDisabledGroup();
                GUI.color = Color.red;
                if (GUILayout.Button("Cancel")) 
                {
                    menu_item = 0;
                    _item = null;
                }
                GUI.color = Color.white;
                EditorGUILayout.EndHorizontal();
                GUILayout.EndArea();
            }
        }
    }

    private void CreateTextField(Options option, Item _item, bool removeable = true)
    {
        EditorGUILayout.BeginHorizontal();
            var name = option.ToString();
            name = char.ToUpper(name[0]) + name.Substring(1);

        if (!_item.Has(option))
        {
            if (GUILayout.Button($"Add {name} Field"))
                _item.Set(option, string.Empty);
        }
        else
        {
            _item.Set(option, EditorGUILayout.TextField(name, _item.Get<string>(option)));
            if(removeable)
            {
                GUI.color = Color.red;
                if (GUILayout.Button("X", GUILayout.ExpandWidth(false)))
                    _item.Remove(option);
                GUI.color = Color.white;
            }
        }
        EditorGUILayout.EndHorizontal();
    }
    private void CreateIntSlider(Options option, Item _item, int min = 0, int max = 9999,  bool removeable = true)
    {
        EditorGUILayout.BeginHorizontal();
        var name = option.ToString();
        name = char.ToUpper(name[0]) + name.Substring(1);

        if (!_item.Has(option))
        {
            if (GUILayout.Button($"Add {name} Field"))
                _item.Set(option, min);
        }
        else
        {
            _item.Set(option, EditorGUILayout.IntSlider(name, _item.Get<int>(option), min, max));
            if (removeable)
            {
                GUI.color = Color.red;
                if (GUILayout.Button("X", GUILayout.ExpandWidth(false)))
                    _item.Remove(option);
                GUI.color = Color.white;
            }
        }
        EditorGUILayout.EndHorizontal();
    }
    private void CreateSlider(Options option, Item _item, float min = 0f, float max = 9999f, bool removeable = true)
    {
        EditorGUILayout.BeginHorizontal();
        var name = option.ToString();
        name = char.ToUpper(name[0]) + name.Substring(1);

        if (!_item.Has(option))
        {
            if (GUILayout.Button($"Add {name} Field"))
                _item.Set(option, min);
        }
        else
        {
            _item.Set(option, EditorGUILayout.Slider(name, _item.Get<float>(option), min, max));
            if (removeable)
            {
                GUI.color = Color.red;
                if (GUILayout.Button("X", GUILayout.ExpandWidth(false)))
                    _item.Remove(option);
                GUI.color = Color.white;
            }
        }
        EditorGUILayout.EndHorizontal();
    }
    private void CreateIconField(Options option, Item container, bool removeable = true)
    {
        EditorGUILayout.BeginHorizontal();
        var name = option.ToString();
        name = char.ToUpper(name[0]) + name.Substring(1);

        if (!_item.Has(option))
        {
            if (GUILayout.Button($"Add {name} Field"))
                _item.Set(option, string.Empty);
        }
        else
        {


            _itemContent.image = _item.Get<Texture2D>(option);
            if (GUILayout.Button(_itemContent, GUI.skin.box, GUILayout.Width(64), GUILayout.Height(64)))
            {
                var image_url = EditorUtility.OpenFilePanel("Select Image", Application.dataPath, "png").Replace("\\", "/");
                image_url = image_url.Replace(Application.dataPath + "/", string.Empty);

                if (!string.IsNullOrEmpty(image_url))
                    _item.Set(option, image_url);
            }

            if (removeable)
            {
                GUI.color = Color.red;
                if (GUILayout.Button("X", GUILayout.ExpandWidth(false)))
                    _item.Remove(option);
                GUI.color = Color.white;
            }
        }
        EditorGUILayout.EndHorizontal();
    }
    private void CreateToggleField(Options option, Item _item, bool removeable = true)
    {
        EditorGUILayout.BeginHorizontal();
        var name = option.ToString();
        name = char.ToUpper(name[0]) + name.Substring(1);

        if (!_item.Has(option))
        {
            if (GUILayout.Button($"Add {name} Field"))
                _item.Set(option, false);
        }
        else
        {
            _item.Set(option, EditorGUILayout.Toggle(name, _item.Get<bool>(option)));
            if (removeable)
            {
                GUI.color = Color.red;
                if (GUILayout.Button("X", GUILayout.ExpandWidth(false)))
                    _item.Remove(option);
                GUI.color = Color.white;
            }
        }
        EditorGUILayout.EndHorizontal();
    }
    private void CreateEquiptmentField(Item container, bool removeable = true)
    {
        var name = "Equipment";
        name = char.ToUpper(name[0]) + name.Substring(1);

        if (!_item.Has(Options.isEquipable) || !_item.Has(Options.equipmentType) || !_item.Has(Options.requireLevel))
        {
            if (GUILayout.Button($"Add {name} Field"))
            {
                _item.Set(Options.isEquipable, false);
                _item.Set(Options.equipmentType, (int)EquiptmentType.None);
                _item.Set(Options.requireLevel, 1);
            }
        }
        else
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Box(name, GUILayout.ExpandWidth(true));
            if (removeable)
            {
                GUI.color = Color.red;
                if (GUILayout.Button("X", GUILayout.ExpandWidth(false)))
                {
                    _item.Remove(Options.isEquipable);
                    _item.Remove(Options.equipmentType);
                    _item.Remove(Options.requireLevel);
                }
                GUI.color = Color.white;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Type / Equipable");
            if(_item.Has(Options.isEquipable))
                _item.Set(Options.equipmentType, (int)(EquiptmentType)EditorGUILayout.EnumPopup(new GUIContent(string.Empty, "Is Equipable"), (EquiptmentType)_item.Get<int>(Options.equipmentType)));
            if (_item.Has(Options.equipmentType))
                _item.Set(Options.isEquipable, EditorGUILayout.Toggle(new GUIContent(string.Empty, "Equipment Type"),_item.Get<bool>(Options.isEquipable), GUILayout.Width(20)));
            EditorGUILayout.EndHorizontal();
            if (_item.Has(Options.requireLevel))
                _item.Set(Options.requireLevel, EditorGUILayout.IntSlider("Require Level", _item.Get<int>(Options.requireLevel), 1, GameManager.maxLevel));
        }
    }
    private void CreateObjectField(Options option, Item _item, bool removeable = true)
    {
        var name = option.ToString();
        name = char.ToUpper(name[0]) + name.Substring(1);

        if (!_item.Has(option))
        {
            if (GUILayout.Button($"Add {name} Field"))
                _item.Set(option, string.Empty);
        }
        else
        {
            EditorGUILayout.BeginHorizontal();
            _item.Set(option, EditorGUILayout.TextField(name, _item.Get<string>(option)));
            if (removeable)
            {
                GUI.color = Color.red;
                if (GUILayout.Button("X", GUILayout.ExpandWidth(false)))
                    _item.Remove(option);
                GUI.color = Color.white;
            }
        EditorGUILayout.EndHorizontal();
            Database.IsValidObject(_item.Get<string>(option));
        }
        
    }
}
#endif