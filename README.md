# Motivation

I've often found myself with a class like :

```cs
class Fooer
{
    private readonly object _qazLock = new object();
    private readonly IQaz _qaz;
    private readonly object _fooLock = new object();
    private readonly IFoo _foo;
    
    public void DoThing()
    {
        lock (_qazLock)
        {
            lock (_fooLock)
            {
                _foo.Bar(_qaz.Quz());
            }
        }
    }
}
```

This `Mutex` class allows coupling the lock object and what it "protects" together :

```cs
class Fooer
{
    private readonly Mutex<IQaz> _qaz;
    private readonly Mutex<IFoo> _foo;
    
    public void DoThing()
    {
        using (var qaz = _qaz.Lock())
        using (var foo = _foo.Lock())
        {
            foo.Value.Bar(qaz.Value.Quz());
        }
    }
}
```

This way I know, at compile time, that `IFoo` and `IQaz` are never accessed without proper locking and unlocking and visual noise is reduced.

___

Single file version [here][gist].

[gist]: https://gist.github.com/jRimbault/d2640e9d8ff3b998d66fbc4a57cf7e0b

___

Inspiration taken from the Rust [`Mutex`][RustMutex] type. Later discovered Jon Skeet had done a similar [`SyncLock`][SyncLock] which is more about timing out than managing concurrent access to a ressource.


[RustMutex]: https://doc.rust-lang.org/stable/std/sync/struct.Mutex.html
[SyncLock]: https://jonskeet.uk/csharp/miscutil/usage/locking.html
