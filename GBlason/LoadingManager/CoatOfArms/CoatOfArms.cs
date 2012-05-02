using System;

namespace GBL.Repository.CoatOfArms
{
    /// <summary>
    /// Represent a coat of arms, the logic side (not linked to any graphical output)
    /// </summary>
    [Serializable]
    public class CoatOfArms
    {
        /// <summary>
        /// Gets or sets the shape. Mandatory component of the coat of arms
        /// </summary>
        /// <value>
        /// The shape of the global coat of arms (the shield).
        /// </value>
        public Shape Shape { get; set; }
    }
}
