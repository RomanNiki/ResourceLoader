using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using Tools.Attributes.TabGroupCollection;
using UnityEngine;
using Object = UnityEngine.Object;
using SerializationUtility = Sirenix.Serialization.SerializationUtility;
#if UNITY_EDITOR && ODIN_INSPECTOR && UNITY_2021_1_OR_NEWER
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.ActionResolvers;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using NamedValue = Sirenix.OdinInspector.Editor.ActionResolvers.NamedValue;
using Sirenix.Utilities.Editor;
using UnityEditor;
#endif

#if UNITY_EDITOR && ODIN_INSPECTOR && UNITY_2021_1_OR_NEWER

namespace Tools.Editor.Attributes.TabGroupCollection
{
    using Object = Object;
    using SerializationUtility = SerializationUtility;

    /// <summary>
    ///     Drawer for <see cref="TabGroupCollectionAttribute" />.
    /// </summary>
    public sealed class TabGroupCollectionAttributeDrawer<T> : OdinAttributeDrawer<TabGroupCollectionAttribute, T>
    {
        #region Utility Methods

        private void ShiftTabs(int amount)
        {
            //do nothing if collection size is 0 or 1
            if (collectionCount == 0 || collectionCount == 1)
            {
                return;
            }

            if (amount < 0)
            {
                int posAmt = Mathf.Abs(amount); //get positive value of amount

                if (StartIndex == 0) //if we're already at the start, tab back by amount
                {
                    SelectedIndex -= SelectedIndex - posAmt < 0 ? SelectedIndex : posAmt;
                    tabGroup.GoToPage(tabs[SelectedIndex]);
                }
                else //otherwise we shift start and end indexs back by amount
                {
                    int tempStart = StartIndex; //cache current start index

                    //start is either equal to zero if tabbing would go lower than index 0,
                    //or subtract the positive amount
                    StartIndex = StartIndex - posAmt < 0 ? 0 : StartIndex - posAmt;

                    //work out difference, and slide selected and start indicies by the dif, then go to current selected index
                    int dif = tempStart - StartIndex;
                    SelectedIndex -= dif;
                    EndIndex -= dif;
                    tabGroup.GoToPage(tabs[SelectedIndex]);
                }
            }
            else if (amount > 0)
            {
                if (EndIndex == LastAvailableIndex) //if we're already at the last index tab along by amount
                {
                    SelectedIndex += SelectedIndex + amount > EndIndex ? EndIndex - SelectedIndex : amount;
                    tabGroup.GoToPage(tabs[SelectedIndex]);
                }
                else //otherwise shift tabs as normal
                {
                    int tempEnd = EndIndex; //cache current end index

                    //add either the difference between end and collection size (if tabbing greater than end of collection)
                    //or merely add the amount provided
                    EndIndex += EndIndex + amount >= collectionCount ? collectionCount - EndIndex - 1 : amount;

                    //work out difference then slide selected and start indicies by the dif, then go to current selected index
                    int dif = EndIndex - tempEnd;
                    SelectedIndex += dif;
                    StartIndex += dif;
                    tabGroup.GoToPage(tabs[SelectedIndex]);
                }
            }
        }

        #endregion

        #region Internal Classes

        private sealed class TabGroupCollectionConfigInfo
        {
            #region Constructor

