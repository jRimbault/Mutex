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
                _foo.Value.Bar(_qaz.Quz());
            }
        }
    }
}
```

This `SyncLock` class allows coupling the lock object and what it "protects" together :

```cs
class Fooer
{
    private readonly SyncLock<IQaz> _qaz;
    private readonly SyncLock<IFoo> _foo;
    
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
