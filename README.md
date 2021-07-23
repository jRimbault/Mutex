# Motivation

I've often found myself with a class like :

```cs
class Zhuli
{
    private readonly object _fooLock = new object();
    private readonly IFoo _foo;
    
    public void DoTheThing()
    {
        lock (_fooLock)
        {
            _foo.Bar();
        }
    }
}
```

This `Mutex` class allows coupling the lock object and what it "protects" together :

```cs
class Zhuli
{
    private readonly Mutex<IFoo> _foo;
    
    public void DoTheThing()
    {
        using var foo = _foo.Lock();
        foo.Value.Bar();
    }
}
```

This way I know, at compile time, that `IFoo` is never[<sup>1</sup>](#note-1) accessed without proper locking and unlocking. The locking isn't simply upheld by convention and developer discipline, it's upheld and checked by the compiler.

___

Single file version [here][gist].

[gist]: https://gist.github.com/jRimbault/d2640e9d8ff3b998d66fbc4a57cf7e0b

___

### Prior art

Inspiration taken from the Rust [`Mutex`][RustMutex] type. Later discovered Jon Skeet had done a similar [`SyncLock`][SyncLock] which is more about timing out than managing concurrent access to a ressource.


<sup id="note-1">1</sup>: though of course C# lacks an ownership system which can actually guarantee that, it's on the user to not take a reference to the wrappee value outside of the wrapper.

[RustMutex]: https://doc.rust-lang.org/stable/std/sync/struct.Mutex.html
[SyncLock]: https://jonskeet.uk/csharp/miscutil/usage/locking.html