            public TabGroupCollectionConfigInfo(InspectorProperty property, TabGroupCollectionAttribute tgc,
                ListDrawerSettingsAttribute lds)
            {
                //general properties
                GroupName = property.GetAttribute<LabelTextAttribute>()?.Text ?? property.NiceName;
                HideLabel = property.Attributes.HasAttribute<HideLabelAttribute>();

                //tab group collection
                Property = property;

                AlwaysSelectNewElement = tgc.AlwaysSelectNewElement;

                if (tgc.HasAlwaysDrawNavigationButtons)
                {
                    AlwaysDrawNavigationButtons = tgc.AlwaysDrawNavigationButtons;
                }

                if (tgc.HasMaximumNumberOfTabs)
                {
                    MaximumNumberOfTabs = tgc.MaximumNumberOfTabs;
                }

                NavigationTabAmount = tgc.HasNavigationTabAmount ? tgc.NavigationTabAmount : MaximumNumberOfTabs;

                IsReadOnly = Property.Attributes.HasAttribute<ReadOnlyAttribute>() ||
                             property.ValueEntry.IsEditable == false;

                //list drawer settings
                ListElementLabelName = lds.ListElementLabelName;
                CustomAddFunction = lds.CustomAddFunction;
                CustomRemoveElementFunction = lds.CustomRemoveElementFunction;
                CustomRemoveIndexFunction = lds.CustomRemoveIndexFunction;
                OnBeginListElementGUI = lds.OnBeginListElementGUI;
                OnEndListElementGUI = lds.OnEndListElementGUI;
                ElementColour = lds.ElementColor;
                OnTitleBarGUI = lds.OnTitleBarGUI;

                if (lds.NumberOfItemsPerPageHasValue && !HasMaximumNumberOfTabs) //only assign if not assigned in TGC
                {
                    MaximumNumberOfTabs = lds.NumberOfItemsPerPage;
                }

                if (lds.PagingHasValue && !HasAlwaysDrawNavigationButtons)
                {
                    AlwaysDrawNavigationButtons = lds.ShowPaging;
                }

                if (lds.ShowIndexLabelsHasValue)
                {
                    ShowIndexLabels = lds.ShowIndexLabels;
                }

                HideAddButton = lds.HideAddButton;
                HideRemoveButton = lds.HideRemoveButton;

                AddCopiesLastElement = lds.AddCopiesLastElement;
                AlwaysAddDefaultValue = lds.AlwaysAddDefaultValue;

                ShowItemCount = lds.ShowItemCount;
                Expanded = lds.Expanded;
                IsReadOnly = IsReadOnly == false && lds.IsReadOnly;
            }

            #endregion

            #region Internal Fields

            private int _maximumNumberOfTabs = 5;
            private bool _alwaysDrawNavigationButtons;

            #endregion

            #region General Properties

            public string GroupName { get; }
            public bool HideLabel { get; }

            #endregion

            #region Tab Group Collection Properties

            public InspectorProperty Property { get; }

            //Tab Group Collection
            public bool AlwaysSelectNewElement { get; } = true;
            public int NavigationTabAmount { get; }

            public bool AlwaysDrawNavigationButtons
            {
                get => _alwaysDrawNavigationButtons;
                set
                {
                    HasAlwaysDrawNavigationButtons = true;
                    _alwaysDrawNavigationButtons = value;
                }
            }

            public bool HasAlwaysDrawNavigationButtons { get; private set; }

            public int MaximumNumberOfTabs
            {
                get => _maximumNumberOfTabs;
                set
                {
                    if (value > 0)
                    {
                        HasMaximumNumberOfTabs = true;
                        _maximumNumberOfTabs = value;
                    }
                }
            }

            public bool HasMaximumNumberOfTabs { get; private set; }

            #endregion

            #region List Drawer Settings Properties

            //List Drawer Settings
            public bool AddCopiesLastElement { get; }
            public bool AlwaysAddDefaultValue { get; }
            public bool IsReadOnly { get; }
            public bool ShowItemCount { get; }
            public bool Expanded { get; }
            public bool ShowIndexLabels { get; } = true;

            public string ListElementLabelName { get; }
            public string CustomAddFunction { get; }
            public string CustomRemoveElementFunction { get; }
            public string CustomRemoveIndexFunction { get; }
            public string OnBeginListElementGUI { get; }
            public string OnEndListElementGUI { get; }
            public string ElementColour { get; }
            public string OnTitleBarGUI { get; }

            public bool HideAddButton { get; }
            public bool HideRemoveButton { get; }

            #endregion
        }

        #endregion

        #region Internal Members

        private TabGroupCollectionAttribute tgc;
        private TabGroupCollectionConfigInfo info;

        private GUITabGroup tabGroup;
        private List<GUITabPage> tabs;
        private bool queueRebuildTabs;

        private ICollectionResolver collectionResolver;
        private int collectionCount;

        private LocalPersistentContext<int> persistantSelectedIndex;
        private LocalPersistentContext<int> persistantStartIndex;
        private LocalPersistentContext<int> persistantEndIndex;

        private ObjectPicker objectPicker;

        #endregion

        #region Action Resolvers

        private ValueResolver customAddValue;
        private ValueResolver<Color> elementColour;
        private ValueResolver listElementLabelName;

        private ActionResolver customAddResolver;
        private ActionResolver customRemoveElementResolver;
        private ActionResolver customRemoveIndexResolver;
        private ActionResolver onBeginListElementGUI;
        private ActionResolver onEndListElementGUI;
        private ActionResolver onTitleBarGUI;

        #endregion

        #region Properties

