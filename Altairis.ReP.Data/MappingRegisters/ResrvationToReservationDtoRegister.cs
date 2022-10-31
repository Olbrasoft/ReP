﻿using Altairis.ReP.Data.Entities;
using Mapster;

namespace Olbrasoft.Blog.Data.MappingRegisters;

public class ResrvationToReservationDtoRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Reservation, ReservationEditDto>().Map(rcd => rcd.UserName, r => r.User!.UserName);
    }
}