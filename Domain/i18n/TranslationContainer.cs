﻿using System.Collections.Generic;

namespace Domain.i18n
{
    public class TranslationContainer
    {
        //  Inner Dictionary key is Id of translated object (ObjectId of translation)
        public Dictionary<Languages, Dictionary<int, string>> Countries { get; set; } = new Dictionary<Languages, Dictionary<int, string>>();
    }
}