        private int SelectedIndex
        {
            get => persistantSelectedIndex.Value;
            set => persistantSelectedIndex.Value = value;
        }

        private int StartIndex
        {
            get => persistantStartIndex.Value;
            set => persistantStartIndex.Value = value;
        }

        private int EndIndex
        {
            get => persistantEndIndex.Value;
            set => persistantEndIndex.Value = value;
        }

        /// <summary>
        ///     Returns the last index available from the collection.
        /// </summary>
        private int LastAvailableIndex => collectionCount - 1;

        #endregion

        #region Drawer Overrides

        protected override bool CanDrawAttributeValueProperty(InspectorProperty property)
        {
            return property.ChildResolver is ICollectionResolver;
        }

        protected override void Initialize()
        {
            tgc = Attribute;

            collectionResolver = Property.ChildResolver as ICollectionResolver;
            collectionResolver.OnAfterChange += change =>
            {
                Property.Children.Update();
                queueRebuildTabs = true; //subscribe rebuilds to collection resolver updates
            };

            collectionCount = Property.ChildResolver.ChildCount;

            ListDrawerSettingsAttribute lds = Property.Attributes.GetAttribute<ListDrawerSettingsAttribute>() ??
                                              new ListDrawerSettingsAttribute();
            info = new TabGroupCollectionConfigInfo(Property, tgc, lds);

            //get persistant values
            string key = ((Object)Property.SerializationRoot.ValueEntry.WeakSmartValue).name + "tgc";

            persistantSelectedIndex = this.GetPersistentValue($"{key}selected", 0);
            persistantStartIndex = this.GetPersistentValue($"{key}start", 0);

            int end = collectionCount > info.MaximumNumberOfTabs ? info.MaximumNumberOfTabs - 1 : LastAvailableIndex;
            persistantEndIndex = this.GetPersistentValue($"{key}end", end);

            //validate persistant values in case the collection has changed for some reason
            if (EndIndex > LastAvailableIndex) //less list elements
            {
                int dif = EndIndex - end;
                EndIndex = end;
                StartIndex -= StartIndex - dif < 0 ? StartIndex : dif;
                SelectedIndex -= SelectedIndex - dif < 0 ? SelectedIndex : dif;
            }

            //get all (but one) custom action and value resolvers
            if (info.OnBeginListElementGUI != null)
            {
                onBeginListElementGUI = ActionResolver.Get(Property, info.OnBeginListElementGUI,
                    new NamedValue("index", typeof(int)));
            }

            if (info.OnEndListElementGUI != null)
            {
                onEndListElementGUI = ActionResolver.Get(Property, info.OnEndListElementGUI,
                    new NamedValue("index", typeof(int)));
            }

            if (info.ElementColour != null)
            {
                elementColour = ValueResolver.Get<Color>(Property, info.ElementColour,
                    new Sirenix.OdinInspector.Editor.ValueResolvers.NamedValue("index", typeof(int)),
                    new Sirenix.OdinInspector.Editor.ValueResolvers.NamedValue("defaultColor", typeof(Color)));
            }

            if (info.OnTitleBarGUI != null)
            {
                onTitleBarGUI = ActionResolver.Get(Property, info.OnTitleBarGUI);
            }

            if (info.CustomAddFunction != null) //check for custom add function
            {
                //try to get a value resolver first
                customAddValue = ValueResolver.Get(collectionResolver.ElementType, Property, info.CustomAddFunction);

                if (customAddValue.HasError) //if an error, try get a custom action resolver first
                {
                    customAddResolver = ActionResolver.Get(Property, info.CustomAddFunction);

                    if (!customAddResolver.HasError) //if successful, wipe out previous error
                    {
                        customAddValue = null;
                    }
                }
            }

            if (info.CustomRemoveElementFunction != null)
            {
                customRemoveElementResolver = ActionResolver.Get(Property, info.CustomRemoveElementFunction,
                    new NamedValue("element", collectionResolver.ElementType));
            }
            else if (info.CustomRemoveIndexFunction != null)
            {
                customRemoveIndexResolver = ActionResolver.Get(Property, info.CustomRemoveIndexFunction,
                    new NamedValue("index", typeof(int)));
            }

            BuildTabGroup();
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            ActionResolver.DrawErrors(
                customAddResolver,
                customRemoveElementResolver,
                customRemoveIndexResolver,
                onBeginListElementGUI,
                onEndListElementGUI,
                onTitleBarGUI);

            ValueResolver.DrawErrors(
                customAddValue,
                elementColour,
                listElementLabelName);

            //check that a rebuild is queue, or we have a different amount of child resolver children than expected
            if (queueRebuildTabs || collectionCount != Property.ChildResolver.ChildCount)
            {
                BuildTabGroup();
            }

            DrawToolbar();
            if (SirenixEditorGUI.BeginFadeGroup(UniqueDrawerKey.Create(Property, this), Property.State.Expanded))
            {
                DrawTabs();
            }

            SirenixEditorGUI.EndFadeGroup();

            if (objectPicker != null && objectPicker.IsReadyToClaim)
            {
                object value = objectPicker.ClaimObject();
                object copy = SerializationUtility.CreateCopy(value);
                object[] values = { copy };
                collectionResolver.QueueAdd(values);
            }
        }

