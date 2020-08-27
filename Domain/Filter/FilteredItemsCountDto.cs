﻿using System;

namespace Domain.Filter
{
    /// <summary>
    /// Сведения о количестве элементов, удовлетворяющих фильтру, и страниц
    /// </summary>
    public class FilteredItemsCountDto
    {
        public FilteredItemsCountDto(int itemsCount, int pageSize)
        {
            ItemsCount = itemsCount;
            PagesCount = Convert.ToInt32(Math.Ceiling((decimal)itemsCount / (decimal)pageSize));
        }

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
