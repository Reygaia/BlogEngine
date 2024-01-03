using BlogEngineClone.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Libs.Entity
{
    public class FollowContainer
    {
        [NotMapped]
        public List<Follow>? Follow { get; set; }
    }
    public class FollowRequest
    {
        public string UserID { get; set; }
        public string TargetID { get; set; }
    }
    public class Follow
    {
        public Follow(string userId)
        {
            UserID = userId;
            FollowList = new FollowList();
        }
        public string? Follower
        {
            get
            {
                return FollowerID ?? string.Empty;
            }
            set
            {
                // Check if the input is a valid JSON array
                if (value.StartsWith("[") && value.EndsWith("]"))
                {
                    FollowerID = value;
                }
                else
                {
                    FollowerID = JsonConvert.SerializeObject(new List<string> { value });
                }
            }
        }
        public string UserID { get; set; }
        public FollowList? FollowList { get; set; } = new FollowList();
        // Navigation properties

        
    }
    public class FollowList
    {
        public List<string>? FollowingList { get; set; } = new List<string>();
        public List<string>? FollowerList { get; set; } = new List<string>();
    }
    
}
            }
            set => FollowerID = JsonConvert.SerializeObject(value);
        }

       

        [NotMapped]
        public List<string>? FollowingList { get; private set; }

        private void UpdateFollowingList()
        {
            if (string.IsNullOrEmpty(_followingID))
            {
                FollowingList = new List<string>();
            }
            else
            {
                FollowingList = JsonConvert.DeserializeObject<List<string>>(_followingID);
            }
        }

        // Method to initialize from a JSON file
        public void InitializeFromJsonFile(string filePath)
        {
            string jsonContent = File.ReadAllText(filePath);
            FollowingID = jsonContent;
        }

        // Navigation properties

    }
    public class FollowContainer
    {
        [NotMapped]
        public List<Follow>? Follow { get; set; }
    }
}
                else
                {
                    FollowerID = JsonConvert.SerializeObject(new List<string> { value });
                }
            }
        }
        public string? Follower
        {
            get
            {
                return FollowerID ?? string.Empty;
            }
            set
            {
                // Check if the input is a valid JSON array
                if (value.StartsWith("[") && value.EndsWith("]"))
                {
                    FollowerID = value;
                }
                else
                {
                    FollowerID = JsonConvert.SerializeObject(new List<string> { value });
                }
            }
        }
        public string UserID { get; set; }
        public FollowList? FollowList { get; set; } = new FollowList();
        // Navigation properties

        
    }
    public class FollowList
    {
        public List<string>? FollowingList { get; set; } = new List<string>();
        public List<string>? FollowerList { get; set; } = new List<string>();
    }
    
}
                else
                {
                    FollowerID = JsonConvert.SerializeObject(new List<string> { value });
                }
            }
        }
        [NotMapped]
        public List<string>? FollowerList
        {
            get
            {
                if (FollowerID == null)
                {
                    return new List<string>(); // Return an empty list or handle it according to your requirements
                }
                else
                {
                    return JsonConvert.DeserializeObject<List<string>>(FollowerID);
                }
            }
            set => FollowerID = JsonConvert.SerializeObject(value);
        }

       

        [NotMapped]
        public List<string>? FollowingList { get; private set; }

        private void UpdateFollowingList()
        {
            if (string.IsNullOrEmpty(_followingID))
            {
                FollowingList = new List<string>();
            }
            else
            {
                FollowingList = JsonConvert.DeserializeObject<List<string>>(_followingID);
            }
        }

        // Method to initialize from a JSON file
        public void InitializeFromJsonFile(string filePath)
        {
            string jsonContent = File.ReadAllText(filePath);
            FollowingID = jsonContent;
        }

        // Navigation properties

    }
    public class FollowContainer
    {
        [NotMapped]
        public List<Follow>? Follow { get; set; }
    }
}
                {
                    FollowerID = JsonConvert.SerializeObject(new List<string> { value });
                }
            }
        }
        public string? Follower
        [NotMapped]
        public List<string>? FollowerList
        {
            get
            {
                if (FollowerID == null)
                {
                    return new List<string>(); // Return an empty list or handle it according to your requirements
                }
                else
                {
                    return JsonConvert.DeserializeObject<List<string>>(FollowerID);
                }
            }
            set => FollowerID = JsonConvert.SerializeObject(value);
        }

       

        [NotMapped]
        public List<string>? FollowingList { get; private set; }

        private void UpdateFollowingList()
        {
            if (string.IsNullOrEmpty(_followingID))
            {
                FollowingList = new List<string>();
            }
            else
            {
                FollowingList = JsonConvert.DeserializeObject<List<string>>(_followingID);
            }
        }

        // Method to initialize from a JSON file
        public void InitializeFromJsonFile(string filePath)
        {
            string jsonContent = File.ReadAllText(filePath);
            FollowingID = jsonContent;
        }

        // Navigation properties

    }
    public class FollowContainer
    {
        [NotMapped]
        public List<Follow>? Follow { get; set; }
    }
}
                {
                    FollowerID = JsonConvert.SerializeObject(new List<string> { value });
                }
            }
        }
        public string? Follower
        [NotMapped]
        public List<string>? FollowerList
        {
            get
            {
                if (FollowerID == null)
                {
                    return new List<string>(); // Return an empty list or handle it according to your requirements
                }
                else
                {
                    return JsonConvert.DeserializeObject<List<string>>(FollowerID);
                }
            }
            set => FollowerID = JsonConvert.SerializeObject(value);
        }

       

        [NotMapped]
        public List<string>? FollowingList { get; private set; }

        private void UpdateFollowingList()
        {
            if (string.IsNullOrEmpty(_followingID))
            {
                FollowingList = new List<string>();
            }
            else
            {
                FollowingList = JsonConvert.DeserializeObject<List<string>>(_followingID);
            }
        }

        // Method to initialize from a JSON file
        public void InitializeFromJsonFile(string filePath)
        {
            string jsonContent = File.ReadAllText(filePath);
            FollowingID = jsonContent;
        }

        // Navigation properties

    }
    public class FollowContainer
    {
        [NotMapped]
        public List<Follow>? Follow { get; set; }
    }
}
                {
                    FollowerID = JsonConvert.SerializeObject(new List<string> { value });
                }
            }
        }
        public string? Follower
        public string UserID { get; set; }
        public FollowList? FollowList { get; set; } = new FollowList();
        // Navigation properties

        
    }
    public class FollowList
    {
        public List<string>? FollowingList { get; set; } = new List<string>();
        public List<string>? FollowerList { get; set; } = new List<string>();
    }
    
}
                {
                    FollowerID = JsonConvert.SerializeObject(new List<string> { value });
                }
            }
        }
        public string? Follower
        [NotMapped]
        public List<string>? FollowerList
        {
            get
            {
                if (FollowerID == null)
                {
                    return new List<string>(); // Return an empty list or handle it according to your requirements
                }
                else
                {
                    return JsonConvert.DeserializeObject<List<string>>(FollowerID);
                }
            }
            set => FollowerID = JsonConvert.SerializeObject(value);
        }

       

        [NotMapped]
        public List<string>? FollowingList { get; private set; }

        private void UpdateFollowingList()
        {
            if (string.IsNullOrEmpty(_followingID))
            {
                FollowingList = new List<string>();
            }
            else
            {
                FollowingList = JsonConvert.DeserializeObject<List<string>>(_followingID);
            }
        }

        // Method to initialize from a JSON file
        public void InitializeFromJsonFile(string filePath)
        {
            string jsonContent = File.ReadAllText(filePath);
            FollowingID = jsonContent;
        }

        // Navigation properties

    }
    public class FollowContainer
    {
        [NotMapped]
        public List<Follow>? Follow { get; set; }
    }
}
                {
                    FollowerID = JsonConvert.SerializeObject(new List<string> { value });
                }
            }
        }
        public string? Follower
        [NotMapped]
        public List<string>? FollowerList
        {
            get
            {
                if (FollowerID == null)
                {
                    return new List<string>(); // Return an empty list or handle it according to your requirements
                }
                else
                {
                    return JsonConvert.DeserializeObject<List<string>>(FollowerID);
                }
            }
            set => FollowerID = JsonConvert.SerializeObject(value);
        }

       

        [NotMapped]
        public List<string>? FollowingList { get; private set; }

        private void UpdateFollowingList()
        {
            if (string.IsNullOrEmpty(_followingID))
            {
                FollowingList = new List<string>();
            }
            else
            {
                FollowingList = JsonConvert.DeserializeObject<List<string>>(_followingID);
            }
        }

        // Method to initialize from a JSON file
        public void InitializeFromJsonFile(string filePath)
        {
            string jsonContent = File.ReadAllText(filePath);
            FollowingID = jsonContent;
        }

        // Navigation properties

    }
    public class FollowContainer
    {
        [NotMapped]
        public List<Follow>? Follow { get; set; }
    }
}
            }
            set => FollowerID = JsonConvert.SerializeObject(value);
        }

       

        [NotMapped]
        public List<string>? FollowingList { get; private set; }

        private void UpdateFollowingList()
        {
            if (string.IsNullOrEmpty(_followingID))
            {
                FollowingList = new List<string>();
            }
            else
            {
                FollowingList = JsonConvert.DeserializeObject<List<string>>(_followingID);
            }
        }

        // Method to initialize from a JSON file
        public void InitializeFromJsonFile(string filePath)
        {
            string jsonContent = File.ReadAllText(filePath);
            FollowingID = jsonContent;
        }

        // Navigation properties

    }
    public class FollowContainer
    {
        [NotMapped]
        public List<Follow>? Follow { get; set; }
    }
}
                {
                    FollowerID = JsonConvert.SerializeObject(new List<string> { value });
                }
            }
        }
        public string? Follower
        [NotMapped]
        public List<string>? FollowerList
        {
            get
            {
                if (FollowerID == null)
                {
                    return new List<string>(); // Return an empty list or handle it according to your requirements
                }
                else
                {
                    return JsonConvert.DeserializeObject<List<string>>(FollowerID);
                }
            }
            set => FollowerID = JsonConvert.SerializeObject(value);
        }

       

        [NotMapped]
        public List<string>? FollowingList { get; private set; }

        private void UpdateFollowingList()
        {
            if (string.IsNullOrEmpty(_followingID))
            {
                FollowingList = new List<string>();
            }
            else
            {
                FollowingList = JsonConvert.DeserializeObject<List<string>>(_followingID);
            }
        }

        // Method to initialize from a JSON file
        public void InitializeFromJsonFile(string filePath)
        {
            string jsonContent = File.ReadAllText(filePath);
            FollowingID = jsonContent;
        }

        // Navigation properties

    }
    public class FollowContainer
    {
        [NotMapped]
        public List<Follow>? Follow { get; set; }
    }
}
                {
                    FollowerID = JsonConvert.SerializeObject(new List<string> { value });
                }
            }
        }
        public string? Follower
        [NotMapped]
        public List<string>? FollowerList
        {
            get
            {
                if (FollowerID == null)
                {
                    return new List<string>(); // Return an empty list or handle it according to your requirements
                }
                else
                {
                    return JsonConvert.DeserializeObject<List<string>>(FollowerID);
                }
            }
            set => FollowerID = JsonConvert.SerializeObject(value);
        }

       

        [NotMapped]
        public List<string>? FollowingList { get; private set; }

        private void UpdateFollowingList()
        {
            if (string.IsNullOrEmpty(_followingID))
            {
                FollowingList = new List<string>();
            }
            else
            {
                FollowingList = JsonConvert.DeserializeObject<List<string>>(_followingID);
            }
        }

        // Method to initialize from a JSON file
        public void InitializeFromJsonFile(string filePath)
        {
            string jsonContent = File.ReadAllText(filePath);
            FollowingID = jsonContent;
        }

        // Navigation properties

    }
    public class FollowContainer
    {
        [NotMapped]
        public List<Follow>? Follow { get; set; }
    }
}
                {
                    FollowerID = JsonConvert.SerializeObject(new List<string> { value });
                }
            }
        }

        [NotMapped]
        public List<string>? FollowerList
        {
            get
            {
                if (FollowerID == null)
                {
                    return new List<string>(); // Return an empty list or handle it according to your requirements
                }
                else
                {
                    return JsonConvert.DeserializeObject<List<string>>(FollowerID);
                }
            }
            set => FollowerID = JsonConvert.SerializeObject(value);
        }

       

        [NotMapped]
        public List<string>? FollowingList { get; private set; }

        private void UpdateFollowingList()
        {
            if (string.IsNullOrEmpty(_followingID))
            {
                FollowingList = new List<string>();
            }
            else
            {
                FollowingList = JsonConvert.DeserializeObject<List<string>>(_followingID);
            }
        }

        // Method to initialize from a JSON file
        public void InitializeFromJsonFile(string filePath)
        {
            string jsonContent = File.ReadAllText(filePath);
            FollowingID = jsonContent;
        }

        // Navigation properties

    }
    public class FollowContainer
    {
        [NotMapped]
        public List<Follow>? Follow { get; set; }
    }
}
