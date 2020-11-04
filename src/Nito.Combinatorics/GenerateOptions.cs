using System;

namespace Nito.Combinatorics
{
    /// <summary>
    /// Indicates whether a permutation, combination or variation generates equivalent result sets.  
    /// </summary>
    [Flags]
    public enum GenerateOptions
    {
        /// <summary>
        /// Default options: result sets are not duplicated, and result sets are not mutated in-place.
        /// </summary>
        Default = 0x0,
        
        /// <summary>
        /// Generate equivalent result sets.
        /// </summary>
        WithRepetition = 0x1,

        /// <summary>
        /// Allow mutating result sets after they have been returned to the user.
        /// </summary>
        MutateInPlace = 0x2,
    }
}
