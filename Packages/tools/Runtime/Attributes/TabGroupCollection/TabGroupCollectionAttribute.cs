using System;
using System.Diagnostics;
using Sirenix.OdinInspector;

#if UNITY_2021_1_OR_NEWER
namespace Tools.Attributes.TabGroupCollection
{
    /// <summary>
    ///     Draws a collection as a series of tab groups instead of the default collection drawer.
    /// </summary>
#if ODIN_INSPECTOR
    [DontApplyToListElements]
#endif
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class)]
    [Conditional("UNITY_EDITOR")]
    public class TabGroupCollectionAttribute : Attribute
    {
        #region Internal Fields

        private int _maximumNumberOfTabs;
        private int _navigationTabAmount;
        private bool _alwaysDrawNavigationButtons;

        #endregion

        #region Properties

        /// <summary>
        ///     Navigates to a the new tab when adding a list element. True by default.
        ///     Note: when using <see cref="Sirenix.OdinInspector.ListDrawerSettingsAttribute.CustomAddFunction" />
        ///     this will always navigate to the last tab even if it's not the newest tab added.
        /// </summary>
        public bool AlwaysSelectNewElement { get; set; } = true;

        /// <summary>
        ///     If true, will always draw the navigation buttons even if there are less than equal number of elements as the
        ///     <see cref="MaximumNumberOfTabs" />.
        ///     False by default.
        /// </summary>
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

        /// <summary>
        ///     Determines the speed at which the tab changing animation plays at.
        /// </summary>
        public float AnimationSpeed { get; set; } = 5.0f;

        /// <summary>
        ///     The maximum number of tabs to be displayed by the tab collection drawer. Defaults to 5 if no value is set.
        ///     Note that large values can cause drawing issues.
        /// </summary>
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

        /// <summary>
        ///     Determines the number of tabs to be shift left or right by the navigation buttons.
        ///     Defaults to <see cref="MaximumNumberOfTabs" /> if no value is set.
        /// </summary>
        public int NavigationTabAmount
        {
            get => _navigationTabAmount;
            set
            {
                if (value > 0)
                {
                    HasNavigationTabAmount = true;
                    _navigationTabAmount = value;
                }
            }
        }

        public bool HasNavigationTabAmount { get; private set; }

        #endregion
    }
}
#endif