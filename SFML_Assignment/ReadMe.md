A double-ended queue is a data structure that generalizes a queue. Deque has two ends, front and rear, where the items remain positioned in the data structure. 
Deque is different from queue as it has an unrestrictive nature of adding and removing data from the collection. 
Items can be added from the collection front or the rear. Likewise, item can also be removed from its front or rear.
By having this capabilities, this data structure is not controlled by FIFO(First in first out) rule as in a stack or LIFO(Last in first out) rule as in queue.

Upon completing this project, I find some difficulties in implementing this project using SFML as I am not familiar with SFML. Luckily, I found a SFML library (TGUI)
that helps me to implement Widget in my application. Jesse, my lecturer, has also been supporting me with feedbacks and ideas that I can use to improve my project.
It really helps me a lot in building this application.

Strength of deque :
- Deque uses less overall memory than an array for the same number of values.
- Deque automatically frees allocated memory when its size drops lows enough.
- The time complexity of all deque of operations is O(1).

Weakness of deque :
- Capacity must be a power of 2.
- Iterating random indexes in deque has a complexity of O(n) as it needs to iterate each element until it gets to the element of specified index.

Deque perform the best when the data that is needed is either on the front or rear of the deque. As it don't have to iterate the deque and can immedietly access
the element. On the other hand, Deque perform worse when the data that is needed is not in the front/rear of the deque as we need to iterate on every element until
specified element is found.

In a real-world, deque has been used quite a lot, for example :
- Multi processor Scheduling using A-steal algorithm.
- Storing a web browser's history.
- Storing a software application's list of undo operations.

Time Complexity :
- All operation in Deque has time complexity of O(1) as each opearations can be perfomed immedietly .
- When iterating in Deque of a random index, it has time complexity of O(n) as it needs to iterate the deque element until it gets to the 
  element of specified index.