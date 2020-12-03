using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.Common
{
    /// <summary>
    /// Help Boxing Boolean values
    /// </summary>
    /// <seealso cref="MahApps.Metro.ValueBoxes.BooleanBoxes"/>
    public static class BooleanBoxes
    {
        /// <summary>
        /// Gets a boxed representation for Boolean's "true" value.
        /// </summary>
        public static readonly object False = false;

        /// <summary>
        ///  Gets a boxed representation for Boolean's "false" value.
        /// </summary>
        public static readonly object True = true;

        /// <summary>
        /// Returns a boxed representation for the specified Boolean value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object Box(this bool value) => value ? True : False;
    }
}
