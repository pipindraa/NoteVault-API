# Notes

## Описание

Необходимо реализовать эффективное и надежное backend-приложение для работы с заметками, которое позволит пользователям получать, создавать, обновлять и удалять заметки.

## Функционал Web API

- Получение списка всех заметок;
- Получение определённой заметки по его Id;
- Регистрация новой заметки;
- Изменение информации о существующей заметке;
- Удаление заметки;
- Работа с изображениями (получение, хранение и получение изображений для заметок).

## Стек

- **C#** & **.NET CORE 8** - язык и фреймворк для создания кроссплатформенных серверных приложений;
- **PostgreSQL** - реляционная база данных для хранения и управления данными;
- **ASP.NET** - фреймворк для построения RESTful API на платформе .NET;
- **EntityFramework Core** - ORM для работы с базой данных через C#-код;
- **xUnit** - инструмент для написания и запуска модульных тестов;
- **Docker** - платформа для контейнеризации и удобного развертывания приложений.

## Дополнительный функционал

- Система аутентификации, используя **JWT**.
- Валидация **DTO**.
- Обработка запроса на получение списка заметок так, чтобы с его помощью можно было осуществить поиск по заметкам, отфильтровать их, отсортировать. Результат также должен быть разбит на страницы.

## Модели

```C#
public interface IEntity
{
    public Guid Id { get; set; }
}

public class Note : IEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Tag> Tags { get; set; }
    public List<string> ImageUrls { get; set; }
    public DateTime CreationDate { get; set; }
}

public class Tag : IEntity
{
    public string Name { get; set; }
}
```

## Так же проект предполагает

- Строгое соответствие принципам **RESTful API**;
- Глобальная обработка ошибок через **middleware**;
- Следование **GitFlow** в процессе разработки;
- Следование **Conventional Commits** в процессе разработки; 
- Использование **Docker** и **Docker-compose**;
- Cоблюдение принципов **SOLID**.

**ВАЖНО!** Реализация должна находиться на **приватном** репозитории, на который необходимо добавить <code>SU-MCC</code> аккаунт.

## Полезные источники
- [C#](https://learn.microsoft.com/ru-ru/dotnet/csharp/)
- [ASP.NET](https://learn.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-9.0)
- [PostgreSQL](https://www.postgresql.org/docs/)
- [EntityFramework](https://learn.microsoft.com/ru-ru/ef/)
- [JWT](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/configure-jwt-bearer-authentication?view=aspnetcore-9.0)
- [xUnit](https://learn.microsoft.com/ru-ru/dotnet/core/testing/unit-testing-csharp-with-xunit)
- [Docker](https://www.docker.com/)
- [GitFlow](https://www.atlassian.com/ru/git/tutorials/comparing-workflows/gitflow-workflow)
- [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/)
