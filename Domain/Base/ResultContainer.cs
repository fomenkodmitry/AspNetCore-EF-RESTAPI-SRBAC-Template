using Domain.Error;

namespace Domain.Base
{
    /// <summary>
    /// Контейнер результата действия
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultContainer<T>
    {
        /// <summary>
        /// Конструктор для успешного результата выполнения
        /// </summary>
        /// <param name="result"></param>
        public ResultContainer(T result)
        {
            Result = result;
        }

        /// <summary>
        /// Конструктор для выполнения с ошибкой
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="errorFields"></param>
        public ResultContainer(ErrorCodes errorCode, params string[] errorFields)
        {
            Error = errorCode;
            ErrorField = errorFields != null ? string.Join(",", errorFields) : null;
        }

        /// <summary>
        /// Код ошибки
        /// </summary>
        public ErrorCodes? Error { get; set; }
        /// <summary>
        /// Поле с неверными данными
        /// </summary>
        public string ErrorField { get; set; }
        /// <summary>
        /// Объект как результат успешного выполнения
        /// </summary>
        public T Result { get; }
    }
}