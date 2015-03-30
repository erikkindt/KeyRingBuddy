using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyRingBuddy.Windows
{
    /// <summary>
    /// Arguments for an event involving a function.
    /// </summary>
    public class FunctionEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="functionName">FunctionName.</param>
        public FunctionEventArgs(string functionName)
        {
            if (String.IsNullOrWhiteSpace(functionName))
                throw new ArgumentException("functionName is null or whitespace", "functionName");

            FunctionName = functionName;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The name of the function.
        /// </summary>
        public string FunctionName { get; private set; }

        #endregion
    }
}
