# Universal-Object-Pooler
An handy utility namespace to create Object Poolers for your game or application, made with Unity 2021's new Object Pool API.

This asset helps you creating flexible Object Pools to store and recycle all kinds of instantiables classes : GameObjects, ScriptableObjects or custom classes.
This project comes with a demo scene with the instantiation and recycling of a prefab using the IPooled interface to add complex behaviour once an object is retrieved or returned to the Pool.

## How to use it :

Each ObjectPooler uses a Pool class to group all the instantiation settings in once place: 

```csharp
Pool newPool = new Pool<TClassToInstantiate>(key: "ClassName", defaultCapacity: 1000, createFunc () => new TClassToInstantiate());
```
- "key" is used to retrieve an item from a specific pool in case you store two classes of the same type (ie. GameObjects or ScriptableObjects). For convenience, you can use nameof(TClassToInstantiate) if you only have one sample of that type.
- "defaultCapacity" is used to set a default size to the ObjectPool's base item array. It will always try to instantiate items only when needed, and will double its capacity if it overshoots its previous one.
- "createFunc" describes how you want the item to be cloned. If TClassToInstantiate is a parent class, the createFunc can accept any subclass type as paramater. For GameObjects, do not forget to use the return keyword to retrieve the object for usage.

The ClassPooler class used to store all ObjectPools is created by adding any number of Pools to its params list:

```csharp
ClassPooler<TParent> pooler = new ClassPooler<TParent>
(
new Pool<TChildClass1>(nameof(TChildClass1), 10, () => new TChildClass1()),
new Pool<TChildClass2>(nameof(TChildClass2), 10, () => new TChildClass2()),
new Pool<TChildClass3>(nameof(TChildClass3), 10, () => new TChildClass3())
//etc...
);
```
This is useful if, for example, you need a way to recycle all states from a state machine from one single Pooler encompassing all states types under a parent type.

The handle methods are:
```csharp
//Retrieves a clone from the pool (the key is optional if you sed nameof() as the key in the constructor).
TChildClass1 clone = pooler.GetFromPool<TChildClass1>(/*key*/); 

//Returns a clone to the pool (the key is optional if you sed nameof() as the key in the constructor).
pooler.RerturnToPool(clone /*, key*/)

```

A PooledObject struct is available if you just want to use a simple ObjectPool directly from Unity's API instead of using the ClassPooler :
```csharp
using(myPool.Get(out MyClass myInstance)) // When leaving the scope myInstance will be returned to the pool.
     {
         // Do something with myInstance
     }
