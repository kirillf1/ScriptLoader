# ScriptLoader
Реализовать на WPF приложение с пользовательским интерфейсом, где должен быть Textbox, принимающий адрес сайта, кнопка, запускающая распознавание, и таблица результатов. 
Проект состоит из 2-х частей Core и UI на WPF.
# Core
Содержится основная логика загрузки и сохранения скриптов.
Ключевой класс это Script
## ScriptLoaders
Содержится 1 реализация через httpClient. Для парсинга использовалась библиотека HtmlAgilityPack. Можно также реализовать через Selenium чтобы подгружать данные через браузер.
## FileService
Содержится реализация записи и чтения на жесткий диск скриптов

# UI
Содержится 2 окна. MainWindow реализует основной функционал программы. ScriptWindow показывает js скрипт (открывается двойным кликом на имя файла).
Для развертывания использовал Microsoft.Extensions.DependencyInjection. Для настроек конфигурации (прокси, папка хранения скриптов и т.д.) используется appsettings.json 
