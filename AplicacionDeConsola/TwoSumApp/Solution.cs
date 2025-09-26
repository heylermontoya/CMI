public class Solution
{
    public int[] TwoSum(int[] nums, int target)
    {
        Dictionary<int, int> dictionary = [];
        for (int i = 0; i < nums.Length; i++)
        {
            int complement = target - nums[i];
            if (dictionary.ContainsKey(complement))
            {
                return [dictionary[complement], i];
            }
            dictionary[nums[i]] = i;
        }
        return [];
    }

    public static void Main()
    {
        var solution = new Solution();

        int[] nums = { 2, 7, 11, 15 };
        int target = 9;

        int[] result = solution.TwoSum(nums, target);

        Console.WriteLine($"Output: [{string.Join(",", result)}]");
    }
}