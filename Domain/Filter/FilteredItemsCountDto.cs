namespace Domain.Filter
{
    /// <summary>
    /// Сведения о количестве элементов, удовлетворяющих фильтру, и страниц
    /// </summary>
    public class FilteredItemsCountDto
    {
        /// <summary>
        /// Количество элементов, удовлетворяющих фильтру
        /// </summary>
        public int ItemsCount { get; set; }
        /// <summary>
        /// Количество страниц в соответствии с заданным пейджингом
        /// </summary>
        public int PagesCount { get; set; }
    }
}
