using CasualRacer.Model;
using System.Windows;

namespace CasualRacer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Singleton

        private static Main main;

        internal static Main MainModel
        {
            get
            {
                if (main == null)
                    main = new Model.Main();
                return main;
            }
        }

        #endregion
    }
}
