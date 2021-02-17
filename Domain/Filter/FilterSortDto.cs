namespace Domain.Filter
{
    /// <summary>
    /// Параметры сортировки для фильтрации
    /// </summary>
    public class FilterSortDto
    {
        /// <summary>
        /// Название поля для сортировки
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// Сортировка в обратном направлении (true - в обратном, false - в прямом)
        /// </summary>
        public bool IsDescending { get; set; }
    }
}