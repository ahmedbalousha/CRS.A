﻿using AutoMapper;
using CRS.Core.Exceptions;
using CRS.Core.ViewModels;
using CRS.Data.Models;
using CRS.Data;
using CRS.Infrastructure.Services.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRS.Core.Dtos;
using CRS.Core.Enums;

namespace CRS.Infrastructure.Services.Advertisements
{
    public class AdvertisementService : IAdvertisementService
    {

        private readonly CRSDbContext _db;
        private readonly IMapper _mapper;
        private readonly IUserService  _userService;
        private readonly IFileService _fileService;

        public AdvertisementService(IFileService fileService,CRSDbContext db, IMapper mapper, IUserService userService)
        {
            _db = db;
            _mapper = mapper;
            _fileService = fileService;
            _userService = userService;
        }

        
        public async Task<List<UserViewModel>> GetAdvertisementAdvertisers()
        {
            var users = await _db.Users.Where(x => !x.IsDelete && x.UserType == UserType.Advertiser).ToListAsync();
            return _mapper.Map<List<UserViewModel>>(users);
        }

        public async Task<ResponseDto> GetAll(Pagination pagination, Query query)
        {
            var queryString = _db.Advertisements.Include(x => x.Advertiser).Where(x => !x.IsDelete).AsQueryable();

            var dataCount = queryString.Count();
            var skipValue = pagination.GetSkipValue();
            var dataList = await queryString.Skip(skipValue).Take(pagination.PerPage).ToListAsync();
            var advertisements = _mapper.Map<List<AdvertisementViewModel>>(dataList);
            var pages = pagination.GetPages(dataCount);
            var result = new ResponseDto
            {
                data = advertisements,
                meta = new Meta
                {
                    page = pagination.Page,
                    perpage = pagination.PerPage,
                    pages = pages,
                    total = dataCount,
                }
            };
            return result;
        }

        public async Task<int> Delete(int id)
        {
            var advertisement = await _db.Advertisements.SingleOrDefaultAsync(x => x.Id == id && !x.IsDelete);
            if(advertisement == null)
            {
                throw new EntityNotFoundException();
            }
            advertisement.IsDelete = true;
            _db.Advertisements.Update(advertisement);
            await _db.SaveChangesAsync();
            return advertisement.Id;
        }

        public async Task<UpdateAdvertisementDto> Get(int id)
        {
            var advertisement = await _db.Advertisements.SingleOrDefaultAsync(x => x.Id == id && !x.IsDelete);
            if (advertisement == null)
            {
                throw new EntityNotFoundException();
            }
            return _mapper.Map<UpdateAdvertisementDto>(advertisement);
        }


        public async Task<int> Create(CreateAdvertisementDto dto)
        {

            if(dto.StartDate >= dto.EndDate)
            {
                throw new InvalidDateException();
            }

            var advertisement = _mapper.Map<Advertisement>(dto);
            if(dto.Image != null)
            {
                advertisement.ImageUrl = await _fileService.SaveFile(dto.Image, "Images");
            }

            await _db.Advertisements.AddAsync(advertisement);
            await _db.SaveChangesAsync();
            return advertisement.Id;
        }


        public async Task<int> Update(UpdateAdvertisementDto dto)
        {

            if (dto.StartDate >= dto.EndDate)
            {
                throw new InvalidDateException();
            }

            var advertisement = await _db.Advertisements.SingleOrDefaultAsync(x => x.Id == dto.Id && !x.IsDelete);
            if(advertisement == null)
            {
                throw new EntityNotFoundException();
            }

            var updatedAdvertisement = _mapper.Map(dto, advertisement);

            if (dto.Image != null)
            {
                updatedAdvertisement.ImageUrl = await _fileService.SaveFile(dto.Image, "Images");
            }

             _db.Advertisements.Update(updatedAdvertisement);
             await _db.SaveChangesAsync();

            return advertisement.Id;
        }

    }
}
