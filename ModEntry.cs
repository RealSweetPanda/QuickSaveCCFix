using System;
using System.Linq;
using QuickSaveCCFix.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace QuickSaveCCFix
{
    // remove Resharper warning about class ModEntry not being instantiated (it's done via SMAPI)
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ModEntry : Mod
    {
        public static ModEntry Instance;

        public interface IQuickSaveAPI
        {
            /* Save Event Order:
             * 1. QS-Saving (IsSaving = true)
             * 2. QS-Saved (IsSaving = false)
             */

            /// <summary>Fires before a Quicksave is being created</summary>
            public event SavingDelegate SavingEvent;

            /// <summary>Fires after a Quicksave has been created</summary>
            public event SavedDelegate SavedEvent;

            public bool IsSaving { get; }

            /* Load Event Order:
             * 1. QS-Loading (IsLoading = true)
             * 2. SMAPI-LoadStageChanged
             * 3. SMAPI-SaveLoaded & SMAPI-DayStarted
             * 4. QS-Loaded (IsLoading = false)
             */

            /// <summary>Fires before a Quicksave is being loaded</summary>
            public event LoadingDelegate LoadingEvent;

            /// <summary>Fires after a Quicksave was loaded</summary>
            public event LoadedDelegate LoadedEvent;

            public bool IsLoading { get; }

            public delegate void SavingDelegate(object sender, ISavingEventArgs e);

            public delegate void SavedDelegate(object sender, ISavedEventArgs e);

            public delegate void LoadingDelegate(object sender, ILoadingEventArgs e);

            public delegate void LoadedDelegate(object sender, ILoadedEventArgs e);
        }

        public interface ISavingEventArgs
        {
        }

        public interface ISavedEventArgs
        {
        }

        public interface ILoadingEventArgs
        {
        }

        public interface ILoadedEventArgs
        {
        }

        public event EventHandler BeforeSave;

        public event EventHandler AfterSave;

        public event EventHandler AfterLoad;

        public override void Entry(IModHelper helper)
        {
            Helper.Events.GameLoop.GameLaunched += ForwardDelegates;


            Instance = this;
        }

        private void ForwardDelegates(object sender, GameLaunchedEventArgs e)
        {
            var quickSave = Helper.ModRegistry.GetApi<IQuickSaveAPI>("DLX.QuickSave");
            if (quickSave != null) //if the API was accessed successfully
            {
                quickSave.LoadedEvent += Api_LoadedEvent;

                quickSave.SavingEvent += Api_SavingEvent;

                quickSave.SavedEvent += Api_SavedEvent;
            }
        }

        private void Api_LoadedEvent(object sender, ILoadedEventArgs e)
        {
            AfterLoad?.GetInvocationList().First(@delegate => @delegate.Method.Module.Name == "CustomCompanions.dll")
                .Method.Invoke(new object(), new[] { sender, e });
        }

        private void Api_SavingEvent(object sender, ISavingEventArgs e)
        {
            BeforeSave?.GetInvocationList().First(@delegate => @delegate.Method.Module.Name == "CustomCompanions.dll")
                .Method.Invoke(new object(), new[] { sender, e });
            
        }

        private void Api_SavedEvent(object sender, ISavedEventArgs e)
        {
            AfterSave?.GetInvocationList().First(@delegate => @delegate.Method.Module.Name == "CustomCompanions.dll")
                .Method.Invoke(new object(), new[] { sender, e });
            
        }


        public override object GetApi()
        {
            return new SaveAnywhereApi();
        }
    }
}