        #endregion

        #region Tab Group Methods

        private void BuildTabGroup()
        {
            tabGroup = SirenixEditorGUI.CreateAnimatedTabGroup(UniqueDrawerKey.Create(Property, this));
            tabGroup.AnimationSpeed = tgc.AnimationSpeed;
            tabGroup.FixedHeight = false;

            if (tabs == null)
            {
                tabs = new List<GUITabPage>();
            }
            else
            {
                tabs.Clear();
            }

            for (var i = 0; i < Property.ChildResolver.ChildCount; i++)
            {
                string name;

                if (info.ListElementLabelName != null)
                {
                    listElementLabelName = ValueResolver.Get(typeof(object), Property.Children[i],
                        info.ListElementLabelName,
                        new Sirenix.OdinInspector.Editor.ValueResolvers.NamedValue("index", typeof(int)),
                        new Sirenix.OdinInspector.Editor.ValueResolvers.NamedValue("collection",
                            Property.ValueEntry.TypeOfValue));

                    if (!listElementLabelName.HasError) //if no error, use the return on the value resolver
                    {
                        listElementLabelName.Context.NamedValues.Set("index", i);
                        listElementLabelName.Context.NamedValues.Set("collection", Property.ValueEntry.WeakSmartValue);
                        object value = listElementLabelName.GetWeakValue();
                        name = value.ToString();
                    }
                    else //otherwise we take the string as-is
                    {
                        listElementLabelName = null;
                        name = info.ListElementLabelName;
                    }
                }
                else //if no resolver, we use the type of value
                {
                    name = Property.Children[i].ValueEntry.TypeOfValue.Name;
                }

                string tabName = info.ShowIndexLabels ? $"{name}-{i}" : name;
                tabs.Add(tabGroup.RegisterTab(tabName));
            }

            int childCount = Property.ChildResolver.ChildCount; //get new child count

            if (childCount < collectionCount) //less elements
            {
                int dif = collectionCount - childCount;

                //check if displayed end index is greater than the collection count,
                //and if there are more collection elements than tabs to be displayed
                //then shift back the difference
                if (EndIndex > childCount - 1 && childCount >= info.MaximumNumberOfTabs)
                {
                    ShiftTabs(-dif);
                }
                else if
                    (childCount <
                     info.MaximumNumberOfTabs) //otherwise check if collection is smaller than number of tabs
                {
                    EndIndex = childCount - 1; //set index below new collection count
                    SelectedIndex -=
                        SelectedIndex > EndIndex
                            ? dif
                            : 0; //reduce selected index by difference if greater than end index

                    if (SelectedIndex > -1)
                    {
                        tabGroup.GoToPage(tabs[SelectedIndex]);
                    }
                }
            }
            else if (childCount > collectionCount) //more elements
            {
                int dif = childCount - collectionCount;

                //check if there are more free tabs than the end index
                if (info.MaximumNumberOfTabs - 1 > EndIndex)
                {
                    //set endIndex to either max tabs, or add the difference in elements
                    EndIndex = childCount > info.MaximumNumberOfTabs ? info.MaximumNumberOfTabs - 1 : EndIndex + dif;
                }

                if (info.AlwaysSelectNewElement)
                {
                    //shift all indicies to the end of the collection
                    int shiftDif = childCount - EndIndex - 1;
                    StartIndex += shiftDif;
                    EndIndex += shiftDif;
                    tabGroup.GoToPage(tabs[^1]);
                }
            }
            else //if no change and we have list elements, to go selected index
            {
                if (childCount > 0)
                {
                    tabGroup.GoToPage(tabs[SelectedIndex]);
                }
            }

            collectionCount = childCount; //update collection count
            queueRebuildTabs = false;
        }

