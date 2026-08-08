<div align="center">
  <h1>📝 Simple Text Editor</h1>
  <p>
    <b>Сучасний, швидкий та мінімалістичний текстовий редактор з підтримкою Markdown та вкладок.</b>
  </p>
  <p>
    <img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 8" />
    <img src="https://img.shields.io/badge/Platform-Windows-0078D6?style=for-the-badge&logo=windows&logoColor=white" alt="Windows" />
    <img src="https://img.shields.io/badge/Architecture-Clean-success?style=for-the-badge" alt="Clean Architecture" />
  </p>
</div>

---

## ✨ Особливості (Features)

- **🎨 Сучасний Дизайн (Slate Dark Theme):** Глибокий темно-синій інтерфейс, що не втомлює очі. Повністю централізоване управління темами через `AppTheme`. Повна підтримка темної теми навіть для стандартних віконних елементів Windows (Scrollbars, ComboBox dropdowns).
- **📝 Два Режими Редагування:** 
  - **Форматований (WYSIWYG):** Повноцінне форматування "на льоту" (як у Word), вставка реальних фотографій та гіперпосилань прямо в текст.
  - **Звичайний текст (Markdown):** Швидкий набір markdown-синтаксису з підтримкою вбудованого **Markdown Preview** для перегляду згенерованого HTML.
- **✨ Унікальні Діалоги:** Власні стилізовані темні вікна (Dialogs) для зручної вставки зображень та посилань.
- **🚀 Ідеальна Плавність (Flicker-Free):** Глибока інтеграція подвійної буферизації (Double Buffering) через Win32 API (`WS_EX_COMPOSITED`), що гарантує відсутність блимання інтерфейсу при зміні розмірів чи прокручуванні.
- **📑 Підтримка Вкладок (Tabs):** Працюйте з кількома документами одночасно в єдиному вікні без нагромадження.
- **🔍 Пошук та Заміна:** Швидкий пошук тексту по всьому документу.
- **🖨️ Друк:** Зручна система виводу документів на друк.

## 🛠 Технологічний Стек

- **Мова:** C#
- **Фреймворк:** .NET 8.0 (Windows Forms)
- **Архітектура:** Модульна (UI Builders, Services, Theme Management)
  
## 📂 Структура Проєкту

Проєкт побудований з використанням принципів чистого коду та розділення відповідальності:

```text
SimpleTextEditor/
├── Theme/                # Централізоване управління кольорами (AppTheme, DarkColorTable)
├── UI/                   # Будівельники інтерфейсу (TabManager, MenuBuilder, StatusBarBuilder, FormatToolbarBuilder)
├── Services/             # Бізнес-логіка (FormatHandler, PrintHandler, FileHandler)
└── Form1.cs              # Головне вікно (Контейнер для UI)
```

## 🚀 Як запустити (Getting Started)

### Вимоги
- Встановлений [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).
- Windows OS (проєкт використовує Windows Forms).

### Запуск з Терміналу

1. Склонуйте репозиторій:
   ```bash
   git clone https://github.com/Lutvunenko-Dmutro/SimpleTextEditor.git
   ```
2. Перейдіть до теки з проєктом:
   ```bash
   cd SimpleTextEditor/SimpleTextEditor
   ```
3. Запустіть застосунок:
   ```bash
   dotnet run
   ```

## 💡 Історія Розробки

Цей застосунок пройшов шлях від класичного сірого WinForms-блокнота до сучасного IDE-подібного редактора. Під час рефакторингу:
- Всі "жорстко закодовані" (hardcoded) кольори були винесені у єдиний статичний клас `AppTheme`.
- Громіздкий код головної форми був розбитий на ізольовані класи-будівельники (Builders) та сервіси (Services).
- Стандартні кнопки Windows Forms були замінені на сучасні ToolStrips зі спеціальним `ProfessionalRenderer`.

---
<div align="center">
  <i>Створено з пристрастю до чистого коду.</i>
</div>