using System.Collections.Generic;
using Combinatorics.Collections;
using Xunit;

namespace UnitTests
{
    /// <summary>
    /// Tests Cases &amp; Examples for Combinations, Permutations &amp; Variations with & without repetition in the output sets.
    /// </summary>
    public class CombinatoricTests
    {
        /// <summary>
        /// Standard permutations simply provide every single ordering of the input set.
        /// Permutations of {A B C}: {A B C}, {A C B}, {B A C}, {B C A}, {C A B}, {C B A}
        /// The number of Permutations can be easily shown to be P(n) = n!, where n is the number of items. 
        /// In the above example, the input set contains 3 items, and the size is 3! = 6. 
        /// This means that the number of permutations grows exponentially with n. 
        /// Even a small n can create massive numbers of Permutations; for example, the number of ways to randomly 
        /// shuffle a deck of cards is 52! or approximately 8.1E67.
        /// </summary>
        [Fact]
        public void Generate_Permutations_Without_Repetition_On_3_Unique_Input_Items_Should_Create_12_Output_Permutations()
        {
            var integers = new List<int> { 1, 2, 3 };

            var p = new Permutations<int>(integers, GenerateOption.WithoutRepetition);

            foreach (var v in p)
            {
                System.Diagnostics.Debug.WriteLine(string.Join(",", v));
            }

            Assert.Equal(6, p.Count); 
        }

        /// <summary>
        /// Permutations with Repetition sets give allowance for repetitive items in the input 
        /// set that reduce the number of permutations: 
        /// Permutations with Repetition of the set {A A B}: {A A B}, {A B A}, {B A A}
        /// The number of Permutations with Repetition is not as large, being reduced by the number and count 
        /// of repetitive items in the input set. For each set of m identical items, the overall count is reduced by m!. 
        /// In the above example, the input set contains 3 items with one subset of 2 identical items, 
        /// the count is 3! / 2! = 6 / 2 = 3. The idea behind the count is easier than the formula since the formula 
        /// requires the product of each repetitive set of size ri. The total size is Pr(n) = n! / Π(ri!) 
        /// (where Π is the product operator). All of the collating and calculating is handled for us using 
        /// the Permutation.Count property.
        /// </summary>        
        [Fact]
        public void Generate_Permutations_With_Repetition_On_4_Input_Items_Including_Duplicates_Should_Create_24_Output_Permutations()
        {
            var integers = new List<int> {1, 1, 2, 3};

            var p = new Permutations<int>(integers, GenerateOption.WithRepetition);

            foreach (var v in p)
            {
                System.Diagnostics.Debug.WriteLine(string.Join(",", v));
            }

            Assert.Equal(24, p.Count);
        }

        /// <summary>
        /// Combinations are subsets of a given size taken from a given input set. 
        /// The size of the set is known as the Upper Index (n) and the size of the subset is known as the Lower Index (k). 
        /// When counting the number of combinations, the terminology is generally "n choose k", 
        /// and is known as the Binomial Coefficient [3]. Unlike permutations, combinations do not have any order 
        /// in the output set. Combinations without Repetition are would be similar to drawing balls from a lottery drum.
        /// Each ball can only be drawn once but the order they are drawn in is unimportant.
        /// </summary>
        [Fact]
        public void Generate_Combinations_of_3_Without_Repetition_On_6_Input_Items_Should_Create_20_Output_Items()
        {
            var integers = new List<int> {1, 2, 3, 4, 5, 6};

            var c = new Combinations<int>(integers, 3, GenerateOption.WithoutRepetition);

            foreach (var v in c)
            {
                System.Diagnostics.Debug.WriteLine(string.Join(",", v));
            }

            Assert.Equal(20, c.Count);
        }

        /// <summary>
        /// Combinations with Repetition are determined by looking at a set of items, 
        /// and selecting a subset while allowing repetition. For example, roll a dice, write down the letter, 
        /// and roll the dice again. The previous result does not preclude you from getting the same result again.
        /// The order is still unimportant
        /// </summary>
        [Fact]
        public void Generate_Combinations_of_2_With_Repetition_On_6_Input_Items_Should_Create_21_Output_Items()
        {
            var integers = new List<int> { 1, 2, 3, 4, 5, 6 };

            var c = new Combinations<int>(integers, 2, GenerateOption.WithRepetition);

            foreach (var v in c)
            {
                System.Diagnostics.Debug.WriteLine(string.Join(",", v));
            }

            Assert.Equal(21, c.Count);
        }

        /// <summary>
        /// Variations combine features of combinations and permutations, they are the set of all ordered 
        /// combinations of items to make up a subset. Like combinations, the size of the set is known as 
        /// the Upper Index (n) and the size of the subset is known as the Lower Index (k). And, the 
        /// generation of variations can be based on the repeating of output items. These are called Variations.
        /// 
        /// Variations are permutations of combinations. That is, a variation of a set of n items choose k, 
        /// is the ordered subsets of size k. For example:
        /// Variations of {A B C} choose 2: {A B}, {A C}, {B A}, {B C}, {C A}, {C B}
        /// The number of outputs in this particular example is similar to the number of combinations of 
        /// n choose k divided by the permutations of k. 
        /// 
        /// It can be calculated as V(n, k) = C(n, k) * P(k) = (n! / ( k! * (n - k)! )) * k! = n! / (n - k)!. 
        /// </summary>
        [Fact]
        public void Generate_Variations_of_3_Without_Repetition_On_6_Input_Items_Should_Create_120_Output_Items()
        {
            var integers = new List<int> {1, 2, 3, 4, 5, 6};

            var v = new Variations<int>(integers, 3, GenerateOption.WithoutRepetition);

            foreach (var vv in v)
            {
                System.Diagnostics.Debug.WriteLine(string.Join(",", vv));
            }

            Assert.Equal(120, v.Count);
        }

        /// <summary>
        /// Variations with Repetition expands on the set of variations, and allows items to be reused. 
        /// Since each item can be re-used, this allows for variations to include all items in the output to be a 
        /// single item from the input. For example:
        /// Variations with Repetition of {A B C} choose 2: {A A}, {A B}, {A C}, {B A}, {B B}, {B C}, {C A}, {C B}, {C C}
        /// The size of the output set for variations is easier to compute since factorials are not involved. 
        /// Each of the p positions can be filled from any of the n positions in the input set.
        /// The first item is one of n items, the second is also one of n, and the pth is also one of n. 
        /// This gives us Vr(n, k) = n^k total variations of n items choose k.
        /// </summary>
        [Fact]
        public void Generate_Variations_of_3_With_Repetition_On_6_Input_Items_Should_Create_216_Output_Items()
        {
            var integers = new List<int> { 1, 2, 3, 4, 5, 6 };

            var v = new Variations<int>(integers, 3, GenerateOption.WithRepetition);

            foreach (var vv in v)
            {
                System.Diagnostics.Debug.WriteLine(string.Join(",", vv));
            }

            Assert.Equal(216, v.Count);
        }
    }
}
