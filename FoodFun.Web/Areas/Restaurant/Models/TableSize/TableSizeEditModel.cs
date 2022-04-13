﻿namespace FoodFun.Web.Areas.Restaurant.Models.TableSize
{
    using FoodFun.Core.ValidationAttributes.TableSize;
    using System.ComponentModel.DataAnnotations;

    using static Constants.GlobalConstants.Messages;
    using static FoodFun.Infrastructure.Common.DataConstants.TableSize;

    public class TableSizeEditModel
    {
        [ShouldBeExistingTableSize]
        public int Id { get; init; }

        [Range(
            SizeMinLength,
            int.MaxValue,
            ErrorMessage = TableSizeError)]
        [ShouldBeUniqueTableSize]
        public int Seats { get; init; }
    }
}
