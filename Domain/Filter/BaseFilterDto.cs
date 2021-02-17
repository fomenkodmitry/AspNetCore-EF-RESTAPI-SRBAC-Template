namespace Domain.Filter
{
    /// <summary>
    /// Базовый контейнер фильтра для всех методов фильтрации
    /// </summary>
    public class BaseFilterDto
    {
        /// <summary>
        /// Параметры сортировки списка. Если не задано - сортировка по дате создания, новые сверху.
        /// </summary>
        public FilterSortDto Sort { get; set; }
        /// <summary>
        /// Параметры пагинации
        /// </summary>
        public FilterPagingDto Paging { get; set; }
        /// <summary>
        /// Статус: активен/заблокирован
        /// </summary>
        public bool? IsActive { get; set; }
    }
}
