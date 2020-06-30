﻿namespace Domain.i18n
{
    public class TranslationModel 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Translation { get; set; }
        public TranslationObjectTypes ObjectType { get; set; }
        public int ObjectId { get; set; }
        public Languages Language { get; set; }
    }
}
