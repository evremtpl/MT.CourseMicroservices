using Dapper;
using Microsoft.Extensions.Configuration;
using MT.FreeCourse.Discount.Services.Interfaces;
using MT.FreeCourse.Shared.Dtos;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MT.FreeCourse.Discount.Services.Concrete
{
    public class DiscountService : IDiscountService
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;

        public DiscountService(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
        }

        public async Task<Response<NoContent>> Create(Model.Discount discount)
        {
            var status = await _dbConnection.ExecuteAsync("insert into discount (userid,rate,code) values (@UserId,@Rate,@Code)", discount);

            if (status > 0)
            {
                return Response<NoContent>.Success(204);
            }
            return Response<NoContent>.Fail("an error occured while adding", 500);
        }

        public async Task<Response<NoContent>> Delete(int id)
        {
            var status = await _dbConnection.ExecuteAsync("Delete from discount where id=@Id", new { Id = id });

            return status > 0 ? Response<NoContent>.Success(204) : Response<NoContent>.Fail("discount not found", 404);
        }

        public async  Task<Response<List<Model.Discount>>> GetAll()
        {
            var discounts = await _dbConnection.QueryAsync<Model.Discount>("Select * from discount");

            return Response<List<Model.Discount>>.Success(discounts.ToList(), 200);
        }

        public async Task<Response<Model.Discount>> GetByCodeAndUserId(string code, string userId)
        {
            var discounts = await _dbConnection.QueryAsync<Model.Discount>("select * from discount where userid=@UserId and code=@Code",
                new {UserId=userId,Code=code });
            var existDiscount = discounts.FirstOrDefault();
            return existDiscount != null ? Response<Model.Discount>.Success(existDiscount, 200) : Response<Model.Discount>.Fail("Discount not found", 404);

        }

        public async Task<Response<Model.Discount>> GetById(int id)
        {
            var discount =( await _dbConnection.QueryAsync<Model.Discount>("Select * from discount where id=@id", new {Id= id })).SingleOrDefault();
            if(discount==null)
            {
                return Response<Model.Discount>.Fail("discount not found", 404);
            }
            return Response<Model.Discount>.Success(discount, 200);
        }

        public async Task<Response<NoContent>> Update(Model.Discount discount)
        {
            var status = await _dbConnection.ExecuteAsync("update discount set userid=@UserId,code=@Code,rate=@Rate where id=@Id", new
            {
                Id=discount.Id,
                UserId=discount.UserId,
                Code=discount.Code,
                Rate=discount.Rate


            });
            if(status>0)
            {
                return Response<NoContent>.Success(204);
            }
            return Response<NoContent>.Fail("discount not found", 404);
        }
    }
}
