using System;
using System.Globalization;
using System.Resources;
using GBlason.Culture;

namespace GBlason.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DisplayInformation : Attribute
    {
        public String DisplayName { get; set; }
        public int MinHeight { get; set; }
        public int MinWidth { get; set; }

        public DisplayInformation(String displayNameKey)
        {
            var keyFinder = new ResourceManager(typeof (BlasonVocabulary));

            DisplayName = keyFinder.GetString(displayNameKey, CultureInfo.CurrentCulture);
        }
    }
}
