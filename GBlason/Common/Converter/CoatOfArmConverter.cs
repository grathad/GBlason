using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GBL.Repository.CoatOfArms;
using GBlason.ViewModel;

namespace GBlason.Common.Converter
{
    public static class CoatOfArmConverter
    {
        /// <summary>
        /// Create a new instance of view model from the repository data
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static CoatOfArmViewModel ConvertToViewModel(this CoatOfArms source)
        {
            return new CoatOfArmViewModel { CurrentShape = source.Shape.ConvertToViewModel() };
        }

        /// <summary>
        /// Converts to view model.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static ShapeViewModel ConvertToViewModel(this Shape source)
        {
            return new ShapeViewModel
                            {
                                Name = source.Name,
                                Description = source.Description,
                                Geometry = source.Geometry,
                                Identifier = source.Identifier,
                                PreferedHeight = source.PathHeight,
                                PreferedWidth = source.PathWidth
                            };
        }
    }
}
