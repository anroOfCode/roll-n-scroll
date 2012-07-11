using System;
using System.Collections.ObjectModel;

namespace Fusao.RollNScroll
{
    /// <summary>
    /// Provides data bindings for the main properties on the prescroll screen
    /// and a method that generates the URI that will navigate to a roll page,
    /// passing the proper query parameters based on the values of these properties.
    /// </summary>
    public class ScrollViewModel
    {

        private static string _defaultRollText = "THIS IS ROLL N' SCROLL";
        private static string _defaultForegroundColor = "magenta";
        private static string _defaultBackgroundColor = "teal";
        private static Boolean _defaultStrobe = false;

        /// <summary>
        /// Constructs a ScrollViewModel with default property values.
        /// </summary>
        public ScrollViewModel()
        {
            this.RollText = _defaultRollText;
            this.Strobe = _defaultStrobe;
            this.ForegroundColor = _defaultForegroundColor;
            this.BackgroundColor = _defaultBackgroundColor;
        }

        /// <summary>
        /// A read-only collection containing the string representation of
        /// all the valid colors to be displayed in the list picker control.
        /// </summary>
        public ReadOnlyCollection<string> ColorPickerColors
        {
            get
            {
                return ColorExtensions.AccentColors();
            }
        }

        /// <summary>
        /// Gets or sets the ForegroundColor property.
        /// </summary>
        public string ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the BackgroundColor property.
        /// </summary>
        public string BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the Strobe boolean property.
        /// </summary>
        public Boolean Strobe { get; set; }

        /// <summary>
        /// Gets or sets the RollText property.
        /// </summary>
        public string RollText { get; set; }

        /// <summary>
        /// Builds a relative Uri for navigating to the RollPage given the current
        /// property values.
        /// </summary>
        /// <returns>A Uri that will navigate to the RollPage.</returns>
        public Uri GetNavigationUri()
        {
            // If anything has been set to null, we set it to a default value.
            if (ForegroundColor == null)
            {
                ForegroundColor = _defaultForegroundColor;
            }

            if (BackgroundColor == null)
            {
                BackgroundColor = _defaultBackgroundColor;
            }

            if (RollText == null)
            {
                RollText = _defaultRollText;
            }

            string [] uriParameters = new string [] {RollText, ForegroundColor, BackgroundColor, Strobe.ToString()};
            string uriString = String.Format("/RollPage.xaml?scrollText={0}&forecolor={1}&backcolor={2}&strobe={3}", uriParameters);
            Uri returnUri = new Uri(uriString, UriKind.Relative);
            return returnUri;
        }
    }
}
