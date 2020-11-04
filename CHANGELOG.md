## 2.0.0

- Made all types sealed.
- Added nullable type checks.
- Add support for `netstandard1.1` and `netstandard2.0`.
- API collection types changed as appropriate:
  - `Permutations<T>` source changed from `ICollection<T>` to `IEnumerable<T>`.
  - `Combinations<T>` source changed from `IList<T>` to `IEnumerable<T>`.
  - `Permutations<T>`, `Combinations<T>`, and `Variations<T>` enumeration type changed from `IEnumerable<IList<T>>` to `IEnumerable<IReadOnlyList<T>>`.
- The type of `Count` for all types changed from `long` to `BigInteger`.
