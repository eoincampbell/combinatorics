using System;
using System.Collections;
using System.Collections.Generic;

namespace Nito.Combinatorics
{
    /// <summary>
    /// Variations defines a sequence of all possible ordered subsets of a particular size from the set of values.  
    /// </summary>
    /// <remarks>
    /// The MetaCollectionType parameter of the constructor allows for the creation of
    /// normal Variations and Variations with Repetition.
    /// 
    /// When given an input collect {A B C} and lower index of 2, the following sets are generated:
    /// MetaCollectionType.WithoutRepetition generates 6 sets: =>
    ///     {A B}, {A B}, {B A}, {B C}, {C A}, {C B}
    /// MetaCollectionType.WithRepetition generates 9 sets:
    ///     {A A}, {A B}, {A B}, {B A}, {B B }, {B C}, {C A}, {C B}, {C C}
    /// 
    /// The equality of multiple inputs is not considered when generating variations.
    /// </remarks>
    /// <typeparam name="T">The type of the values within the list.</typeparam>
    public sealed class Variations<T> : IMetaCollection<T>
    {
        /// <summary>
        /// Create a variation set from the indicated list of values.
        /// The upper index is calculated as values.Count, the lower index is specified.
        /// Collection type defaults to MetaCollectionType.WithoutRepetition
        /// </summary>
        /// <param name="values">List of values to select Variations from.</param>
        /// <param name="lowerIndex">The size of each variation set to return.</param>
        public Variations(IEnumerable<T> values, int lowerIndex)
            : this(values, lowerIndex, GenerateOption.WithoutRepetition)
        {
        }

        /// <summary>
        /// Create a variation set from the indicated list of values.
        /// The upper index is calculated as values.Count, the lower index is specified.
        /// </summary>
        /// <param name="values">List of values to select variations from.</param>
        /// <param name="lowerIndex">The size of each variation set to return.</param>
        /// <param name="type">Type indicates whether to use repetition in set generation.</param>
        public Variations(IEnumerable<T> values, int lowerIndex, GenerateOption type)
        {
            Type = type;
            LowerIndex = lowerIndex;
            _myValues = new List<T>();
            _myValues.AddRange(values);

            if (type != GenerateOption.WithoutRepetition)
            {
                return;
            }

            var myMap = new List<int>();
            var index = 0;
            for (var i = 0; i < _myValues.Count; ++i)
            {
                myMap.Add(i >= _myValues.Count - LowerIndex ? index++ : int.MaxValue);
            }

            _myPermutations = new Permutations<int>(myMap);
        }

        /// <summary>
        /// Gets an enumerator for the collection of Variations.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<IList<T>> GetEnumerator()
        {
            if (Type == GenerateOption.WithRepetition)
            {
                return new EnumeratorWithRepetition(this);
            }
            return new EnumeratorWithoutRepetition(this);
        }

        /// <summary>
        /// Gets an enumerator for the collection of Variations.
        /// </summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            if (Type == GenerateOption.WithRepetition)
            {
                return new EnumeratorWithRepetition(this);
            }
            return new EnumeratorWithoutRepetition(this);
        }

        /// <summary>
        /// An enumerator for Variations when the type is set to WithRepetition.
        /// </summary>
        public sealed class EnumeratorWithRepetition : IEnumerator<IList<T>>
        {
            /// <summary>
            /// Construct a enumerator with the parent object.
            /// </summary>
            /// <param name="source">The source Variations object.</param>
            public EnumeratorWithRepetition(Variations<T> source)
            {
                _myParent = source;
                Reset();
            }

            /// <summary>
            /// Resets the Variations enumerator to the first variation.  
            /// </summary>
            public void Reset()
            {
                _myCurrentList = null;
                _myListIndexes = null;
            }

            /// <summary>
            /// Advances to the next variation.
            /// </summary>
            /// <returns>True if successfully moved to next variation, False if no more variations exist.</returns>
            /// <remarks>
            /// Increments the internal myListIndexes collection by incrementing the last index
            /// and overflow/carrying into others just like grade-school arithmetic.  If the 
            /// final carry flag is set, then we would wrap around and are therefore done.
            /// </remarks>
            public bool MoveNext()
            {
                var carry = 1;
                if (_myListIndexes == null)
                {
                    _myListIndexes = new List<int>();
                    for (var i = 0; i < _myParent.LowerIndex; ++i)
                    {
                        _myListIndexes.Add(0);
                    }
                    carry = 0;
                }
                else
                {
                    for (var i = _myListIndexes.Count - 1; i >= 0 && carry > 0; --i)
                    {
                        _myListIndexes[i] += carry;
                        carry = 0;

                        if (_myListIndexes[i] < _myParent.UpperIndex)
                        {
                            continue;
                        }

                        _myListIndexes[i] = 0;
                        carry = 1;
                    }
                }
                _myCurrentList = null;
                return carry != 1;
            }

            /// <summary>
            /// The current variation
            /// </summary>
            public IList<T> Current
            {
                get
                {
                    ComputeCurrent();
                    return _myCurrentList;
                }
            }

            /// <summary>
            /// The current variation.
            /// </summary>
            object IEnumerator.Current
            {
                get
                {
                    ComputeCurrent();
                    return _myCurrentList;
                }
            }

            /// <summary>
            /// Cleans up non-managed resources, of which there are none used here.
            /// </summary>
            public void Dispose()
            {

            }

