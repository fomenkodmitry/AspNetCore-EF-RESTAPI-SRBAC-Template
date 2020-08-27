﻿using Domain.Base;
using System.Collections.Generic;

namespace Domain.Filter
{
    public class FilteredItemsDto<T> where T: BaseModel
    {
        /// <summary>
        /// Отфильтрованные элементы с учетом пагинации (текущая страница)
        /// </summary>
        public IEnumerable<T> Items { get; set; }
        /// <summary>
        /// Информация о количестве страниц 
        /// </summary>
        public FilteredItemsCountDto Paging { get; set; }
    }
}
