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
using YesLocation.Application.DTOs.Season;
using YesLocation.Application.DTOs.DurationTier;
using YesLocation.Application.DTOs.VehiclePricing;
using YesLocation.Application.DTOs.Agency;

namespace YesLocation.Api.Mappings;

public class MappingProfile : Profile
{
  public MappingProfile()
  {
    // Booking mappings
    CreateMap<BookingInputDto, Booking>()
      .ForMember(dest => dest.Customer, opt => opt.Ignore())
      .ForMember(dest => dest.Vehicle, opt => opt.Ignore())
      .ForMember(dest => dest.PickupLocation, opt => opt.Ignore())
      .ForMember(dest => dest.ReturnLocation, opt => opt.Ignore())
      .ForMember(dest => dest.Quotation, opt => opt.Ignore())
      .ForMember(dest => dest.Invoice, opt => opt.Ignore());
    CreateMap<Booking, BookingDto>();

    // Customer mappings
    CreateMap<CustomerInputDto, Customer>()
      .ForMember(dest => dest.Bookings, opt => opt.Ignore())
      .ForMember(dest => dest.Quotations, opt => opt.Ignore())
      .ForMember(dest => dest.Invoices, opt => opt.Ignore());
    CreateMap<Customer, CustomerDto>();

    // Invoice mappings
    CreateMap<InvoiceInputDto, Invoice>()
      .ForMember(dest => dest.Booking, opt => opt.Ignore())
      .ForMember(dest => dest.Payments, opt => opt.Ignore())
      .ForMember(dest => dest.PaidAmount, opt => opt.Ignore());
    CreateMap<Invoice, InvoiceDto>();

    // Location mappings
    CreateMap<LocationInputDto, Location>()
      .ForMember(dest => dest.PickupBookings, opt => opt.Ignore())
      .ForMember(dest => dest.ReturnBookings, opt => opt.Ignore());
    CreateMap<Location, LocationDto>();

    // MaintenanceRecord mappings
    CreateMap<MaintenanceRecordInputDto, MaintenanceRecord>()
      .ForMember(dest => dest.Vehicle, opt => opt.Ignore());
    CreateMap<MaintenanceRecord, MaintenanceRecordDto>();

    // Payment mappings
    CreateMap<PaymentInputDto, Payment>()
      .ForMember(dest => dest.Invoice, opt => opt.Ignore());
    CreateMap<Payment, PaymentDto>();

    // Quotation mappings
    CreateMap<QuotationInputDto, Quotation>()
      .ForMember(dest => dest.Booking, opt => opt.Ignore())
      .ForMember(dest => dest.Customer, opt => opt.Ignore())
      .ForMember(dest => dest.Vehicle, opt => opt.Ignore());
    CreateMap<Quotation, QuotationDto>();

    // Role mappings
    CreateMap<RoleInputDto, Role>()
      .ForMember(dest => dest.UserRoles, opt => opt.Ignore());
    CreateMap<Role, RoleDto>();

    // User mappings
    CreateMap<UserInputDto, User>()
      .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
      .ForMember(dest => dest.Auth, opt => opt.Ignore());
    CreateMap<User, UserInputDto>();
    CreateMap<User, UserDto>();

    // Vehicle mappings
    CreateMap<VehicleInputDto, Vehicle>()
      .ForMember(dest => dest.Bookings, opt => opt.Ignore())
      .ForMember(dest => dest.MaintenanceRecords, opt => opt.Ignore())
      .ForMember(dest => dest.Pricings, opt => opt.Ignore());
    CreateMap<Vehicle, VehicleDto>();

    // Season mappings
    CreateMap<SeasonInputDto, Season>()
      .ForMember(dest => dest.VehiclePricings, opt => opt.Ignore());
    CreateMap<Season, SeasonDto>();

    // DurationTier mappings
    CreateMap<DurationTierInputDto, DurationTier>()
      .ForMember(dest => dest.VehiclePricings, opt => opt.Ignore());
    CreateMap<DurationTier, DurationTierDto>();

    // VehiclePricing mappings
    CreateMap<VehiclePricingInputDto, VehiclePricing>()
      .ForMember(dest => dest.Vehicle, opt => opt.Ignore())
      .ForMember(dest => dest.Season, opt => opt.Ignore())
      .ForMember(dest => dest.DurationTier, opt => opt.Ignore());
    CreateMap<VehiclePricing, VehiclePricingDto>()
      .ForMember(dest => dest.Vehicle, opt => opt.ExplicitExpansion())
      .ForMember(dest => dest.Season, opt => opt.ExplicitExpansion())
      .ForMember(dest => dest.DurationTier, opt => opt.ExplicitExpansion());

    // Agency mappings
    CreateMap<AgencyInputDto, Agency>();
    CreateMap<Agency, AgencyDto>();
  }
}