            /// <summary>
            /// Computes the current list based on the internal list index.
            /// </summary>
            private void ComputeCurrent()
            {
                if (_myCurrentList != null)
                {
                    return;
                }

                _myCurrentList = new List<T>();

                foreach (var index in _myListIndexes)
                {
                    _myCurrentList.Add(_myParent._myValues[index]);
                }
            }

            /// <summary>
            /// Parent object this is an enumerator for.
            /// </summary>
            private readonly Variations<T> _myParent;

            /// <summary>
            /// The current list of values, this is lazy evaluated by the Current property.
            /// </summary>
            private List<T> _myCurrentList;

            /// <summary>
            /// An enumerator of the parents list of lexicographic orderings.
            /// </summary>
            private List<int> _myListIndexes;
        }

        /// <summary>
        /// An enumerator for Variations when the type is set to WithoutRepetition.
        /// </summary>
        public sealed class EnumeratorWithoutRepetition : IEnumerator<IList<T>>
        {
            /// <summary>
            /// Construct a enumerator with the parent object.
            /// </summary>
            /// <param name="source">The source Variations object.</param>
            public EnumeratorWithoutRepetition(Variations<T> source)
            {
                _myParent = source;
                _myPermutationsEnumerator = (Permutations<int>.Enumerator)_myParent._myPermutations.GetEnumerator();
            }

            /// <summary>
            /// Resets the Variations enumerator to the first variation.  
            /// </summary>
            public void Reset()
            {
                _myPermutationsEnumerator.Reset();
            }

            /// <summary>
            /// Advances to the next variation.
            /// </summary>
            /// <returns>True if successfully moved to next variation, False if no more variations exist.</returns>
            public bool MoveNext()
            {
                var ret = _myPermutationsEnumerator.MoveNext();
                _myCurrentList = null;
                return ret;
            }

            /// <summary>
            /// The current variation.
            /// </summary>
            public IList<T> Current
            {
                get
                {
                    ComputeCurrent();
                    return _myCurrentList;
                }
            }

            /// <summary>
            /// The current variation.
            /// </summary>
            object IEnumerator.Current
            {
                get
                {
                    ComputeCurrent();
                    return _myCurrentList;
                }
            }

            /// <summary>
            /// Cleans up non-managed resources, of which there are none used here.
            /// </summary>
            public void Dispose() => _myPermutationsEnumerator.Dispose();

            /// <summary>
            /// Creates a list of original values from the int permutation provided.  
            /// The exception for accessing current (InvalidOperationException) is generated
            /// by the call to .Current on the underlying enumeration.
            /// </summary>
            /// <remarks>
            /// To compute the current list of values, the element to use is determined by 
            /// a permutation position with a non-MaxValue value.  It is placed at the position in the
            /// output that the index value indicates.
            /// 
            /// E.g. Variations of 6 choose 3 without repetition
            /// Input array:   {A B C D E F}
            /// Permutations:  {- 1 - - 3 2} (- is Int32.MaxValue)
            /// Generates set: {B F E}
            /// </remarks>
            private void ComputeCurrent()
            {
                if (_myCurrentList != null)
                {
                    return;
                }

                _myCurrentList = new List<T>();
                var index = 0;
                var currentPermutation = _myPermutationsEnumerator.Current;

                for (var i = 0; i < _myParent.LowerIndex; ++i)
                {
                    _myCurrentList.Add(_myParent._myValues[0]);
                }

                foreach (var position in currentPermutation)
                {
                    if (position != int.MaxValue)
                    {
                        _myCurrentList[position] = _myParent._myValues[index];
                        if (_myParent.Type == GenerateOption.WithoutRepetition)
                        {
                            ++index;
                        }
                    }
                    else
                    {
                        ++index;
                    }
                }
            }

            /// <summary>
            /// Parent object this is an enumerator for.
            /// </summary>
            private readonly Variations<T> _myParent;

            /// <summary>
            /// The current list of values, this is lazy evaluated by the Current property.
            /// </summary>
            private List<T> _myCurrentList;

            /// <summary>
            /// An enumerator of the parents list of lexicographic orderings.
            /// </summary>
            private readonly Permutations<int>.Enumerator _myPermutationsEnumerator;
        }

        /// <summary>
        /// The number of unique variations that are defined in this meta-collection.
        /// </summary>
        /// <remarks>
        /// Variations with repetitions does not behave like other meta-collections and it's
        /// count is equal to N^P, where N is the upper index and P is the lower index.
        /// </remarks>
        public long Count
        {
            get
            {
                if (Type == GenerateOption.WithoutRepetition)
                {
                    return _myPermutations.Count;
                }
                return (long)Math.Pow(UpperIndex, LowerIndex);
            }
        }

        /// <summary>
        /// The type of Variations set that is generated.
        /// </summary>
        public GenerateOption Type { get; private set; }

        /// <summary>
        /// The upper index of the meta-collection, equal to the number of items in the initial set.
        /// </summary>
        public int UpperIndex => _myValues.Count;

        /// <summary>
        /// The lower index of the meta-collection, equal to the number of items returned each iteration.
        /// </summary>
        public int LowerIndex { get; private set; }

        /// <summary>
        /// Copy of values object is initialized with, required for enumerator reset.
        /// </summary>
        private List<T> _myValues;

        /// <summary>
        /// Permutations object that handles permutations on int for variation inclusion and ordering.
        /// </summary>
        private Permutations<int> _myPermutations;
    }
}
