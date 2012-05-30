﻿namespace Domain

open System
open System.Collections
open System.Collections.Generic

/// seealso: http://blogs.msdn.com/b/mcsuksoldev/archive/2012/05/10/net-implementation-of-a-priority-queue-aka-heap.aspx
type PriorityQueue<'TKey, 'TValue when 'TKey : comparison> private (data:IEnumerable<KeyValuePair<'TKey, 'TValue>> option, capacity:int, comparer:IComparer<'TKey>) =   
 
    let mutable heapList:List<KeyValuePair<'TKey, 'TValue>> =  null
    let mutable positionDict:Dictionary<'TKey, int> = null
 
    // Determines if a value is in the heap
    let inRange index =
        if index >= 0 && index < heapList.Count then Some(index) else None
    let checkIndex index =
        if ((inRange index).IsNone) then raise (ArgumentException(sprintf "Index specified is not within range - %i" index))
 
    // Gets the children of a node
    let getChildren (index:int) =
        // children left[2*pos] and right[2*pos + 1] where pos = index + 1
        let left = (2 * index) + 1
        let right = (2 * index) + 2     
        (inRange left, inRange right)
 
    // Gets the parent of an index
    let getParent (index:int) =
        // parent index [pos/2] where index = pos - 1
        if (index = 0) then None
        else Some((index-1) / 2)
 
    // Tests to see if the first value is greater than the first
    let isGreater parent child =
        if (comparer.Compare(heapList.[parent].Key, heapList.[child].Key) > 0) then true
        else false
            
    // Swaps two elements of the heap list
    let swapElements idx1 idx2 =
        let element1 = heapList.[idx1]
        let element2 = heapList.[idx2]
        heapList.[idx1] <- heapList.[idx2]
        heapList.[idx2] <- element1
        positionDict.[element1.Key] <- idx2
        positionDict.[element2.Key] <- idx1
 
    // Heapifys toward the parent
    let rec heapifyUp (index:int) =
        if (index > 0) then
            let parent = getParent index
            if (isGreater parent.Value index) then
                swapElements parent.Value index
                heapifyUp parent.Value
 
    // Heapifys down to the children
    let rec heapifyDown (index:int) =
        let (left, right) = getChildren index
        if (left.IsSome) then
            let childindex =
                if (right.IsSome && (isGreater left.Value right.Value)) then right.Value
                else left.Value
            if (isGreater index childindex) then
                swapElements index childindex
                heapifyDown childindex
 
    // Heapifys down to the children
    let heapifyUpDown (index:int) =
        let parent =  getParent index
        if (parent.IsSome && (isGreater parent.Value index)) then
            heapifyUp index
        else
            heapifyDown index
 
    // Adds an items and heapifys
    let insertItem (key:'TKey) (value:'TValue) =
        let insertindex = heapList.Count
        positionDict.Add(key, insertindex)
        heapList.Add(new KeyValuePair<'TKey, 'TValue>(key, value))
        heapifyUp(insertindex)
 
    // Delete the root node and heapifys
    let deleteItem index =
        if (heapList.Count <= 1) then
            heapList.Clear()
            positionDict.Clear()
        else
            let lastindex = heapList.Count - 1
            let indexKey =  heapList.[index].Key
            let lastKey = heapList.[lastindex].Key
            heapList.[index] <- heapList.[lastindex]
            positionDict.[lastKey] <- index
            heapList.RemoveAt(lastindex)
            positionDict.Remove(indexKey) |> ignore
            heapifyDown index
 
    // Default do bindings
    do
        if (comparer = null) then
            raise (ArgumentException("Comparer cannot be null"))
 
        let equalityComparer =
            { new IEqualityComparer<'TKey> with
                override x.Equals(c1, c2) =
                    if ((comparer.Compare(c1, c2)) = 0) then true
                    else false
                override x.GetHashCode(value) =
                    value.GetHashCode()
            }
 
        heapList <- new List<KeyValuePair<'TKey, 'TValue>>(capacity)
        positionDict <- new Dictionary<'TKey, int>(capacity, equalityComparer)
 
        if data.IsSome then
            data.Value |> Seq.iter (fun item -> insertItem item.Key item.Value)
 
    // Set of constructors
    new() = PriorityQueue(None, 0, ComparisonIdentity.Structural<'TKey>)
    new(capacity:int) = PriorityQueue(None, capacity, ComparisonIdentity.Structural<'TKey>)
    new(data:IEnumerable<KeyValuePair<'TKey, 'TValue>>) = PriorityQueue(Some(data), 0, ComparisonIdentity.Structural<'TKey>)
    new(comparer:IComparer<'TKey>) = PriorityQueue(None, 0, comparer)
    new(capacity:int, comparer:IComparer<'TKey>) = PriorityQueue(None, capacity, comparer)
    new(data:IEnumerable<KeyValuePair<'TKey, 'TValue>>, comparer:IComparer<'TKey>) = PriorityQueue(Some(data), 0, comparer)
 
    // Checks to see if the heap is empty
    member this.IsEmpty
        with get() = (heapList.Count = 0)
 
    // Enqueues a new entry into the heap
    member this.Enqueue (key:'TKey) (value:'TValue) =
        insertItem key value
        ()
 
    // Peeks at the head of the heap
    member this.Peek() =
        if not(this.IsEmpty) then
            heapList.[0]
        else
            raise (InvalidOperationException("Priority Queue is empty"))
 
    // Dequeues the head entry into the heap
    member this.Dequeue() =
        let value = this.Peek()
        deleteItem 0
        value
 
    // Determines whether an item is in the queue
    member this.Contains (key:'TKey) (value:'TValue) =
        heapList.Contains(new KeyValuePair<'TKey, 'TValue>(key, value))
 
    // Returns the index of a specified item
    member this.IndexOf (key:'TKey) (value:'TValue) =
        heapList.IndexOf(new KeyValuePair<'TKey, 'TValue>(key, value))
 
    // Removes an item from the queue at the specified index
    member this.RemoveAt (index:int) =
        checkIndex index
        deleteItem index
 
    // Determines whether an item is in the queue
    member this.ContainsKey (key:'TKey) =
        positionDict.ContainsKey key
 
    // Determines whether an item is in the queue
    member this.ChangeKey (key:'TKey) (value:'TKey)=
        let index = positionDict.[key]
        let item = heapList.[index]
        heapList.[index] <- new KeyValuePair<'TKey, 'TValue>(value, item.Value)
        positionDict.Remove(key) |> ignore
        positionDict.Add(value, index)
        heapifyUpDown index
 
    // Removes an item from the queue for the specified key
    member this.RemoveKey (key:'TKey) =
        let index = positionDict.[key]
        this.RemoveAt index
 
    // Modifies elements based on index values
    member this.Item
        with get(key) =
            let index = positionDict.[key]
            heapList.[index]
        and set(key) (value) =
            let index = positionDict.[key]
            heapList.[index] <- new KeyValuePair<'TKey, 'TValue>(key, value)
            heapifyUpDown index
 
    // Returns the count of the queue
    member this.Count
        with get() = heapList.Count
 
    // Resets the capacity of the Queue
    member this.TrimExcess() =
        heapList.TrimExcess()
 
    // Returns the capacity of the queue
    member this.Capacity
        with get() = heapList.Capacity
 
    // Clears the queue
    member this.Clear() =
        heapList.Clear()
 
 
    // Standard IList members
    interface ICollection<KeyValuePair<'TKey, 'TValue>> with
 
        member this.Add(item:KeyValuePair<'TKey, 'TValue>) =
            this.Enqueue item.Key item.Value
 
        member this.Clear() =
            heapList.Clear()
 
        member this.Contains(item:KeyValuePair<'TKey, 'TValue>) =
            heapList.Contains(item)
 
        member this.Count
            with get() = heapList.Count
 
        member this.CopyTo(toArray:KeyValuePair<'TKey, 'TValue>[], arrayIndex:int) =
            heapList.CopyTo(toArray, arrayIndex)
 
        member this.IsReadOnly
            with get() = false
 
        member this.Remove(item:KeyValuePair<'TKey, 'TValue>) =
            let index = heapList.IndexOf(item)
            if (inRange index).IsSome then
                deleteItem index
                true
            else
                false
 
        member this.GetEnumerator() =
            upcast heapList.GetEnumerator()
 
    // IEnumerable GetEnumerator implementation
    interface IEnumerable with
        member this.GetEnumerator() =  
            upcast heapList.GetEnumerator()