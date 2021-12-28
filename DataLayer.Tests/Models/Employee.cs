using System;
using Dapper;
using Models;
using Models.Constants;
using TableAttribute = Dapper.Contrib.Extensions.TableAttribute;

namespace DataLayer.Tests.Models
{
    [Database(DatabaseNames.YourDatabase)]
    [Table("Employee")]
    public class Employee : BaseModel
    {
        [Key]
        public int Id { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("date_of_birth")]
        public DateTime DateOfBirth { get; set; }

        [Column("create_date")]
        public DateTime? CreateDate { get; set; }
    }
}
