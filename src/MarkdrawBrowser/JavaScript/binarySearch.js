export default (haystack, needle, comparator) => {
  let low = 0;
  let high = haystack.length - 1;
  let middle = 0;

  while(low <= high) {
    middle = low + ((high - low) >>> 1);
    const lowOrHigh = +comparator(haystack[middle], needle, middle, haystack);

    if (lowOrHigh < 0.0) {
      low = middle + 1;
    } else if (lowOrHigh > 0.0) {
      high = middle - 1;
    } else {
      return middle;
    }
  }

  return null;
}