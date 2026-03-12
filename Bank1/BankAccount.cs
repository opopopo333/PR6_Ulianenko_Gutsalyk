using System;  // Подключение пространства имён для работы с Console и исключениями

namespace BankAccountNS
{
    /// <summary>
    /// Представляет банковский счёт клиента с базовыми операциями: пополнение и снятие средств.
    /// </summary>
    /// <remarks>
    /// Класс обеспечивает проверку корректности операций и не позволяет уйти в минус.
    /// Потокобезопасность не гарантируется.
    /// </remarks>
    public class BankAccount
    {
        // Поле хранит имя клиента, только для чтения после инициализации
        private readonly string m_customerName;
        public const string DebitAmountExceedsBalanceMessage = "Debit amount exceeds balance";
        public const string DebitAmountLessThanZeroMessage = "Debit amount is less than zero";
        public const string CreditAmountLessThanZeroMessage = "Credit amount is less than zero";

        // Поле хранит текущий баланс счёта
        private double m_balance;

        /// <summary>
        /// Приватный конструктор по умолчанию (защита от создания пустого счёта).
        /// </summary>
        private BankAccount() { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="BankAccount"/>.
        /// </summary>
        /// <param name="customerName">Имя владельца счёта.</param>
        /// <param name="balance">Начальный баланс счёта.</param>
        /// <exception cref="ArgumentException">
        /// Может быть выброшено, если имя клиента пустое (рекомендуется добавить проверку).
        /// </exception>
        public BankAccount(string customerName, double balance)
        {
            m_customerName = customerName;
            m_balance = balance;
        }

        /// <summary>
        /// Получает имя владельца счёта.
        /// </summary>
        /// <value>Имя клиента в виде строки.</value>
        public string CustomerName
        {
            get { return m_customerName; }
        }

        /// <summary>
        /// Получает текущий баланс счёта.
        /// </summary>
        /// <value>Баланс в виде числа с плавающей точкой.</value>
        public double Balance
        {
            get { return m_balance; }
        }

        /// <summary>
        /// Снимает указанную сумму со счёта (дебетование).
        /// </summary>
        /// <param name="amount">Сумма для снятия.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Выбрасывается, если сумма отрицательная или превышает текущий баланс.
        /// </exception>
        /// <remarks>
        /// Операция уменьшает баланс только если все проверки пройдены.
        /// </remarks>
        public void Debit(double amount)
        {
            // Проверка: нельзя снять больше, чем есть на счёте
            if (amount > m_balance)
            {
                throw new ArgumentOutOfRangeException(nameof(amount),DebitAmountExceedsBalanceMessage);
            }

            // Проверка: сумма должна быть неотрицательной
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), DebitAmountLessThanZeroMessage);
            }
            // Уменьшаем баланс на указанную сумму
            m_balance -= amount;
        }

        /// <summary>
        /// Зачисляет указанную сумму на счёт (кредитование).
        /// </summary>
        /// <param name="amount">Сумма для зачисления.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Выбрасывается, если сумма отрицательная.
        /// </exception>
        public void Credit(double amount)
        {
            // Проверка: нельзя зачислить отрицательную сумму
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount),CreditAmountLessThanZeroMessage);
            }

            // Увеличиваем баланс на указанную сумму
            m_balance += amount;
        }

        /// <summary>
        /// Точка входа в программу. Демонстрирует использование класса.
        /// </summary>
        public static void Main()
        {
            // Создаём новый счёт с начальным балансом
            BankAccount ba = new BankAccount("Mr. Roman Abramovich", 11.99);

            // Пополняем счёт на 5.77
            ba.Credit(5.77);

            // Снимаем 11.22
            ba.Debit(11.22);

            // Выводим текущий баланс в консоль
            // Ожидаемый результат: 11.99 + 5.77 - 11.22 = 6.54
            Console.WriteLine("Current balance is ${0}", ba.Balance);

            // Ожидаем нажатия клавиши, чтобы консоль не закрылась сразу
            Console.ReadLine();
        }
    }
}