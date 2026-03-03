# Принципи програмування у проєкті Wordle

---

## 1. Single Responsibility Principle (SRP)

Більшість класів проєкту мають чітко визначену одну відповідальність:

| Клас | Відповідальність |
|------|-----------------|
| [`Board`](ClassLibrary1/Board.cs) | Зберігає стан поля: список здогадок, ліміт спроб |
| [`WordChecker`](ClassLibrary1/WordChecker.cs) | Перевіряє здогадку відносно цільового слова |
| [`Word`](ClassLibrary1/Word.cs) | Завантажує словник і генерує цільове слово |
| [`Guess`](ClassLibrary1/Guess.cs) | Зберігає одну здогадку: слово та стани літер |

**⚠️ Порушення:** клас [`Game`](ClassLibrary1/Game.cs) порушує SRP — одночасно керує ігровим циклом і відповідає за весь UI: очищення консолі, малювання поля, виставлення кольорів, читання вводу. Методи [`Render`](ClassLibrary1/Game.cs#L63-L79), [`DrawGuess`](ClassLibrary1/Game.cs#L81-L89), [`DrawEmptyRow`](ClassLibrary1/Game.cs#L91-L95), [`SetConsoleColors`](ClassLibrary1/Game.cs#L97-L114) не мають відношення до логіки гри.

---

## 2. Encapsulation (Інкапсуляція)

У класі [`Board`](ClassLibrary1/Board.cs) інкапсуляція реалізована коректно — внутрішній список здогадок прихований, назовні доступний лише для читання:

[`Board.cs`, рядки 14–15](ClassLibrary1/Board.cs#L14-L15)
```csharp
private readonly List<Guess> guesses = new();
public IReadOnlyList<Guess> Guesses => guesses;
```

Додавання здогадок контролюється через метод [`AddGuess`](ClassLibrary1/Board.cs#L25-L34), який перевіряє інваріанти перед змінами.

**⚠️ Порушення:** у [`Guess.cs`, рядки 11–12](ClassLibrary1/Guess.cs#L11-L12) поля оголошені публічними — будь-який код може змінити слово або стани після створення об'єкта. У [`Word.cs` `TargetWord` та `AllWords`](ClassLibrary1/Guess.cs#L9-L10) публічно доступні, що дозволяє читерство та мутацію словника ззовні.

---

## 3. Принцип відкритості/закритості (Open/Closed Principle)

Класи мають бути відкриті для розширення, але закриті для змін. У проєкті це досягається через композицію та ін'єкцію залежностей:

- [`Game` отримує `WordChecker` через конструктор](ClassLibrary1/Game.cs#L11-L13), тому можна замінити логіку перевірки, створивши новий клас з тими ж методами — без змін у `Game`.  

- `Word` дозволяє розширення через спадкування або створення нового класу з аналогічним інтерфейсом, що не потребує модифікації існуючого коду.  
  [ClassLibrary1/Word.cs](ClassLibrary1/Word.cs)

Хоча інтерфейси відсутні, архітектура дає змогу додавати нову поведінку без зміни наявних класів, що відповідає OCP.

---

## 4. Fail Fast

Некоректний стан виявляється одразу і сигналізується виключенням, а не ігнорується:

[`Board.cs`, рядки 27–31](ClassLibrary1/Board.cs#L27-L31)
```csharp
if (!CanGuess)
    throw new InvalidOperationException("Більше немає спроб.");

if (guess.Word.Length != WordLength)
    throw new ArgumentException("Невірна довжина слова.");
```

[`Word.cs`, рядки 21–22](ClassLibrary1/Word.cs#L21-L22)
```csharp
if (!File.Exists(path))
    throw new FileNotFoundException($"Файл не знайдено: {path}");
```

---

## 5. DRY (Don't Repeat Yourself)

У коді відсутнє суттєве дублювання логіки:

Логіка перевірки слів зосереджена в одному місці — у класі [`WordChecker`](ClassLibrary1/WordChecker.cs).

Методи для роботи з кольорами консолі винесено в окремий метод [`SetConsoleColors`](ClassLibrary1/Game.cs#L97-L114) у класі Game, хоча в ідеалі їх варто було б винести в окремий клас.

---

## 6. Composition Over Inheritance

У проєкті не використовується успадкування, всі зв'язки реалізовано через композицію: [`Game`](ClassLibrary1/Game.cs) містить посилання на [`Word`](ClassLibrary1/Word.cs), [`WordChecker`](ClassLibrary1/WordChecker.cs) та [`Board`](ClassLibrary1/Board.cs). Це робить систему гнучкішою та легшою для тестування.

[`Game.cs`, рядки 11–13](ClassLibrary1/Game.cs#L11-L13)
```csharp
private readonly Word wordProvider;
private readonly WordChecker checker;
private readonly Board board;
```

Кожен компонент можна замінити незалежно, не торкаючись інших.
