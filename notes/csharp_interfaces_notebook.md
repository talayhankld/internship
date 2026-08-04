# C# Interfaces — My Notebook

## 1. What is an Interface? (The Core Idea)

An interface is a **contract**. It says:
> "Any class that implements me MUST have these methods."

It does **not** say *how* those methods work — just that they must exist, with a specific name, specific inputs, and a specific return type.

**Analogy — Shapes:** A circle and a square both have an "area," but calculate it differently:
- Circle: `π × r²`
- Square: `side × side`

An interface captures the *shared idea* ("has an area") without caring about the formula.

---

## 2. The Three Ingredients of an Interface

1. **The `interface` keyword** — declares the contract
2. **A method signature** — name, parameters, return type (NO body, NO logic)
3. **A class that implements it** — writes `: IInterfaceName` after the class name, and provides the actual logic

---

## 3. Full Working Example: Shapes

```csharp
// 1. THE CONTRACT
public interface IShape
{
    double GetArea();   // no body — just a promise
}

// 2. A CLASS THAT FULFILLS THE PROMISE
public class Circle : IShape
{
    public double Radius;

    public double GetArea()
    {
        return 3.14159 * Radius * Radius;
    }
}

// 3. ANOTHER CLASS, DIFFERENT LOGIC, SAME CONTRACT
public class Square : IShape
{
    public double Side;

    public double GetArea()
    {
        return Side * Side;
    }
}

// 4. TRIANGLE — SAME PATTERN
public class Triangle : IShape
{
    public double Height;
    public double Base;

    public double GetArea()
    {
        return Base * Height / 2;
    }
}
```

---

## 4. Why It's Useful (The Payoff)

```csharp
public void PrintArea(IShape shape)
{
    Console.WriteLine("Area: " + shape.GetArea());
}
```

This method works for `Circle`, `Square`, `Triangle` — or any future shape — **without ever changing**. It only cares that whatever gets passed in has a `GetArea()` method, because it's an `IShape`.

**This is the whole point of interfaces: write code once against the contract, and it works for anything that follows that contract — even things that don't exist yet.**

---

## 5. Common Syntax Mistakes (Things I Got Wrong Before)

| ❌ Wrong | ✅ Correct | Why |
|---|---|---|
| `public class Triangle : IShape public void getarea...` (no braces) | Everything inside `{ }` | Curly braces define where the class/method body starts and ends |
| `public void getarea.triangle` | `public double GetArea()` | No dots in method names. Method must match the interface's return type (`double`, not `void`) and **exact name/casing** (`GetArea`, not `getarea`) |
| `return = base*height/2` | `return Base * Height / 2;` | No `=` after `return`. Every statement ends with `;` |
| Using `height`/`base` without declaring them | `public double Height;` / `public double Base;` first | You must declare a variable before using it — it's the class's data |

**Case sensitivity matters in C#.** `GetArea` and `getarea` are two different names to the compiler.

---

## 6. Quick Self-Test

Answer these without looking above:

1. What are the 3 parts needed to define and use an interface?
2. If I create a `Rectangle : IShape` class, what's the ONE method I'm required to write?
3. Why can't `PrintArea(IShape shape)` be called with an object that has no `GetArea()` method?
4. What symbol/keyword is missing if my code won't compile because "not all statements ended properly"?

*(Answers: 1. interface declaration, method signature, implementing class · 2. `GetArea()` returning a `double` · 3. Because C# won't compile a class that claims `: IShape` but doesn't implement everything the interface promises · 4. A semicolon `;`)*

---

## 7. Connecting to Real Work (Fintech / Git Transactions Service)

Same pattern, applied to my internship task:

```csharp
public interface IGitTransactionService
{
    string GetTransactionById(string id);
    void AddTransaction(string id, string message);
}

public class GitTransactionService : IGitTransactionService
{
    public string GetTransactionById(string id)
    {
        return $"Transaction {id}";
    }

    public void AddTransaction(string id, string message)
    {
        // logic here
    }
}
```

Why this matters at work: ASP.NET Core's Dependency Injection system lets you register "whenever someone asks for `IGitTransactionService`, give them a `GitTransactionService`." Later, you can swap in a different implementation (e.g. one that talks to a real database) without changing any code that depends on the interface.
