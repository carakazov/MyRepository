using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;
using MySocialNetwork.DAO;
using MySocialNetwork.DTO;
using MySocialNetwork.Models.SocialNetwork;

namespace MySocialNetwork.Utils
{
    public class WallMapper
    {
        private SocialNetworkDbContext dbContext = new SocialNetworkDbContext();
        private Mapper mapper = new Mapper();

        public Wall WallFromCreation(AlbumCreateDto newAlbumCreate, int ownerId)
        {
            WallType wallType = dbContext.WallTypes.Where(wt => wt.Title == newAlbumCreate.Type.ToString()).First();
            Wall wall = new Wall()
            {
                OwnerId = ownerId,
                Title = newAlbumCreate.Title,
                WallTypeId = wallType.Id
            };
            return wall;
        }

        public IEnumerable<AlbumDto> ConvertInAlbumDtoList(IEnumerable<Wall> walls)
        {
            List<AlbumDto> albums = new List<AlbumDto>();
            walls.ForEach(w => albums.Add(GetAlbumDto(w)));
            return albums;
        }
        
        
        public AlbumDto GetAlbumDto(Wall wall)
        {
            AlbumDto createDto = new AlbumDto();
            createDto.AlbumTitle = wall.Title;
            createDto.AlbumWall = mapper.FromWallToWallDto(wall);
            return createDto;
        }
    }
}