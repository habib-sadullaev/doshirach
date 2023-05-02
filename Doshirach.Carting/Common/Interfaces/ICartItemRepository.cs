﻿using Doshirach.Carting.Common.Models;

namespace Doshirach.Carting.Common.Interfaces;

public interface ICartItemRepository
{
	public CartItem[] GetCartItems(int cartId);
	public void AddCartItem(int cartId, CartItem cartItem);
	public void RemoveCartItem(int cartId, int cartItemId);
}
