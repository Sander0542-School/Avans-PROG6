﻿using AutoMapper;

namespace ShipsInSpace.Logic
{
    public static class ObjectCloner
    {
        public static T Clone<T>(this T value) => new MapperConfiguration(cfg => { cfg.CreateMap<T, T>(); }).CreateMapper().Map<T, T>(value);
    }
}