using System;
using System.Collections.Generic;
using System.Text;

namespace RobloxApiImplementation.CallsObjects.GetAuditLogV1
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Actor
    {
        public User user { get; set; }
        public Role role { get; set; }
    }

    public class Datum
    {
        public Actor actor { get; set; }
        public string actionType { get; set; }
        public Description description { get; set; }
        public DateTime created { get; set; }
    }

    public class Description
    {
        public object TargetId { get; set; }
        public int NewRoleSetId { get; set; }
        public int OldRoleSetId { get; set; }
        public string TargetName { get; set; }
        public string NewRoleSetName { get; set; }
        public string OldRoleSetName { get; set; }
        public string NewDescription { get; set; }
        public string NewName { get; set; }
        public string OldDescription { get; set; }
        public string OldName { get; set; }
        public int? RoleSetId { get; set; }
        public string RoleSetName { get; set; }
    }

    public class Role
    {
        public int id { get; set; }
        public string name { get; set; }
        public int rank { get; set; }
    }

    public class Root
    {
        public object previousPageCursor { get; set; }
        public object nextPageCursor { get; set; }
        public List<Datum> data { get; set; }
    }

    public class User
    {
        public bool hasVerifiedBadge { get; set; }
        public int userId { get; set; }
        public string username { get; set; }
        public string displayName { get; set; }
    }


}
