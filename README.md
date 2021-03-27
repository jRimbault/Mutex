# Motivation

I often find myself with a class like :

```cs
class Fooer
{
    private readonly object _fooLock = new object();
    private readonly IFoo _foo;
    
    public void CallTheBarOnTheFoo()
    {
        lock (_fooLock)
        {
            _foo.Bar();
        }
    }
}
```

This `SyncLock` class allows coupling the lock object and what it "protects" together :

```cs
class Fooer
{
    private readonly SyncLock<IFoo> _foo;
    
    public void CallTheBarOnTheFoo()
    {
        using (var guard = _foo.Lock())
        {
            guard.Value.Bar();
        }
    }
}
```

This way I know, at compile time, that the `IFoo` is never accessed without proper locking and unlocking.
