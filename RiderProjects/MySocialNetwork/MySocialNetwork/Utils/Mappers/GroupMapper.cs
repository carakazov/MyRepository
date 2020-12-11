using System.Collections.Generic;
using System.Linq;
using MySocialNetwork.DAO;
using MySocialNetwork.DTO;
using System.Data.Entity;
using Antlr.Runtime.Misc;
using Microsoft.Ajax.Utilities;

namespace MySocialNetwork.Utils
{
    public class GroupMapper
    {
        private SocialNetworkDbContext dbContext = new SocialNetworkDbContext();
        public Group GetNewGroup(GroupCreationDto creatingDto)
        {
            Group group = new Group();
            group.Title = creatingDto.Title;
            GroupType type = dbContext.GroupTypes.Where(r => r.Title == creatingDto.Type.ToString()).First();
            group.GroupTypeId = type.Id;
            return group;
        }

        public GroupDto GetGroupInDto(Group group, int userId)
        {
            GroupDto groupDto = new GroupDto()
            {
                Id = group.Id,
                Title = group.Title
            };
            ReaderProfile profile = dbContext.ReaderProfiles.Where(p => p.UserId == userId && p.GroupId == group.Id)
                .Include(p => p.GroupRole)
                .Include(p => p.ReaderProfileState).First();
            if (profile.ReaderProfileState.Title == ReaderProfileStates.FullAccess.ToString())
            {
                groupDto.ReaderState = ReaderProfileStates.FullAccess;
            }
            else if (profile.ReaderProfileState.Title == ReaderProfileStates.ReadOnly.ToString())
            {
                groupDto.ReaderState = ReaderProfileStates.ReadOnly;
            }
            else
            {
                groupDto.ReaderState = ReaderProfileStates.Banned;
            }

            if (profile.GroupRole.Title == GroupRoles.Admin.ToString())
            {
                groupDto.GroupRole = GroupRoles.Admin;
            }
            else if (profile.GroupRole.Title == GroupRoles.Moder.ToString())
            {
                groupDto.GroupRole = GroupRoles.Moder;
            }
            else
            {
                groupDto.GroupRole = GroupRoles.Reader;
            }

            return groupDto;
        }

        public List<GroupDto> GetGroupDtoList(List<Group> groups, int userId)
        {
            List<GroupDto> groupsDto = new ListStack<GroupDto>();
            groups.ForEach(g => groupsDto.Add(GetGroupInDto(g, userId)));
            return groupsDto;
        } 
    }
}