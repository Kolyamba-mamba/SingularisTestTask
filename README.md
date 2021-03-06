# Описание тестового задания
Необходимо реализовать два приложения на С# (платформа .net core), которые реализуют следующую логику:
## Watcher
Это приложение должно следить за появлением новых файлов (файлы png, jpg или bmp) в папке <app>\incoming. 
Как только в папке появится новый файл необходимо отправить его через шину - в нашем случае второму сервису и удалить из исходной папки.
## Converter
Это приложение ожидает сообщения в шине и, как только оно будет получено, должно наложить на полученное изображение watermark с логотипом Сингуляриса. Результат сохранить в папке <app>\outcoming.
## Доп условия
В качестве шины использовать rabbitmq.
Упрощения: считается, что в папку <app>\incoming будут помещаться только корректные файлы изображения с размером не меньше, чем размер watermark и не больше чем 1024 х 1024.
# Описание структуры решения
## Watcher
Реализует первое приложение.
* Для слежения за папкой используется `System.IO.FileSystemWatcher`.
* Для отправки сообщений используется библиотека `EasyNetQ`.
* Для обработки параметров командной строки используется библиотека `MatthiWare.CommandLine`.
## Converter
Реализует второе приложение.
* Для получения сообщений используется библиотека `EasyNetQ`.
* Для наложения водяного знака используется библиотека `ImageProcessor`.
* Для обработки параметров командной строки используется библиотека `MatthiWare.CommandLine`.
## Tests
Реализует тесты логики приложений.
* Используется библиотека для unit-тестирования `NUnit`.
* Для создания mock-объектов используется библиотека `FakeItEasy`.
* Также используется библиотека `FluentAssertions`
## Common
Реализует общие части двух приложений
# Описание параметров командной строки
## Converter
* `-o`, `--opacity` - Устанавливает прозрачность водяного знака в процентах. Допустимые значения - целые числа. Значение по-умолчанию - `100`.
* `-s`, `--relative-size` - Устанавливает размер водяного знака относительно изначальной картинки в процентах. Допустимые значения - целые числа. Значение по-умолчанию - `50`.
* `--position` - Устанавливает позицию водяного знака на картинке. Значение по-умолчанию - `Center`. Допустимые значения:
  - `Center` - Центр исходной картинки;
  - `LeftTop` - Левый верхний угол исходной картинки;
  - `LeftBottom` - Левый нижний угол исходной картинки;
  - `RightTop` - Правый верхний угол исходной картинки;
  - `RightBottom` - Правый нижний угол исходной картинки;
* `--path` - Относительный или полный путь к водяному знаку. Допустимые значения - корректный путь к файлу. Значение по-умолчанию - `"logo.png"`(При запуске из IDE логотип должен находиться в текущей рабочей директории исполняемого файла или укажите путь к нему самостоятельно через параметры командной строки).
* `-r`, `--resize` - Определяет нужно ли менять размер водяного знака. Допустимые значения - true или false. Значение по-умолчанию - true.
* `--host` - Определяет имя хоста для `RabbitMQ` (Имя должно совпадать у `watcher` и `converter`). Значение по-умолчанию - `"localhost"`.
* `-f`, `--folder` - Определяет место сохранения измененного файла. Допустимые значения - корректный путь к файлу. Значение по-умолчанию - `"outcoming"`.
## Watcher
* `--host` - Определяет имя хоста для `RabbitMQ` (Имя должно совпадать у `watcher` и `converter`). Значение по-умолчанию - `"localhost"`.
* `-f`, `--folder` - Определяет место сохранения измененного файла. Допустимые значения - корректный путь к файлу. Значение по-умолчанию - `"incoming"`.
* `-d`, `--delete` - Определяет нужно ли удалять файл после отправки. Допустимые значения - true или false. Значение по-умолчанию - true.