        private void DrawTabs()
        {
            if (Property.Children.Count == 0) //draw nothing if no child elements
            {
                return;
            }

            if (Property.Children.Count == 1) //if only one element, draw it in a box instead
            {
                SirenixEditorGUI.BeginBox();
                DrawCollectionElement(0);
                SirenixEditorGUI.EndBox();
                return;
            }

            tabGroup.BeginGroup();
            {
                for (int i = StartIndex; i <= EndIndex && i < tabs.Count; i++)
                {
                    if (tabs[i].BeginPage())
                    {
                        DrawCollectionElement(i);
                    }

                    tabs[i].EndPage();
                }
            }
            tabGroup.EndGroup();
        }

        private void DrawCollectionElement(int index)
        {
            if (index >= Property.Children.Count) //opt out if we're outside child count for whatever reason
            {
                return;
            }

            Color color = SirenixGUIStyles.ListItemColorEven;

            if (elementColour != null && !elementColour.HasError)
            {
                elementColour.Context.NamedValues.Set("index", index);
                elementColour.Context.NamedValues.Set("defaultColor", color);
                color = elementColour.GetValue();
            }

            //some magic I don't quite understant to draw the list element with a coloured background properly
            Rect rect = GUIHelper.GetCurrentLayoutRect();
            SirenixEditorGUI.DrawSolidRect(rect, color, false);
            SirenixEditorGUI.DrawBorders(rect, 1, 1, 0, 1, false);

            InspectorProperty child = Property.Children[index];

            if (info.HideLabel) //hide label if so desired
            {
                child.Label = null;
            }

            if (onBeginListElementGUI != null && !onBeginListElementGUI.HasError)
            {
                onBeginListElementGUI.Context.NamedValues.Set("index", index);
                onBeginListElementGUI.DoAction();
            }

            child.Draw();

            if (onEndListElementGUI != null && !onEndListElementGUI.HasError)
            {
                onEndListElementGUI.Context.NamedValues.Set("index", index);
                onEndListElementGUI.DoAction();
            }

            SelectedIndex = index;
        }

        #endregion

        #region Toolbar

