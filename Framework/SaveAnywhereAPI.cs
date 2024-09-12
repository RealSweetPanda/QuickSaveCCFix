using System;

namespace QuickSaveCCFix.Framework
{
    public class SaveAnywhereApi
    {
        /// <summary>
        ///     Fires before save data starts writing
        /// </summary>
        public event EventHandler BeforeSave
        {
            add => ModEntry.Instance.BeforeSave += value;
            remove => ModEntry.Instance.BeforeSave -= value;
        }

        /// <summary>
        ///     Fires after saving is complete
        /// </summary>
        public event EventHandler AfterSave
        {
            add => ModEntry.Instance.AfterSave += value;
            remove => ModEntry.Instance.AfterSave -= value;
        }

        /// <summary>
        ///     Fires if the game has loaded with a mid-day save
        /// </summary>
        public event EventHandler AfterLoad
        {
            add => ModEntry.Instance.AfterLoad += value;
            remove => ModEntry.Instance.AfterLoad -= value;
        }
    }
}