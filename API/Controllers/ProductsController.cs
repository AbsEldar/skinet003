using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specification;
using AutoMapper;
using API.Dtos;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController: ControllerBase
    {
      private readonly IGenericRepository<Product> _productsRepo;
      private readonly IGenericRepository<ProductBrand> _productBrandsRepo;
      private readonly IGenericRepository<ProductType> _productTypesRepo;
      private readonly IMapper _mapper;
      public ProductsController(IGenericRepository<Product> productsRepo, 
                                 IGenericRepository<ProductBrand> productBrandsRepo,
                                 IGenericRepository<ProductType> productTypesRepo,
                                 IMapper mapper )
      {
          _productBrandsRepo = productBrandsRepo;
          _productsRepo = productsRepo;
          _productTypesRepo = productTypesRepo;
          _mapper = mapper;
      }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            // return Ok(await _productsRepo.ListAllAsync());
            var spec = new ProductsWithTypesAndBrandsSpecification();
            var products = await _productsRepo.ListAsync(spec);
            var ret = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
            return Ok(ret);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            // return Ok(await _productsRepo.GetByIdAsync(id));
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _productsRepo.GetEntityWithSpec(spec);
            var ret = _mapper.Map<Product, ProductToReturnDto>(product);
            return Ok(ret);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<List<ProductBrand>>> GetProductBrands()
        {
          return Ok(await _productBrandsRepo.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<List<ProductType>>> GetProductTypes()
        {
          return Ok(await _productTypesRepo.ListAllAsync());
        }
    }
}