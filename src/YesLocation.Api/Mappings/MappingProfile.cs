using AutoMapper;
using YesLocation.Domain.Entities;
using YesLocation.Application.DTOs;
using YesLocation.Application.DTOs.Booking;
using YesLocation.Application.DTOs.Vehicle;
using YesLocation.Application.DTOs.Location;
using YesLocation.Application.DTOs.Customer;
using YesLocation.Application.DTOs.Quotation;
using YesLocation.Application.DTOs.Invoice;
using YesLocation.Application.DTOs.Payment;
using YesLocation.Application.DTOs.MaintenanceRecord;
using YesLocation.Application.DTOs.User;
using YesLocation.Application.DTOs.Role;

namespace YesLocation.Api.Mappings;

public class MappingProfile : Profile
{
  public MappingProfile()
  {
    // Booking mappings
    CreateMap<CreateBookingDto, Booking>()
      .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
      .ForMember(dest => dest.Customer, opt => opt.Ignore())
      .ForMember(dest => dest.Vehicle, opt => opt.Ignore())
      .ForMember(dest => dest.PickupLocation, opt => opt.Ignore())
      .ForMember(dest => dest.ReturnLocation, opt => opt.Ignore())
      .ForMember(dest => dest.Quotation, opt => opt.Ignore())
      .ForMember(dest => dest.Invoice, opt => opt.Ignore())
      .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
      .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
      .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
      .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());

    CreateMap<Booking, BookingDto>()

    // Vehicle mappings
    CreateMap<CreateVehicleDto, Vehicle>()
      .ForMember(dest => dest.Bookings, opt => opt.Ignore())
      .ForMember(dest => dest.MaintenanceRecords, opt => opt.Ignore())
      .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
      .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
      .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
      .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());

    CreateMap<Vehicle, VehicleDto>();

    // Location mappings
    CreateMap<CreateLocationDto, Location>()
      .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
      .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.PostalCode))
      .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
      .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
      .ForMember(dest => dest.PickupBookings, opt => opt.Ignore())
      .ForMember(dest => dest.ReturnBookings, opt => opt.Ignore())
      .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
      .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
      .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
      .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());

    CreateMap<Location, LocationDto>()
      .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone));

    // Customer mappings
    CreateMap<CreateCustomerDto, Customer>()
      .ForMember(dest => dest.Bookings, opt => opt.Ignore())
      .ForMember(dest => dest.Quotations, opt => opt.Ignore())
      .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
      .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
      .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
      .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());

    CreateMap<Customer, CustomerDto>();

    // Quotation mappings
    CreateMap<CreateQuotationDto, Quotation>()
      .ForMember(dest => dest.Customer, opt => opt.Ignore())
      .ForMember(dest => dest.Vehicle, opt => opt.Ignore())
      .ForMember(dest => dest.ValidUntil, opt => opt.MapFrom(src => src.ValidUntil))
      .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
      .ForMember(dest => dest.Booking, opt => opt.Ignore())
      .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
      .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
      .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
      .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());

    CreateMap<Quotation, QuotationDto>()
      .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
      .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
      .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Amount));

    // Invoice mappings
    CreateMap<CreateInvoiceDto, Invoice>()
      .ForMember(dest => dest.Booking, opt => opt.Ignore())
      .ForMember(dest => dest.InvoiceNumber, opt => opt.MapFrom(src => src.InvoiceNumber))
      .ForMember(dest => dest.IssueDate, opt => opt.MapFrom(src => src.IssueDate))
      .ForMember(dest => dest.PaidAmount, opt => opt.MapFrom(src => src.PaidAmount))
      .ForMember(dest => dest.Payments, opt => opt.Ignore())
      .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
      .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
      .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
      .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());

    CreateMap<Invoice, InvoiceDto>();

    // Payment mappings
    CreateMap<CreatePaymentDto, Payment>()
      .ForMember(dest => dest.Invoice, opt => opt.Ignore())
      .ForMember(dest => dest.TransactionReference, opt => opt.MapFrom(src => src.TransactionReference))
      .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
      .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
      .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
      .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());

    CreateMap<Payment, PaymentDto>()
      .ForMember(dest => dest.RentalId, opt => opt.MapFrom(src => src.InvoiceId));

    // MaintenanceRecord mappings
    CreateMap<CreateMaintenanceRecordDto, MaintenanceRecord>()
      .ForMember(dest => dest.Vehicle, opt => opt.Ignore())
      .ForMember(dest => dest.ServiceDate, opt => opt.MapFrom(src => src.MaintenanceDate))
      .ForMember(dest => dest.Mileage, opt => opt.MapFrom(src => src.Mileage))
      .ForMember(dest => dest.ServiceProvider, opt => opt.MapFrom(src => src.ServiceProvider))
      .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
      .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
      .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
      .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
      .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());

    CreateMap<MaintenanceRecord, MaintenanceRecordDto>()
      .ForMember(dest => dest.MaintenanceDate, opt => opt.MapFrom(src => src.ServiceDate));

    // User mappings
    CreateMap<UserCreateDto, User>()
      .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
      .ForMember(dest => dest.Auth, opt => opt.Ignore())
      .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
      .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
      .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
      .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());

    CreateMap<User, UserCreateDto>();
    CreateMap<User, UserDto>();

    // Role mappings
    CreateMap<Role, RoleDto>();
  }
}