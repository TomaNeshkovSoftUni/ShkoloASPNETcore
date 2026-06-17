# Школо (ShkoloASPNETcore) – Училищна Уеб Система

Това е уеб базирана платформа, която (се опитва да) клонира Shkolo. 

Основни Функционалности

- Има 3 роли (Учиел, Ученик, Администратор)
- Ученици се създават чрез регистрация, а учителите и администраторите само чрез seed-ване в DB
- Оценки, отсъствия, закъснения, похвали, забележки и т.н. могат да се добавят динамично в приложението
- Системата автоматично записва името/имейла на профила (Учител или Администратор), който е въвел или коригирал съответната оценка, за максимална прозрачност.

Technology stack:

Backend: .NET / ASP.NET Core (MVC / Razor Pages)
Database ORM: Entity Framework Core (EF Core)
База Данни: Microsoft SQL Server
Frontend: HTML5, CSS3, Bootstrap 5, JavaScript
Сигурност: ASP.NET Core Identity (authN & authZ)

Инсталация и Стартиране

За да стартирате проекта локално следвайте следните стъпки:

 1. Клониране на хранилището
```bash
git clone [https://github.com/your-username/ShkoloASPNETcore.git](https://github.com/your-username/ShkoloASPNETcore.git)
cd ShkoloASPNETcor
```
2. Update-ване на базата данни чрез PMC:

```PMC
Update-Database
```

3. Рънване ✓
