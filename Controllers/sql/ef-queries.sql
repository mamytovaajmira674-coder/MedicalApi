-- ==========================================
-- SQL-запросы, которые генерирует EF Core
-- ==========================================

-- 1. GET /api/Patients (получить всех пациентов)
-- SQL:
SELECT p."Id", p."Address", p."BirthDate", p."IsMale", p."Name", p."PhoneNumber"
FROM "Patients" AS p;

-- 2. GET /api/Patients/1 (получить одного пациента по Id)
-- SQL:
SELECT p."Id", p."Address", p."BirthDate", p."IsMale", p."Name", p."PhoneNumber"
FROM "Patients" AS p
WHERE p."Id" = 1
LIMIT 1;

-- 3. POST /api/Patients (создать пациента)
-- SQL:
INSERT INTO "Patients" ("Address", "BirthDate", "IsMale", "Name", "PhoneNumber")
VALUES ('ул. Ленина', '1990-05-15', true, 'Иван', '123456')
RETURNING "Id";

-- 4. PUT /api/Patients/1 (обновить пациента)
-- SQL:
UPDATE "Patients" 
SET "Address" = 'Новый адрес', "BirthDate" = '1990-05-15', "IsMale" = true, "Name" = 'Иван', "PhoneNumber" = '123456'
WHERE "Id" = 1;

-- 5. DELETE /api/Patients/1 (удалить пациента)
-- SQL:
DELETE FROM "Patients"
WHERE "Id" = 1;

-- ==========================================
-- ОТВЕТЫ НА ВОПРОСЫ
-- ==========================================

-- Вопрос 1: Чем SQL от GET /api/patients отличается от GET /api/patients/1?
-- Ответ: GET /api/patients (без Id) не имеет условия WHERE, поэтому возвращает все строки.
-- GET /api/patients/1 (с Id) добавляет WHERE "Id" = 1 и LIMIT 1, поэтому возвращает только одну строку.

-- Вопрос 2: Как приложение узнаёт id, который выдала база при POST?
-- Ответ: В SQL-запросе INSERT EF Core добавляет RETURNING "Id". База данных возвращает сгенерированный Id обратно в приложение, и EF Core записывает его в свойство patient.Id.

-- Вопрос 3: Что делать с '?' в логах?
-- Ответ: Символ '?' означает, что EF Core скрывает реальные значения параметров в целях безопасности. Чтобы увидеть реальные значения, можно временно добавить .EnableSensitiveDataLogging() в Program.cs после UseNpgsql(...).