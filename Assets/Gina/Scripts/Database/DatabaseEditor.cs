using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Object = UnityEngine.Object;
using System.IO;
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
    private bool _itemErrors;
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
                context.image = item.Get<Texture2D>(paramname.icon);
                context.text = item.Get<string>(paramname.name);
                GUI.skin.box.alignment = TextAnchor.MiddleLeft;
                GUILayout.Box(context, GUILayout.Height(40), GUILayout.Width(position.width - 180));
                GUI.skin.box.alignment = TextAnchor.MiddleCenter;
                if(GUILayout.Button("Edit", GUILayout.Height(40), GUILayout.Width(40))) 
                {
                    _item = item;
                    _itemContent = new GUIContent();
                    menu_item = (int)MenuItems.Editor;
                }
                GUI.color = Color.yellow;
                if (GUILayout.Button("Duplicate", GUILayout.Height(40), GUILayout.Width(70)))
                {
                    _item = Database.Duplicate(item);
                    _itemContent = new GUIContent();
                    menu_item = (int)MenuItems.Editor;
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
            if(_item != null)
            {
                GUILayout.Space(5);
                esv = EditorGUILayout.BeginScrollView(esv, GUILayout.Height(position.height - 60));
                try
                {
                    CreateIconField(paramname.icon, _item);
                    CreateTextField(paramname.name, _item);
                    CreateTextField(paramname.desc, _item);
                    CreateEquiptmentField(_item);
                    CreateIntMinMaxField(paramname.curStack, paramname.maxStack, _item, 1, 64);
                    GUILayout.Box("Stats", GUILayout.ExpandWidth(true));
                    CreateSlider(paramname.health, _item, 0, 10);
                    CreateSlider(paramname.stamina, _item, 0, 10);
                    CreateSlider(paramname.mana, _item, 0, 10);
                    CreateSlider(paramname.strength, _item, 0, 10);
                    CreateSlider(paramname.agility, _item, 0, 10);
                    CreateSlider(paramname.dexterity, _item, 0, 10);

                    GUILayout.Box("Equipt Item", GUILayout.ExpandWidth(true));
                    CreateToggleField(paramname.isEquipable, _item);
                    CreateObjectField(paramname.prefab, _item);

                    CreateFloatField(paramname.worth, _item);

                    _itemErrors = false;
                }
                catch { _itemErrors = true; }
                EditorGUILayout.EndScrollView();
                GUILayout.BeginArea(new Rect(new Rect(5, position.height - 25, position.width - 10, 20)));
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(!_item.IsValid || _itemErrors);
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

    private void CreateTextField(paramname option, Item _item, bool removeable = true)
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
    private void CreateFloatField(paramname option, Item _item, bool removeable = true)
    {
        EditorGUILayout.BeginHorizontal();
        var name = option.ToString();
        name = char.ToUpper(name[0]) + name.Substring(1);

        if (!_item.Has(option))
        {
            if (GUILayout.Button($"Add {name} Field"))
                _item.Set(option, 0f);
        }
        else
        {
            _item.Set(option, EditorGUILayout.FloatField(name, _item.Get<float>(option)));
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
    private void CreateIntSlider(paramname option, Item _item, int min = 0, int max = 9999,  bool removeable = true)
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
    private void CreateSlider(paramname option, Item _item, float min = 0f, float max = 9999f, bool removeable = true)
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
    private void CreateIconField(paramname option, Item container, bool removeable = true)
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
                var startpath = string.IsNullOrEmpty(_item.Get<string>(option)) ? Application.dataPath : _item.Get<string>(option);
                var image_url = EditorUtility.OpenFilePanel("Select Image", startpath, "png").Replace("\\", "/");
                image_url = image_url.Replace(Application.dataPath + "/", string.Empty);

                if (!string.IsNullOrEmpty(image_url))
                    _item.Set(option, image_url);
                _item._texture = null;
                _item._sprite = null;
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
    private void CreateToggleField(paramname option, Item _item, bool removeable = true)
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

        if (!_item.Has(paramname.isEquipable) || !_item.Has(paramname.equipmentType) || !_item.Has(paramname.requireLevel))
        {
            if (GUILayout.Button($"Add {name} Field"))
            {
                _item.Set(paramname.isEquipable, false);
                _item.Set(paramname.equipmentType, (int)EquiptmentType.None);
                _item.Set(paramname.requireLevel, 1);
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
                    _item.Remove(paramname.isEquipable);
                    _item.Remove(paramname.equipmentType);
                    _item.Remove(paramname.requireLevel);
                }
                GUI.color = Color.white;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Type / Equipable");
            if(_item.Has(paramname.isEquipable))
                _item.Set(paramname.equipmentType, (int)(EquiptmentType)EditorGUILayout.EnumPopup(new GUIContent(string.Empty, "Is Equipable"), (EquiptmentType)_item.Get<int>(paramname.equipmentType)));
            if (_item.Has(paramname.equipmentType))
                _item.Set(paramname.isEquipable, EditorGUILayout.Toggle(new GUIContent(string.Empty, "Equipment Type"),_item.Get<bool>(paramname.isEquipable), GUILayout.Width(20)));
            EditorGUILayout.EndHorizontal();
            if (_item.Has(paramname.requireLevel))
                _item.Set(paramname.requireLevel, EditorGUILayout.IntSlider("Require Level", _item.Get<int>(paramname.requireLevel), 1, GameManager.maxLevel));
        }
    }
    private void CreateObjectField(paramname option, Item _item, bool removeable = true)
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

    private void CreateIntMinMaxField(paramname option1, paramname option2, Item _item, int min, int max, bool removeable = true)
    {
        EditorGUILayout.BeginHorizontal();
        var _name1 = option1.ToString();
        _name1 = char.ToUpper(_name1[0]) + _name1.Substring(1);
        var _name2 = option2.ToString();
        _name2 = char.ToUpper(_name2[0]) + _name2.Substring(1);

        var name = $"{_name1}/{_name2}";

        if (!_item.Has(option1) || !_item.Has(option2))
        {
            if (GUILayout.Button($"Add {name} Field"))
            {
                _item.Set(option1, min);
                _item.Set(option2, max);
            }
        }
        else
        {
            var minref = (float)_item.Get<int>(option1);
            var maxref = (float)_item.Get<int>(option2);
            EditorGUILayout.BeginHorizontal();
            minref = EditorGUILayout.IntField((int)minref, GUILayout.Width(40));
            GUILayout.Space(5);
            EditorGUILayout.MinMaxSlider(ref minref, ref maxref, (float)min, (float)max);
            GUILayout.Space(5);
            maxref = EditorGUILayout.IntField((int)maxref, GUILayout.Width(40));
            EditorGUILayout.EndHorizontal();
            _item.Set(option1, (int)minref);
            _item.Set(option2, (int)maxref);
            if (removeable)
            {
                GUI.color = Color.red;
                if (GUILayout.Button("X", GUILayout.ExpandWidth(false)))
                {
                    _item.Remove(option1);
                    _item.Remove(option2);
                }
                GUI.color = Color.white;
            }
        }
        EditorGUILayout.EndHorizontal();
    }
    private void CreateMinMaxField(paramname option1, paramname option2, Item _item, float min, float max, bool removeable = true)
    {
        EditorGUILayout.BeginHorizontal();
        var _name1 = option1.ToString();
        _name1 = char.ToUpper(_name1[0]) + _name1.Substring(1);
        var _name2 = option2.ToString();
        _name2 = char.ToUpper(_name2[0]) + _name2.Substring(1);

        var name = $"{_name1}/{_name2}";


        if (!_item.Has(option1) || !_item.Has(option2))
        {
            if (GUILayout.Button($"Add {name} Field"))
            {
                _item.Set(option1, min);
                _item.Set(option2, max);
            }
        }
        else
        {
            var minref = _item.Get<float>(option1);
            var maxref = _item.Get<float>(option2);
            EditorGUILayout.BeginHorizontal();
            minref = EditorGUILayout.FloatField(minref, GUILayout.Width(40));
            GUILayout.Space(5);
            EditorGUILayout.MinMaxSlider(ref minref, ref maxref, min, max);
            GUILayout.Space(5);
            maxref = EditorGUILayout.FloatField(maxref, GUILayout.Width(40));
            EditorGUILayout.EndHorizontal();

            _item.Set(option1, minref);
            _item.Set(option2, maxref);



            if (removeable)
            {
                GUI.color = Color.red;
                if (GUILayout.Button("X", GUILayout.ExpandWidth(false)))
                {
                    _item.Remove(option1);
                    _item.Remove(option2);
                }
                GUI.color = Color.white;
            }
        }
        EditorGUILayout.EndHorizontal();
    }
}
#endif