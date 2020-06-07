using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Plark_MobileClient.Models
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long UserId { get; set; }
        public ObservableCollection<Car> Cars { get; set; }
    }
}
