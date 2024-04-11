You are given a certain integer, n, n > 0. You have to search the partition or partitions, of n, with maximum product value.

Let'see the case for n = 8.

Partition                 Product
[8]                          8
[7, 1]                       7
[6, 2]                      12
[6, 1, 1]                    6
[5, 3]                      15
[5, 2, 1]                   10
[5, 1, 1, 1]                 5
[4, 4]                      16
[4, 3, 1]                   12
[4, 2, 2]                   16
[4, 2, 1, 1]                 8
[4, 1, 1, 1, 1]              4
[3, 3, 2]                   18   <---- partition with maximum product value
[3, 3, 1, 1]                 9
[3, 2, 2, 1]                12
[3, 2, 1, 1, 1]              6
[3, 1, 1, 1, 1, 1]           3

[3, 1, 1, 1, 1, 1]           3
[3, 2, 1, 1, 1]           3
[3, 2, 2, 1]           3
[3, 3, 1, 1]           3
[3, 3, 2]           3




[2, 2, 2, 2]                16
[2, 2, 2, 1, 1]              8
[2, 2, 1, 1, 1, 1]           4
[2, 1, 1, 1, 1, 1, 1]        2
[1, 1, 1, 1, 1, 1, 1, 1]     1

So our needed function will return a tuple of two elements, the first being a list of all the partitions,
sorted by increasing length, and the second being the maximum product value.

Examples (input -> output)

8 -> ([[3, 3, 2]], 18)

10 --> ([[4, 3, 3], [3, 3, 2, 2]], 36)

https://www.codewars.com/kata/5716a4c2794d305f4900156b

Done GetMaxProductPartition
ToDo implement CreatePartition