        private void DrawToolbar()
        {
            SirenixEditorGUI.BeginHorizontalToolbar();
            {
                bool drawFoldout = !((GeneralDrawerConfig.Instance.HideFoldoutWhileEmpty && collectionCount == 0) ||
                                     info.Expanded);

                if (!drawFoldout)
                {
                    SirenixEditorGUI.Title(info.GroupName, null, TextAlignment.Left, false, false);
                }
                else
                {
                    Rect foldoutRect = EditorGUILayout.GetControlRect(false);
                    Property.State.Expanded = SirenixEditorGUI.Foldout(foldoutRect, Property.State.Expanded,
                        new GUIContent(info.GroupName));
                }

                //draw navigation buttons if there are more tabs than max tabs, or specified to always be the case
                if (collectionCount > info.MaximumNumberOfTabs || info.AlwaysDrawNavigationButtons)
                {
                    DrawNavigationButtons();
                }
                else if (info.ShowItemCount) //otherwise we just draw the item count if specified
                {
                    DrawItemCount();
                }

                if (!info.IsReadOnly && !info.HideAddButton)
                {
                    DrawAddButton();
                }

                if (!info.IsReadOnly && !info.HideRemoveButton)
                {
                    DrawRemoveButton();
                }

                if (onTitleBarGUI != null && !onTitleBarGUI.HasError)
                {
                    onTitleBarGUI.DoAction();
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        private void DrawNavigationButtons()
        {
            if (SirenixEditorGUI.ToolbarButton(EditorIcons.Previous) && collectionCount > 0)
            {
                ShiftTabs(-StartIndex);
                SelectedIndex = 0;
                tabGroup.GoToPage(tabs[SelectedIndex]);
            }

            if (SirenixEditorGUI.ToolbarButton(EditorIcons.ArrowLeft))
            {
                ShiftTabs(-info.NavigationTabAmount);
            }

            DrawItemCount();

            if (SirenixEditorGUI.ToolbarButton(EditorIcons.ArrowRight))
            {
                ShiftTabs(info.NavigationTabAmount);
            }

            if (SirenixEditorGUI.ToolbarButton(EditorIcons.Next) && collectionCount > 0)
            {
                int dif = LastAvailableIndex - EndIndex;
                ShiftTabs(dif);
                SelectedIndex = LastAvailableIndex;
                tabGroup.GoToPage(tabs[SelectedIndex]);
            }
        }

        private void DrawItemCount(float width = 50)
        {
            int start = collectionCount > 0 ? StartIndex + 1 : 0;
            int end = EndIndex + 1;
            EditorGUILayout.LabelField($"{start}-{end}/{collectionCount}", SirenixGUIStyles.CenteredGreyMiniLabel,
                GUILayoutOptions.MaxWidth(width));
        }

        private void DrawAddButton()
        {
            objectPicker = ObjectPicker.GetObjectPicker(Property, collectionResolver.ElementType);

            if (SirenixEditorGUI.ToolbarButton(EditorIcons.Plus))
            {
                Property.RecordForUndo($"Adding {collectionResolver.ElementType.Name} collection element.");

                if (customAddResolver != null)
                {
                    customAddResolver.DoActionForAllSelectionIndices();
                    queueRebuildTabs = true;

                    if (Property.SerializationRoot.ValueEntry.WeakSmartValue is Object obj)
                    {
                        InspectorUtilities.RegisterUnityObjectDirty(obj);
                    }
                }
                else
                {
                    var values = new object[1];
                    values[0] = GetValueToAdd(out bool wasFallback);

                    if (wasFallback)
                    {
                        objectPicker.ShowObjectPicker(null, Property.Attributes.HasAttribute<AssetsOnlyAttribute>(),
                            GUIHelper.GetCurrentLayoutRect(),
                            !Property.ValueEntry.SerializationBackend.SupportsPolymorphism);
                    }
                    else
                    {
                        collectionResolver.QueueAdd(values);
                    }
                }
            }
        }

        private void DrawRemoveButton()
        {
            if (SirenixEditorGUI.ToolbarButton(EditorIcons.Minus))
            {
                if (Property.ChildResolver.ChildCount == 0)
                {
                    return;
                }

                if (customRemoveElementResolver != null)
                {
                    Property.RecordForUndo("Custom List Element Remove");
                    object value = Property.Children[SelectedIndex].ValueEntry.WeakValues[0];
                    customRemoveElementResolver.Context.NamedValues.Set("element", value);
                    customRemoveElementResolver.DoAction();
                    queueRebuildTabs = true;
                }
                else if (customRemoveIndexResolver != null)
                {
                    Property.RecordForUndo($"Custom Remove List Index at index {SelectedIndex}");
                    customRemoveIndexResolver.Context.NamedValues.Set("index", SelectedIndex);
                    customRemoveIndexResolver.DoAction();
                    queueRebuildTabs = true;
                }
                else
                {
                    Property.RecordForUndo($"Removing element at index {SelectedIndex}");

                    InspectorProperty child = Property.Children[SelectedIndex];

                    var values = new object[child.ValueEntry.ValueCount];

                    for (var i = 0; i < values.Length; i++)
                    {
                        values[i] = child.ValueEntry.WeakValues[i];
                    }

                    collectionResolver.QueueRemove(values);
                }
            }
        }

        private object GetValueToAdd(out bool wasFallback)
        {
            wasFallback = false;

            if (customAddValue != null) //if there's a custom value resolver, use that
            {
                return customAddValue.GetWeakValue();
            }

            if (info.AlwaysAddDefaultValue) //otherwise, add a default value
            {
                if (!Property.ValueEntry.SerializationBackend.SupportsPolymorphism)
                {
                    return UnitySerializationUtility.CreateDefaultUnityInitializedObject(collectionResolver
                        .ElementType);
                }

                if (collectionResolver.ElementType.IsValueType)
                {
                    return Activator.CreateInstance(collectionResolver.ElementType);
                }

                return null;
            }

            if (info.AddCopiesLastElement &&
                collectionCount > 0) //otherwise, if one or more elements, copy the last value
            {
                IPropertyValueEntry lastElementProperty = Property.Children[^1].ValueEntry;
                object lastObject = lastElementProperty.WeakValues[^1];

                return SerializationUtility.CreateCopy(lastObject);
            }

            if (collectionResolver.ElementType.InheritsFrom<Object>() &&
                Event.current.modifiers == EventModifiers.Control)
            {
                return null;
            }

            wasFallback = true;
            Type elementType = collectionResolver.ElementType;
            if (!ValueEntry.SerializationBackend.SupportsPolymorphism)
            {
                return UnitySerializationUtility.CreateDefaultUnityInitializedObject(elementType);
            }

            return elementType.IsValueType ? Activator.CreateInstance(elementType) : null;
        }

        #endregion
    }
}
#endif