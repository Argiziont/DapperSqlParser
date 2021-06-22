using System;

namespace DapperSqlParser.TestRepository.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ExternalId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime SyncDate { get; set; }
        public int ProductState { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
    }
}