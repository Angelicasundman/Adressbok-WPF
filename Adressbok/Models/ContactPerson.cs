using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adressbok.Models
{
    
        public class ContactPerson
        {
            public Guid Id { get; set; } = Guid.NewGuid(); //skapar ett nytt unikt id för varje ny kontakt som blir
            public string FirstName { get; set; } 
            public string LastName { get; set; } 

            public string Email { get; set; }

            public string StreetName { get; set; } 

            public string PostalCode { get; set; }

            public string City { get; set; }  

            public string FullName => $"{FirstName} {LastName}";


        }
    
}
