using System;
using System.Collections.Generic;
using System.Text;

namespace RobloxApiImplementation.CallsObjects.GroupInfoV1
{
    public class Owner
    {
        public bool HasVerifiedBadge { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
    }

    public class Poster
    {
        public bool HasVerifiedBadge { get; set; }
        public long UserId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
    }

    public class Root
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Owner Owner { get; set; }
        public Shout Shout { get; set; }
        public int MemberCount { get; set; }
        public bool IsBuildersClubOnly { get; set; }
        public bool PublicEntryAllowed { get; set; }
        public bool HasVerifiedBadge { get; set; }
    }

    public class Shout
    {
        public string Body { get; set; }
        public Poster Poster { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }

}
